using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialUI : MonoBehaviour {

    public Transform panelButton;
    public RadialItem itemPrefab;
    public List<Sprite> listIcon;

    public MenuInteractionController menuInteraction;

    void Start()
    {
        AddListener();

        gameObject.GetComponent<Canvas>().sortingOrder = 101;
    }

    void OnDestroy()
    {
        RemoveListener();
    }

    private void AddListener()
    {
        if (menuInteraction != null)
            menuInteraction.OnControll += OnControll;
    }

    private void RemoveListener()
    {
        if (menuInteraction != null)
            menuInteraction.OnControll -= OnControll;
    }

    private void OnControll(bool isControlling)
    {
        if (!isControlling)
            ClearAllItem(panelButton);
    }

    public void InitFeatures(Features f)
    {
        ClearAllItem(panelButton);
        if (f == null || f.features == null)
            return;

        for(int i=0;i<f.features.Length;i++)
        {
            RadialItem item = Instantiate(itemPrefab, panelButton);
            Sprite itemIcon = Resources.Load<Sprite>(AppConstant.PATH_ICON + f.features[i]);
            if (itemIcon == null)
                itemIcon = listIcon[0];
            item.SetData(f.features[i], itemIcon, (float)(360*i) / f.features.Length);
            item.gameObject.SetActive(true);
        }
    }

    public void ClearAllItem(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
