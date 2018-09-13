using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject prf; 

    void Start()
    {
        Destroy(prf,3f);
    }
    
    
}



