using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class muhanupdate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

	// Update is called once per frame
	void Update () {
        
    }
}
