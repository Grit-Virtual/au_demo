using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircularObject {
	public string activity;
	public List<ProjectStepObject> projectStepObjects = new List<ProjectStepObject>();

	public CircularObject(string activity, ArrayList circularObject){
		this.activity = activity;
		foreach (Hashtable circular in circularObject) {
			projectStepObjects.Add (new ProjectStepObject (circular["objectId"].ToString(), circular["activity"].ToString()));
		}
	}
}
