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

public class GPSSceneButton : MonoBehaviour
{

    [Header("Quit")]
    public GameObject QuitPanelObj;


    public void QuitYesBtn()
    {
        Application.Quit();
        loginScript.Instance.cScokClose();
    }

    public void QuitNoBtn()
    {
        QuitPanelObj.SetActive(false);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitPanelObj.SetActive(true);
        }

    }
}

