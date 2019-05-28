using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Client : MonoBehaviour {

    public SocketIOComponent socket;
    JSONObject jSONObject = new JSONObject();

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
        socket.On(MS_SERVER_TO_CLIENT.USER_CONNECTED, OnUserConnected);
        socket.On(MS_SERVER_TO_CLIENT.RESPONSE_FEATURE, OnResponseFeature);
    }

    private void OnResponseFeature(SocketIOEvent obj)
    {

        string data = obj.data.ToString();
        Debug.Log(data);
        Features f = JsonUtility.FromJson<Features>(data);

        Debug.Log(f.features[0].ToString());
    }

    IEnumerator CheckConnectToServer()
    {
        yield return new WaitForSeconds(3);
        socket.Emit(MS_CLIENT_TO_SERVER.CHECK_CONNECTED);
    }

    public void OnUserConnected(SocketIOEvent evt)
    {
        Debug.Log("User Connected");
        RequestFeature();
    }

    public void RequestFeature()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = "2";
        socket.Emit(MS_CLIENT_TO_SERVER.REQUEST_FEATURE, new JSONObject(data));
    }
}

public class Features
{
    public string[] features;
}
