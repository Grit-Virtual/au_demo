using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectSteps : MonoBehaviour {

	public static ProjectSteps instance;

	public List<ProjectStep> projectSteps = new List<ProjectStep>();

	public ProjectStep lastScheduledStep;

	// Use this for initialization
	void Awake () {
		instance = this;
	}

	public void AddProjectStep(ProjectStep step, bool canUndo = false){
		projectSteps.Add (step);
		if (canUndo) {
			UndoMenu.instance.AddUndoStep(step);
		}
	}

	public void ClearProjectSteps(){
		projectSteps.Clear ();
	}

	public void AddScheduledActivitiesToObjects(){
		foreach (ProjectStep projectStep in projectSteps) {
			foreach (ProjectStepObject projectStepObject in projectStep.projectObjects) {
				ProjectObjects.instance.GetObjectById(projectStepObject.objectId).AddScheduledActivity(projectStepObject.activity);
			}
		}

    }

	public bool IsObjectScheduled(ProjectObject projectObject){
		if (projectObject.activities.Count > 0) {
			return projectObject.scheduledActivities.Count >= projectObject.activities.Count;
		} else {
			return false;
		}
	}

	public void FadeScheduledObjects(ProjectStep projectStep){
		foreach (ProjectStepObject projectStepObject in projectStep.projectObjects) {
			ProjectObjects.instance.GetObjectById(projectStepObject.objectId).AddScheduledActivity (projectStepObject.activity);
		}
	}

	public void UnFadeScheduledObjects(){
		if (lastScheduledStep != null) {
			foreach (ProjectStepObject projectStepObject in lastScheduledStep.projectObjects) {
				ProjectObjects.instance.GetObjectById(projectStepObject.objectId).RemoveScheduledActivity (projectStepObject.activity);
			}
			lastScheduledStep = null;
		}
	}
}
