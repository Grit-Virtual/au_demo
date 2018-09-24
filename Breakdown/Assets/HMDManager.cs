using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HMDManager : MonoBehaviour {

	public GameObject[] gameObjectsToDeactivateWhenTheApplicationIsPaused;
	public static bool paused;

	void Update () {
        if(Input.GetKeyDown(KeyCode.P)){
			if(!paused){
	            HandleHMDUnmounted();
			} else {
				HandleHMDMounted();
			}
        }
		Application.runInBackground = false;
    }

	void OnEnable () {
		OVRManager.HMDUnmounted += HandleHMDUnmounted;
		OVRManager.HMDMounted += HandleHMDMounted;
	}
	
	void OnDisable () {
		OVRManager.HMDUnmounted -= HandleHMDUnmounted;
		OVRManager.HMDMounted -= HandleHMDMounted;
	}

	void HandleHMDUnmounted(){
		paused = true;
		foreach(GameObject go in gameObjectsToDeactivateWhenTheApplicationIsPaused){
			go.SetActive(false);
		}
	}

	void HandleHMDMounted(){
		paused = false;
		foreach(GameObject go in gameObjectsToDeactivateWhenTheApplicationIsPaused){
			go.SetActive(true);
		}
	}
}
