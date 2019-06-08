using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class MainUI : MonoBehaviour {
    public List<ButtonCommon> listButton = new List<ButtonCommon>();
    public int currButtonIndex=-1;

    public MenuInteractionController menuInteraction;

	// Use this for initialization
	void Start () {
        //Invoke("Test0", 2);
#if !UNITY_EDITOR
        AddListener();
        InvokeRepeating("UpdateCurrButtonSelect", 1f, 0.1f);
#endif
    }

    void Update()
    {
#if !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (currButtonIndex > 0)
                currButtonIndex--;
            else
                currButtonIndex = listButton.Count - 1;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currButtonIndex < listButton.Count - 1)
                currButtonIndex++;
            else
                currButtonIndex = 0;
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            if (currButtonIndex >= 0 && currButtonIndex <= listButton.Count - 1)
                listButton[currButtonIndex].OnClicked();
        }
#endif
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
        UIEvent.OnUpdateUI += OnUpdateUI;
        InteractionManager.InteractionSourcePressed += InteractionSourcePressed;
        InteractionManager.InteractionSourceUpdated += InteractionSourceUpdated;
    }

    private void RemoveListener()
    {
        UIEvent.OnUpdateUI -= OnUpdateUI;
        InteractionManager.InteractionSourcePressed -= InteractionSourcePressed;
        InteractionManager.InteractionSourceUpdated -= InteractionSourceUpdated;
    }

    public void OnUpdateUI()
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
            {
                if(listButton[i]!=null)
                    listButton[i].OnSelected();
            }
            else
            {
                if(listButton[i]!=null)
                    listButton[i].OnUnselected();
            }
        }
    }

    private void InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        Vector2 pos = obj.state.thumbstickPosition;
        if (pos == Vector2.zero)
            return;

        if ((Mathf.Abs(pos.x) >= Mathf.Abs(pos.y) && pos.x <= 0) || (Mathf.Abs(pos.x) <= Mathf.Abs(pos.y) && pos.y <= 0))
        {
            if (currButtonIndex > 0)
                currButtonIndex--;
            else
                currButtonIndex = listButton.Count - 1;
        }
        else
        {
            if (currButtonIndex < listButton.Count - 1)
                currButtonIndex++;
            else
                currButtonIndex = 0;
        }
    }

    private void InteractionSourcePressed(InteractionSourcePressedEventArgs obj)
    {
        if(obj.pressType == InteractionSourcePressType.Select || obj.pressType == InteractionSourcePressType.Grasp)
        {
            if (currButtonIndex >= 0 && currButtonIndex <= listButton.Count - 1)
                listButton[currButtonIndex].OnClicked();
        }
    }
}
