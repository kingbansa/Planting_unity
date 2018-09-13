using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Battle_DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler{

    public Battle_Draggable.Slot typeOfState = Battle_Draggable.Slot.Before_Activation; //구역 지정 해주는 곳
    public GameObject Panel;

    private static Battle_DropZone instance;
    public static Battle_DropZone Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        Panel = this.gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        if (eventData.pointerDrag == null)
            return;

        Battle_Draggable d = eventData.pointerDrag.GetComponent<Battle_Draggable>();
        if (d != null)
        {
            d.placeholderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        if (eventData.pointerDrag == null)
            return;

        Battle_Draggable d = eventData.pointerDrag.GetComponent<Battle_Draggable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag + " was dropped on " + gameObject.name);
        Battle_Draggable d = eventData.pointerDrag.GetComponent<Battle_Draggable>();
        if (d != null)
        {
            if (typeOfState == d.typeOfState)
            {
                d.parentToReturnTo = this.transform;
            }
            /*if (d.Select_Panel.transform.childCount >= 5)
            {
                d.parentToReturnTo = d.Choose_Panel.transform;
                Debug.Log("Delete Select Panel Child ");
            }

            else
            {
                d.parentToReturnTo = this.transform;
            }*/
        }
    }
    #region
    //스위치나 더 좋은 방법으로 바꿔야함
    public static void PoisonMushroom_B()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/PoisonMushroom_B_Lv2") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void PoisonMushroom_R()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/PoisonMushroom_R_Lv2") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void Baby_plant()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/Baby plant") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void BlowFishORCactus()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/BlowFishORCactus_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void Cosmos()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/Cosmos_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void DeliciousMushRoom()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/DeliciousMushRoom_Lv2") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void GreenbristleGrass()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/GreenbristleGrass_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void JustBamboo()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/JustBamboo_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void NotTreeButRock()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/NotTreeButRock_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void PoisonMushroom_G()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/PoisonMushroom_G_Lv2") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void PurpleFlower()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/PurpleFlower_Lv2") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void ShiningFlower()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/ShiningFlower_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void UnknownOrangeColorFlower()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/UnknownOrangeColorFlower_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void UselessRock()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/UselessRock_Lv2") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void Weed()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/Weed_Lv3") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void Yellow_Flower()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/Yellow Flower_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void YouCantSeeMyFaceFlower()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/YouCantSeeMyFaceFlower_Lv1") as GameObject, new Vector3(0, 4, -38f), Quaternion.identity);
    }

    public static void PoisonMushroom_B_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/PoisonMushroom_B_Lv2_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void PoisonMushroom_R_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/PoisonMushroom_R_Lv2_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void Baby_plant_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/Baby plant_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void BlowFishORCactus_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/BlowFishORCactus_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void Cosmos_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/Cosmos_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void DeliciousMushRoom_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/DeliciousMushRoom_Lv2_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void GreenbristleGrass_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/GreenbristleGrass_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void JustBamboo_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/JustBamboo_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void NotTreeButRock_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/NotTreeButRock_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void PoisonMushroom_G_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/PoisonMushroom_G_Lv2_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void PurpleFlower_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/PurpleFlower_Lv2_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void ShiningFlower_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/ShiningFlower_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void UnknownOrangeColorFlower_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/UnknownOrangeColorFlower_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void UselessRock_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/UselessRock_Lv2_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void Weed_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/Weed_Lv3_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void Yellow_Flower_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/Yellow Flower_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }

    public static void YouCantSeeMyFaceFlower_Ai()
    {
        Instantiate(Resources.Load("Prefabs/BattleScenePrefabs/RealPrefabs/YouCantSeeMyFaceFlower_Lv1_Ai") as GameObject, new Vector3(0, 4, 38f), Quaternion.identity);
    }
    #endregion
    void Start ()
    {

    }

    void Update () {

    }
}
