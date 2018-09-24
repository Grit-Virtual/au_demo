using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ControllerSelection;

public class SwipeControl : MonoBehaviour {

	Vector2 swipeStart, swipeEnd;
	bool swiping;
	float swipeTimer;
	public float swipeTime;
	public float swipeThreshold;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!HMDManager.paused){
			if(OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)){
				if(!swiping){
					swipeStart = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
					swiping = true;
				}
			} else {
				if(swiping){
					swiping = false;
					ActOnSwipe();
				}
			}

			if(swiping){
				swipeEnd = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
				swipeTimer += Time.deltaTime;
			} else {
				swipeTimer = 0f;
			}
		}
	}
	
	void ActOnSwipe(){
		if(swipeTimer < swipeTime){
			if(swipeEnd.x - swipeStart.x > swipeThreshold){
				Controller.instance.Rotate(false);
			} else if(swipeEnd.x - swipeStart.x < swipeThreshold * -1){
				Controller.instance.Rotate(true);
			} else if(swipeEnd.y - swipeStart.y > swipeThreshold){
				Controller.instance.Elevate(true);
			} else if(swipeEnd.y - swipeStart.y < swipeThreshold * -1){
				Controller.instance.Elevate(false);
			}
		}
	}
}
