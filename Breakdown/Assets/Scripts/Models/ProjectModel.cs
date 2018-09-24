using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectModel {
	public string id;
	public string urn;
	public string sceneId;
	public byte[] data;

	public ProjectModel(string id, string urn){
		this.id = id;
		this.urn = urn;
		this.sceneId = id;	
	}
}
