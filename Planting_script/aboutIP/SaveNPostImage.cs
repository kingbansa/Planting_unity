using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;
using UnityEngine.UI;

public class SaveNPostImage : MonoBehaviour
{

    public ScreenCapture sCapture;
    public postToPHP phpRequest;
    public UDTEventHandler mUDTeventHandler;
    public SingleTonCaptureBtn mSingleTonCaptureBtn;
    public Text qualityText;

    //public loginScript loginSC;
    //public SingleTonCaptureBtn stBtn;


    public void callSaveNPostImage()
    {
        if (mUDTeventHandler.mFrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_MEDIUM || mUDTeventHandler.mFrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_HIGH)
        {
            StartCoroutine(waitForSave(2.0f)); //사진 저장될때까지 기다려야돼
        }

    }

    IEnumerator waitForSave(float waitTime)
    {
        sCapture.StartScreenCapture(); //화면 캡처 시작
        Debug.Log("what is screenshoturl" + sCapture.screenShotUrl + "\n");
        yield return new WaitForSeconds(waitTime); // 2초 기다림

        loginScript.Instance.CaptureBtn();  //로그인 스크립트로 url보냄
        Debug.Log("시간멈추니1" + Time.time);

        phpRequest.image = Convert.ToBase64String(sCapture.bytes); //이미지 정보 저장하고
        yield return new WaitForSeconds(waitTime);  // 2초 기다림

        Debug.Log("시간멈추니2" + Time.time);
        phpRequest.callPostImageInfo(); // php로 이미지 전송
        yield return new WaitForSeconds(waitTime);  // 2초 기다림
        Debug.Log("시간멈추니3" + Time.time);
        
        SceneManager.LoadScene("PlantInfo");
    }

    void Awake()
    {
        sCapture = GetComponent<ScreenCapture>();
        phpRequest = GetComponent<postToPHP>();
        mUDTeventHandler = GetComponent<UDTEventHandler>();
        mSingleTonCaptureBtn = GetComponent<SingleTonCaptureBtn>(); ;
        //loginSC = GetComponent<loginScript>();  
        //loginScript.Instance.onConnect();
    }



    // Use this for initialization
    void Start()
    {
        qualityText.text ="" + mUDTeventHandler.mFrameQuality;
    }

    // Update is called once per frame
    void Update()
    {
        qualityText.text = mUDTeventHandler.mFrameQuality.ToString();
    }
}
