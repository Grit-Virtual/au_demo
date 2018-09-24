using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autodesk.Forge.ARKit;

public class Projects : MonoBehaviour {

	public static Projects instance;

	public Project project;
	public List<ProjectModel> projectModels = new List<ProjectModel> ();
	public List<string> modelIds = new List<string>();

	void Awake () {
		instance = this;
	}

	public void SetProject(Project project){
		this.project = project;
	}
}
