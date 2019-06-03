using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoundBoxItem : MonoBehaviour {

	public Text lbContent;
    public Text lbDesc;
    public RectTransform panelBGDes;
    public RectTransform line;
    public float posXLeft;
    public float posXRight;
    public float posYUp;
    public float posYDown;
    public float rotZLeftTop;
    public float rotZRightTop;
    public float rotZLeftDown;
    public float rotZRightDown;

    public void OptimizeDes(float x,float y, float screenWidth, float screenHeight)
    {
        // Nguoc Lai
        if (x<(screenWidth/2)) //right
            panelBGDes.localPosition = new Vector3(posXRight, panelBGDes.localPosition.y, panelBGDes.localPosition.z);
        else //left
            panelBGDes.localPosition = new Vector3(posXLeft, panelBGDes.localPosition.y, panelBGDes.localPosition.z);


        if (y<(-screenHeight/2)) //top
            panelBGDes.localPosition = new Vector3(panelBGDes.localPosition.x, posYUp, panelBGDes.localPosition.z);
        else //down
            panelBGDes.localPosition = new Vector3(panelBGDes.localPosition.x, posYDown, panelBGDes.localPosition.z);

        if (x < (screenWidth / 2) && y < (-screenHeight / 2)) //right top
            line.rotation = Quaternion.Euler(0, 0, rotZRightTop);
        else if(x >= (screenWidth / 2) && y < (-screenHeight / 2)) //left top
            line.rotation = Quaternion.Euler(0, 0, rotZLeftTop);
        else if(x < (screenWidth / 2) && y >= (-screenHeight / 2)) //right down
            line.rotation = Quaternion.Euler(0, 0, rotZRightDown);
        else if(x >= (screenWidth / 2) && y >= (-screenHeight / 2)) //left down
            line.rotation = Quaternion.Euler(0, 0, rotZLeftDown);

    }
}
