using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour {
    public List<ButtonCommon> listButton = new List<ButtonCommon>();
    public int currButtonIndex=-1;

    public MenuInteractionController menuInteraction;

	// Use this for initialization
	void Start () {
        AddListener();

        Invoke("Test0", 2);
        InvokeRepeating("UpdateCurrButtonSelect", 1f, 0.1f);
    }

    public void Test0()
    {
        RecognizeObject testObj = new RecognizeObject();
        testObj.name = "laptop";
        menuInteraction.OnMainObjectDetected(testObj);
        Invoke("Test1", 3);
    }

    public void Test1()
    {
        RecognizeObject testObj = new RecognizeObject();
        testObj.name = "laptop";
        menuInteraction.OnMainObjectDetected(testObj);
        menuInteraction.OnClickCreateMenuInteraction();
        Invoke("UpdateListButton", 1f);
    }

    void OnDestroy()
    {
        RemoveListener();
        CancelInvoke();
    }

    private void AddListener()
    {
        UIEvent.OnButtonClicked += OnButtonClicked;
    }

    private void RemoveListener()
    {
        UIEvent.OnButtonClicked -= OnButtonClicked;
    }

    public void OnButtonClicked()
    {
        Invoke("UpdateListButton", 1f);
    }

    private void UpdateListButton()
    {
        listButton.Clear();
        currButtonIndex = -1;

        ButtonCommon[] currButtonOnScene = gameObject.GetComponentsInChildren<ButtonCommon>();
        if (currButtonOnScene == null || currButtonOnScene.Length == 0)
            return;

        for (int i = 0; i < currButtonOnScene.Length; i++)
        {
            if (currButtonOnScene[i].gameObject.activeInHierarchy)
                listButton.Add(currButtonOnScene[i]);
        }

        currButtonIndex = 0;
    }

    public void IncreaseIndex()
    {
        currButtonIndex++;
    }

    public void DecreaseIndex()
    {
        currButtonIndex--;
    }

    private void UpdateCurrButtonSelect()
    {
        for(int i=0;i<listButton.Count;i++)
        {
            if (i == currButtonIndex)
                listButton[i].OnSelected();
            else
                listButton[i].OnUnselected();
        }
    }
}
