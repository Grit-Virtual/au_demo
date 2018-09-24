using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Project {
	public string name;
	public string account;
	public string id;
	public string address;

	public Project(Hashtable project){
		this.name = project ["projectName"].ToString();
		this.account = project ["accountName"].ToString();
		this.id = project ["id"].ToString();
		this.address = project ["address"] != null ? project ["address"].ToString () : "";	
	}
}
