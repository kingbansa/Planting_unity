using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStats : MonoBehaviour {

    public int CurrentLevel;
    public int CurrentExp;
    public int[] toLevelUp; //레벨별 필요 경험치

    private static PlantStats instance;
    public static PlantStats Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
     
        CurrentExp = loginScript.Exp;
	}
	
	// Update is called once per frame
	void Update () {

      
        CurrentExp = loginScript.Exp;

		if(CurrentExp  >= toLevelUp[CurrentLevel])
        {
            CurrentLevel = CurrentLevel + 1; //CurrentLevel++;
        }
	}
}
