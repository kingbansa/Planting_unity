using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlantOnPanel : MonoBehaviour
{
    public GameObject Panel;
    public GameObject[] gameobjects = new GameObject[4];


    void Awake()
    {
        Panel = GameObject.Find("Select_Panel");
    }

    IEnumerator SearchName()
    {
        yield return new WaitForSeconds(0.3f);
        gameobjects[0] = Resources.Load("Prefabs/" + loginScript.Instance.name1 + "_Btn_Select") as GameObject;
        gameobjects[1] = Resources.Load("Prefabs/" + loginScript.Instance.name2 + "_Btn_Select") as GameObject;
        gameobjects[2] = Resources.Load("Prefabs/" + loginScript.Instance.name3 + "_Btn_Select") as GameObject;
        gameobjects[3] = Resources.Load("Prefabs/" + loginScript.Instance.name4 + "_Btn_Select") as GameObject;

        Debug.Log("" + gameobjects[0].ToString().Substring(0,16) + " " + gameobjects[1].ToString().Substring(0, 16));
        for (int i = 0; gameobjects.Length > i; i++)
        {
            if (loginScript.Instance.name1 == gameobjects[i].ToString().Substring(0, 16))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.Instance.name2 == gameobjects[i].ToString().Substring(0, 16))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.Instance.name3 == gameobjects[i].ToString().Substring(0, 16))
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else if (loginScript.Instance.name4 == gameobjects[i].ToString().Substring(0, 16)) //이것도 나중에 바꿔줘야한다.
            {
                GameObject a = (GameObject)Instantiate(gameobjects[i]);
                a.transform.SetParent(Panel.transform, false);
            }
            else
            {
                Debug.Log("BYEBYEBYEBYEBYEBYEBYE "+ gameobjects.Length + " 1 " + loginScript.Instance.name1 + " 2 " + loginScript.Instance.name2 + " 3 " +loginScript.Instance.name3 + " 4 " + loginScript.Instance.name4);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SearchName());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
