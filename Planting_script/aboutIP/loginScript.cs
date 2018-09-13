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
    // 처음 로그인할때 디비에서 로그인 아이디에 맞는 아이템 디비값 담는 변수      static 선언시 다른 스크립트에서 loginScript.변수명 으로 호출
    // 일반 선언시 다른 스크립트에서 loginScript.instance.변수명 으로 호출   차이는??
    //public static string wItem = "wItem", fItem, sItem, nItem, sfsItem, csItem, tsItem;
    // 서버에 전달할 아이템 이름 선언
    public static int Exp = 0;

    //public int itemNum;  //인벤토리에서 아이템 사용할 때 몇개 사용할지 개수 넘겨받는 변수

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
        connect(); // loginScene 실행하면 서버와 연결
        name1 = "";
        name2 = "";
        name3 = "";
        name4 = "";
    }

    // ---------------------------------------------- UI 및 버튼 ----------------------------------------------
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

    public void LoginBtn() // 로그인 버튼 (loginScene - LoginPanel - LoginButton)
    {
        userName = IDInputField.text;
        pass = PassInputField.text;
        logInAccount(userName, pass);
        AllDeletePlantName(); //이거 안해주면 덱 저장됨
        GetPlantName();
        //ItemCountCheck(userName, pass, wItem);
        Debug.Log("" + userName + pass);
        Debug.Log("" + name1 + name2 + name3 + name4);
    }

    public void OpenCreateAccountBtn() // CreateAccountPanel창으로 전환 (loginScene - LoginPanel - CreateAccountButton)
    {
        CreateAccountPanelObj.SetActive(true);
    }

    public void CreateAccountBtn() // 새로운 계정 등록 버튼 (loginScene - CreateAccountPanel - CreateButton)
    {
        new_userName = New_IDInputField.text;
        new_pass = New_PassInputField.text;
        Register(new_userName, new_pass);
        Debug.Log("" + new_userName + new_pass);
    }

    public void IdCheckBtn() // 계정 등록 할 때 아이디 중복 확인 버튼 (LoginScene - CreateAccountPanel - IdCheck - IdCheckButton)
    {
        IdCheckObj.SetActive(false);
    }

    public void CancleBtn() // 새로운 계정 등록 취소 (loginScene - CreateAccountPanel - CancleButton)
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

    public void QuitNoBtn() // 게임 종료 취소 버튼 (loginScene - Quit?Panel - NoButton)
    {
        QuitObj.SetActive(false);
    }

    public void CheckInfoBtn() // 로그인 실패 확인 버튼 (loginScene - CheckUserInfo - CheckInfoButton)
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

    // ------------------------------------------------ 함 수 ------------------------------------------------


    //아이템 사용시 호출      itN은 itName라는 뜻
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

    //아이템 먹엇을 때 
    public void GetItem(string itN)
    {
        GetItem(userName, itN);
        Debug.Log("loginScript - GetItem - " + itN + " received from " + itN + "ItemManager.");

    }

    //사용자 이름(account)으로 만들어진 식물리스트테이블 정보 업데이트
    public void UpdatePlantListTable(string plantName, int plantID, string itemName, int posNumber, int level, float expAmount, bool isSeedItem)
    {
        UpdatePlantListTable(userName, plantName, plantID, itemName, posNumber, level, expAmount, isSeedItem);
    }

    //아이템리스트테이블 필드(PlantPos, PlantName, PlantID, Lv, WaterEXP, SunEXP, FertilizerEXP)
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

    public void GetItem(string userName, string itemName) // 아이템을 얻기 위해 서버로 얻을 계정과 아이템 정보 전달 (전에는 ItemCount)
    {
        scObject data = new scObject("itemcountinfo"); 
        data.addString("account", userName); 
        data.addString("itemName", itemName);
        message mes = new message("getItem");
        mes.addSCObject(data);
        SendServerMessage(mes);
        Debug.Log("loginScene - GetItem Success.");
    } // userName이 가지고 있는 itemName


    //이거 지금 사실상 거의 씨앗심을때만 사용하고잇음..
    //나중에 테이블을 추가해야된다면 테이블 이름도 변수로 받아서 생성되게 변경할 수 있다. 테이블명에 userName가 들어간다면 조합해서 사용하면되고
    //아니면 그냥 테이블 이름 직접 써주면되고
    //식물리스트테이블에다가 식물의 이름 위치 해당 식물의 경험치를 넣어주는 함수. 씨앗심거나 위치 옮길때 사용
    //userName는 아이디, plantName 식물이름, itemName는 올릴경험치종류(= PlantList테이블의 필드명, WaterEXP, SunEXP, FertilizerEXP, 중 하나)
    //level은 레벨, expAmount는 올려주고싶은 경험치량, posNumber 식물 위치
    //우리 경험치 다 따로 만들기로 했던거 알지? 그래서 이렇게 해놓음...
    //아이템리스트테이블 필드(PlantPos, PlantName, PlantID, Lv, WaterEXP, SunEXP, FertilizerEXP)
    //////////수정 필수,,,, 더이상 update의 의미가없다
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
        //expName 에는 waterExp, sunExp 등등 중에 하나 들어가야됨, 나중에 이것도 이넘형으로 바꾸기
        //PlantPos가 기본키니깐...필요...
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

    public void UseItem(string userName, string itemName, int itemNum) // 아이템을 사용하기 위해 서버로 계정과 사용할 아이템 정보 전달
    {
        scObject data = new scObject("useiteminfo");
        data.addString("account", userName);
        data.addString("itemName", itemName);
        data.addInt("itemNum", itemNum);
        message mes = new message("UseItem");
        mes.addSCObject(data);
        SendServerMessage(mes);
        Debug.Log("loginScene - UseItem Success.");
    } // userName이 itemName 사용

    public void ItemCountCheck(string userName, string itemName) 
    {
        scObject data = new scObject("itemaccountcheckinfo");
        data.addString("account", userName);
        data.addString("itemName", itemName);
        message mes = new message("ItemCountCheck");
        mes.addSCObject(data);
        SendServerMessage(mes);
        Debug.Log("loginScene - ItemCountCheck Success.");
    } //userName이 itemName을 몇 개 소유했는지 확인. (전 버전은 itemaccountcheck 함수였음!)

    public void cScokClose() // 서버 연결 종료.
    {
        cSock.Close();
    }

    ScreenCapture sc = new ScreenCapture(); // sc 인스턴스화

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

    // ------------------------------------------ 서버에서 오는 반응 ----------------------------------------------
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
                    Debug.Log("URL 등록 성공");
                }
                else
                {
                    Debug.Log("URL 등록 실패");
                }
                break;

            case "ItemCountCheckResponse":
                
                Debug.Log("server send message ItemCountCheck");
                if(mes.getSCObject(0).getBool("response")) // 서버에서 ItemCountCheck가 잘 실행되면 각 아이템에 계정에 맞게 아이템 수 저장.
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
                if (mes.getSCObject(0).getBool("response")) ///범용성잇게만들려면 여기도 다시
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
                    } //sendmessage 생각해보자, cloudrecotrackableventhandler
                    Debug.Log("SelectQuery success" );
                    Debug.Log("plantPos[0] = " + plantPos[0] + " & plantPos.count is " + plantPos.Count);
                }
                else
                {
                    Debug.Log("loginScript - SelectQueryResponse 실행안됨");
                }
                break;

                case "UpdatePlantExpResponse":
                Debug.Log("server send message UpdatePlantExpResponse");
                if (mes.getSCObject(0).getBool("response")) ///범용성잇게만들려면 여기도 다시
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

            case "getItemResponse": // 서버에서 getItem의 결과를 받아옴
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
                    Debug.Log("UseItemResponse - 서버에서 true 값 받아옴.");
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
                    Debug.Log("UseItemResponse - 서버에서 false 값 받아옴.");
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
            case "SendipaddressResponse": //서버에서 보낸 IP주소를 받아와서 클라이언트 ip에 저장.
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

            case "MyCardResponse": //내가 받는 메세지 //여기 해결 못함 //계속 디버그만 반복됨 ;
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

            case "SendCreateAICardResponse": //상대가 받는 메세지
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

            case "SendDestroyMyObjectResponse"://나의 오브젝트가 뿌셔졌을때. //여기도 무슨 몬스터가 죽었는지 표시해주면 더 좋아짐.
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

            case "SendDestroyAiObjectResponse"://AI의 오브젝트가 뿌셔졌을때 ////여기도 무슨 몬스터가 죽었는지 표시해주면 더 좋아짐.
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
    //------------------------------------------ 서버와 메세지 주고 받는 함수 -------------------------------------
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