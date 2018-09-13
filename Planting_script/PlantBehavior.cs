using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : MonoBehaviour {
    //모든 식물 오브젝트 프리팹에 적용시켜주어야하며, 각 식물마다 알맞은 enum값으로 설정해주어야함, 그래서 plantNameEnum을 이넘으로 설정해준것
    public CloudRecoTrackableEventHandler _cloudRecoTrackableEventHandler;
    public PlantNameEnum plantNameEnum;

    //식물오브젝트 고유의 이넘값을 정수형으로 반환
    public int GetPlantNameEnum()
    {
        return (int)plantNameEnum;
    }

    // Use this for initialization
    void Start () {
        _cloudRecoTrackableEventHandler = new CloudRecoTrackableEventHandler();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
