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

public class FItemDatabase : MonoBehaviour {

    public GameObject fertilizerObj;
    public Button fertilizerButton;
    public GameObject itemNumberObj;
    public GameObject invActingMenuPanel;
    public GameObject cofirmPanelObj;
    public GameObject cloudRecoTargetObj;
    public GameObject _plantExpPanelObj;
    public Dropdown plantListDropdown;
    public Text itemNumberText;
    private float x = 0;
    private float y = 0;
    private Vector3 UIPadding;
    public Image btColor;
    string state = "enable";
    public int itemNumber = 0;
    public int itemNum;                //텍스트필드에서 넘겨받은값, confirmMenuCtrl스크립트에서 넘겨줌
    public static int currentExp = 0;
    public ConfirmMenuCtrl _confirmMenuCtrl;
    public CloudRecoTrackableEventHandler _cloudRecoTrackableEventHandler;
    public PlantExpPanel _plantExpPanel;
    public bool isDropBtn;
    string selectedPlantName;
    public GameObject ferAnim;
    public Transform fAnimPlace;

    List<int> plantPosIndex = new List<int>();                //디비에서 받아온 위치값 인덱스
    List<string> plantName = new List<string>();                 //디비에서 받아온 식물 이름
    List<int> plantLv = new List<int>();                      //디비에서 받아온 식물 레벨
    List<int> plantID = new List<int>();
    List<float> waterExp = new List<float>();                          //디비에서 받아온 식물 경험치
    List<float> sunExp = new List<float>();
    List<float> fertilizerExp = new List<float>();

    /*float[] waterLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    float[] sunLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    float[] fertilizerLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };*/

    float[] waterLvUpExp = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
    float[] sunLvUpExp = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
    float[] fertilizerLvUpExp = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };

    public void OnMouseDown()
    {
        if(state == "enable")
        {
            state = "disable";
            invActingMenuPanel.SetActive(true);
            invActingMenuPanel.transform.position = fertilizerObj.transform.position - UIPadding;
            _confirmMenuCtrl.SetClickedItem(ConfirmMenuCtrl.ClickedItem.Fertilizer);
            selectedPlantName = plantListDropdown.options[plantListDropdown.value].text;
            state = "enable";
        }

    }

    public void StartItemEnable()
    {
        StartCoroutine(ItemEnable());
    }

    IEnumerator ItemEnable()
    {
        //식물 선택 안돼있을때
        if (plantListDropdown.value == 0)
        {
            Debug.Log("Please Select Plant");
        }
        //사용개수 제한하는건 ConfirmMenuCtrl스크립트에서 해줌
        else if (state == "enable" && !isDropBtn)
        {
            state = "disable";
            GameObject instance = Instantiate(ferAnim, new Vector3(39, 221, -45), fAnimPlace.rotation) as GameObject; // 애니메이션 재생.
            fertilizerButton.interactable = false;
            btColor.color = new Color32(152, 152, 152, 255);
            Debug.Log("Selected Plant is  = " + selectedPlantName);
            loginScript.Instance.UseItem("fItem", itemNum);
            loginScript.Instance.ItemCountCheck("fItem");
            float exp = 1.0f * (float)itemNum;
            //loginScript.Instance.UpdatePlantListTable(selectedPlantName, 0, "FertilizerEXP", 0, 1, exp, false);
            // water 0, sun 1, fertilizer 2, nutrient 3으로 설정한다 일단
            StartCoroutine(RenewPlantList());
            
            yield return new WaitForSeconds(1.0f);
            
            PlantLvUpLogic(2, exp);
            //_plantExpPanel.CallExpList();///////////////////////
            itemNumber = loginScript.fGetItem;
            currentExp = loginScript.Exp;
            yield return new WaitForSeconds(2.5f);
            state = "enable";
            fertilizerButton.interactable = true;
            btColor.color = new Color32(255, 255, 255, 255);
            //이게 안먹히네*****************************************************
            _cloudRecoTrackableEventHandler.CallExpLogic();
        }

        if (isDropBtn)
        {
            Debug.Log("아이템 대기시간");
            loginScript.Instance.UseItem("fItem", itemNum);
            loginScript.Instance.ItemCountCheck("fItem");
            itemNumber = loginScript.fGetItem;
        }
    }
    private static FItemDatabase instance;
    public static FItemDatabase Instance
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
        //StartCoroutine(RenewPlantList());
        int DBIndex = plantListDropdown.value - 1;
        int LvUpExpIndex = plantLv[DBIndex] - 1;
        if (DBIndex >= 0)
        {
            if (plantLv[DBIndex] == 10 && fertilizerExp[DBIndex] + expAmount >= fertilizerLvUpExp[LvUpExpIndex])
            {
                //이경우 DB의 waterEXP = waterLvUpExp[LvUpExpIndex]
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], 10, expName, fertilizerLvUpExp[LvUpExpIndex]);
            }
            if (plantLv[DBIndex] == 10 && fertilizerExp[DBIndex] + expAmount <= fertilizerLvUpExp[LvUpExpIndex])
            {
                //이경우 DB의 waterEXP = waterEXP + expAmount
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], 10, expName, fertilizerExp[DBIndex] + expAmount);
            }
            if (plantLv[DBIndex] < 10 && fertilizerExp[DBIndex] + expAmount >= fertilizerLvUpExp[LvUpExpIndex] && waterExp[DBIndex] >= waterLvUpExp[LvUpExpIndex]
                && sunExp[DBIndex] >= sunLvUpExp[LvUpExpIndex])
            {
                //이경우 각 exp = exp - waterLvUpExp[LvUpExpIndex] 하고 Lv = Lv + 1
                //그리고 plantID 레벨 구간에 맞게 바꿔줘야하고...
                //loginScript.Instance.UpdatePlantListTable(plantName[DBIndex], plantID[DBIndex], ExpName, plantPosIndex[DBIndex], plantLv[DBIndex] + 1, 0, false);
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], plantLv[DBIndex] + 1, 0, waterExp[DBIndex] - waterLvUpExp[LvUpExpIndex]);
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], plantLv[DBIndex] + 1, 1, sunExp[DBIndex] - sunLvUpExp[LvUpExpIndex]);
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], plantLv[DBIndex] + 1, expName, fertilizerExp[DBIndex] - fertilizerLvUpExp[LvUpExpIndex]);
                if (plantLv[DBIndex] + 1 == 4 || plantLv[DBIndex] == 7)
                {
                    //여기서 PlantID = PlantID + 1 해주면 됨
                    loginScript.Instance.UpdatePlantID(plantPosIndex[DBIndex], plantID[DBIndex]);
                }

            }
            else if (plantLv[DBIndex] < 10)
            {
                //이 경우는 EXP중에 랩업경험치에 도달하지 못한게 잇다는 뜻이기 때문에 걍 w = w + expAmount 해주면 될거같은데...
                loginScript.Instance.UpdatePlantExp(plantPosIndex[DBIndex], plantLv[DBIndex], expName, fertilizerExp[DBIndex] + expAmount);
            }
        }
    }

    void Start()
    {
        Debug.Log("FItemDatabase start");
        loginScript.Instance.ItemCountCheck("fItem");
        //_plantExpPanel = _plantExpPanelObj.GetComponent<PlantExpPanel>();////////////////
        itemNumber = loginScript.fGetItem;
        
        currentExp = loginScript.Exp;

        x = invActingMenuPanel.transform.localScale.x * 0.5f;
        y = invActingMenuPanel.transform.localScale.y * 0.5f;
        UIPadding = new Vector3(-10, 10, 0);
        //_confirmMenuCtrl = new ConfirmMenuCtrl();
        //StartCoroutine(Loading());
    }

    //IEnumerator Loading()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    UpdateFerData();
    //}

     void Update() // Update함수 이름 이렇게 바꾸고 ConfirmMenuCtrl에 있는 case ClickedItem.Fertilizer: 에서 호출함.
    {
        itemNumber = loginScript.fGetItem;
        currentExp = loginScript.Exp;
        itemNumberText.text = "" + itemNumber;

        if (itemNumber == 0)
        {
            fertilizerObj.SetActive(false);
            itemNumberObj.SetActive(false);
        }

        if (itemNumber >= 1)
        {
            fertilizerObj.SetActive(true);
            itemNumberObj.SetActive(true);
        }
    }
}
