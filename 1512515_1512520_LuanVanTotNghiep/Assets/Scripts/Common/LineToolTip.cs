using HoloToolkit.Unity.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToolTip : MonoBehaviour {
    public LineBase lineBase;
    public MeshRenderer label;
	// Update is called once per frame
	void Update () {
        if (label.isVisible)
            lineBase.LineEndClamp = 1;
        else
            lineBase.LineEndClamp = 0;
    }
}
