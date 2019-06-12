using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCommon : MonoBehaviour {
    public Image btnImage;
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
        if(btnImage!=null)
            btnImage.color = colorHighLight;
    }

    public void OnUnselected()
    {
        transform.localScale = originScale;
        if (btnImage != null)
            btnImage.color = colorNormal;
    }

    public void OnClicked()
    {
        btn.onClick.Invoke();
    }
}
