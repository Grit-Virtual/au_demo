using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camtest : MonoBehaviour {
	public Transform t;
	
	void Update () {
		transform.position = t.position;
		transform.rotation = t.rotation;
	}
}
