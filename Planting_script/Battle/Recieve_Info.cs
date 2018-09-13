using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recieve_Info : MonoBehaviour {


    public List<string> plantName = new List<string>();
    public List<int> plantLv = new List<int>();
    public List<int> plantID = new List<int>();


    public void ClearList()
    {
        loginScript.plantName.Clear();
        loginScript.Lv.Clear();
        loginScript.plantID.Clear();

        plantID.Clear();
        plantName.Clear();
        plantLv.Clear();
    }

    public void CallRenewPlantList()
    {
        StartCoroutine(RenewPlantList());
    }

    IEnumerator RenewPlantList()
    {
        ClearList();

        yield return new WaitForSeconds(0.2f);

        loginScript.Instance.SelectQuery("*", "PlantList_");

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i <= loginScript.plantPos.Count - 1; i++)
        {
            plantName.Add(loginScript.plantName[i]);
            plantID.Add(loginScript.plantID[i]);
            plantLv.Add(loginScript.Lv[i]);
        }

    }
    // Use this for initialization
	
    void Awake()
    {
        CallRenewPlantList();
    }

	// Update is called once per frame
	void Update () {

	}
}
