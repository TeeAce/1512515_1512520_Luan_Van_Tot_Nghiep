using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteractionController : MonoBehaviour {

    public static MenuInteractionController Instance;
    public Client client;
    public PiUIManager piUi;
    private bool menuOpened;
    private PiUI normalMenu;
    public Color colorNormal;
    public Color colorHightlight;

    public RectTransform btnControll;
    public Animator btnExitControll;
    public Animator panelDeviceInfo;
    public Text nameDevice;
    public bool isControlling;
    private Features currFeatures;
    public MainController mainController;

    public Action<string> OnClickFeature;
    public Action<bool> OnControll;

    private void Start()
    {
        MakeInstance();
        //Get menu for easy not repetitive getting of the menu when setting joystick input
        normalMenu = piUi.GetPiUIOf("Normal Menu");

        if (normalMenu == null)
            Debug.Log("Null");

        if(mainController != null)
        {
            mainController.OnMainObjectDetected += OnMainObjectDetected;
            mainController.OnNotFoundObject += OnNotFoundTarget;
        }

        gameObject.GetComponent<Canvas>().sortingOrder = 1;
    }

    private void OnDestroy()
    {
        if (mainController != null)
        {
            mainController.OnMainObjectDetected -= OnMainObjectDetected;
            mainController.OnNotFoundObject -= OnNotFoundTarget;
        }
    }

    private void Update()
    {
        if (isControlling && btnControll.gameObject.activeInHierarchy)
            btnControll.gameObject.SetActive(false);
    }

    public void OnMainObjectDetected(RecognizeObject recognizeObject)
    {
        if (isControlling)
            return;

        currFeatures = client.GetFeaturesById(recognizeObject.name);
        if (currFeatures == null)
        {
            Debug.Log("k co Feature");
            btnControll.gameObject.SetActive(false);
            return;
        }
        nameDevice.text = "Device: " + recognizeObject.name;

        btnControll.localPosition = new Vector3(recognizeObject.x + (float)recognizeObject.width/2, -(recognizeObject.y+ (float)recognizeObject.height/2), btnControll.localPosition.z);
        btnControll.gameObject.SetActive(true);

        //Debug.Log(recognizeObject.x + " : " + recognizeObject.y +" , "+ recognizeObject.width + " : " + recognizeObject.height);
    }



    public void OnClickCreateMenuInteraction()
    {
        isControlling = true;
        if (OnControll != null)
            OnControll(true);

        ShowMenuInteraction(currFeatures);

        if (UIEvent.OnUpdateUI != null)
            UIEvent.OnUpdateUI();
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

        btnExitControll.gameObject.SetActive(true);
        panelDeviceInfo.gameObject.SetActive(true);
        btnExitControll.Play(AppConstant.INTRO_ANIM, -1, 0f);
        panelDeviceInfo.Play(AppConstant.INTRO_ANIM, -1, 0f);
    }

    public void OnClickExitControll()
    {
        btnExitControll.gameObject.SetActive(false);
        panelDeviceInfo.gameObject.SetActive(false);
        normalMenu.ClearMenu();

        isControlling = false;
        if (OnControll != null)
            OnControll(false);

        if (UIEvent.OnUpdateUI != null)
            UIEvent.OnUpdateUI();
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
        if(OnClickFeature!=null)
            OnClickFeature(id);
    }

    private void MakeInstance()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }

    public void OnNotFoundTarget()
    {
        if (btnControll.gameObject.activeInHierarchy)
            btnControll.gameObject.SetActive(false);
    }
}
