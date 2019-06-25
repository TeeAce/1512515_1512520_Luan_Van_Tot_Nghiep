using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Client : MonoBehaviour {

    public SocketIOComponent socket;
    public SocketIOComponent socketIOT;
    private Dictionary<string, Features> dicObjFeatures = new Dictionary<string, Features>();
    private string currDeviceIdRequest;
    public bool isConnected;

    public MenuInteractionController menuInteraction;
    public MainController mainController;

    public void Start()
    {
        AddListener();
        StartCoroutine(CheckConnectToServer());
    }

    public void Update()
    {

    }

    public void AddListener()
    {
        //Message
        socket.On(MS_SERVER_TO_CLIENT.USER_CONNECTED, OnUserConnected);
        socket.On(MS_SERVER_TO_CLIENT.RESPONSE_FEATURE, OnResponseFeature);

        //Event
        if(menuInteraction != null)
            menuInteraction.OnClickFeature += ExcuteFeature;
    }

    public void OnDestroy()
    {
        if (menuInteraction != null)
            menuInteraction.OnClickFeature -= ExcuteFeature;
    }

    private void OnResponseFeature(SocketIOEvent obj)
    {
        string data = obj.data["data"].ToString();
        Debug.Log(data);
        Features f = JsonUtility.FromJson<Features>(data);

        string id = obj.data["id"].ToString();
        id = id.Substring(1, id.Length - 2);

        if (!dicObjFeatures.ContainsKey(id))
            dicObjFeatures[id] = f;
        //chi cho mobile
#if !UNITY_EDITOR
        if (mainController == null && mainController.currRecognizeObj == null)
            return;

        if (mainController.currRecognizeObj.name == id)
            mainController.UpdateBoundBox(mainController.currJsonData);
#endif
    }

    IEnumerator CheckConnectToServer()
    {
        yield return new WaitForSeconds(3);
        socket.Emit(MS_CLIENT_TO_SERVER.CHECK_CONNECTED);
    }

    public void OnUserConnected(SocketIOEvent evt)
    {
        isConnected = true;
        Debug.Log("User Connected");
    }

    public void RequestFeature(string id)
    {
        if (dicObjFeatures.ContainsKey(id))
            return;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = id;
        socket.Emit(MS_CLIENT_TO_SERVER.REQUEST_FEATURE, new JSONObject(data));
        //Debug.Log("Da Gui: " + id);
    }

    public Features GetFeaturesById(string id)
    {
        if (dicObjFeatures.ContainsKey(id))
            return dicObjFeatures[id];
        else
        {
            if (!isConnected)
                return null;
            if (currDeviceIdRequest == id)
                return null;
            currDeviceIdRequest = id;
            RequestFeature(id);
            return null;
        }
    }

    public void EnableBtnControll()
    {

    }

    public void DisableBtnControll()
    {

    }

    public void ExcuteFeature(string feature)
    {
        Debug.Log("Excuted " + feature);
        socketIOT.Emit(feature);
    }

    public void GetSpeakerVolume()
    {
        socketIOT.Emit(MS_CLIENT_TO_SERVER.REQUEST_GET_VOLUME);
    }
}

public class Features
{
    public string[] features;
}
