using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class victory_or_defeat : MonoBehaviour
{

    [Header("Victory")]
    public GameObject VictoryPanelObj;
    public Text VictoryRankPoint;

    [Header("Defeat")]
    public GameObject DefeatPanelObj;
    public Text DefeatRankPoint;

    private static victory_or_defeat instance;
    public static victory_or_defeat Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    public void OkButton()
    {
        StartCoroutine(Wait());
        SceneManager.LoadScene("LobbyScene");
        VictoryPanelObj.SetActive(false);
        DefeatPanelObj.SetActive(false);
    }

    public void PP()
    {
        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        yield return new WaitForSeconds(1.0f);
        Battle_DropZone.PoisonMushroom_B();
        Battle_DropZone.PoisonMushroom_G();
        Battle_DropZone.YouCantSeeMyFaceFlower();
        Battle_DropZone.Yellow_Flower();
        Battle_DropZone.Weed();
        Battle_DropZone.UselessRock();
        Battle_DropZone.UnknownOrangeColorFlower();
        Battle_DropZone.ShiningFlower();
        Battle_DropZone.PurpleFlower();
        Battle_DropZone.PoisonMushroom_G();
        Battle_DropZone.NotTreeButRock();
        Battle_DropZone.GreenbristleGrass();
        Battle_DropZone.DeliciousMushRoom();
        Battle_DropZone.Cosmos();
        Battle_DropZone.BlowFishORCactus();
        Battle_DropZone.Baby_plant();
    }

    IEnumerator Wait()
    {
        loginScript.Instance.AllDeletePlantName();
        yield return new WaitForSeconds(0.5f);
    }

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update () {
        VictoryRankPoint.text = "" + loginScript.rankpoint.ToString();
        DefeatRankPoint.text = "" + loginScript.rankpoint.ToString();
    }
}
