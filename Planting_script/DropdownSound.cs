using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownSound : MonoBehaviour {

    public AudioClip dropDownChangeSound;
    AudioSource dropDownSound;
	// Use this for initialization
	void Start () {
        dropDownSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	public void PlaySound()
    {
        dropDownSound.PlayOneShot(dropDownChangeSound);
	}
}
