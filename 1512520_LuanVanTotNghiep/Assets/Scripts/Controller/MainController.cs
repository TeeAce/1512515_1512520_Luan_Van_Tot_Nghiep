using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

    public static MainController Instance;
    public FileController fileController;
    public Transform boundBoxContainer;
    public Image imgBoundBox;
    public List<Color> colors;
    public float timeDelay = 0.5f;
    public float timeCheckDelay = 0.5f;
    public bool isHaveTarget;

	// Use this for initialization
	void Start () {
        MakeInstance();

        StringBuilder strBuilder = new StringBuilder("python ");
        strBuilder.AppendFormat("{0} -i={1} -o={2}", "F:/Thach/tf_object_detection/tools/image_detection_v2.py", AppConstant.PATH_CAMERA_TEXTURE_OUT_PUT + "Output.png", AppConstant.PATH_RECORGNIZE_IN_PUT + "ConfigRecorgnizeInput.csv");

        string command = strBuilder.ToString();
        UnityEngine.Debug.Log("command: " + command);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateBoundBox();
	}

    private void MakeInstance()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }

    private void UpdateBoundBox()
    {
        CheckTatgetDelay();
        if(timeCheckDelay<=0)
        {
            DisableAllChild(boundBoxContainer);
            timeCheckDelay = timeDelay;
        }

        RecognizeInputStruct data = fileController.LoadRecorgnizeInput();
        if (data == null)
        {
            imgBoundBox.rectTransform.sizeDelta = Vector2.zero;
            return;
        }

        isHaveTarget = data.recognizeObjects.Length > 0;

        for (int i = 0; i < Mathf.Min(data.recognizeObjects.Length, boundBoxContainer.childCount); i++)
        {
            Image boundBoxItem = boundBoxContainer.GetChild(i).GetComponent<Image>();
            boundBoxItem.rectTransform.localPosition = new Vector3(data.recognizeObjects[i].x, -(data.recognizeObjects[i].y), boundBoxItem.rectTransform.position.z);
            boundBoxItem.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            boundBoxItem.color = colors[i % colors.Count];
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.rectTransform.sizeDelta= new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            boundBoxItem.GetComponent<BoundBoxItem>().lbContent.text = string.Format("{0} {1}%", data.recognizeObjects[i].name.ToUpper(),(int)(data.recognizeObjects[i].score * 100));
            boundBoxItem.GetComponent<BoundBoxItem>().lbContent.color= colors[i % colors.Count];
            boundBoxItem.gameObject.SetActive(true);
        }

        for (int i = boundBoxContainer.childCount; i < data.recognizeObjects.Length; i++)
        {
            Image boundBoxItem = Instantiate(imgBoundBox, boundBoxContainer);
            boundBoxItem.rectTransform.localPosition = new Vector3(data.recognizeObjects[i].x, -(data.recognizeObjects[i].y), boundBoxItem.rectTransform.position.z);
            boundBoxItem.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            boundBoxItem.color = colors[i % colors.Count];
            //boundBoxItem.GetComponent<BoundBoxItem>().lbContent.rectTransform.sizeDelta = new Vector2(data.recognizeObjects[i].width, data.recognizeObjects[i].height);
            boundBoxItem.GetComponent<BoundBoxItem>().lbContent.text = string.Format("{0} {1}%", data.recognizeObjects[i].name.ToUpper(), (int)(data.recognizeObjects[i].score * 100));
            boundBoxItem.GetComponent<BoundBoxItem>().lbContent.color = colors[i % colors.Count];
            boundBoxItem.gameObject.SetActive(true);
        }

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
        if (isHaveTarget)
            timeCheckDelay = timeDelay;
        else
            timeCheckDelay -= Time.deltaTime;
    }
}
