using HoloToolkit.Unity.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class TestMC : MonoBehaviour {

    public void Start()
    {
        InteractionManager.InteractionSourcePressed += InteractionSourcePressed;
        InteractionManager.InteractionSourceUpdated += InteractionSourceUpdate;
    }


    private void InteractionSourceUpdate(InteractionSourceUpdatedEventArgs obj)
    {
        Debug.Log("ThumStick Postion: " + obj.state.thumbstickPosition);
    }

    private void InteractionSourcePressed(InteractionSourcePressedEventArgs obj)
    {
        Debug.Log(obj.pressType.ToString());
        if (obj.pressType == InteractionSourcePressType.Touchpad)
            Debug.Log("Position: " + obj.state.touchpadPosition);
    }
}
