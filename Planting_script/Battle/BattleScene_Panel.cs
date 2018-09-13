using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene_Panel : MonoBehaviour {

    public GameObject HandPanel;
    public GameObject[] gameobjects = new GameObject[4];
    public string[] objectnames = new string[4];

    /*
        gameobjects[0] = Resources.Load("Prefabs/" + loginScript.Instance.name1 + "_Btn_Select") as GameObject;
        gameobjects[1] = Resources.Load("Prefabs/" + loginScript.Instance.name2 + "_Btn_Select") as GameObject;
        gameobjects[2] = Resources.Load("Prefabs/" + loginScript.Instance.name3 + "_Btn_Select") as GameObject;
        gameobjects[3] = Resources.Load("Prefabs/" + loginScript.Instance.name4 + "_Btn_Select") as GameObject;
     */
    IEnumerator GetPlantName()
    {
        yield return new WaitForSeconds(1.0f);
        objectnames[0] = loginScript.Instance.name1;
        objectnames[1] = loginScript.Instance.name2;
        objectnames[2] = loginScript.Instance.name3;
        objectnames[3] = loginScript.Instance.name4;

        for(int i = 0; objectnames.Length > i; i++)
        {
            gameobjects[i] = Resources.Load("Prefabs/BattleScenePrefabs/" + objectnames[i] + "") as GameObject;
            GameObject a = (GameObject)Instantiate(gameobjects[i]);
            a.transform.SetParent(HandPanel.transform, false);
        }
    }

    void Awake()
    {
        HandPanel = GameObject.Find("HandPanel");
    }
	// Use this for initialization
	void Start () {
        StartCoroutine(GetPlantName());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
