using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCastle : MonoBehaviour {

    public int Hp = 200;

    public GameObject My_Castle;

    private static MyCastle instance;
    public static MyCastle Instance
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

    public void DestoryCastle()
    {
        DestroyObject(My_Castle);
    }
    // Use this for initialization
    void Start () {
        My_Castle = GameObject.Find("MyCastle");
	}
	
	// Update is called once per frame
	void Update () {
		if(Hp == 0)
        {
            //loginScript.Instance.SendDestroyCastle();//이제 이부분에 loginSript 써야징!
        }
	}
}
