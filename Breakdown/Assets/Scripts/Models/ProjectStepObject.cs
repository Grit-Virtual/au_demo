using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectStepObject {
	public string objectId;
	public string activity;

	public ProjectStepObject(string objectId, string activity){
		this.objectId = objectId;
		this.activity = activity;
	}

	public ProjectStepObject(ProjectObject projectObject, string activity){
		this.objectId = projectObject.id;
		this.activity = activity;
	}

	public ProjectStepObject(Hashtable projectStepObject){
		this.objectId = projectStepObject ["objectId"].ToString ();
		this.activity = projectStepObject ["activity"].ToString ();
		ProjectObject projectObject = ProjectObjects.instance.GetObjectById (projectStepObject ["objectId"].ToString());
	}
}
