using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarManager : MonoBehaviour {

    public int Exp;
    public int CurrentLevel;

	// Use this for initialization
	void Start () {
        //loginScript.Instance.instanceCheckExp();
        Exp = loginScript.Exp;
        Debug.Log("Current EXP : " + Exp);
	}
	
	// Update is called once per frame
	void Update () {

	}

}
