using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCommon : MonoBehaviour {
    public string id;
    //not use for PiPiece UI
    public Image btnImage;
    //only use for PiPiece UI
    public PiPiece piPiece;
    public float scaleoffset = 1.3f;
    public Color colorHighLight;
    public Color colorNormal;

    public void OnSelected()
    {
        if (piPiece != null)
            piPiece.isOver = true;
        else
        {
            transform.localScale = new Vector3(scaleoffset, scaleoffset, 1);
            btnImage.color = colorHighLight;
        }

    }

    public void OnUnselected()
    {
        if (piPiece != null)
            piPiece.isOver = false;
        else
        {
            transform.localScale = Vector3.one;
            btnImage.color = colorNormal;
        }
    }
}
