using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteractionController : MonoBehaviour {

    public Client client;
    public PiUIManager piUi;
    private bool menuOpened;
    private PiUI normalMenu;
    public Color colorNormal;
    public Color colorHightlight;

    public GameObject panelRecognize;
    public GameObject btnExitControll;
    public string currObjId = "2";

    private void Start()
    {
        //Get menu for easy not repetitive getting of the menu when setting joystick input
        normalMenu = piUi.GetPiUIOf("Normal Menu");
    }

    public void OnClickCreateMenuInteraction()
    {
        client.GetFeaturesById(currObjId);
    }

    public void ShowMenuInteraction(Features f)
    {
        //Make all angles equal 
        normalMenu.equalSlices = true;
        normalMenu.iconDistance = 1f;
        //Changes the piDataLength and adds new piData
        normalMenu.piData = new PiUI.PiData[f.features.Length];
        for (int j = 0; j < f.features.Length; j++)
        {
            normalMenu.piData[j] = new PiUI.PiData();
        }
        //Turns of the syncing of colors
        normalMenu.syncColors = false;
        //Changes open/Close animations
        normalMenu.openTransition = PiUI.TransitionType.ScaleAndFan;
        normalMenu.closeTransition = PiUI.TransitionType.SlideRight;
        int i = 0;
        foreach (PiUI.PiData data in normalMenu.piData)
        {
            //new
            data.id = f.features[i];
            data.onClickMenu += OnClickMenu;

            //Set new highlight/non highlight colors
            data.nonHighlightedColor = colorNormal;
            data.highlightedColor = colorHightlight;
            data.disabledColor = colorNormal;
            //Changes slice label
            data.sliceLabel = f.features[i];
            //Creates a new unity event and adds the testfunction to it
            data.onSlicePressed = new UnityEngine.Events.UnityEvent();
            data.onSlicePressed.AddListener(OnSlidePressed);
            i += 1;
            //Enables hoverFunctions
            data.hoverFunctions = true;
            //Creates a new unity event to adds on hovers function
            data.onHoverEnter = new UnityEngine.Events.UnityEvent();
            data.onHoverEnter.AddListener(OnHoverEnter);
            data.onHoverExit = new UnityEngine.Events.UnityEvent();
            data.onHoverExit.AddListener(OnHoverExit);
        }
        piUi.RegeneratePiMenu("Normal Menu");
        piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));

        panelRecognize.SetActive(false);
        btnExitControll.SetActive(true);
    }

    public void OnClickExitControll()
    {
        panelRecognize.SetActive(true);
        btnExitControll.SetActive(false);
        normalMenu.ClearMenu();

    }

    private void OnSlidePressed()
    {
        
    }

    private void OnHoverExit()
    {
        
    }

    private void OnHoverEnter()
    {
       
    }

    public void OnClickMenu(string id)
    {
        Debug.Log("rev: " + id);
    }
}
