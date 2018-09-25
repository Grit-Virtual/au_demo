using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanMenuManager : MonoBehaviour {

	public static PlanMenuManager instance;

	public bool planning;

	public enum PlannerForm {
		CREW_SIZE,
		DURATION,
		LEAD_LAG,
		SUBMIT		
	}

	public PlannerForm currentForm;

	void Awake(){
		instance = this;
	}

	public void ShowPlanMenu(){
		currentForm = PlannerForm.CREW_SIZE;
		ObjectSelection.instance.EndSelectingPrereqs();
		ShowCurrentPlannerForm();
	}

	void ShowCurrentPlannerForm(){
		switch(currentForm){
			case PlannerForm.CREW_SIZE:
				CrewSizeMenu.instance.ShowCrewSizeMenu();
				break;
			case PlannerForm.DURATION:
				DurationMenu.instance.ShowDurationMenu();
				break;
			case PlannerForm.LEAD_LAG:
				LeadLagMenu.instance.ShowLeadLagMenu();
				break;
			case PlannerForm.SUBMIT:
				PlannerSubmitMenu.instance.ShowSubmitMenu();
				break;
		}
	}

	public void NextPlannerMenu(){
		switch(currentForm){
			case PlannerForm.CREW_SIZE:
				CrewSizeMenu.instance.NextMenu();
				break;
			case PlannerForm.DURATION:
				DurationMenu.instance.NextMenu();
				break;
			case PlannerForm.LEAD_LAG:
				LeadLagMenu.instance.NextMenu();
				break;
		}
		currentForm = (PlannerForm)((int)currentForm + 1);
		ShowCurrentPlannerForm();
	}

	public void HidePlannerMenu(){
		CrewSizeMenu.instance.HideCrewSizeMenu();
		DurationMenu.instance.HideDurationMenu();
		LeadLagMenu.instance.HideLeadLagMenu();
		PlannerSubmitMenu.instance.HideSubmitMenu();
		ResetMenu();
		currentForm = PlannerForm.CREW_SIZE;
	}

	public void ResetMenu(){
		CrewSizeMenu.instance.ResetForm();
		DurationMenu.instance.ResetForm();
		LeadLagMenu.instance.ResetForm();
	}

	public void SendProjectStep(){
        ProjectStep projectStep = new ProjectStep(
            PlannerSubmitMenu.instance.GetNameInput(),
            Utils_Prefs.GetProjectId(),
            CreateStepObjects(ObjectSelection.instance.GetSelectedObjects()),
            DurationMenu.instance.GetDurationInput(),
            CrewSizeMenu.instance.GetCrewSizeInput(),
            LeadLagMenu.instance.GetLeadInput(),
            LeadLagMenu.instance.GetLagInput(),
            CreateStepObjects(ObjectSelection.instance.GetPrerequisiteObjects(), true)
		);
        
		Menu.instance.DeselectButtonClick();
    }

	List<ProjectStepObject> CreateStepObjects(List<ProjectObject> projectObjects, bool prereq = false){
		List<string> addedIds = new List<string> ();
		List<ProjectStepObject> projectStepObjects = new List<ProjectStepObject> ();
		for(int i = 0; i < projectObjects.Count; i++) {
			if (!addedIds.Contains (projectObjects[i].id)) {
				if (!prereq) {
					projectStepObjects.Add (new ProjectStepObject (projectObjects [i].id, ActivityMenu.instance.GetSelectedActivity().activity));
				} else {
					projectStepObjects.Add (new ProjectStepObject(projectObjects [i].id, ObjectSelection.instance.prereqActivities[i]));
				}
				addedIds.Add (projectObjects [i].id);
			}
		}
		return projectStepObjects;
	}

	public void BackButtonPress(){
		ObjectSelection.instance.EndSelectingPrereqs();
		planning = false;
		ResetMenu();
	}

	public void PlanButtonClick(){
		if(!planning){
			planning = true;
			if(ActivityMenu.instance.ActivitySelectionRequired()){
				ActivityMenu.instance.InitializeActivityMenu();
			}
		} else {
			planning = false;
			Menu.instance.DeselectButtonClick();
		}
	}
}
