/*===============================================================================
Copyright (c) 2015-2016 PTC Inc. All Rights Reserved.
 
Copyright (c) 2010-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;
using scMessage;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class PlantLocation : MonoBehaviour
{
    #region PUBLIC_MEMBERS
    /// <summary>
    /// The scan-line rendered in overlay when Cloud Reco is in scanning mode.
    /// </summary>
    /// 식물위치 옮기고 업데이트하는것만 만들어주면 끝... trackableLost일때 안꺼지게해주는건... 나중에하자
    //public int expDB;
    public GameObject[] pointObj;
    public Transform[] pointTr;         //식물 위치 설정을 위한 위치

    //public GameObject[] Cube;
    //public GameObject[] createdCube = new GameObject[12];

    public Dropdown plantListDropdown;  //식물이름들 나와잇는 드랍다운메뉴
    public Text plantNameText;      //선택된식물 이름

    public static List<int> currentPlantLv;
    #endregion // PUBLIC_MEMBERS

    #region PRIVATE_MEMBERS
    List<int> plantPosIndex = new List<int>();                //디비에서 받아온 위치값 인덱스
    List<string> plantName = new List<string>();                 //디비에서 받아온 식물 이름
    List<int> plantLv = new List<int>();                      //디비에서 받아온 식물 레벨
    List<int> plantID = new List<int>();
    List<float> waterExp = new List<float>();                          //디비에서 받아온 식물 경험치
    List<float> sunExp = new List<float>();
    List<float> fertilizerExp = new List<float>();

    Dictionary<string, string> plantNameToKor = new Dictionary<string, string>();  //식물 영문명을 한글로...
    //디비에 올라갈 식물들의 아이디, 레벨 구분하려고 쓰는거, instantiate 용도로 사용하기 편함
    public Dictionary<int, GameObject> plantNameID = new Dictionary<int, GameObject>();
    //모든 식물 프리팹을 등록할 게임오브젝트 배열, 딕셔너리에 등록할 용도
    public GameObject[] allPlantPrefabs;
    //씬에 생성될 모든 식물 오브젝트, 최대 12개만 심을 수 있기 때문에 크기가 12, 디비에서 받아온 식물들만 하나씩 조건 검사해서 추가
    List<GameObject> createdPlantObj = new List<GameObject>();
    //GameObject[] createdPlantObj = new GameObject[12];

    //각 경험치가 이거 이상되면 식물 렙업시키고 올라간 레벨 디비로 전송
    float[] waterLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    float[] sunLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    float[] fertilizerLvUpExp = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    public void Start()
    {
        pointObj = GameObject.FindGameObjectsWithTag("Point");
        pointTr = new Transform[pointObj.Length];
        //loginScript.Instance.SelectQuery("*", "PlantList_");
        plantName.Add("Please Select Your Plant");
        AddPlantListToDropdown();

        CallRenewPlantList();
        RegisterPlantPrefabs();

        //Debug.Log("CloudRecoTrackableEventHandler - start - plantPosIndex & plantName = " + plantPosIndex[0] + ", " + plantName[0]);
        //식물이 심어질 위치Transform가져오는 부분
        for (int i = 0; i <= pointObj.Length - 1; i++)
        {
            pointTr[i] = pointObj[i].GetComponent<Transform>();  //pointObj[i].Getcomponent<Transform>(); 같은문장임 pointObj[i].transform
        }
    }
    #endregion //MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>

    //이거 일단 다른스크립트에서 접근 가능하도록 하긴했는데 쓸일이 있을지는 모르겟다
    public float[] CloudReco_GetWaterLvUpExp()
    {
        return waterLvUpExp;
    }
    public float[] CloudReco_GetSunLvUpExp()
    {
        return sunLvUpExp;
    }
    public float[] CloudReco_GetFertilizerLvUpExp()
    {
        return fertilizerLvUpExp;
    }

    //드랍다운 메뉴에 옵션 추가하는거
    public void AddPlantListToDropdown()
    {
        Debug.Log("1");
        plantListDropdown.ClearOptions();
        plantListDropdown.AddOptions(plantName);
    }

    //드랍다운리스트에서 다른 식물 선택했을때
    public void Dropdown_IndexChanged(int index)
    {
        plantNameText.text = plantName[index];
    }



    public void CallRenewPlantList()
    {
        StartCoroutine(RenewPlantList());
    }

    IEnumerator RenewPlantList()
    {
        ClearList();

        yield return new WaitForSeconds(0.2f);

        loginScript.Instance.SelectQuery("*", "PlantList_"); //select * from PlantList_account;

        yield return new WaitForSeconds(0.3f);        ///이거 나중에 이렇게 강제적으로 시간 주는게아니라 서버에서 값 다 받아오면 자동으로 실행되게...

        //DB에서 받아온 값 저장시키는 부분
        plantName.Add("Please Select Your Plant");
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

        for (int i = 0; i <= plantID.Count - 1; i++)
        {
            createdPlantObj.Add(Instantiate(plantNameID[plantID[i]]) as GameObject);
            //createdPlantObj[i] = Instantiate(plantNameID[ plantID[i] ]) as GameObject;
            createdPlantObj[i].transform.SetParent(pointTr[plantPosIndex[i]]);
            createdPlantObj[i].transform.localPosition = new Vector3(0, 0.19f, 0);
            //createdPlantObj[i].transform.localScale = new Vector3(1, 1, 1);
            createdPlantObj[i].SetActive(true);
        }
        AddPlantListToDropdown();
        //Debug.Log("cloudRecoTrackableEvnetHandler - RenewPlantList is called");
        //OnTrackingFound();
    }

    public void RegisterPlantPrefabs()
    {
        //PlantNameEnum plantNameEnum, GameObject plantObj
        //plantNameID.Add(plantNameEnum, plantObj);
        //plantNameID.Add(allPlantPrefabs[0].GetComponent<plantBeha
        for (int i = 0; i <= allPlantPrefabs.Length - 1; i++)
        {
            plantNameID.Add(allPlantPrefabs[i].GetComponent<PlantBehavior>().GetPlantNameEnum(), allPlantPrefabs[i]);
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


   
    
    #endregion //PUBLIC_METHODS


    #region PRIVATE_METHODS
    private void OnTrackingFound()
    {
        // 다 완성되면 이 메서드 안에잇는거 다 밑에있는 if문안에 넣으면됨, 이 if문이 사용자 자신이 찍은 사진만 인식할 수 있게 하는 부분
        //if (mTrackableBehaviour.TrackableName == loginScript.userName)
        //{

        //}
        Debug.Log("plantLocationSC - OnTrackingFound is called");
        //CloudRecoTrackableEventHandler CR = new CloudRecoTrackableEventHandler();
        //CallRenewPlantList();    ////////지금은 임시방편으로 여기서 갱신시켜주지만 아이템 데이터베이스스크립트에서 센드메시지같은걸로 사용하도록 바꿔야...
        //업데이트마다 호출되도록 바꿔야한다.
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        //createdCube[0] = Instantiate(Cube[0]) as GameObject;
        //createdCube[0].transform.SetParent(pointTr[6]);
        //createdCube[0].transform.localPosition = new Vector3(0, 0.5f, 0);
        //createdCube[0].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }

        for (int i = 0; i <= createdPlantObj.Count - 1; i++)
        {
            createdPlantObj[i].SetActive(true);
        }

        // Stop finder since we have now a result, finder will be restarted again when we lose track of the result
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        if (objectTracker != null)
        {
            objectTracker.TargetFinder.Stop();
        }
    }

    
    #endregion //PRIVATE_METHODS
}