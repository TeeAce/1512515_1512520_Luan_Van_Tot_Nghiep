using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConstant{

    public static string PATH_CAMERA_TEXTURE_OUT_PUT = Application.dataPath+ "/Resources/Output/";
    public static string PATH_RECORGNIZE_IN_PUT = Application.dataPath + "/Resources/Input/";
}

public class MS_CLIENT_TO_SERVER
{
    public static string CHECK_CONNECTED        = "CHECK_CONNECTED";
    public static string CLIENT_TO_SERVER       = "CLIENT_TO_SERVER";
    public static string REQUEST_FEATURE        = "REQUEST_FEATURE";
}

public class MS_SERVER_TO_CLIENT
{
    public static string USER_CONNECTED         = "USER_CONNECTED";
    public static string RESPONSE_FEATURE       = "RESPONSE_FEATURE";

}

public class FEATURE
{
    //For PC
    public static string OPEN_NOTEPAD           = "Open Notepad";
    public static string OPEN_PAINT             = "Open Paint";
    public static string CLOSE_NOTEPAD          = "Close Notepad";
    public static string CLOSE_PAINT            = "Close Paint";
}