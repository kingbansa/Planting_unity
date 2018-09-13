using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using scMessage;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleTonCaptureBtn : MonoBehaviour
{
    public static string userID;

    public void CaptureBtn()
    {
        loginScript.Instance.CaptureBtn();
        userID = loginScript.userName;
        Debug.Log("싱글턴에서 되는지"+userID);
    }
}
