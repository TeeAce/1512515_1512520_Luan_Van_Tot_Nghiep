using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RadialItem : MonoBehaviour {

    public RectTransform currRT;
    public RectTransform btnRT;
    public Image icon;
    public Text lbContent;

    private string id;
    public static Action<string> OnItemClicked;

    public void SetData(string content, Sprite sprite, float zRot)
    {
        id = content;
        icon.sprite = sprite;
        lbContent.text = content;

        currRT.localRotation = Quaternion.Euler(0, 0, zRot);
        btnRT.localRotation = Quaternion.Euler(0, 0, -zRot);
    }

    public void OnClicked()
    {
        if (OnItemClicked != null)
            OnItemClicked(id);
    }
}
