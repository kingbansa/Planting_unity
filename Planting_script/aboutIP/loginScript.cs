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

public class loginScript : MonoBehaviour
{
    [Header("LoginPanel")]
    public InputField IDInputField;
    public InputField PassInputField;
    public GameObject LoginPanelObj;
    [Header("CreateAccountPanel")]
    public InputField New_IDInputField;
    public InputField New_PassInputField;
    public GameObject CreateAccountPanelObj;
    public GameObject IdCheckObj;
    [Header("TransferPanel")]
    public InputField TransferInputField;
    public GameObject TransferPanelObj;
    [Header("QuitPanel")]
    public GameObject QuitObj;
    public GameObject DisconnectServerObj;
    [Header("CheckUserInfoPanel")]
    public GameObject CheckUserInfoObj;

    private int
        sPort = 3000, // server port
        pfrPort = 2999; // policy file request port

    private Socket
        cSock; // client socket

    public bool Pause;
    public static List<string> plantName = new List<string>();
    public static List<int> plantPos = new List<int>();
    public static List<int> plantID = new List<int>();
    public static List<int> Lv = new List<int>();
    public static List<float> waterEXP = new List<float>();
    public static List<float> sunEXP = new List<float>();
    public static List<float> fertilizerEXP = new List<float>();

    public static int wGetItem, fGetItem, sGetItem, nGetItem, sfsGetItem, csGetItem, tsGetItem = 0;
    //public string plantName;
    // ó�� �α����Ҷ� ��񿡼� �α��� ���̵� �´� ������ ��� ��� ����      static ����� �ٸ� ��ũ��Ʈ���� loginScript.������ ���� ȣ��
    // �Ϲ� ����� �ٸ� ��ũ��Ʈ���� loginScript.instance.������ ���� ȣ��   ���̴�??
    //public static string wItem = "wItem", fItem, sItem, nItem, sfsItem, csItem, tsItem;
    // ������ ������ ������ �̸� ����
    public static int Exp = 0;

    //public int itemNum;  //�κ��丮���� ������ ����� �� � ������� ���� �Ѱܹ޴� ����

    public static string
        ipAddress = "117.16.44.175",// server ip address    "127.0.0.1"       117.16.45.160    "117.16.44.175"     192.168.1.12    117.16.44.160     172.30.1.11
        userName = "",
        pass = "",
        new_userName = "",
        new_pass = "",
        url = "",
        itemName = "",
        userip = "";

    public string
    name1 = "",
    name2 = "",
    name3 = "",
    name4 = "",
    plant_name = "";

    public static string servermessage;
    public static int rankpoint;

    public bool
        connectedToServer = false;

    private List<message>
        incMessages = new List<message>();

    private static loginScript instance;
    public static loginScript Instance
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

    void Start()
    {
        connect(); // loginScene �����ϸ� ������ ����
        name1 = "";
        name2 = "";
        name3 = "";
        name4 = "";
    }

    // ---------------------------------------------- UI �� ��ư ----------------------------------------------
    public void sendclose()
    {
        scObject data = new scObject("closeReponse");
        message mes = new message("close");
        SendServerMessage(mes);
    }

    public void QuitYesBtn()
    {
        sendclose();
        QuitObj.SetActive(false);
    }

    public void DisconnectYesBtn()
    {
        DisconnectServerObj.SetActive(false);
        Application.Quit();
    }

    #region

    public void LoginBtn() // �α��� ��ư (loginScene - LoginPanel - LoginButton)
    {
        userName = IDInputField.text;
        pass = PassInputField.text;
        logInAccount(userName, pass);
        AllDeletePlantName(); //�̰� �����ָ� �� �����
        GetPlantName();
        //ItemCountCheck(userName, pass, wItem);
        Debug.Log("" + userName + pass);
        Debug.Log("" + name1 + name2 + name3 + name4);
    }

    public void OpenCreateAccountBtn() // CreateAccountPanelâ���� ��ȯ (loginScene - LoginPanel - CreateAccountButton)
    {
        CreateAccountPanelObj.SetActive(true);
    }

    public void CreateAccountBtn() // ���ο� ���� ��� ��ư (loginScene - CreateAccountPanel - CreateButton)
    {
        new_userName = New_IDInputField.text;
        new_pass = New_PassInputField.text;
        Register(new_userName, new_pass);
        Debug.Log("" + new_userName + new_pass);
    }

    public void IdCheckBtn() // ���� ��� �� �� ���̵� �ߺ� Ȯ�� ��ư (LoginScene - CreateAccountPanel - IdCheck - IdCheckButton)
    {
        IdCheckObj.SetActive(false);
    }

    public void CancleBtn() // ���ο� ���� ��� ��� (loginScene - CreateAccountPanel - CancleButton)
    {
        CreateAccountPanelObj.SetActive(false);
        LoginPanelObj.SetActive(true);
    }

    public void TransferBtn() // (loginScene - TransferPanel - TransferButton)
    {
        url = TransferInputField.text;
        userName = IDInputField.text;
        Debug.Log("" + userName);
        Transfer(url, userName);
        IDInputField.text = userName;
    }

    public void QuitNoBtn() // ���� ���� ��� ��ư (loginScene - Quit?Panel - NoButton)
    {
        QuitObj.SetActive(false);
    }

    public void CheckInfoBtn() // �α��� ���� Ȯ�� ��ư (loginScene - CheckUserInfo - CheckInfoButton)
    {
        CheckUserInfoObj.SetActive(false);
    }

    public void CaptureBtn()
    {
        url = sc.getCaptureName();
        Debug.Log("loginsScript" + userName);
        Debug.Log("loginsScript" + url);
        Transfer(url, userName);
    }

    // ------------------------------------------------ �� �� ------------------------------------------------


    //������ ���� ȣ��      itN�� itName��� ��
    public void UseItem(string itN, int itemNum)
    {
        UseItem(userName, itN, itemNum);
        Debug.Log("loginScript - useItem - "+itN+" received from "+itN+"ItemDatabase.");
    }

    public void ItemCountCheck(string itN)
    {
        itemName = itN;
        ItemCountCheck(userName, itemName);
    }

    //������ �Ծ��� �� 
    public void GetItem(string itN)
    {
        GetItem(userName, itN);
        Debug.Log("loginScript - GetItem - " + itN + " received from " + itN + "ItemManager.");

    }

    //����� �̸�(account)���� ������� �Ĺ�����Ʈ���̺� ���� ������Ʈ
    public void UpdatePlantListTable(string plantName, int plantID, string itemName, int posNumber, int level, float expAmount, bool isSeedItem)
    {
        UpdatePlantListTable(userName, plantName, plantID, itemName, posNumber, level, expAmount, isSeedItem);
    }

    //�����۸���Ʈ���̺� �ʵ�(PlantPos, PlantName, PlantID, Lv, WaterEXP, SunEXP, FertilizerEXP)
    //SELECT PlantName, PlantID, Lv, PlantPos FROM plantlist_userName WHERE
    public void SelectQuery(string columnName, string tableName)
    {
        SelectQuery(userName, columnName, tableName);
    }

    public void UpdatePlantExp(int plantPos,int level, int expName, float expAmount)
    {
        UpdatePlantExp(userName, plantPos, level, expName, expAmount);
    }

    public void UpdatePlantID(int plantPos, int plantID)
    {
        UpdatePlantID(userName, plantPos, plantID);
    }

    public void GetItem(string userName, string itemName) // �������� ��� ���� ������ ���� ������ ������ ���� ���� (������ ItemCount)
    {
        scObject data = new scObject("itemcountinfo"); 
        data.addString("account", userName); 
        data.addString("itemName", itemName);
        message mes = new message("getItem");
        mes.addSCObject(data);
        SendServerMessage(mes);
        Debug.Log("loginScene - GetItem Success.");
    } // userName�� ������ �ִ� itemName


    //�̰� ���� ��ǻ� ���� ���ѽ������� ����ϰ�����..
    //���߿� ���̺��� �߰��ؾߵȴٸ� ���̺� �̸��� ������ �޾Ƽ� �����ǰ� ������ �� �ִ�. ���̺�� userName�� ���ٸ� �����ؼ� ����ϸ�ǰ�
    //�ƴϸ� �׳� ���̺� �̸� ���� ���ָ�ǰ�
    //�Ĺ�����Ʈ���̺��ٰ� �Ĺ��� �̸� ��ġ �ش� �Ĺ��� ����ġ�� �־��ִ� �Լ�. ���ѽɰų� ��ġ �ű涧 ���
    //userName�� ���̵�, plantName �Ĺ��̸�, itemName�� �ø�����ġ����(= PlantList���̺��� �ʵ��, WaterEXP, SunEXP, FertilizerEXP, �� �ϳ�)
    //level�� ����, expAmount�� �÷��ְ���� ����ġ��, posNumber �Ĺ� ��ġ
    //�츮 ����ġ �� ���� ������ �ߴ��� ����? �׷��� �̷��� �س���...
    //�����۸���Ʈ���̺� �ʵ�(PlantPos, PlantName, PlantID, Lv, WaterEXP, SunEXP, FertilizerEXP)
    //////////���� �ʼ�,,,, ���̻� update�� �ǹ̰�����
    public void UpdatePlantListTable(string userName, string plantName, int plantID, string itemName, int posNumber, int level,
        float expAmount, bool isSeedItem)
    {
        Debug.Log("loginScript - UpdatePlantListTable is called");
        scObject data = new scObject("UpdatePlantListTableInfo");
        data.addString("account", userName);
        data.addString("plantName", plantName);
        data.addString("itemName", itemName);
        data.addInt("plantID", plantID);
        data.addInt("posNumber", posNumber);
        data.addInt("level", level);
        data.addFloat("expAmount", expAmount);
        data.addBool("isSeedItem", isSeedItem);
        message mes = new message("UpdatePlantListTable");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void UpdatePlantID(string userName, int plantPos, int plantID)
    {
        Debug.Log("loginScript - UpdatePlantID is called");
        scObject data = new scObject("UpdatePlantIDInfo");
        data.addString("account", userName);
        data.addInt("plantPos", plantPos);
        data.addInt("plantID", plantID);
        message mes = new message("UpdatePlantID");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void SelectQuery(string userName, string columnName, string tableName)
    {
        Debug.Log("loginScript - SelectQuery is called");
        scObject data = new scObject("SelectQueryInfo");
        data.addString("account", userName);
        data.addString("columnName", columnName);
        data.addString("tableName", tableName);
        message mes = new message("SelectQuery");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void addRank()
    {
        scObject data = new scObject("addrankinfo");
        data.addString("account", userName);
        message mes = new message("addrank");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void subtractionRank()
    {
        scObject data = new scObject("subtractionrankinfo");
        data.addString("account", userName);
        message mes = new message("subtractionrank");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }


    public void GetPlantName()
    {
        scObject data = new scObject("getplantnameinfo");
        data.addString("account", userName);
        message mes = new message("getplantname");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void UpdatePlantExp(string userName ,int plantPos, int level, int expName, float expAmount)
    {
        //expName ���� waterExp, sunExp ��� �߿� �ϳ� ���ߵ�, ���߿� �̰͵� �̳������� �ٲٱ�
        //PlantPos�� �⺻Ű�ϱ�...�ʿ�...
        Debug.Log("*******************loginScript - UpdatePlantExp is success************************");
        Debug.Log("Plant Position = " + plantPos + "      Level is = " + level + "      expName is = " + expName);
        scObject data = new scObject("UpdatePlantExpInfo");
        data.addString("account", userName);
        data.addInt("plantPos", plantPos);
        data.addInt("level", level);
        data.addInt("expName", expName);
        data.addFloat("expAmount", expAmount);
        message mes = new message("UpdatePlantExp");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void UseItem(string userName, string itemName, int itemNum) // �������� ����ϱ� ���� ������ ������ ����� ������ ���� ����
    {
        scObject data = new scObject("useiteminfo");
        data.addString("account", userName);
        data.addString("itemName", itemName);
        data.addInt("itemNum", itemNum);
        message mes = new message("UseItem");
        mes.addSCObject(data);
        SendServerMessage(mes);
        Debug.Log("loginScene - UseItem Success.");
    } // userName�� itemName ���

    public void ItemCountCheck(string userName, string itemName) 
    {
        scObject data = new scObject("itemaccountcheckinfo");
        data.addString("account", userName);
        data.addString("itemName", itemName);
        message mes = new message("ItemCountCheck");
        mes.addSCObject(data);
        SendServerMessage(mes);
        Debug.Log("loginScene - ItemCountCheck Success.");
    } //userName�� itemName�� �� �� �����ߴ��� Ȯ��. (�� ������ itemaccountcheck �Լ�����!)

    public void cScokClose() // ���� ���� ����.
    {
        cSock.Close();
    }

    ScreenCapture sc = new ScreenCapture(); // sc �ν��Ͻ�ȭ

    public void SendTime(float time, string scenename)
    {
        scObject data = new scObject("sendtimeinfo");
        data.addFloat("time", time);
        data.addString("account", userName);
        data.addString("scenename", scenename);
        message mes = new message("sendtime");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void AllDeletePlantName()
    {
        scObject data = new scObject("alldeleteplantnameinfo");
        data.addString("account", userName);
        message mes = new message("alldeleteplantname");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void DeletePlantName(string plantname)
    {
        scObject data = new scObject("deleteplantnameinfo");
        data.addString("account", userName);
        data.addString("plantname", plantname);
        message mes = new message("deleteplantname");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }
    public void SendPlantName(string plantname)
    {
        scObject data = new scObject("sendplantnameinfo");
        data.addString("account", userName);
        data.addString("plantname", plantname);
        message mes = new message("sendplantname");
        mes.addSCObject(data);
        SendServerMessage(mes);
        Debug.Log("loginScript Recieve " + plantname);
    }

    public void SendCardMessage(string plantname)
    {
        scObject data = new scObject("sendcardmessageinfo");
        data.addBool("SCM", true);
        data.addString("plantname", plantname);
        message mes = new message("sendcardmessage");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void SendDestroyOtherObject()
    {
        scObject data = new scObject("senddestroyotherobjectinfo");
        data.addBool("SDOO", true);
        message mes = new message("senddestroyotherobject");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void SendDestroyCastle()
    {
        Debug.Log("??");
        scObject data = new scObject("senddestroycastleinfo");
        data.addBool("SDC", true);
        message mes = new message("senddestroycastle");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public void TransferIP(string userip, string userName)
    {
        scObject data = new scObject("transferipinfo");
        data.addString("battleip", userip);
        data.addString("account", userName);
        message mes = new message("transferip");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }


    public void logInAccount(string userName, string p)
    {
        scObject data = new scObject("loginInfo");
        data.addString("account", userName);
        string nPass = calculateMD5Hash(p);
        data.addString("password", p);
        message mes = new message("login");
        mes.addSCObject(data);
        SendServerMessage(mes);
        Debug.Log("loginScene - loginAccountSuccess.");
    }

    public void existurlcheck(string userName, string p)
    {
        scObject data = new scObject("urlcheckInfo");
        data.addString("account", userName);
        //string nPass = calculateMD5Hash(p);
        data.addString("password", p);
        message mes = new message("urlcheck");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }


    private void Register(string userName, string p)
    {
        scObject data = new scObject("registerinfo");
        data.addString("account", userName);
        //string nPass = calculateMD5Hash(p);
        data.addString("password", p);
        message mes = new message("register");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    private void Transfer(string url, string userName)
    {
        scObject data = new scObject("urltransferinfo");
        data.addString("url", url);
        data.addString("account", userName);
        message mes = new message("url");
        mes.addSCObject(data);
        SendServerMessage(mes);
    }

    public string calculateMD5Hash(string p)
    {
        MD5 md = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(p);
        byte[] hash = md.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
#endregion

    void connect()
    {
        try
        {
            // get policy if we are on the web or in editor
            //if ((Application.platform == RuntimePlatform.WindowsWebPlayer) || (Application.platform == RuntimePlatform.WindowsEditor))
            if ((Application.platform == RuntimePlatform.WebGLPlayer) || (Application.platform == RuntimePlatform.WindowsEditor))
            {///////////////////
                //Security.PrefetchSocketPolicy(ipAddress, pfrPort);
            }
            cSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            cSock.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), sPort));
            clientConnection gsCon = new clientConnection(cSock);
        }
        catch
        {
            Debug.Log("Unable to connect to server.");
        }
    }

    public void onConnect()
    {
        connectedToServer = true;

        /*test the connection
        message testMessage = new message("Park Chan Hyung");
        SendServerMessage(testMessage);*/
    }

    private void OnApplicationQuit()
    {
        try { cSock.Close(); }
        catch { }
    }

    public void homesend()
    {
        scObject data = new scObject("homeReponse");
        message mes = new message("home");
        SendServerMessage(mes);
    }
    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Pause = true;
            homesend();
        }
        else
        {
            if(Pause)
            {
                Pause = false;
                DisconnectServerObj.SetActive(true);
            }
        }
    }

    public void addServerMessageToQue(message msg)
    {
        incMessages.Add(msg);
    }

    void Update()
    {
        if (incMessages.Count > 0)
        {
            doMessages();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitObj.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Home))
        {
            OnApplicationPause(true);
        }
    }

    private void doMessages()
    {
        // do messages
        List<message> completedMessages = new List<message>();
        for (int i = 0; i < incMessages.Count; i++)
        {
            try
            {
                handleData(incMessages[i]);
                completedMessages.Add(incMessages[i]);
            }
            catch { }
        }
        
        // delete completed messages
        for (int i = 0; i < completedMessages.Count; i++)
        {
            try
            {
                incMessages.Remove(completedMessages[i]);
            }
            catch { }
        }
    }

    // ------------------------------------------ �������� ���� ���� ----------------------------------------------
    #region
    public void handleData(message mes)
    {
        switch (mes.messageText)
        {
            case "homeResponse":
                if(mes.getSCObject(0).getBool("response"))
                {
                    cSock.Close();
                }
                break;

            case "closeResponse":
                if(mes.getSCObject(0).getBool("response"))
                {
                    cSock.Close();
                    Application.Quit();
                }
                break;
            
            case "loginResponse":
                Debug.Log(""+ mes.getSCObject(0).getBool("response"));
                Debug.Log("The login response returned: " + (mes.getSCObject(0).getBool("response") ? "correct." : "not correct."));

                if (mes.getSCObject(0).getBool("response"))
                {
                    existurlcheck(userName, pass);
                    //ItemCountCheck(userName, wItem);
                }

                else
                {
                    CheckUserInfoObj.SetActive(true);
                    LoginPanelObj.SetActive(true);
                }
                break;

            case "registerResponse":

                Debug.Log("" + mes.getSCObject(0).getBool("response"));
                Debug.Log("The register response returned: " + (mes.getSCObject(0).getBool("response") ? "correct." : "not correct."));
                if(mes.getSCObject(0).getBool("response"))
                {
                    CreateAccountPanelObj.SetActive(false);
                    LoginPanelObj.SetActive(true);
                }
                else
                {
                    IdCheckObj.SetActive(true);
                }
                break;

            case "urlResponse":
                Debug.Log("" + mes.getSCObject(0).getBool("response"));
                Debug.Log("The url response returned: " + (mes.getSCObject(0).getBool("response") ? "correct." : "not correct."));
                if(mes.getSCObject(0).getBool("response"))
                {
                    Debug.Log("URL ��� ����");
                }
                else
                {
                    Debug.Log("URL ��� ����");
                }
                break;

            case "ItemCountCheckResponse":
                
                Debug.Log("server send message ItemCountCheck");
                if(mes.getSCObject(0).getBool("response")) // �������� ItemCountCheck�� �� ����Ǹ� �� �����ۿ� ������ �°� ������ �� ����.
                {
                    wGetItem = mes.getSCObject(0).getInt("wItemNum");
                    Debug.Log("wGetItem = mes.getSCObject(0).getInt(wItemNum);");
                    fGetItem = mes.getSCObject(0).getInt("fItemNum");
                    Debug.Log("fGetItem = mes.getSCObject(0).getInt(fItemNum);");
                    sGetItem = mes.getSCObject(0).getInt("sItemNum");
                    Debug.Log("sGetItem = mes.getSCObject(0).getInt(sItemNum);");
                    nGetItem = mes.getSCObject(0).getInt("nItemNum");
                    Debug.Log("nGetItem = mes.getSCObject(0).getInt(nItemNum);");
                    sfsGetItem = mes.getSCObject(0).getInt("sfsItemNum");
                    Debug.Log("sfsGetItem = mes.getSCObject(0).getInt(sfsItemNum);");
                    csGetItem = mes.getSCObject(0).getInt("csItemNum");
                    Debug.Log("csGetItem = mes.getSCObject(0).getInt(csItemNum);");
                    tsGetItem = mes.getSCObject(0).getInt("tsItemNum");
                    Debug.Log("tsGetItem = mes.getSCObject(0).getInt(tsItemNum);");
                    Debug.Log("ItemCountCheckResponse success.");
                }
                else
                {
                    Debug.Log("ItemCountCheckResponse fail");
                }
                break;

            case "UpdatePlantListTableResponse":
                Debug.Log("" + mes.getSCObject(0).getBool("response"));
                Debug.Log("The url response returned: " + (mes.getSCObject(0).getBool("response") ? "correct." : "not correct."));
                if (mes.getSCObject(0).getBool("response"))
                {
                    Debug.Log("UpdatePlantListTableResponse Success");
                }
                else
                {
                    Debug.Log("UpdatePlantListTableResponse Fail");
                }
                break;

            case "SelectQueryResponse":
                Debug.Log("server send message SelectQueryResponse");
                if (mes.getSCObject(0).getBool("response")) ///���뼺�հԸ������ ���⵵ �ٽ�
                {
                    for (int i = 0; i <= mes.getSCObject(0).getInt("plantListTableCount") - 1; i++)
                    {
                        plantName.Add(mes.getSCObject(0).getString("plantName[" + i + "]"));
                        plantPos.Add(mes.getSCObject(0).getInt("plantPos[" + i + "]"));
                        plantID.Add(mes.getSCObject(0).getInt("plantID[" + i + "]"));
                        Lv.Add(mes.getSCObject(0).getInt("Lv[" + i + "]"));
                        waterEXP.Add(mes.getSCObject(0).getFloat("waterEXP[" + i + "]"));
                        sunEXP.Add(mes.getSCObject(0).getFloat("sunEXP[" + i + "]"));
                        fertilizerEXP.Add(mes.getSCObject(0).getFloat("fertilizerEXP[" + i + "]"));
                    } //sendmessage �����غ���, cloudrecotrackableventhandler
                    Debug.Log("SelectQuery success" );
                    Debug.Log("plantPos[0] = " + plantPos[0] + " & plantPos.count is " + plantPos.Count);
                }
                else
                {
                    Debug.Log("loginScript - SelectQueryResponse ����ȵ�");
                }
                break;

                case "UpdatePlantExpResponse":
                Debug.Log("server send message UpdatePlantExpResponse");
                if (mes.getSCObject(0).getBool("response")) ///���뼺�հԸ������ ���⵵ �ٽ�
                {
                    Debug.Log("UpdatePlantExpResponse success");
                }
                else
                {
                    Debug.Log("loginScript - UpdatePlantExpResponse Error");
                }
                break;

            case "urlcheckResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    LoginPanelObj.SetActive(false);
                    SceneManager.LoadScene("PlantInfo");
                }

                else
                {
                    LoginPanelObj.SetActive(false);
                    SceneManager.LoadScene("SetPlantsBedScene");
                    //cSock.Close();
                }
                break;

            case "getItemResponse": // �������� getItem�� ����� �޾ƿ�
                Debug.Log("" + mes.getSCObject(0).getBool("response"));
                Debug.Log("The getItem response returned: " + (mes.getSCObject(0).getBool("response") ? "correct." : "not correct."));
                if (mes.getSCObject(0).getBool("response"))
                {
                    Debug.Log(itemName + "increase OK");
                }
                else
                {
                    Debug.Log(itemName + "increase Fali");
                    SceneManager.LoadScene("GPSScene");
                }
                break;


            case "UseItemResponse":
                Debug.Log("using OK?");
                if (mes.getSCObject(0).getBool("response"))
                {
                    Debug.Log("UseItemResponse - �������� true �� �޾ƿ�.");
                    //wGetItem, fGetItem, sGetItem, nGetItem, sfsGetItem, csGetItem, tsGetItem
                    /*wGetItem = mes.getSCObject(0).getInt("wItemNum");
                    fGetItem = mes.getSCObject(0).getInt("fItemNum");
                    sGetItem = mes.getSCObject(0).getInt("sItemNum");
                    nGetItem = mes.getSCObject(0).getInt("nItemNum");
                    sfsGetItem = mes.getSCObject(0).getInt("sfsItemNum");
                    csGetItem = mes.getSCObject(0).getInt("csItemNum");
                    tsGetItem = mes.getSCObject(0).getInt("tsItemNum");*/
                }
                else
                {
                    Debug.Log("UseItemResponse - �������� false �� �޾ƿ�.");
                    SceneManager.LoadScene("PlantInfo");
                }
                break;

            case "TransferIPResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    Debug.Log("TransferIPResponse Working");
                    Battle Bt = new Battle();
                    Bt.NextScene();
                }

                else
                {
                    Debug.Log("Wait");
                }
                break;

            case "GetPlantNameResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    name1 = mes.getSCObject(0).getString("name1");
                    name2 = mes.getSCObject(0).getString("name2");
                    name3 = mes.getSCObject(0).getString("name3");
                    name4 = mes.getSCObject(0).getString("name4");
                    Debug.Log("" + name1 + " " + name2 + " " + name3 + " " + name4);
                }
                else
                {
                    Debug.Log("Fail to Get PlantName");
                }
                break;
            case "SendipaddressResponse": //�������� ���� IP�ּҸ� �޾ƿͼ� Ŭ���̾�Ʈ ip�� ����.
                userip = mes.getSCObject(0).getString("useripaddress");
                if (mes.getSCObject(0).getBool("response"))
                {
                    Debug.Log("PPPPPPPPPPPPPPPPPPPPPP" + userip);
                }
                else
                {
                    cSock.Close();
                    SceneManager.LoadScene("loginScene");
                }
                break;

            case "servermessageResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    servermessage = mes.getSCObject(0).getString("servermessage");
                }
                else
                {
                    Debug.Log("NOT OK");
                }
                break;

            case "MyCardResponse": //���� �޴� �޼��� //���� �ذ� ���� //��� ����׸� �ݺ��� ;
                if (mes.getSCObject(0).getBool("response"))
                {
                    switch (mes.getSCObject(0).getString("plantname"))
                    {
                        case "PoisonMushroom_B(Clone)":
                            Battle_DropZone.PoisonMushroom_B();
                            break;
                        case "PoisonMushroom_R(Clone)":
                            Battle_DropZone.PoisonMushroom_R();
                            break;
                        case "Baby plant(Clone)":
                            Battle_DropZone.Baby_plant();
                            break;
                        case "BlowFishORCactus(Clone)":
                            Battle_DropZone.BlowFishORCactus();
                            break;
                        case "Cosmos(Clone)":
                            Battle_DropZone.Cosmos();
                            break;
                        case "DeliciousMushRoom(Clone)":
                            Battle_DropZone.DeliciousMushRoom();
                            break;
                        case "GreenbristleGrass(Clone)":
                            Battle_DropZone.GreenbristleGrass();
                            break;
                        case "JustBamboo(Clone)":
                            Battle_DropZone.JustBamboo();
                            break;
                        case "NotTreeButRock(Clone)":
                            Battle_DropZone.NotTreeButRock();
                            break;
                        case "PoisonMushroom_G(Clone)":
                            Battle_DropZone.PoisonMushroom_G();
                            break;
                        case "PurpleFlower(Clone)":
                            Battle_DropZone.PurpleFlower();
                            break;
                        case "ShiningFlower(Clone)":
                            Battle_DropZone.ShiningFlower();
                            break;
                        case "UnknownOrangeColorFlower(Clone)":
                            Battle_DropZone.UnknownOrangeColorFlower();
                            break;
                        case "UselessRock(Clone)":
                            Battle_DropZone.UselessRock();
                            break;
                        case "Weed(Clone)":
                            Battle_DropZone.Weed();
                            break;
                        case "Yellow Flower(Clone)":
                            Battle_DropZone.Yellow_Flower();
                            break;
                        case "YouCantSeeMyFaceFlower(Clone)":
                            Battle_DropZone.YouCantSeeMyFaceFlower();
                            break;
                    }
                }
                break;

            case "SendCreateAICardResponse": //��밡 �޴� �޼���
                if (mes.getSCObject(0).getBool("response"))
                {
                    switch (mes.getSCObject(0).getString("plantname"))
                    {
                        case "PoisonMushroom_B(Clone)":
                            Battle_DropZone.PoisonMushroom_B_Ai();
                            break;
                        case "PoisonMushroom_R(Clone)":
                            Battle_DropZone.PoisonMushroom_R_Ai();
                            break;
                        case "Baby plant(Clone)":
                            Battle_DropZone.Baby_plant_Ai();
                            break;
                        case "BlowFishORCactus(Clone)":
                            Battle_DropZone.BlowFishORCactus_Ai();
                            break;
                        case "Cosmos(Clone)":
                            Battle_DropZone.Cosmos_Ai();
                            break;
                        case "DeliciousMushRoom(Clone)":
                            Battle_DropZone.DeliciousMushRoom_Ai();
                            break;
                        case "GreenbristleGrass(Clone)":
                            Battle_DropZone.GreenbristleGrass_Ai();
                            break;
                        case "JustBamboo(Clone)":
                            Battle_DropZone.JustBamboo_Ai();
                            break;
                        case "NotTreeButRock(Clone)":
                            Battle_DropZone.NotTreeButRock_Ai();
                            break;
                        case "PoisonMushroom_G(Clone)":
                            Battle_DropZone.PoisonMushroom_G_Ai();
                            break;
                        case "PurpleFlower(Clone)":
                            Battle_DropZone.PurpleFlower_Ai();
                            break;
                        case "ShiningFlower(Clone)":
                            Battle_DropZone.ShiningFlower_Ai();
                            break;
                        case "UnknownOrangeColorFlower(Clone)":
                            Battle_DropZone.UnknownOrangeColorFlower_Ai();
                            break;
                        case "UselessRock(Clone)":
                            Battle_DropZone.UselessRock_Ai();
                            break;
                        case "Weed(Clone)":
                            Battle_DropZone.Weed_Ai();
                            break;
                        case "Yellow Flower(Clone)":
                            Battle_DropZone.Yellow_Flower_Ai();
                            break;
                        case "YouCantSeeMyFaceFlower(Clone)":
                            Battle_DropZone.YouCantSeeMyFaceFlower_Ai();
                            break;
                    }
                }
                break;

            case "SendDestroyMyObjectResponse"://���� ������Ʈ�� �Ѽ�������. //���⵵ ���� ���Ͱ� �׾����� ǥ�����ָ� �� ������.
                if (mes.getSCObject(0).getBool("response"))
                {
                    Enemy1_info.Instance.DestroyMyObj();
                    Debug.Log("SendDestroyMyObjectResponse");
                }
                else
                {
                    Debug.Log("NOT OK");
                }
                break;

            case "SendDestroyAiObjectResponse"://AI�� ������Ʈ�� �Ѽ������� ////���⵵ ���� ���Ͱ� �׾����� ǥ�����ָ� �� ������.
                if (mes.getSCObject(0).getBool("response"))
                {
                    Enemy2_info.Instance.DestroyMyObj();
                    Debug.Log("SendDestroyAiObjectResponse");
                }
                else
                {
                    Debug.Log("NOT OK");
                }
                break;

            case "SendDestroyMyCastleResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    MyCastle.Instance.DestoryCastle();
                    Debug.Log("SendDestroyMyCastleResponse");
                }
                else
                {
                    Debug.Log("NOT OK");
                }
                break;

            case "SendDestroyAiCastleResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    EnemyCastle.Instance.DestoryCastle();
                    Debug.Log("SendDestroyAiCastleResponse");
                }
                else
                {
                    Debug.Log("NOT OK");
                }
                break;

            case "SendVictoryResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    addRank();
                    StartCoroutine(Victory());
                }
                else
                {
                    Debug.Log("NOT OK");
                }
                break;

            case "SendLoseResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    subtractionRank();
                    StartCoroutine(Defeat());
                }
                else
                {
                    Debug.Log("NOT OK");
                }
                break;

            case "addRankResponse":
                if(mes.getSCObject(0).getBool("response"))
                {
                    rankpoint = mes.getSCObject(0).getInt("rankpoint");
                    Debug.Log("addRank " + rankpoint);
                }
                else
                {
                    Debug.Log("Not OK");
                }
                break;

            case "subtractionRankResponse":
                if (mes.getSCObject(0).getBool("response"))
                {
                    rankpoint = mes.getSCObject(0).getInt("rankpoint");
                    Debug.Log("subtractionRank " + rankpoint);
                }
                else
                {
                    Debug.Log("Not OK");
                }
                break;

            default:
                Debug.Log("The server sent a message: " + mes.messageText);
                break;

        }
    }
    IEnumerator Victory()
    {

        yield return new WaitForSeconds(2.0f);
        victory_or_defeat.Instance.VictoryPanelObj.SetActive(true);
    }

    IEnumerator Defeat()
    {
        yield return new WaitForSeconds(2.0f);
        victory_or_defeat.Instance.DefeatPanelObj.SetActive(true);
    }
    #endregion
    //------------------------------------------ ������ �޼��� �ְ� �޴� �Լ� -------------------------------------
    #region
    public void SendServerMessage(message mes)
    {
        if (connectedToServer)
        {
            try
            {
                // convert message into a byte array, wrap the message with framing
                byte[] messageObject = conversionTools.convertObjectToBytes(mes);
                byte[] readyMessage = conversionTools.wrapMessage(messageObject);

                // send completed message
                cSock.Send(readyMessage);
            }
            catch
            {
                Debug.Log("There was an error sending server message " + mes.messageText);
            }
        }
    }

    public void ReceiveServerMessage(message mes)
    {
        if (connectedToServer)
        {
            try
            {
                byte[] messageObject = conversionTools.convertObjectToBytes(mes);
                byte[] readyMessage = conversionTools.wrapMessage(messageObject);

                // send completed message
                cSock.Receive(readyMessage);
            }
            catch
            {
                Debug.Log("There was an error receiveing server message" + mes.messageText);
            }
        }
    }
#endregion
}