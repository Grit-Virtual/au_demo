using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenFollow : MonoBehaviour {

	public Transform cam;
	Quaternion targetRotation;
	bool rotating;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = cam.position;
		if(Vector3.Angle(transform.forward, cam.forward) > 45f){
			targetRotation = cam.rotation;
			rotating = true;
		}
		if(rotating){
			if(Vector3.Angle(transform.forward, cam.forward) > 1f){
				transform.rotation = Quaternion.Lerp(transform.rotation, cam.rotation, Time.deltaTime * 3f);
			} else {
				rotating = false;
			}
		}
	}
}
