using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasManager : MonoBehaviour {

    private PlantStats PS;
    public Text LevelText;
    public Text ExpText;
	// Use this for initialization
	void Start () {

        PS = GetComponent<PlantStats>();

	}
	
	// Update is called once per frame
	void Update () {

        LevelText.text = "Level: " + PS.CurrentLevel;
        ExpText.text = "Exp: " + PS.CurrentExp;

	}
}
