using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnchor : MonoBehaviour {

	public static MenuAnchor instance;

	public Transform camTransform;

	Quaternion targetRotation;

	void Awake(){
		instance = this;
	}

	void Update(){
		transform.position = camTransform.position;
		if (Quaternion.Angle (transform.rotation, targetRotation) > 0.1f) {
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, 0.035f);
		}
	}

	public void SetRotation(){
		transform.position = camTransform.position;
		targetRotation = Quaternion.Euler (new Vector3(0, camTransform.rotation.eulerAngles.y, 0));
	}
}
