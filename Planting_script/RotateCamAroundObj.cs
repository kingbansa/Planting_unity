using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateCamAroundObj : TouchLogic
{
    public GameObject plantBedObj;
    private Transform plantBedTr;

    private float rotateSpeed = 12.0f;
    private float rotateFactor = 0.0f;

    private Vector2 currTouch1 = Vector2.zero;
    private Vector2 lastTouch1 = Vector2.zero;

    void Start () {
        plantBedTr = plantBedObj.GetComponent<Transform>();
	}

    public override void OnTouchMovedAnywhere()
    {
        if (Input.touchCount == 1)
        {
            RotateCam();
        }
    }

    private void RotateCam()
    {
        switch (TouchLogic.currTouch)
        {
            case 0://first touch
                //currTouch1 = Input.GetTouch(0).position;
                lastTouch1 = Input.GetTouch(0).deltaPosition;
                //lastTouch1 =Input.GetTouch(0).deltaPosition;
                break;
        }
        rotateFactor = lastTouch1.x;
        Camera.main.transform.RotateAround(plantBedTr.position, Vector3.up, rotateFactor * rotateSpeed * Time.deltaTime );


    }

    //void OnMouseDrag()
    //{
    //    float rotY = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
    //    transform.RotateAround(plantBedTr.position, Vector3.right, rotY);
    //    //transform.Rotate(Vector3.up, -rotY);   //plantBedTr.rotate()에서 바꾸기
    //}
}
