using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Battle_Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;
   
    GameObject placeholder = null;

    public GameObject Panel_4;
    public GameObject My;
    public GameObject HandPanel;
    public enum Slot { Before_Activation, After_Activation }; //이게 약간 분류같은거 이게 다르면 패널에 안올라간다. 이걸 이용하면 문제 해결 될듯;//구역지정

    public Slot typeOfState = Slot.Before_Activation;//구역 지정

    void Start()
    {
        
    }

    void Update()
    {

    }

    void Awake()
    {
        Panel_4 = GameObject.Find("Panel_4");
        HandPanel = GameObject.Find("HandPanel");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent; //다시 캔버스로 돌아온다.
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent); //카드가 캔버스에서 벗어난다.

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //this.transform.position = eventData.position;
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 100.0f;
        this.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        if (placeholder.transform.parent != placeholderParent)
            placeholder.transform.SetParent(placeholderParent);
        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;
                break;
            }
        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /*
            this.transform.SetParent(parentToReturnTo); //다시 캔버스로 돌아온다.
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy(placeholder);*/
        if (parentToReturnTo == Panel_4.transform && GameObject.FindGameObjectWithTag("ally") == false)
        {
            Destroy(My);
            Destroy(placeholder);
            Debug.Log("this game object name" + this.gameObject.name);
            loginScript.Instance.SendCardMessage(this.gameObject.name);
        }
        else if(GameObject.FindGameObjectWithTag("ally") == true || parentToReturnTo != Panel_4.transform)
        {
            this.transform.SetParent(HandPanel.transform); //다시 캔버스로 돌아온다.
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy(placeholder);
        }
        //Debug.Log("OnEndDrag");
    }
}

/*if (placeholderParent == Panel_4.transform && (GameObject.FindGameObjectsWithTag("ally").Length >= 1))
            {
                Debug.Log("@" + GameObject.FindGameObjectsWithTag("ally").Length + "@" + GameObject.FindGameObjectsWithTag("enemy").Length);
                this.transform.SetParent(parentToReturnTo); //다시 캔버스로 돌아온다.
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
GetComponent<CanvasGroup>().blocksRaycasts = true;
Destroy(placeholder);
Debug.Log("Don't Create My Character");*/