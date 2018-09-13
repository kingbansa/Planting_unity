using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using scMessage;
using System.Text;
using System;
using System.Security.Cryptography;
using UnityEngine.UI;

public class ConfirmMenuCtrl : MonoBehaviour {
    //스크립트이름을 useItemCtrl이나 뭐 이런걸로 바꿔야될듯
    public enum ClickedItem {Water, SunPiece, Nutrient, Fertilizer, SunFlowerSeed, CactusSeed, TomatoSeed};

    public static ClickedItem _clickedItem;
    public RectTransform conMenuRTr;   //confirmMenuRectTransform
    public RectTransform actingMenuRTr; //actingMenuRecttransform
    private Vector3 UIPadding;
    private float x;
    public InputField itemmNumInputField;
    private int itemNum;   //텍스트필드에서 받는 값, 몇개 사용할지
    private string inputFieldText;
    //public string clickedItem;
    public GameObject _itemDatabase;
    public WItemDatabase _wItemDatabase;
    public FItemDatabase _fItemDatabase;
    public SItemDatabase _sItemDatabase;
    public NItemDatabase _nItemDatabase;
    public SunflowerSeedItemDatabase _sunFlowerSeedItemDatabase;
    public CactusSeedItemDatabase _cactusSeedItemDatabase;
    public TomatoSeedItemDatabase _tomatoSeedItemDatabase;
    private bool isDropBtn;
    public List<Transform> animPlace = new List<Transform>();  // 애니메이션이 재생 될 위치를 animPlace 리스트에 저장

    //ui 실험중 W아이템베이스 부터
    //게임 버튼 클릭 이벤트 다시 확인
    //클릭하면 바로 1개씩 사용되는 기능도 추가해야 된다.

    public void ShowConfirmMenu ()
    {
        conMenuRTr.gameObject.SetActive(true);
        conMenuRTr.position = actingMenuRTr.position + UIPadding;
        //loginScript.Instance.itemNum = itemNum;   //이게 아니라 로그인스크립트 안의 아이템사용하는 함수 직접 호출해야뎀
    }

    public void SetClickedItem(ClickedItem str)
    {
        _clickedItem = str;
    }

    public void IsDropBtn()
    {
        isDropBtn = true;
        //이것도 클릭했던 아이템에 따라서 바뀌어야함 스위치 문처럼
        _wItemDatabase.isDropBtn = true;
    }

    public void IsUseBtn()
    {
        isDropBtn = false;
        //이것도 클릭했던 아이템에 따라서 바뀌어야함 스위치 문처럼
        _wItemDatabase.isDropBtn = false;
    }

    // Use this for initialization
    void Start () {

        animPlace.Add(GameObject.Find("WaterAnimPlace").transform);
        animPlace.Add(GameObject.Find("FerAnimPlace").transform);
        animPlace.Add(GameObject.Find("SunAnimPlace").transform);
        animPlace.Add(GameObject.Find("NutrAnimPlace").transform); // 시작하면 animPlace 리스트에 애니메이션 재생 될 위치를 순서대로 저장.
        itemNum = 0;
        _wItemDatabase = _itemDatabase.GetComponent<WItemDatabase>(); //이런식으로 초기화해서 witembase스크립트 안에있는 코루틴 함수 호출
        _fItemDatabase = _itemDatabase.GetComponent<FItemDatabase>();
        _sItemDatabase = _itemDatabase.GetComponent<SItemDatabase>();
        _nItemDatabase = _itemDatabase.GetComponent<NItemDatabase>();
        _sunFlowerSeedItemDatabase = _itemDatabase.GetComponent<SunflowerSeedItemDatabase>();
        _cactusSeedItemDatabase = _itemDatabase.GetComponent<CactusSeedItemDatabase>();
        _tomatoSeedItemDatabase = _itemDatabase.GetComponent<TomatoSeedItemDatabase>();
        conMenuRTr = this.gameObject.GetComponent<RectTransform>();
        actingMenuRTr = gameObject.GetComponent<RectTransform>();
        //gameObject와 GameObject의 차이 : 소문자는 보통 publci로 선언되어서 attached된 오브젝트들에서 사용되고
        //대문자는 보통 find같은 함수로 찾은 다른 오브젝트에 대해 뭔가 작업할 때 사용
        x = actingMenuRTr.rect.width * 0.5f + conMenuRTr.rect.width * 0.5f;
        UIPadding = new Vector3(x, 0, 0);
	}

    //확인버튼이 눌렸을때호출     //try catch 추가해서 itemNum이 스트링일경우 잡아내야됨
    public void OKBtnCtrl()
    {
        inputFieldText = itemmNumInputField.text;
        itemNum = Convert.ToInt32(inputFieldText);  //텍스트필드에서 입력한 숫자를 itemNum에 저장
        //itemNum = int.Parse(itemmNumInputField.text);  // 위라인이랑 같은 문법
        Debug.Log("Text Field Number is " + itemNum);
        Debug.Log("" + _clickedItem);
        
        switch (_clickedItem)
        {
            case ClickedItem.Water:
                int wItemNum = loginScript.wGetItem;
                if (itemNum >= 1 && itemNum <= wItemNum)
                {
                    Debug.Log("Enum type is " + _clickedItem + " Is DropBtn?" + isDropBtn);
                    _wItemDatabase.itemNum = itemNum;//텍스트필드로 넘겨받은 수를 워터아이템베이스의 itemNum으로 넘겨줌
                    _wItemDatabase.StartItemEnable();
                }
                else
                {
                    Debug.Log("1이상 현재 가진 물 아이템 개수 이하의 수를 입력하세요");
                }
                break;

            case ClickedItem.Fertilizer:
                int fItemNum = loginScript.fGetItem;
                if (itemNum >= 1 && itemNum <= fItemNum)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _fItemDatabase.itemNum = itemNum;//텍스트필드로 넘겨받은 수를 워터아이템베이스의 itemNum으로 넘겨줌
                    _fItemDatabase.StartItemEnable();
                }
                else
                {
                    Debug.Log("1이상 현재 가진 물 아이템 개수 이하의 수를 입력하세요");
                }
                break;

            case ClickedItem.SunPiece:
                int sItemNum = loginScript.sGetItem;
                if (itemNum >= 1 && itemNum <= sItemNum)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _sItemDatabase.itemNum = itemNum;//텍스트필드로 넘겨받은 수를 워터아이템베이스의 itemNum으로 넘겨줌
                    _sItemDatabase.StartItemEnable();

                }
                else
                {
                    Debug.Log("1이상 현재 가진 물 아이템 개수 이하의 수를 입력하세요");
                }
                break;

            case ClickedItem.Nutrient:
                int nItemNum = loginScript.nGetItem;
                if (itemNum >= 1 && itemNum <= nItemNum)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _nItemDatabase.itemNum = itemNum;//텍스트필드로 넘겨받은 수를 워터아이템베이스의 itemNum으로 넘겨줌
                    _nItemDatabase.StartItemEnable();

                }
                else
                {
                    Debug.Log("1이상 현재 가진 물 아이템 개수 이하의 수를 입력하세요");
                }
                break;

            case ClickedItem.SunFlowerSeed:
                int SFSeedItemNum = loginScript.sfsGetItem;
                if (!isDropBtn && itemNum == 1)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _sunFlowerSeedItemDatabase.itemNum = itemNum;
                    _sunFlowerSeedItemDatabase.StartItemEnable();
                    //SunflowerSeedItemDatabase.Instance.UpdateSFData();
                }
                else if (isDropBtn && itemNum>=1 && itemNum<=SFSeedItemNum)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _sunFlowerSeedItemDatabase.itemNum = itemNum;
                    _sunFlowerSeedItemDatabase.StartItemEnable();
                    //SunflowerSeedItemDatabase.Instance.UpdateSFData();
                }
                else
                {
                    Debug.Log("씨앗은 한번에 한개만 사용가능합니다. 0이상 아이템 개수 이하의 값을 입력하시오.");
                }
                break;

            case ClickedItem.CactusSeed:
                int cactusSeedItemNum = loginScript.csGetItem;
                if (!isDropBtn && itemNum == 1)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _cactusSeedItemDatabase.itemNum = itemNum;
                    _cactusSeedItemDatabase.StartItemEnable();
                    //CactusSeedItemDatabase.Instance.UpdateCSData();
                }
                else if (isDropBtn && itemNum >= 1 && itemNum <= cactusSeedItemNum)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _cactusSeedItemDatabase.itemNum = itemNum;
                    _cactusSeedItemDatabase.StartItemEnable();
                    //CactusSeedItemDatabase.Instance.UpdateCSData();
                }
                else
                {
                    Debug.Log("씨앗은 한번에 한개만 사용가능합니다. 0이상 아이템 개수 이하의 값을 입력하시오.");
                }
                break;

            case ClickedItem.TomatoSeed:
                int tomatoSeedItemNum = loginScript.sfsGetItem;
                if (!isDropBtn && itemNum == 1)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _tomatoSeedItemDatabase.itemNum = itemNum;
                    _tomatoSeedItemDatabase.StartItemEnable();
                    //TomatoSeedItemDatabase.Instance.UpdateTSData();
                }
                else if (isDropBtn && itemNum >= 1 && itemNum <= tomatoSeedItemNum)
                {
                    Debug.Log("Enum type is " + _clickedItem + "   Is DropBtn?" + isDropBtn);
                    _tomatoSeedItemDatabase.itemNum = itemNum;
                    _tomatoSeedItemDatabase.StartItemEnable();
                    //TomatoSeedItemDatabase.Instance.UpdateTSData();

                }
                else
                {
                    Debug.Log("씨앗은 한번에 한개만 사용가능합니다. 0이상 아이템 개수 이하의 값을 입력하시오.");
                }
                break;

            default:
                break;
        }
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
