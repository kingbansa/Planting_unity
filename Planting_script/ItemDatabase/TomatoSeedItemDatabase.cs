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

public class TomatoSeedItemDatabase : MonoBehaviour{
    public GameObject seedObj;
    public Button tSeedButton;
    public GameObject itemNumberObj;
    public GameObject _plantExpPanelObj;
    public Text itemNumberText;
    string state = "enable";
    public Image btColor;
    public int itemNumber = 0;
    public static int currentExp = 0;
    private Vector3 UIPadding;
    public bool isDropBtn;
    public GameObject cofirmPanelObj;
    public GameObject cloudRecoTargetObj;
    public ConfirmMenuCtrl _confirmMenuCtrl;
    public CloudRecoTrackableEventHandler _cloudRecoTrackableEventHandler;
    public PlantExpPanel _plantExpPanel;
    public GameObject invActingMenuPanel;
    public int itemNum;
    const int maxVal = 10000;
    float probabilityVar;
    List<int> plantPosIndex = new List<int>();                //디비에서 받아온 위치값 인덱스
    List<string> plantName = new List<string>();                 //디비에서 받아온 식물 이름
    List<int> plantLv = new List<int>();                      //디비에서 받아온 식물 레벨
    List<float> waterExp = new List<float>();                          //디비에서 받아온 식물 경험치
    List<float> sunExp = new List<float>();
    List<float> fertilizerExp = new List<float>();

    Dictionary<int, string> plantNameID = new Dictionary<int, string>();

    public void OnMouseDown()
    {
        if(state == "enable")
        {
            state = "disable";
            invActingMenuPanel.SetActive(true);
            invActingMenuPanel.transform.position = seedObj.transform.position - UIPadding;
            //_confirmMenuCtrl._clickedItem = ConfirmMenuCtrl.ClickedItem.SunFlowerSeed;  // 왜 이줄이 null리퍼런스라고 뜨는지 모르겟네
            _confirmMenuCtrl.SetClickedItem(ConfirmMenuCtrl.ClickedItem.TomatoSeed);
            state = "enable";
        }

    }

    public void ClearList()
    {
        loginScript.plantName.Clear();
        loginScript.plantPos.Clear();
        loginScript.Lv.Clear();
        loginScript.waterEXP.Clear();
        loginScript.sunEXP.Clear();
        loginScript.fertilizerEXP.Clear();
        plantPosIndex.Clear();
        plantName.Clear();
        plantLv.Clear();
        waterExp.Clear();
        sunExp.Clear();
        fertilizerExp.Clear();
    }

    //언젠가 나중에 cloudRecoTrackableHandler에서 값 직접 받아와서 사용하도록 고쳐야한다... 
    //어처피 씨앗 먹엇을때 cloudRecoTrackableHandler의 callRenewScript다시 실행시켜줘야하기때문에...
    //또 여기서 받아오는역할만 하게 해주면 나중에 코드 바꿀일 생겼을 때 cloudRecoTrackableHandler만 고쳐주면 되기 때문에...
    IEnumerator RenewPlantList()
    {
        ClearList();

        yield return new WaitForSeconds(0.2f);

        loginScript.Instance.SelectQuery("*", "PlantList_"); //select * from PlantList_account;

        yield return new WaitForSeconds(0.3f);        ///이거 나중에 이렇게 강제적으로 시간 주는게아니라 서버에서 값 다 받아오면 자동으로 실행되게...

        //DB에서 받아온 값 저장시키는 부분
        for (int i = 0; i <= loginScript.plantPos.Count - 1; i++)
        {
            plantPosIndex.Add(loginScript.plantPos[i]);
            plantName.Add(loginScript.plantName[i]);
            plantLv.Add(loginScript.Lv[i]);
            waterExp.Add(loginScript.waterEXP[i]);
            sunExp.Add(loginScript.sunEXP[i]);
            fertilizerExp.Add(loginScript.fertilizerEXP[i]);
        }
    }


    public void StartItemEnable()
    {
        StartCoroutine(ItemEnable());
    }

    IEnumerator ItemEnable()
    {
        if (state == "enable" && !isDropBtn)
        {
            probabilityVar = ((float)UnityEngine.Random.Range(0, maxVal)) / maxVal;
            int posRan = UnityEngine.Random.Range(0, 11);
            bool isHave = true;
            while (isHave)
            {
                posRan = UnityEngine.Random.Range(0, 11);
                if (!plantPosIndex.Contains(posRan))
                {
                    isHave = false;
                }
            }

            if (plantPosIndex.Count <= 12)
            {
                if (probabilityVar <= 0.5f && probabilityVar > 0 && !plantName.Contains("NotTreeButRock"))
                {
                    state = "disable";
                    tSeedButton.interactable = false;
                    btColor.color = new Color32(152, 152, 152, 255);
                    Debug.Log("아이템 대기시간");
                    int plantIDToInt = (int)PlantNameEnum.NotTreeButRock_Lv1;
                    loginScript.Instance.UseItem("tsItem", itemNum);
                    loginScript.Instance.ItemCountCheck("tsItem");

                    loginScript.Instance.UpdatePlantListTable("NotTreeButRock", plantIDToInt, "WaterEXP", posRan, 1, 0.0f, true);
                    itemNumber = loginScript.tsGetItem;
                    currentExp = loginScript.Exp;
                    //_plantExpPanel.CallExpList();
                    yield return new WaitForSeconds(3f);
                    state = "enable";
                    tSeedButton.interactable = true;
                    btColor.color = new Color32(255, 255, 255, 255);
                    //이게 안먹히네*****************************************************
                    _cloudRecoTrackableEventHandler.CallRenewPlantList();
                    Debug.Log("꼬지모 생성!");
                }
                else if (probabilityVar > 0.5f && !plantName.Contains("JustBamboo"))
                {
                    state = "disable";
                    tSeedButton.interactable = false;
                    btColor.color = new Color32(152, 152, 152, 255);
                    Debug.Log("아이템 대기시간");
                    int plantIDToInt = (int)PlantNameEnum.JustBamboo_Lv1;
                    loginScript.Instance.UseItem("tsfsItem", itemNum);
                    loginScript.Instance.ItemCountCheck("tsItem");

                    loginScript.Instance.UpdatePlantListTable("JustBamboo", plantIDToInt, "WaterEXP", posRan, 1, 0.0f, true);
                    //posNumber값을 8로준 이유는 8이 화분 딱 중앙임, 물론 랜덤랜지 해서 줘도 되긴함, 나중에 해당 자리에 식물이 잇는지 없는지 판단해서 넣는거 추가하면 좋을듯
                    itemNumber = loginScript.tsGetItem;
                    currentExp = loginScript.Exp;
                    //_plantExpPanel.CallExpList();
                    yield return new WaitForSeconds(3f);
                    state = "enable";
                    tSeedButton.interactable = true;
                    btColor.color = new Color32(255, 255, 255, 255);
                    //이게 안먹히네*****************************************************
                    _cloudRecoTrackableEventHandler.CallRenewPlantList();
                    Debug.Log("대나무 생성!");
                }
                else
                {
                    Debug.Log("Nothing Happened");
                }
            }
        }

        if (isDropBtn)
        {
            loginScript.Instance.UseItem("tsItem", itemNum);
            loginScript.Instance.ItemCountCheck("tsItem");
        }
        
    }

    private static TomatoSeedItemDatabase instance;
    public static TomatoSeedItemDatabase Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        _confirmMenuCtrl = cofirmPanelObj.GetComponent<ConfirmMenuCtrl>();
        _cloudRecoTrackableEventHandler = cloudRecoTargetObj.GetComponent<CloudRecoTrackableEventHandler>();
    }

    void Start()
    {
        Debug.Log("TomatoSeeditemDatabase Start");
        loginScript.Instance.ItemCountCheck("tsItem");
        //_plantExpPanel = _plantExpPanelObj.GetComponent<PlantExpPanel>();
        itemNumber = loginScript.tsGetItem;
     
        currentExp = loginScript.Exp;
        Debug.Log("" + itemNumber);
        Debug.Log("" + currentExp);
        //_confirmMenuCtrl = new ConfirmMenuCtrl();
        UIPadding = new Vector3(-10, 10, 0);
        //_cloudRecoTrackableEventHandler = gameObject.GetComponent<CloudRecoTrackableEventHandler>();
        //StartCoroutine(Loading());
    }

    //IEnumerator Loading()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    UpdateTSData();
    //}
    void Update() // Update함수 이름 이렇게 바꾸고 ConfirmMenuCtrl에 있는 case ClickedItem.TomatoSeed: 에서 호출함.
    {
        itemNumber = loginScript.tsGetItem;
        currentExp = loginScript.Exp;
        itemNumberText.text = "" + itemNumber;

        if (itemNumber == 0)
        {
            seedObj.SetActive(false);
            itemNumberObj.SetActive(false);
        }

        if (itemNumber >= 1)
        {
            seedObj.SetActive(true);
            itemNumberObj.SetActive(true);
        }
    }
}

