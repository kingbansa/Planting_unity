using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InventorySceneChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void inventorytogps()
    {
        SceneManager.LoadScene("GPSScene");
    }

    public void inventorytoplantinfo()
    {
        SceneManager.LoadScene("PlantInfo");
    }

    public void inventorytosetplantsbedscene()
    {
        SceneManager.LoadScene("SetPlantsBedScene");
    }
}
