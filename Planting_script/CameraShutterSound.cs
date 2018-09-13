using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShutterSound : MonoBehaviour {

    public AudioClip cameraShutter;
    AudioSource cameraSound;


    void Start () {
        cameraSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	public void PlaySound () {
        cameraSound.PlayOneShot(cameraShutter);
	}
}
