using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectProperty {
	public string label;
	public string value;

	public ObjectProperty(string label, string value){
		this.label = label;
		this.value = value;
	}
}
