using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using SocketIO;
using System.Runtime.InteropServices;
using System;
using Plugins.SystemVolumePlugin;
using PPt = Microsoft.Office.Interop.PowerPoint;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using System.IO;
using UnityEngine.UI;

public class ClientControllPC : MonoBehaviour {
    public SocketIOComponent socketIO;
    private float prevVolume;

    PPt.Application pptApplication = null;
    PPt.Presentation presentation = null;
    PPt.Slides slides = null;
    PPt.Slide slide = null;
    int slidescount;
    int slideIndex;

    [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

    public void Start()
    {
        AddListener();
        StartCoroutine(RegisIOT());
        SystemVolumePlugin.InitializeVolume();

        InvokeRepeating("UpdateVolume", 3f, 1f);
    }

    public void AddListener()
    {
        socketIO.On(FEATURE.SLEEP, Sleep);
        socketIO.On(FEATURE.OPEN_NOTEPAD, OpenNotePad);
        socketIO.On(FEATURE.OPEN_PAINT, OpenPaint);
        socketIO.On(FEATURE.CLOSE_NOTEPAD, CloseNotePad);
        socketIO.On(FEATURE.CLOSE_PAINT, ClosePaint);
        socketIO.On(FEATURE.UP_VOLUME, UpVolume);
        socketIO.On(FEATURE.DOWN_VOLUME, DownVolume);
        socketIO.On(FEATURE.BEGIN_PRESENTATION, BeginPresentation);
        socketIO.On(FEATURE.END_PRESENTATION, EndPresentation);
        socketIO.On(FEATURE.NEXT_SLIDE, NextSlide);
        socketIO.On(FEATURE.PREV_SLIDE, PrevSlide);

        socketIO.On("REGIS_SUCCESS", OnRegisSuccess);

        socketIO.On(MS_SERVER_TO_CLIENT.REQUEST_GET_VOLUME, RequestGetVolume);
    }

    void UpdateVolume()
    {
        float volume = SystemVolumePlugin.GetVolume() * 100;
        if (prevVolume == volume)
            return;
        prevVolume = volume;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["volume"] = volume.ToString();
        socketIO.Emit(MS_CLIENT_TO_SERVER.UPDATE_VOLUME, new JSONObject(data));
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

    public void RequestGetVolume(SocketIOEvent socketIOEvent)
    {
        //Send Volume To Server
    }

    public void Sleep(SocketIOEvent socketIOEvent)
    {
        SetSuspendState(false, true, true);
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

    /// <summary> 
    /// Check whether PowerPoint is running  
    /// </summary> 
    /// <param name="sender"></param> 
    /// <param name="e"></param> 
    private void CheckPowerPoint()
    {
        try
        {
            // Get Running PowerPoint Application object 
            pptApplication = Marshal.GetActiveObject("PowerPoint.Application") as PPt.Application;
        }
        catch(Exception ex)
        {
            pptApplication = new PPt.Application();
            pptApplication.Visible = MsoTriState.msoTrue;
        }

        if (pptApplication != null)
        {
            // Get Presentation Object 
            presentation = pptApplication.ActivePresentation;
            // Get Slide collection object 
            slides = presentation.Slides;
            // Get Slide count 
            slidescount = slides.Count;
            // Get current selected slide  
            try
            {
                // Get selected slide object in normal view 
                slide = slides[pptApplication.ActiveWindow.Selection.SlideRange.SlideNumber];
                if (slide == null)
                    UnityEngine.Debug.Log("Null");
            }
            catch
            {
                // Get selected slide object in reading view 
                slide = pptApplication.SlideShowWindows[1].View.Slide;

                if (slide == null)
                    UnityEngine.Debug.Log("Null");
            }
        }
        else
        {
            UnityEngine.Debug.Log("Not Found");
        }
    }

    //Begin Presentation
    public void BeginPresentation(SocketIOEvent socketIOEvent)
    {
        CheckPowerPoint();

        if (pptApplication == null)
            return;

        pptApplication.Activate();
        PPt.SlideShowSettings slideShowSettings = presentation.SlideShowSettings;
        slideShowSettings.Run();
    }

    //End Presentation
    public void EndPresentation(SocketIOEvent socketIOEvent)
    {
        CheckPowerPoint();

        if (pptApplication == null)
            return;

        pptApplication.Activate();
        PPt.SlideShowWindow slideShowWindow = presentation.SlideShowWindow;
        slideShowWindow.View.Exit();
    }

    // Transform to next page 
    public void NextSlide(SocketIOEvent socketIOEvent)
    {
        CheckPowerPoint();

        if (pptApplication == null)
            return;

        slideIndex = slide.SlideIndex + 1;
        if (slideIndex > slidescount)
        {
            UnityEngine.Debug.Log("It is already last page");
        }
        else
        {
            try
            {
                slide = slides[slideIndex];
                slides[slideIndex].Select();
            }
            catch
            {
                pptApplication.SlideShowWindows[1].View.Next();
                slide = pptApplication.SlideShowWindows[1].View.Slide;
            }
        }
    }

    // Transform to Last page 
    public void PrevSlide(SocketIOEvent socketIOEvent)
    {
        CheckPowerPoint();

        if (pptApplication == null)
            return;

        slideIndex = slide.SlideIndex - 1;
        if (slideIndex >= 1)
        {
            try
            {
                slide = slides[slideIndex];
                slides[slideIndex].Select();
            }
            catch
            {
                pptApplication.SlideShowWindows[1].View.Previous();
                slide = pptApplication.SlideShowWindows[1].View.Slide;
            }
        }
        else
        {
            UnityEngine.Debug.Log("It is already Fist Page");
        }
    }
}
