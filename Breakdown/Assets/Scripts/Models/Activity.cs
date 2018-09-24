using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[System.Serializable]
public class Activity {
	public string name;
	public string activity;

	public Activity(JSONNode tc){
		this.name = tc["name"];
		this.activity = tc["id"];
	}
}
