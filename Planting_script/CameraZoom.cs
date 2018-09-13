using UnityEngine;
using System.Collections;

public class CameraZoom : TouchLogic//NOTE: This script has been updated to V2 after video recording
{
    public GameObject plantObj;   //화분 오브젝트

    private Transform plantTr;   //화분 위치
    public Transform camTr;   //카메라 트랜스폼(위치)

    private float zoomSpeed = 8.0f;
    private float maxZoomDist = 70.0f;  //카메라와 오브젝트 사이거리의 최대값 = 최대 축소
    private float minZoomDist = 12.0f;   //카메라와 오브젝트 사이거리의 최소값 = 최대 확대
    //buckets for caching our touch positions
    private Vector2 currTouch1 = Vector2.zero,
    lastTouch1 = Vector2.zero,
    currTouch2 = Vector2.zero,
    lastTouch2 = Vector2.zero;
    //used for holding our distances and calculating our zoomFactor
    private float currDist = 0.0f, lastDist = 0.0f, zoomFactor = 0.0f;
    private float zoomDist;

    public override void OnTouchMovedAnywhere()
    {
        Zoom();
    }
    public override void OnTouchStayedAnywhere()
    {
        Zoom();
    }
    private void Start()
    {
        camTr = Camera.main.transform;   //메인카메라 트랜스폼 받아옴, 카메라 여러개면 plantObject.Getcomponent<Transform>()사용
        plantTr = plantObj.GetComponent<Transform>();
        zoomDist = Vector3.Distance(camTr.position, plantTr.position);
    }
    //find distance between the 2 touches 1 frame before & current frame
    //if the delta distance increased, zoom in, if delta distance decreased, zoom out
    void Zoom()
    {
        //Caches touch positions for each finger
        switch (TouchLogic.currTouch)
        {
            case 0://first touch
                currTouch1 = Input.GetTouch(0).position;
                lastTouch1 = currTouch1 - Input.GetTouch(0).deltaPosition;
                break;
            case 1://second touch
                currTouch2 = Input.GetTouch(1).position;
                lastTouch2 = currTouch2 - Input.GetTouch(1).deltaPosition;
                break;
        }
        //finds the distance between your moved touches
        //we dont want to find the distance between 1 finger and nothing
        if (TouchLogic.currTouch >= 1)
        {
            currDist = Vector2.Distance(currTouch1, currTouch2);
            lastDist = Vector2.Distance(lastTouch1, lastTouch2);
        }
        else
        {
            currDist = 0.0f;
            lastDist = 0.0f;
        }
        //Calculate the zoom magnitude
        zoomFactor = Mathf.Clamp(currDist - lastDist, -20.0f, 20.0f);    //zoomFactor의 값의 범위를 지정

        zoomDist = Vector3.Distance(camTr.position, plantTr.position); //카메라와 오브젝트 사이의 거리값계산


        if (zoomDist <= maxZoomDist && zoomDist >= minZoomDist)
        {
            Camera.main.transform.Translate(Vector3.forward * zoomFactor * zoomSpeed * Time.deltaTime);  //- Vector3.forward * zoomFactor
        }
        else if (zoomDist > maxZoomDist) //가장 축소 됐을 때
        {
            if (zoomFactor > 0)  //확대만 가능
            {
                Camera.main.transform.Translate(Vector3.forward * zoomFactor * zoomSpeed * Time.deltaTime);
            }
        }
        else if (zoomDist < minZoomDist)   //가장 확대 됐을 때
        {
            if (zoomFactor < 0)  //축소만 가능
            {
                Camera.main.transform.Translate(Vector3.forward * zoomFactor * zoomSpeed * Time.deltaTime);
            }
        }

        //apply zoom to our camera
        //Camera.main.transform.Translate(Vector3.forward * zoomFactor * zoomSpeed * Time.deltaTime);
    }
}