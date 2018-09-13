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

public class Battle : MonoBehaviour
{
    public string TransferIp = "";
    public string TransferUserName = "";

    private static Battle instance;
    public static Battle Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void IPtransfertoServer()
    { 
        loginScript.Instance.TransferIP(TransferIp, TransferUserName);
    }

    public void NextScene()
    {
        loginScript.Instance.GetPlantName();
        SceneManager.LoadScene("WhatScene");
    }

	// Use this for initialization
	void Start ()
    {
        TransferIp = loginScript.userip;
        TransferUserName = loginScript.userName;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }
}
