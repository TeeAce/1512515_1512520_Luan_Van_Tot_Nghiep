using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Client : MonoBehaviour {

    public SocketIOComponent socket;
    public SocketIOComponent socketIOT;
    JSONObject jSONObject = new JSONObject();
    public string currObjId;
    private Dictionary<string, Features> dicObjFeatures = new Dictionary<string, Features>();
    public MenuInteractionController menuInteraction;

    public void Start()
    {
        AddListener();
        StartCoroutine(CheckConnectToServer());
    }

    public void Update()
    {
        if (dicObjFeatures.ContainsKey(currObjId))
            EnableBtnControll();
        else
            DisableBtnControll();
    }

    public void AddListener()
    {
        //Message
        socket.On(MS_SERVER_TO_CLIENT.USER_CONNECTED, OnUserConnected);
        socket.On(MS_SERVER_TO_CLIENT.RESPONSE_FEATURE, OnResponseFeature);

        //Event
        menuInteraction.OnClickFeature += ExcuteFeature;
    }

    public void OnDestroy()
    {
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

        if (currObjId == id)
            menuInteraction.ShowMenuInteraction(f);
    }

    IEnumerator CheckConnectToServer()
    {
        yield return new WaitForSeconds(3);
        socket.Emit(MS_CLIENT_TO_SERVER.CHECK_CONNECTED);
    }

    public void OnUserConnected(SocketIOEvent evt)
    {
        Debug.Log("User Connected");
    }

    public void RequestFeature(string id)
    {
        if (dicObjFeatures.ContainsKey(id))
            return;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = id;
        socket.Emit(MS_CLIENT_TO_SERVER.REQUEST_FEATURE, new JSONObject(data));
    }

    public void GetFeaturesById(string id)
    {
        currObjId = id;

        if (dicObjFeatures.ContainsKey(id))
            menuInteraction.ShowMenuInteraction(dicObjFeatures[id]);
        else
            RequestFeature(id);
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
}

public class Features
{
    public string[] features;
}
