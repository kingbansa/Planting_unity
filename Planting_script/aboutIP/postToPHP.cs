using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using JsonFx.Json;
using UnityEngine.Networking;

public class postToPHP : MonoBehaviour {

    public string name;
    public string width;
    public string image;
    public Texture2D target;
    //string PostURL = "http://117.16.45.160/vuforia/PostNewTarget.php";
    string PostURL = "http://117.16.44.175/vuforia/PostNewTarget.php";
    //string PostURL = "http://117.16.44.175/vuforia/PostNewTarget.php";
    // Use this for initialization
    //192.168.1.12     "117.16.44.175"     PostURL = "http://localhost/vuforia/PostNewTarget.php";
    void Start()
    {
        
    }

    public void callPostImageInfo()
    {
        StartCoroutine(PostImageInfo());
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.L)) StartCoroutine(LoginToDB(inputUsername, inputPassword));
    //}

    IEnumerator PostImageInfo()
    {
        //PostURL = "http://localhost/vuforia/SampleSelector.php?select=PostNewTarget";
        //PostURL = "http://117.16.44.175/vuforia/PostNewTarget.php";
        //PostURL = "http://localhost/vuforia/testTarget.php";
        PostURL = "http://117.16.44.175/vuforia/PostNewTarget.php";
        //PostURL = "http://117.16.45.160/vuforia/PostNewTarget.php";

        name = "phpNewJTimage";
        width = "500.0";
        //image = Convert.ToBase64String(target.EncodeToJPG());
        //이거 다 버리고 ID를 받아와서 메타데이터로 넘기고, 파일저장경로받아와서 넘겨서 클라우드에 업로드하자

        WWWForm form = new WWWForm();
        //form.AddField("namePost", name);
        //form.AddField("widthPost", width);
        //form.AddField("imageByte", image);
        form.AddField("ImageJsonData", image);
        form.AddField("userNamePost", loginScript.userName);

        //form.AddField("passwordPost", password);


        WWW www = new WWW(PostURL, form);
        

        //UnityWebRequest UWR = UnityWebRequest.Post(PostURL, form);
        //yield return UWR.Send();

        yield return www;

        Debug.Log(www.text); //Login.php echo를 호출하는거
    }
}
