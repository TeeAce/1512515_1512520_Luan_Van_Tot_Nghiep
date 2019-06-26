using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ClientAI : MonoBehaviour {

    public MainController mainController;

    // Use this for initialization
    void Start()
    {
        
    }

    public void GetRecognize(byte[] image, string name)
    {
        StartCoroutine(Upload(image, name));
    }

    public IEnumerator Upload(byte[] image,string name)
    {
        mainController.ShowLoading();
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", image, name);

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.137.1:8000/detection-api", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("POST successful!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
                Debug.Log(sb.ToString());

                // Print Body
                Debug.Log(www.downloadHandler.text);

                mainController.UpdateBoundBox(www.downloadHandler.text);

            }

            mainController.HideLoading();
        }
    }
}
