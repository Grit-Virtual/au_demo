using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInitialTransform : MonoBehaviour {

    public Vector3 rotation;
    public Vector3 location;
	public Vector3 scale = new Vector3(1f, 1f, 1f);

    // Use this for initialization
    void Start () {
        this.transform.localRotation = Quaternion.Euler(rotation);
        this.transform.localPosition = location;
		this.transform.localScale = scale;
	}
}
