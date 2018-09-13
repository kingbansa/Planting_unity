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

public class Quit : MonoBehaviour {

    [Header("Quit")]
    public GameObject QuitPanelObj;
    public GameObject DisconnectServerObj;
    public bool Pause;

    public void QuitYesBtn()
    {
        loginScript.Instance.sendclose();
        QuitPanelObj.SetActive(false);
    }

    public void DisconnectYesBtn()
    {
        DisconnectServerObj.SetActive(false);
        Application.Quit();
    }

    public void QuitNoBtn()
    {
        QuitPanelObj.SetActive(false);
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Pause = true;
            loginScript.Instance.homesend();
        }
        else
        {
            if (Pause)
            {
                Pause = false;
                DisconnectServerObj.SetActive(true);
            }
        }
    }

    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitPanelObj.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            OnApplicationPause(true);
        }
    }
}
