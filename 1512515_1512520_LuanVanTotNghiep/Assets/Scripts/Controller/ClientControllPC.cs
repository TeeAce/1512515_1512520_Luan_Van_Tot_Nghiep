using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using SocketIO;
using System.Runtime.InteropServices;
using System;
using Plugins.SystemVolumePlugin;

public class ClientControllPC : MonoBehaviour {
    public SocketIOComponent socketIO;

    public void Start()
    {
        AddListener();
        StartCoroutine(RegisIOT());
        SystemVolumePlugin.InitializeVolume();
    }

    public void AddListener()
    {
        socketIO.On(FEATURE.OPEN_NOTEPAD, OpenNotePad);
        socketIO.On(FEATURE.OPEN_PAINT, OpenPaint);
        socketIO.On(FEATURE.CLOSE_NOTEPAD, CloseNotePad);
        socketIO.On(FEATURE.CLOSE_PAINT, ClosePaint);
        socketIO.On(FEATURE.UP_VOLUME, UpVolume);
        socketIO.On(FEATURE.DOWN_VOLUME, DownVolume);

        socketIO.On("REGIS_SUCCESS", OnRegisSuccess);
    }

    IEnumerator RegisIOT()
    {
        yield return new WaitForSeconds(3);
        socketIO.Emit("REGIS_IOT");
    }

    public void OnRegisSuccess(SocketIOEvent socketIOEvent)
    {
        UnityEngine.Debug.Log("Regis Success");
    }

    public void OpenNotePad(SocketIOEvent socketIOEvent)
    {
        Process.Start("notepad");
    }

    public void OpenPaint(SocketIOEvent socketIOEvent)
    {
        Process.Start("mspaint");
    }

    public void CloseNotePad(SocketIOEvent socketIOEvent)
    {
        foreach (Process process in Process.GetProcessesByName("notepad"))
            process.Kill();
    }

    public void ClosePaint(SocketIOEvent socketIOEvent)
    {
        foreach (Process process in Process.GetProcessesByName("mspaint"))
            process.Kill();
    }

    public void UpVolume(SocketIOEvent socketIOEvent)
    {
        SystemVolumePlugin.SetVolume(SystemVolumePlugin.GetVolume() + 0.02f);
    }

    public void DownVolume(SocketIOEvent socketIOEvent)
    {
        SystemVolumePlugin.SetVolume(SystemVolumePlugin.GetVolume() - 0.02f);
    }
}
