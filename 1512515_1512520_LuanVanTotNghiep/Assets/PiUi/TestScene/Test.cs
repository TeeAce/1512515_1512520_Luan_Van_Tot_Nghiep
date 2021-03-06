﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Color colorNormal;

    [SerializeField]
    PiUIManager piUi;
    private bool menuOpened;
    private PiUI normalMenu;
    // Use this for initialization
    void Start()
    {
        //Get menu for easy not repetitive getting of the menu when setting joystick input
        normalMenu = piUi.GetPiUIOf("Normal Menu");


        //Ensure menu isnt currently open on regenerate so it doesnt spasm
        if (!piUi.PiOpened("Normal Menu"))
        {

            //Make all angles equal 
            normalMenu.equalSlices = true;
            normalMenu.iconDistance = 1f;
            //Changes the piDataLength and adds new piData
            normalMenu.piData = new PiUI.PiData[8];
            for (int j = 0; j < 8; j++)
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
                data.id = "Feature" + i.ToString();

                //Set new highlight/non highlight colors
                data.nonHighlightedColor = colorNormal;
                data.highlightedColor = Color.white;
                data.disabledColor = colorNormal;
                //Changes slice label
                data.sliceLabel = "Feature" + i.ToString();
                //Creates a new unity event and adds the testfunction to it
                data.onSlicePressed = new UnityEngine.Events.UnityEvent();
                data.onSlicePressed.AddListener(TestFunction);
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
        }
        piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
    }

    // Update is called once per frame
    void Update()
    {
        //Bool function that returns true if on a menu
        if (piUi.OverAMenu( ))
            Debug.Log("You are over a menu");
        else
            Debug.Log("You are not over a menu");
        //Just open the normal Menu if A is pressed
        if (Input.GetKeyDown(KeyCode.A))
        {
            piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        }
        //Update the menu and add the Testfunction to the button action if s or Fire1 axis is pressed
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Ensure menu isnt currently open on update just for a cleaner look
            if (!piUi.PiOpened("Normal Menu"))
            {
                int i = 0;
                //Iterate through the piData on normal menu
                foreach (PiUI.PiData data in normalMenu.piData)
                {
                    //Changes slice label
                    data.sliceLabel = "Test" + i.ToString( );
                    //Creates a new unity event and adds the testfunction to it
                    data.onSlicePressed = new UnityEngine.Events.UnityEvent( );
                    data.onSlicePressed.AddListener(TestFunction);
                    i++;
                }
                //Since PiUI.sliceCount or PiUI.equalSlices didnt change just calling update
                piUi.UpdatePiMenu("Normal Menu");
            }
            //Open or close the menu depending on it's current state at the center of the screne
            piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Ensure menu isnt currently open on regenerate so it doesnt spasm
            if (!piUi.PiOpened("Normal Menu"))
            {

                //Make all angles equal 
                normalMenu.equalSlices = true;
                normalMenu.iconDistance = 0f;
                //Changes the piDataLength and adds new piData
                normalMenu.piData = new PiUI.PiData[10];
                for(int j = 0; j < 10; j++)
                {
                    normalMenu.piData[j] = new PiUI.PiData( );
                }
                //Turns of the syncing of colors
                normalMenu.syncColors = false;
                //Changes open/Close animations
                normalMenu.openTransition = PiUI.TransitionType.Scale;
                normalMenu.closeTransition = PiUI.TransitionType.SlideRight;
                int i = 0;
                foreach (PiUI.PiData data in normalMenu.piData)
                {
                    //Turning off the interactability of a slice
                    if(i % 2 ==0)
                    {
                        data.isInteractable = false;
                    }
                    //Set new highlight/non highlight colors
                    data.nonHighlightedColor = Color.white;
                    data.highlightedColor = Color.white;
                    data.disabledColor = Color.grey;
                    //Changes slice label
                    data.sliceLabel = "Test" + i.ToString( );
                    //Creates a new unity event and adds the testfunction to it
                    data.onSlicePressed = new UnityEngine.Events.UnityEvent( );
                    data.onSlicePressed.AddListener(TestFunction);
                    i += 1;
                    //Enables hoverFunctions
                    data.hoverFunctions = true;
                    //Creates a new unity event to adds on hovers function
                    data.onHoverEnter = new UnityEngine.Events.UnityEvent( );
                    data.onHoverEnter.AddListener(OnHoverEnter);
                    data.onHoverExit = new UnityEngine.Events.UnityEvent( );
                    data.onHoverExit.AddListener(OnHoverExit);
                }
                piUi.RegeneratePiMenu("Normal Menu");
            }
            piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            //Ensure menu isnt currently open on regenerate so it doesnt spasm
            if (!piUi.PiOpened("Normal Menu"))
            {

                //Make all angles equal 
                normalMenu.equalSlices = true;
                normalMenu.iconDistance = 1f;
                //Changes the piDataLength and adds new piData
                normalMenu.piData = new PiUI.PiData[8];
                for (int j = 0; j < 8; j++)
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
                    data.id= "Feature" + i.ToString();

                    //Set new highlight/non highlight colors
                    data.nonHighlightedColor = colorNormal;
                    data.highlightedColor = Color.white;
                    data.disabledColor = colorNormal;
                    //Changes slice label
                    data.sliceLabel = "Feature" + i.ToString();
                    //Creates a new unity event and adds the testfunction to it
                    data.onSlicePressed = new UnityEngine.Events.UnityEvent();
                    data.onSlicePressed.AddListener(TestFunction);
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
            }
            piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

    }
    //Test function that writes to the console and also closes the menu
    public void TestFunction()
    {
        //Closes the menu
        piUi.ChangeMenuState("Normal Menu");
        Debug.Log("You Clicked me!");
    }

    public void OnHoverEnter()
    {
        Debug.Log("Hey get off of me!");
    }
    public void OnHoverExit()
    {
        Debug.Log("That's right and dont come back!");
    }
}
