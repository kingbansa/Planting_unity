 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]  
public class Item {
    public string itemName;
    public int itemAmount;
    public GameObject itemSlot;
    public Text itemAmountT;


    


    public Item(string iName, int iAmount, GameObject islot, Text iAmountT)
    {
        iName = itemName;
        iAmount = itemAmount;
        islot = itemSlot;
        iAmountT.text = "" + itemAmountT;
    }
     
}

