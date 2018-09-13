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

public class WItemDatabase : MonoBehaviour
{
    public GameObject waterObj;
    public Button waterButton;
    public GameObject itemNumberObj;
    public GameObject invActingMenuPanel;
    public GameObject cofirmPanelObj;
    public GameObject cloudRecoTargetObj;
    public GameObject _plantExpPanelObj;
    public Text itemNumberText;
    public Dropdown plantListDropdown;
    public Image btColor;
    private float x = 0;
    private float y = 0;
    private Vector3 UIPadding;
    string state = "enable";
    bool s = true;
    public int itemNumber = 0;
    public int itemNum;   //텍스트필드에서 넘겨받은값, confirmMenuCtrl스크립트에서 넘겨줌
    public static int currentExp = 0;
    public ConfirmMenuCtrl _confirmMenuCtrl;
    public CloudRecoTrackableEventHandler _cloudRecoTrackableEventHandler;
    public PlantExpPanel _plantExpPanel;
    public bool isDropBtn;
    string selectedPlantName;
    public GameObject wAnim;
    public Transform wAnimPlace;

    List<int> plantPosIndex = new List<int>();                //디비에서 받아온 위치값 인덱스
    List<string> plantName = new List<string>();                 //디비에서 받아온 식물 이름
    List<int> plantLv = new List<int>();                      //디비에서 받아온 식물 레벨
    List<int> plantID = new List<int>();
    List<float> waterExp = new List<float>();                          //디비에서 받아온 식물 경험치
    List<float> sunExp = new List<float>();
    List<float> fertilizerExp = new List<float>();

    float[] waterLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    float[] sunLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    float[] fertilizerLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

    public void OnMouseDown()
    {
        if(state == "enable")
        {
            state = "disable";
            invActingMenuPanel.SetActive(true);
            invActingMenuPanel.transform.position = waterObj.transform.position - UIPadding;
            _confirmMenuCtrl.SetClickedItem(ConfirmMenuCtrl.ClickedItem.Water);
            selectedPlantName = plantListDropdown.options[plantListDropdown.value].text;
            state = "enable";
        }

        //ConfirmMenuCtrl _confirmMenuCtrl = new ConfirmMenuCtrl();
        //_confirmMenuCtrl._clickedItem = ConfirmMenuCtrl.ClickedItem.Water;

        //StartCoroutine(ItemEnable());
        //GameObject.Find("")
    }

    //confirmMenuCtrl 스크립트에서 ok 버튼 눌렸을 때 호출
    public void StartItemEnable()
    {
        StartCoroutine(ItemEnable());
    }

    IEnumerator ItemEnable()
    {
        //일단 방법은 대충 적어놓으면 cloudRecoTrackableHandler에서 list 값들 가져오고 itemExp값들 가져와서 

        //식물 선택 안돼있을때
        if (plantListDropdown.value == 0)
        {
            Debug.Log("Please Select Plant");
        }
        //사용개수 제한하는건 ConfirmMenuCtrl스크립트에서 해줌
        else if (state == "enable" && !isDropBtn)
        {
       
            state = "disable";
            GameObject instance = Instantiate(wAnim, new Vector3(192, 503, 58), wAnimPlace.rotation) as GameObject; // 애니메이션 재생.
            waterButton.interactable = false;
            btColor.color = new Color32(152, 152, 152, 255);
            Debug.Log("Selected Plant is  = " + selectedPlantName);
            loginScript.Instance.UseItem("wItem", itemNum);
            loginScript.Instance.ItemCountCheck("wItem");
            float exp = 1.0f * (float)itemNum;
            // where = 식물이름으로 찾는게 아니라 where = 선택된 식물이름의 식물위치 인덱스값 으로 해서 추가 
            // water 0, sun 1, fertilizer 2, nutrient 3으로 설정한다 일단
            StartCoroutine(RenewPlantList());
            yield return new WaitForSeconds(1.0f);
            Debug.Log(plantLv.Count);
            PlantLvUpLogic(0, exp);
            //_plantExpPanel.CallExpList();
            itemNumber = loginScript.wGetItem;
            currentExp = loginScript.Exp;
            yield return new WaitForSeconds(2.5f);
            state = "enable";
            waterButton.interactable = true;
            btColor.color = new Color32(255, 255, 255, 255);
            _cloudRecoTrackableEventHandler.CallExpLogic();
           
        }

        if (isDropBtn)
        {
            Debug.Log("아이템 대기시간");
            loginScript.Instance.UseItem("wItem", itemNum);
            loginScript.Instance.ItemCountCheck("wItem");
            itemNumber = loginScript.wGetItem;
        }
    }

    private static WItemDatabase instance;
    public static WItemDatabase Instance
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

    IEnumerator RenewPlantList()
    {
        ClearList();
        yield return new WaitForSeconds(0.2f);
        loginScript.Instance.SelectQuery("*", "PlantList_"); //select * from PlantList_account;
        yield return new WaitForSeconds(0.5f);        ///이거 나중에 이렇게 강제적으로 시간 주는게아니라 서버에서 값 다 받아오면 자동으로 실행되게...
        //DB에서 받아온 값 저장시키는 부분
        for (int i = 0; i <= loginScript.plantPos.Count - 1; i++)
        {
            plantPosIndex.Add(loginScript.plantPos[i]);
            plantName.Add(loginScript.plantName[i]);
            plantID.Add(loginScript.plantID[i]);
            plantLv.Add(loginScript.Lv[i]);
            waterExp.Add(loginScript.waterEXP[i]);
            sunExp.Add(loginScript.sunEXP[i]);
            fertilizerExp.Add(loginScript.fertilizerEXP[i]);
        }
    }

    public void ClearList()
    {
        loginScript.plantName.Clear();
        loginScript.plantPos.Clear();
        loginScript.Lv.Clear();
        loginScript.plantID.Clear();
        loginScript.waterEXP.Clear();
        loginScript.sunEXP.Clear();
        loginScript.fertilizerEXP.Clear();
        plantPosIndex.Clear();
        plantID.Clear();
        plantName.Clear();
        plantLv.Clear();
        waterExp.Clear();
        sunExp.Clear();
        fertilizerExp.Clear();
    }

    public void PlantLvUpLogic(int expName, float expAmount)
    {
        int DBIndex = plantListDropdown.value - 1;
        Debug.Log("************************************PlantLvUpLogic - DBIndex = " + DBIndex);
        Debug.Log(plantLv.Count);
        //중요**********이부분 코루틴으로 만들어서 시간 줘야한다.   밑에줄 에러라는데 왜그럴까~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //왜인지 모르겠으나 서버에서 테이블 데이터를 아예 못받아옴을 확인함.... 어쩌면 순서꼬여서 받아온 다음 삭제하는걸 수도.....
        int LvUpExpIndex = plantLv[DBIndex] - 1;
        
        
        if (DBIndex >= 0)
        {
            if (plantLv[DBIndex] == 10 && waterExp[DBIndex] + expAmount >= waterLvUpExp[LvUpExpIndex] )
            {
                //이경우 DB의 waterEXP = waterLvUpExp[LvUpExpIndex]
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], 10, 0, waterLvUpExp[LvUpExpIndex]);
            }
            if(plantLv[DBIndex] == 10 && waterExp[DBIndex] + expAmount <= waterLvUpExp[LvUpExpIndex])
            {
                //이경우 DB의 waterEXP = waterEXP + expAmount
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], 10, 0, waterExp[DBIndex] + expAmount);
            }
            if (plantLv[DBIndex] < 10 && waterExp[DBIndex] + expAmount >= waterLvUpExp[LvUpExpIndex] && fertilizerExp[DBIndex] >= fertilizerLvUpExp[LvUpExpIndex]
                && sunExp[DBIndex] >= sunLvUpExp[LvUpExpIndex])
            {
                //이경우 각 exp = exp - waterLvUpExp[LvUpExpIndex] 하고 Lv = Lv + 1
                //그리고 plantID 레벨 구간에 맞게 바꿔줘야하고...
                //loginScript.Instance.UpdatePlantListTable(plantName[DBIndex], plantID[DBIndex], ExpName, plantPosIndex[DBIndex], plantLv[DBIndex] + 1, 0, false);
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], plantLv[DBIndex] + 1, 0, waterExp[DBIndex] - waterLvUpExp[LvUpExpIndex]);
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], plantLv[DBIndex] + 1, 1, sunExp[DBIndex] - sunLvUpExp[LvUpExpIndex]);
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], plantLv[DBIndex] + 1, 2, fertilizerExp[DBIndex] - fertilizerLvUpExp[LvUpExpIndex]);
                if (plantLv[DBIndex] + 1 == 4 || plantLv[DBIndex] == 7)
                {
                    //여기서 PlantID = PlantID + 1 해주면 됨
                    loginScript.Instance.UpdatePlantID(plantPosIndex[DBIndex], plantID[DBIndex]);
                }
                
            }
            else if (plantLv[DBIndex] < 10)
            {
                
                //이 경우는 EXP중에 랩업경험치에 도달하지 못한게 잇다는 뜻이기 때문에 걍 w = w + expAmount 해주면 될거같은데...
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], plantLv[DBIndex], expName, waterExp[DBIndex] + expAmount);
            }
        }
    }

    void Start()
    {
        wAnimPlace = GameObject.Find("WaterAnimPlace").transform;
        loginScript.Instance.ItemCountCheck("wItem");
        itemNumber = loginScript.wGetItem;
        //_plantExpPanel = _plantExpPanelObj.GetComponent<PlantExpPanel>();
        //btColor = GetComponent<Image>();
        currentExp = loginScript.Exp;
        x = invActingMenuPanel.transform.localScale.x * 0.5f;
        y = invActingMenuPanel.transform.localScale.y * 0.5f;
        //UIPadding = new Vector3(-x, y, 0);
        UIPadding = new Vector3(-10, 10, 0);
        //ConfirmMenuCtrl _confirmMenuCtrl = new ConfirmMenuCtrl();
        //_confirmMenuCtrl = gameObject.GetComponent<ConfirmMenuCtrl>() as ConfirmMenuCtrl;
        //StartCoroutine(Loading());
    }

    //IEnumerator Loading()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    UpdateWatData();
    //}

    void Update()// Update함수 이름 이렇게 바꾸고 ConfirmMenuCtrl에 있는 case ClickedItem.Water: 에서 호출함.
    {
        itemNumber = loginScript.wGetItem;
        currentExp = loginScript.Exp;
        itemNumberText.text = "" + itemNumber;
        if (itemNumber == 0)
        {
            waterObj.SetActive(false);
            itemNumberObj.SetActive(false);
        }

        if (itemNumber >= 1)
        {
            waterObj.SetActive(true);
            itemNumberObj.SetActive(true);
        }
    }
}