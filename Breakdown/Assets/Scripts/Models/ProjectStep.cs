using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[System.Serializable]
public class ProjectStep {
	public string stepId;
	public string projectId;
	public List<ProjectStepObject> projectObjects = new List<ProjectStepObject>();
	public string name;
	public float durationHours;
	public int crewSize;
	public float lead;
	public float lag;
	public List<ProjectStepObject> prerequisiteObjects = new List<ProjectStepObject>();
	public Texture2D image;

	public ProjectStep(string name, string projectId, List<ProjectStepObject> projectObjects, float durationHours, int crewSize, float leadTime, float lagTime, List<ProjectStepObject> prerequisiteObjects){
		this.name = name;
		this.projectId = projectId;
		this.projectObjects = new List<ProjectStepObject>(projectObjects);
		this.durationHours = durationHours;
		this.crewSize = crewSize;
		this.lead = leadTime;
		this.lag = lagTime;
		this.prerequisiteObjects =  new List<ProjectStepObject>(prerequisiteObjects);
	}

	public ProjectStep(JSONNode projectStep){
		this.projectId = projectStep["projectId"];

        JSONArray objs = projectStep["objects"].AsArray;
        for(int o = 0; o < objs.Count; o++) {
            this.projectObjects.Add(new ProjectStepObject(objs[o]["objectId"], objs[0]["activity"]));
        }
		this.crewSize = (int)projectStep["crewSize"];

		string durationString = projectStep["durationHours"];
		float durationFloat;
		float.TryParse (durationString, out durationFloat);
		this.durationHours = durationFloat;

        JSONArray prereqs = projectStep["prerequisiteObjects"].AsArray;
        for (int p = 0; p < prereqs.Count; p++) {
            this.prerequisiteObjects.Add(new ProjectStepObject(prereqs[p]["objectId"], prereqs[0]["activity"]));
        }
		
		this.stepId =  projectStep["id"];
	}
}
