using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCommon : MonoBehaviour {
    public List<Image> listBtnImage = new List<Image>();
    public Button btn;
    private float scaleoffset = 1.5f;
    public Color colorHighLight;
    public Color colorNormal;
    public Vector3 originScale;

    public void Start()
    {
        originScale = transform.localScale;
    }

    public void OnSelected()
    {
        transform.localScale = originScale * scaleoffset;
        if(listBtnImage != null)
        {
            for (int i = 0; i < listBtnImage.Count; i++)
                listBtnImage[i].color = colorHighLight;
        }
    }

    public void OnUnselected()
    {
        transform.localScale = originScale;
        if (listBtnImage != null)
        {
            for (int i = 0; i < listBtnImage.Count; i++)
                listBtnImage[i].color = colorNormal;
        }
    }

    public void OnClicked()
    {
        btn.onClick.Invoke();
    }
}
