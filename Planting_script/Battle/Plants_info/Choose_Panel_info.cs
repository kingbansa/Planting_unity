using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choose_Panel_info : MonoBehaviour
{
    public GameObject Panel;
    public GameObject[] gameobjects = new GameObject[12];
    public string[] gameobjectname = new string[12];
    
    void Awake()
    {
        Panel = GameObject.Find("Choose_Panel");
    }

    IEnumerator SearchName()
    { 
        yield return new WaitForSeconds(0.3f);

        for(int i = 0; loginScript.plantName.Count > i; i++)
        {
            gameobjectname[i] = loginScript.plantName[i].ToString();
        }

        for(int i = 0; gameobjects.Length > i; i++)
        {
            gameobjects[i] = Resources.Load("Prefabs/" + gameobjectname[i] + "_Btn") as GameObject;
        }

        for (int i = 0; gameobjects.Length > i; i++)
        {
            if (loginScript.plantName.Contains("PoisonMushroom_B"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("PoisonMushroom_R"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("Baby plant"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("BlowFishORCactus"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("Cosmos"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("DeliciousMushRoom"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("GreenbristleGrass"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("JustBamboo"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("NotTreeButRock"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("PoisonMushroom_G"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("PurpleFlower"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("ShiningFlower"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("UnknownOrangeColorFlower"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("UselessRock"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("Weed"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("Yellow Flower"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.plantName.Contains("YouCantSeeMyFaceFlower"))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(SearchName());
    }
	
	// Update is called once per frame
	void Update () {

    }
}
