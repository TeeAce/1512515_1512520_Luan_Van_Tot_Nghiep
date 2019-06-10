using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using KAINamespace;
using UnityEditor;
using System;

public class FileController : MonoBehaviour {

    public float timeDelay = 5f;
    private float timeCountDown;
    public Texture cameraTexture;
    private bool isDone;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //CheckTimeCountDown();
        SaveCameraTextureAsPNG(cameraTexture, AppConstant.PATH_CAMERA_TEXTURE_OUT_PUT, "Output.png");
    }

    private void CheckTimeCountDown()
    {
        timeCountDown -= Time.deltaTime;
        if (timeCountDown <= 0f)
        {
            timeCountDown = timeDelay;
            //Saved File
            SaveCameraTextureAsPNG(cameraTexture, AppConstant.PATH_CAMERA_TEXTURE_OUT_PUT,"Output.png");
        }
    }

    private void SaveCameraTextureAsPNG(Texture texture, string path, string name)
    {
        if (File.Exists(path + name) == true)
            return;
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        //Color[] pixels = texture2D.GetPixels();

        RenderTexture.active = currentRT;


        byte[] bytes = texture2D.EncodeToPNG();

        try
        {
            File.WriteAllBytes(path + name, bytes);
        }
        catch(Exception ex)
        {

        }

        Destroy(texture2D);
        Destroy(renderTexture);
    }

    public RecognizeInputStruct LoadRecorgnizeInput()
    {
        if (KAIConfigManager.Instance == null)
            return null;

        KAIConfigManager.Instance.LoadRecorgnizeInput();

        if (KAIConfigManager.Instance.ConfigQuizData.records.Count<=0 || KAIConfigManager.Instance.ConfigQuizData.records[0] == null)
            return null;

        RecognizeInputStruct inputData = KAIConfigManager.Instance.ConfigQuizData.records[0].GetRecorgnizeInputStruct();

        //delete file after read
        try
        {
            //File.Delete(Application.dataPath + "/Resources/Input/ConfigRecorgnizeInput.csv");
        }
        catch (Exception ex)
        {

        }

        KAIConfigManager.Instance.ConfigQuizData = new ConfigRecorgnizeInput();

        //Debug.Log("Da Chay: "+ AppConstant.PATH_RECORGNIZE_IN_PUT + "ConfigRecorgnizeInput.csv");

        return inputData;

    }
}
