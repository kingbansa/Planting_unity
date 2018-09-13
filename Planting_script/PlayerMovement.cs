using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    float Speed = 10;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-1 * Speed * Time.deltaTime, 0, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, 1 * Speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -1 * Speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(1 * Speed * Time.deltaTime, 0, 0, Space.World);
        }

    }
}
