using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destory : MonoBehaviour {

    public GameObject prf;
	// Use this for initialization
	void Start () {
        Destroy(prf, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
