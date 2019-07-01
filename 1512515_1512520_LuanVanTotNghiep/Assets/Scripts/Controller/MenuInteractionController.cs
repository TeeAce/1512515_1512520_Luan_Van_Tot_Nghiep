using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInteractionController : MonoBehaviour {

    public static MenuInteractionController Instance;
    public Client client;
    public RadialUI radialUI;

    public RectTransform btnControll;
    public GameObject btnExitControll;
    public Text nameDevice;
    public bool isControlling;
    private Features currFeatures;
    public MainController mainController;

    public Action<string> OnClickFeature;
    public Action<bool> OnControll;
    private string prevDevice;

    private void Start()
    {
        MakeInstance();
        AddListener();

        gameObject.GetComponent<Canvas>().sortingOrder = 100;
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    private void Update()
    {
        if (isControlling && btnControll.gameObject.activeInHierarchy)
            btnControll.gameObject.SetActive(false);
    }

    private void AddListener()
    {
        if (mainController != null && mainController.appType == APPLICATION_TYPE.Control)
        {
            mainController.OnMainObjectDetected += OnMainObjectDetected;
            mainController.OnNotFoundObject += OnNotFoundTarget;
        }

        RadialItem.OnItemClicked += OnClickMenu;
    }

    private void RemoveListener()
    {
        if (mainController != null && mainController.appType == APPLICATION_TYPE.Control)
        {
            mainController.OnMainObjectDetected -= OnMainObjectDetected;
            mainController.OnNotFoundObject -= OnNotFoundTarget;
        }

        RadialItem.OnItemClicked -= OnClickMenu;
    }

    public void OnMainObjectDetected(RecognizeObject recognizeObject)
    {
        if (isControlling)
        {
            prevDevice = "";
            return;
        }

        //Debug.Log("Id: " + recognizeObject.name);
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

        if (UIEvent.OnUpdateUI != null)
            UIEvent.OnUpdateUI();

        //Debug.Log(recognizeObject.x + " : " + recognizeObject.y +" , "+ recognizeObject.width + " : " + recognizeObject.height);
//#if !UNITY_EDITOR
//        if (recognizeObject.name == AppConstant.SPEAKER && prevDevice != AppConstant.SPEAKER)
//            client.GetSpeakerVolume();
//#endif

        prevDevice = recognizeObject.name;
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
        radialUI.InitFeatures(f);

        btnExitControll.gameObject.SetActive(true);
    }

    public void OnClickExitControll()
    {
        btnExitControll.gameObject.SetActive(false);

        isControlling = false;
        if (OnControll != null)
            OnControll(false);

        if (UIEvent.OnUpdateUI != null)
            UIEvent.OnUpdateUI();
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
