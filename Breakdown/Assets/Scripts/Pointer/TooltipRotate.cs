using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipRotate : MonoBehaviour {

	void OnEnable(){
		transform.LookAt(Camera.main.transform);
		transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z  - 175f));
	}

	void Update () {
		transform.LookAt(Camera.main.transform);
		transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z  - 175f));
	}
}
