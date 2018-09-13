using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

    public AudioSource loginScriptBGM;

	// Use this for initialization
	void Start () {
        loginScriptBGM = GetComponent<AudioSource>();
        loginScriptBGM.volume = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
