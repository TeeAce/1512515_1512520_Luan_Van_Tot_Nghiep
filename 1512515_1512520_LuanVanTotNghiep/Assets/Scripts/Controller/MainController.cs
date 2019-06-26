using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainController : MonoBehaviour {

    public static MainController Instance;
    public FileController fileController;
    public Transform boundBoxContainer;
    public Image imgBoundBox;
    public List<Color> colors;
    public float timeDelay = 0.5f;
    public float timeCheckDelay = 0.5f;
    private bool isDelay;
    public int numBoundBoxActive;
    public Dictionary<string, int> listTargets = new Dictionary<string, int>();
    private int typeTargetCount;
    public bool isSelectMainObj=true;
    public bool isControlling;
    public MenuInteractionController menuInteraction;
    public GameObject panelLoading;
    public GameObject btnGetRecognize;
    public GameObject btnBack;
    public RawImage panelShowMobile;

    public Action<RecognizeObject> OnMainObjectDetected;
    public Action OnNotFoundObject;
    public float screenWidth;
    public float screenHeight;
    [HideInInspector]
    public RecognizeObject currRecognizeObj;
    public string currJsonData;

	// Use this for initialization
	void Start () {
        MakeInstance();

        CmdLine();

        //event
        if (menuInteraction != null)
            menuInteraction.OnControll += OnControll;

        gameObject.GetComponent<Canvas>().sortingOrder = 0;
    }

    private void OnDestroy()
    {
        //event
        if (menuInteraction != null)
            menuInteraction.OnControll -= OnControll;
    }

    void CmdLine()
    {
        StringBuilder strBuilder = new StringBuilder("python ");
        strBuilder.AppendFormat("{0} -i={1} -o={2}", "F:/Thach/tf_object_detection/tools/image_detection_v2.py", AppConstant.PATH_CAMERA_TEXTURE_OUT_PUT + "Output.png", AppConstant.PATH_RECORGNIZE_IN_PUT + "ConfigRecorgnizeInput.csv");

        string command = strBuilder.ToString();
        UnityEngine.Debug.Log("command: " + command);
    }
	
	// Update is called once per frame
	void Update () {
        // chi chay tren PC
#if UNITY_EDITOR
        UpdateBoundBox();
#endif 
    }

    private void MakeInstance()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }

    public void UpdateBoundBox()
    {
        RecognizeInputStruct data = fileController.LoadRecorgnizeInput();

        CheckTatgetDelay();
        if(timeCheckDelay<=0)
        {
            DisableAllChild(boundBoxContainer);
            timeCheckDelay = timeDelay;

            if (data == null || data.recognizeObjects.Length == 0)
            {
                if (OnNotFoundObject != null)
                    OnNotFoundObject();
            }
        }

        if (data == null)
        {
            imgBoundBox.rectTransform.sizeDelta = Vector2.zero;
            return;
        }

        //Select Main Object
        if (data.recognizeObjects.Length > 0 && isSelectMainObj)
        {
            if(isControlling)
            {
                data.recognizeObjects = new RecognizeObject[0];
            }
            else
            {
                RecognizeObject bestObj = data.recognizeObjects[0];
                for (int i = 1; i < data.recognizeObjects.Length; i++)
                {
                    if (bestObj.score < data.recognizeObjects[i].score)
                        bestObj = data.recognizeObjects[i];
                }

                data.recognizeObjects = new RecognizeObject[1];
                data.recognizeObjects[0] = bestObj;

                if (OnMainObjectDetected != null)
                    OnMainObjectDetected(data.recognizeObjects[0]);
            }
        }

        for (int i = 0; i < Mathf.Min(data.recognizeObjects.Length, boundBoxContainer.childCount); i++)
        {
            Image boundBoxItem = boundBoxContainer.GetChild(i).GetComponent<Image>();
            boundBoxItem.rectTransform.localPosition = new Vector3(data.recognizeObjects[i].x, -(data.recognizeObjects[i].y), boundBoxItem.rectTransform.localPosition.z);
            boundBoxItem.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.rectTransform.sizeDelta= new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            //add list
            string objectName = data.recognizeObjects[i].name.ToUpper();
            if (!listTargets.ContainsKey(objectName))
            {
                listTargets[objectName] = typeTargetCount;
                typeTargetCount++;
            }

            boundBoxItem.GetComponent<BoundBoxItem>().lbContent.text = string.Format("{0} {1}%", objectName, (int)(data.recognizeObjects[i].score * 100));
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.color= colors[listTargets[objectName] % colors.Count];
            boundBoxItem.GetComponent<BoundBoxItem>().lbDesc.text = data.recognizeObjects[i].description;
            boundBoxItem.GetComponent<BoundBoxItem>().OptimizeDes(data.recognizeObjects[i].x + data.recognizeObjects[i].width/2, -(data.recognizeObjects[i].y + data.recognizeObjects[i].height/2), screenWidth, screenHeight);
            boundBoxItem.GetComponent<BoundBoxItem>().panelDes.SetActive(data.recognizeObjects[i].description != "");
            boundBoxItem.color = colors[listTargets[objectName] % colors.Count];
            boundBoxItem.gameObject.SetActive(true);
        }

        for (int i = boundBoxContainer.childCount; i < data.recognizeObjects.Length; i++)
        {
            Image boundBoxItem = Instantiate(imgBoundBox, boundBoxContainer);
            boundBoxItem.rectTransform.localPosition = new Vector3(data.recognizeObjects[i].x, -(data.recognizeObjects[i].y), boundBoxItem.rectTransform.localPosition.z);
            boundBoxItem.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            //add list
            string objectName = data.recognizeObjects[i].name.ToUpper();
            if (!listTargets.ContainsKey(objectName))
            {
                listTargets[objectName] = typeTargetCount;
                typeTargetCount++;
            }

            boundBoxItem.GetComponent<BoundBoxItem>().lbContent.text = string.Format("{0} {1}%", objectName, (int)(data.recognizeObjects[i].score * 100));
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.color = colors[listTargets[objectName] % colors.Count];
            boundBoxItem.GetComponent<BoundBoxItem>().lbDesc.text = data.recognizeObjects[i].description;
            boundBoxItem.GetComponent<BoundBoxItem>().OptimizeDes(data.recognizeObjects[i].x + data.recognizeObjects[i].width / 2, -(data.recognizeObjects[i].y + data.recognizeObjects[i].height / 2), screenWidth, screenHeight);
            boundBoxItem.GetComponent<BoundBoxItem>().panelDes.SetActive(data.recognizeObjects[i].description != "");
            boundBoxItem.color = colors[listTargets[objectName] % colors.Count];
            boundBoxItem.gameObject.SetActive(true);
        }

        UpdateNumBoundBoxActive();
        isDelay = (data.recognizeObjects.Length > 0 && data.recognizeObjects.Length == numBoundBoxActive);
        //Debug.Log("Update Success");
    }

    public void UpdateBoundBox(string json)
    {
        currJsonData = json;
        RecognizeInputStruct data = JsonUtility.FromJson<RecognizeInputStruct>(json);

        CheckTatgetDelay();
        if (timeCheckDelay <= 0)
        {
            DisableAllChild(boundBoxContainer);
            timeCheckDelay = timeDelay;

            if (data == null || data.recognizeObjects.Length == 0)
            {
                if (OnNotFoundObject != null)
                    OnNotFoundObject();
            }
        }

        if (data == null)
        {
            imgBoundBox.rectTransform.sizeDelta = Vector2.zero;
            return;
        }

        //Select Main Object
        if (data.recognizeObjects.Length > 0 && isSelectMainObj)
        {
            if (isControlling)
            {
                data.recognizeObjects = new RecognizeObject[0];
            }
            else
            {
                RecognizeObject bestObj = data.recognizeObjects[0];
                for (int i = 1; i < data.recognizeObjects.Length; i++)
                {
                    if (bestObj.score < data.recognizeObjects[i].score)
                        bestObj = data.recognizeObjects[i];
                }

                data.recognizeObjects = new RecognizeObject[1];
                data.recognizeObjects[0] = bestObj;

                if (OnMainObjectDetected != null)
                    OnMainObjectDetected(data.recognizeObjects[0]);

                //chi cho mobile
                currRecognizeObj = data.recognizeObjects[0];
            }
        }

        for (int i = 0; i < Mathf.Min(data.recognizeObjects.Length, boundBoxContainer.childCount); i++)
        {
            Image boundBoxItem = boundBoxContainer.GetChild(i).GetComponent<Image>();
            boundBoxItem.rectTransform.localPosition = new Vector3(data.recognizeObjects[i].x, -(data.recognizeObjects[i].y), boundBoxItem.rectTransform.localPosition.z);
            boundBoxItem.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.rectTransform.sizeDelta= new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            //add list
            string objectName = data.recognizeObjects[i].name.ToUpper();
            if (!listTargets.ContainsKey(objectName))
            {
                listTargets[objectName] = typeTargetCount;
                typeTargetCount++;
            }

            boundBoxItem.GetComponent<BoundBoxItem>().lbContent.text = string.Format("{0} {1}%", objectName, (int)(data.recognizeObjects[i].score * 100));
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.color= colors[listTargets[objectName] % colors.Count];
            boundBoxItem.GetComponent<BoundBoxItem>().lbDesc.text = data.recognizeObjects[i].description;
            boundBoxItem.GetComponent<BoundBoxItem>().OptimizeDes(data.recognizeObjects[i].x + data.recognizeObjects[i].width / 2, -(data.recognizeObjects[i].y + data.recognizeObjects[i].height / 2), screenWidth, screenHeight);
            boundBoxItem.GetComponent<BoundBoxItem>().panelDes.SetActive(data.recognizeObjects[i].description != "");
            boundBoxItem.color = colors[listTargets[objectName] % colors.Count];
            boundBoxItem.gameObject.SetActive(true);
        }

        for (int i = boundBoxContainer.childCount; i < data.recognizeObjects.Length; i++)
        {
            Image boundBoxItem = Instantiate(imgBoundBox, boundBoxContainer);
            boundBoxItem.rectTransform.localPosition = new Vector3(data.recognizeObjects[i].x, -(data.recognizeObjects[i].y), boundBoxItem.rectTransform.localPosition.z);
            boundBoxItem.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            //add list
            string objectName = data.recognizeObjects[i].name.ToUpper();
            if (!listTargets.ContainsKey(objectName))
            {
                listTargets[objectName] = typeTargetCount;
                typeTargetCount++;
            }

            boundBoxItem.GetComponent<BoundBoxItem>().lbContent.text = string.Format("{0} {1}%", objectName, (int)(data.recognizeObjects[i].score * 100));
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.color = colors[listTargets[objectName] % colors.Count];
            boundBoxItem.GetComponent<BoundBoxItem>().lbDesc.text = data.recognizeObjects[i].description;
            boundBoxItem.GetComponent<BoundBoxItem>().OptimizeDes(data.recognizeObjects[i].x + data.recognizeObjects[i].width / 2, -(data.recognizeObjects[i].y + data.recognizeObjects[i].height / 2), screenWidth, screenHeight);
            boundBoxItem.GetComponent<BoundBoxItem>().panelDes.SetActive(data.recognizeObjects[i].description != "");
            boundBoxItem.color = colors[listTargets[objectName] % colors.Count];
            boundBoxItem.gameObject.SetActive(true);
        }

        UpdateNumBoundBoxActive();
        isDelay = (data.recognizeObjects.Length > 0 && data.recognizeObjects.Length == numBoundBoxActive);
        //Debug.Log("Update Success");
    }

    private void ClearAllChild(Transform parent)
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void DisableAllChild(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void CheckTatgetDelay()
    {
        if (isDelay)
            timeCheckDelay = timeDelay;
        else
            timeCheckDelay -= Time.deltaTime;
    }

    private void UpdateNumBoundBoxActive()
    {
        numBoundBoxActive = 0;
        foreach (Transform child in boundBoxContainer)
        {
            if (child.gameObject.activeInHierarchy)
                numBoundBoxActive++;
        }
    }

    private void FilterMainBoundBox(RecognizeObject[] recognizeObjects)
    {
        if (recognizeObjects == null || recognizeObjects.Length == 0)
            return;

        RecognizeObject bestObj = recognizeObjects[0];
        for(int i=1;i<recognizeObjects.Length;i++)
        {
            if(bestObj.score < recognizeObjects[i].score)
                bestObj = recognizeObjects[i];
        }

        recognizeObjects = new RecognizeObject[1];
        recognizeObjects[0] = bestObj;
    }

    public void OnControll(bool isControlling)
    {
        this.isControlling = isControlling;
        // Chi chay tren mobile
#if !UNITY_EDITOR
        if(isControlling)
        {
            panelLoading.SetActive(false);
            btnGetRecognize.SetActive(false);
            btnBack.SetActive(false);
            panelShowMobile.gameObject.SetActive(false);

            //clear all bound box
            ClearAllChild(boundBoxContainer);
        }
        else
        {
            panelLoading.SetActive(false);
            btnGetRecognize.SetActive(true);
            btnBack.SetActive(false);
            panelShowMobile.gameObject.SetActive(false);

            //clear all bound box
            ClearAllChild(boundBoxContainer);
        }
#endif

    }

    public void ShowLoading()
    {
        panelLoading.SetActive(true);
        btnGetRecognize.SetActive(false);
    }

    public void HideLoading()
    {
        panelLoading.SetActive(false);
        btnGetRecognize.SetActive(false);
        btnBack.SetActive(true);
        panelShowMobile.gameObject.SetActive(true);
    }

    public void OnClickGetRecognize()
    {
        Texture2D t = fileController.GetRecognize();
        panelShowMobile.texture = t;
    }

    public void OnClickBack()
    {
        panelShowMobile.gameObject.SetActive(false);
        btnGetRecognize.SetActive(true);
        btnBack.SetActive(false);
        //clear all bound box
        ClearAllChild(boundBoxContainer);

        if (OnNotFoundObject != null)
            OnNotFoundObject();
    }
}
