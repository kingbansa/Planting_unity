using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS_Test : MonoBehaviour
{
	// GPS
	private bool gpsInit = false;
	private LocationInfo currentGPSPosition;
	private string logtxt;

	void Start ()
	{
		Input.location.Start (0.5f);
		int wait = 1000;
		if (Input.location.isEnabledByUser) {
			while (Input.location.status == LocationServiceStatus.Initializing && wait > 0) {
				wait--;
			}
			if (Input.location.status == LocationServiceStatus.Failed) {
			} else {
				gpsInit = true;
				InvokeRepeating ("RetrieveGPSData", 0f, 3f);
			}
		} else {
			logtxt = "GPS is not availabel";
		}
	}

	void RetrieveGPSData ()
	{
		currentGPSPosition = Input.location.lastData;
		string gpsString = "::latitude:" + currentGPSPosition.latitude + "//longitude" + currentGPSPosition.longitude + "//altitude:" + currentGPSPosition.altitude;
		logtxt = gpsString;
	}
	// Update is called once per frame
	void Update ()
	{
		
	}
}
