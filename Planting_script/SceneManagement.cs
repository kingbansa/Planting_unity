using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class SceneManagement : MonoBehaviour {

    public float Time = 0;
    public GameObject Manager;
    Stopwatch Sw = new Stopwatch();

    public void SendTime(float time, string scenename)
    {
        loginScript.Instance.SendTime(time, scenename);
        Debug.Log("@@@@@@@@@@@@@@@@@TIME@@@@@@@@@@@@@@@@@" + time +" "+scenename);
    }

    void Awake()
    {
        Manager = GameObject.Find("SceneManger");
        Sw.Reset();
    }

    // Use this for initialization
    void Start ()
    {
        Sw.Start();
	}

	// Update is called once per frame
	void Update ()
    {

    }

    public void callGPSScene()
    {
        Sw.Stop();
        Time = Sw.ElapsedMilliseconds;
        SendTime(Time, this.gameObject.name.ToString());
        StartCoroutine(Wait());
        SceneManager.LoadScene("GPSScene");
    }

    public void callPlantInfoScene()
    {
        Sw.Stop();
        Time = Sw.ElapsedMilliseconds;
        SendTime(Time, this.gameObject.name.ToString());
        StartCoroutine(Wait());
        SceneManager.LoadScene("PlantInfo");
    }

    public void callSetPlantBedScene()
    {
        Sw.Stop();
        Time = Sw.ElapsedMilliseconds;
        SendTime(Time, this.gameObject.name.ToString());
        StartCoroutine(Wait());
        SceneManager.LoadScene("SetPlantsBedScene");
    }

    public void callLobbyScene()
    {
        Sw.Stop();
        Time = Sw.ElapsedMilliseconds;
        SendTime(Time, this.gameObject.name.ToString());
        StartCoroutine(Wait());
        SceneManager.LoadScene("LobbyScene");
    }

    public void callPlantInfoModelB()
    {
        Sw.Stop();
        Time = Sw.ElapsedMilliseconds;
        SendTime(Time, this.gameObject.name.ToString());
        StartCoroutine(Wait());
        SceneManager.LoadScene("PlantInfoModelB");
    }

    IEnumerator Wait()
    {
        loginScript.Instance.AllDeletePlantName();
        yield return new WaitForSeconds(0.5f);
    }
}
