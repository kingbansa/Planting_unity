using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCastle : MonoBehaviour {

    public int Hp = 200;

    public GameObject Enemy_Castle;

    private static EnemyCastle instance;
    public static EnemyCastle Instance
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
        DestroyObject(Enemy_Castle);
    }
    // Use this for initialization
    void Start () {
        Enemy_Castle = GameObject.Find("EnemyCastle");
	}
	
	// Update is called once per frame
	void Update () {
		if(Hp == 0)
        {
            //loginScript.Instance.SendDestroyCastle();//이제 이부분에 loginSript 써야징!
        }
    }
}
