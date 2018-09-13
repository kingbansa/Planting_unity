using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenCapture : MonoBehaviour
{
    int height = Screen.height;
    int width = Screen.width;

    Texture2D captureTexture;
    float startTime;
    string saveDir = "PlantsBed"; // 저장 폴더 이름
    public string screenShotUrl = ""; // 파일 url
    public byte[] bytes;

    public bool draw = false;
    public static string userID;

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 50, 100, 50), "Capture"))
    //    {
    //        StartCoroutine(screenCapture());
    //    }
    //}
     void Start()
    {
        Screen.SetResolution(918, 1632, true);
    }

    IEnumerator screenCapture()
    {
        userID = loginScript.userName;
        yield return new WaitForEndOfFrame();
        captureTexture = new Texture2D(width, height, TextureFormat.RGB24, true); // texture 세팅
        captureTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0, true); // 화면을 픽셀로 읽기
        captureTexture.Apply(); // 읽은 픽셀 저장

        bytes = captureTexture.EncodeToPNG(); //PNG형식으로 바꾸기

        screenShotUrl = getCaptureName();

        //loginScript.url = screenShotUrl; //loginScript의 url변수에 파일 경로 저장

        File.WriteAllBytes(screenShotUrl, bytes);
        Debug.Log(string.Format("Capture Success: {0}", getCaptureName()));

    }
    public void StartScreenCapture()
    {
        StartCoroutine("screenCapture");
    }



    public string getCaptureName() //파일 이름
    {
        ScreenCapture.userID = loginScript.userName;
        //File file = new File(getFileDir(), "ARtest");
        //여기서 파일저장경로를 postToPHP로주고 저장경로 PostNewTarget으로 보낸다

        //Environment.getExternalStorageDirectory().getAbsolutePath();
        // / data /    내 PC\Galaxy Note5\Phone\DCIM\Screenshots
        //string dirPath = Application.dataPath + "/" + saveDir;  //노트북에서 할 때
        string dirPath = "mnt/sdcard/DCIM/" + saveDir;  //폰에 저장 할 때
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        //return string.Format("{0}/capture_{1}.png", dirPath, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        return string.Format("{0}/{1}.png", dirPath, userID);  //userID가되는지확인하기


    }

}
//Environment.getExternalStorageDirectory().getAbsolutePath()
