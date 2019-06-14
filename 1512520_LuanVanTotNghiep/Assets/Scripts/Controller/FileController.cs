using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using KAINamespace;
using UnityEditor;
using System;

public class FileController : MonoBehaviour {

    public float timeDelay = 5f;
    private float timeCountDown=5f;
    public Texture cameraTexture;
    private bool isDone;

    public ClientAI clientAI;

    // Use this for initialization
    void Start() {
        ClearInput();
    }

    // Update is called once per frame
    void Update() {
        //CheckTimeCountDown();
        //Chi Chay tren PC
#if UNITY_EDITOR
        SaveCameraTextureAsPNG(cameraTexture, AppConstant.PATH_CAMERA_TEXTURE_OUT_PUT, "Output.png");
#endif
    }

    private void CheckTimeCountDown()
    {
        timeCountDown -= Time.deltaTime;
        if (timeCountDown <= 0f)
        {
            timeCountDown = 10000f;
            //Saved File
            //SaveCameraTextureAsPNG(cameraTexture, AppConstant.PATH_CAMERA_TEXTURE_OUT_PUT,"Output.png");
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

    public Texture2D GetRecognize()
    {
        return RequestRecognize(cameraTexture, "Output.png");
    }

    public Texture2D RequestRecognize(Texture texture, string name)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        //Color[] pixels = texture2D.GetPixels();

        RenderTexture.active = currentRT;


        byte[] bytes = texture2D.EncodeToJPG();

        try
        {
            clientAI.GetRecognize(bytes, name);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        Destroy(renderTexture);
        return texture2D;
    }

    public RecognizeInputStruct LoadRecorgnizeInput()
    {
        if (KAIConfigManager.Instance == null)
        {
            return null;
        }

        KAIConfigManager.Instance.LoadRecorgnizeInput();

        if (KAIConfigManager.Instance.ConfigRecorgnizeData.records.Count<=0 || KAIConfigManager.Instance.ConfigRecorgnizeData.records[0] == null)
        {
            return null;
        }

        RecognizeInputStruct inputData = KAIConfigManager.Instance.ConfigRecorgnizeData.records[0].GetRecorgnizeInputStruct();

        //delete file after read
        try
        {
            File.Delete(Application.dataPath + "/Resources/Input/ConfigRecorgnizeInput.csv");
        }
        catch (Exception ex)
        {

        }

        KAIConfigManager.Instance.ConfigRecorgnizeData = new ConfigRecorgnizeInput();

        //Debug.Log("Da Chay: "+ AppConstant.PATH_RECORGNIZE_IN_PUT + "ConfigRecorgnizeInput.csv");

        return inputData;

    }

    private void ClearInput()
    {
        try
        {
            File.Delete(Application.dataPath + "/Resources/Input/ConfigRecorgnizeInput.csv");
        }
        catch (Exception ex)
        {

        }
    }
}
