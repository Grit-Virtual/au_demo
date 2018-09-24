using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ControllerSelection;
using TMPro;

public class PlanMenu : MonoBehaviour {

	// public static PlanMenu instance;

	// public GameObject planMenu;
	// public Animator animator;
	// public GameObject defaultForm;
	// public GameObject overrideForm;

	// public RadioButtonUI[] crewRadioButtons;
	// public RadioButtonUI[] hourRadioButtons;
	// public RadioButtonUI[] minRadioButtons;

	// public TextMeshProUGUI selectedActivityText;

	// int crewSize = 0;
	// float durationHours = 0;
	// float durationMins = 0;
	// float leadTime = 0;
	// float lagTime = 0;

	// public DefaultInput nameInput;
	// public PlannerFormNumberInput crewSizeTextInput;
	// public PlannerFormNumberInput durationTextInput;
	// public PlannerFormNumberInput leadTimeTextInput;
	// public PlannerFormNumberInput lagTimeTextInput;


	// ProjectStep lastScheduledStep;
	// public GameObject selectingPrereqsIndicator;
	// bool selectingPrereqs;

	// void Awake () {
	// 	instance = this;
	// }

	// void Start () {
	// 	// planMenu.SetActive (false);
	// 	// animator.SetInteger("State", 0);
	// }

	// void Update(){
	// 	if (Controller.instance.BackButtonPress()) {
	// 		if (selectingPrereqs) {
	// 			EndSelectingPrereqs();
	// 		} else {
	// 			if (animator.GetBool("MenuShowing")) {
	// 				BackButtonPress();
	// 			}
	// 		}
    //     }
    // }

	// public void PlanButtonClick(){
	// 	if(planButton.interactable){
	// 		if(planMenu.activeInHierarchy){
	// 			HidePlanMenu();
	// 		} else {
	// 			ShowPlanMenu();
	// 		}
	// 	}
	// }

	// public void ShowPlanMenu(){
	// 	// animator.SetInteger("State", 1);
    //     // planMenu.SetActive (true);
	// 	// if(!planButton.active){
	// 	// 	planButton.SetActive(true);
	// 	// }
	// 	// if(selectingPrereqs){
	// 	// 	EndSelectingPrereqs();
	// 	// }
	// }

	// public void HidePlanMenu(){
		// animator.SetInteger("State", 0);
		// planMenu.SetActive (false);
		// if(planButton.active){
		// 	planButton.SetActive(false);
		// }
	// }

	// public void BackButtonPress() {
		// EndSelectingPrereqs();
    // }

    // public void ShowDefaultForm(){
	// 	defaultForm.SetActive (true);
	// 	overrideForm.SetActive (false);
	// }

	// public void ShowOverrideForm(){
	// 	defaultForm.SetActive (false);
	// 	overrideForm.SetActive (true);
	// }

	// public void SelectCrewSize(int size) {
	// 	crewSize = size;
	// 	crewSizeTextInput.ReceiveInputFromForm (size.ToString());
	// }

	// public void OverrideInputCrewSize(string input){
	// 	crewSize = int.Parse (input);
	// 	if (crewSize >= 1 && crewSize <= 4) {
	// 		if (!crewRadioButtons [crewSize - 1].active) {
	// 			crewRadioButtons [crewSize - 1].Click ();
	// 		}
	// 	} else {
	// 		foreach (RadioButtonUI crewButton in crewRadioButtons) {
	// 			crewButton.SetActive (false);
	// 		}
	// 	}
	// }

	// public void SelectHour(int hour) {
	// 	if (!hourRadioButtons [hour - 1].active) {
	// 		durationHours = hour;
	// 	} else {
	// 		durationHours = 0;
	// 	}
	// 	durationTextInput.ReceiveInputFromForm ((durationHours + durationMins).ToString ());
	// }

	// public void OverrideInputHour(float input){
	// 	durationHours = input;
	// 	if (durationHours >= 1 && durationHours <= 4) {
	// 		if (!hourRadioButtons [(int)durationHours - 1].active) {
	// 			hourRadioButtons [(int)durationHours - 1].Click ();
	// 		}
	// 	} else {
	// 		foreach (RadioButtonUI hourButton in hourRadioButtons) {
	// 			hourButton.SetActive (false);
	// 		}
	// 	}
	// }

	// public void SelectMin(float min) {
	// 	int index = GetMinButtonIndex (min / 100);
	// 	if (index != -1) {
	// 		if (!minRadioButtons [index].active) {
	// 			durationMins = min / 100;
	// 		} else {
	// 			durationMins = 0;
	// 		}
	// 	}
	// 	durationTextInput.ReceiveInputFromForm ((durationHours + durationMins).ToString());
	// }

	// public void OverrideInputMin(float input){
	// 	durationMins = RoundMinInput(input);
	// 	if (durationMins >= 1) {
	// 		OverrideInputHour (durationHours + 1);
	// 		durationMins = 0;
	// 		foreach (RadioButtonUI minButton in minRadioButtons) {
	// 			minButton.SetActive (false);
	// 		}
	// 	} else if(durationMins <= 0){
	// 		foreach (RadioButtonUI minButton in minRadioButtons) {
	// 			minButton.SetActive (false);
	// 		}
	// 	} else {
	// 		int index = 0;
	// 		if (durationMins == 0.5f) {
	// 			index = 1;
	// 		} else if(durationMins == 0.75f){
	// 			index = 2;
	// 		}
	// 		if (!minRadioButtons [index].active) {
	// 			minRadioButtons [index].Click ();
	// 		}
	// 	}
	// 	durationTextInput.ReceiveInputFromForm ((durationHours + durationMins).ToString());
	// }

	// float RoundMinInput(float input){
	// 	if (input <= 0) {
	// 		return 0;
	// 	} else if (input <= 0.25f) {
	// 		return 0.25f;
	// 	} else if (input <= 0.5f) {
	// 		return 0.5f;
	// 	} else if (input <= 0.75f) {
	// 		return 0.75f;
	// 	} else {
	// 		return 1;
	// 	}
	// }

	// int GetMinButtonIndex(float mins){
	// 	if (mins == 0.25f) {
	// 		return 0;
	// 	} else if (mins == 0.5f) {
	// 		return 1;
	// 	} else if (mins == 0.75f) {
	// 		return 2;
	// 	} else {
	// 		return -1;
	// 	}
	// }

	// public void SelectLeadTime(float lead) {
	// 	leadTime = lead;
	// }

	// public void selectLagTime(float lag) {
	// 	lagTime = lag;
	// }

	// public void StartSelectingPrereqs(){
	// 	ObjectSelection.instance.selectingPrereqs = true;
	// 	selectingPrereqs = true;
	// 	MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.NONE);
	// 	// planMenu.SetActive (false);
	// 	Raycaster.instance.ApplyPrereqMask();
	// 	selectingPrereqsIndicator.SetActive (true);
	// 	FilterMenu.instance.RevertToFilter ();
	// }

	// void EndSelectingPrereqs(){
	// 	ObjectSelection.instance.selectingPrereqs = false;
	// 	selectingPrereqs = false;
	// 	Raycaster.instance.ApplySelectionMask();
	// 	selectingPrereqsIndicator.SetActive (false);
	// }

	// MOVE TO OBJECT SELECTION
	public void PrereqClearButtonClick(){
		ObjectSelection.instance.UnHighlightPrereqs ();
		ObjectSelection.instance.DeselectPrereqs ();
	}

	// public void SendProjectStep(){
		// ProjectStep projectStep = new ProjectStep (
		// 	nameInput.text.text,
		// 	Utils_Prefs.GetProjectId (),
		// 	CreateStepObjects (ObjectSelection.instance.GetSelectedObjects ()),
		// 	durationHours + durationMins,
		// 	crewSize,
		// 	leadTime,
		// 	lagTime,
		// 	CreateStepObjects (ObjectSelection.instance.GetPrerequisiteObjects ()
		// ));
		// lastScheduledStep = projectStep;
		// FadeScheduledObjects (projectStep);
		// Menu.instance.DeselectButtonClick();
		// animator.SetInteger("State", 0);
		// // planMenu.SetActive (false);
		// ProjectStepService.SendProjectStep(projectStep);
	// }

	// void FadeScheduledObjects(ProjectStep projectStep){
	// 	foreach (ProjectStepObject projectStepObject in projectStep.projectObjects) {
	// 		ProjectObjects.instance.GetObjectById(projectStepObject.objectId).AddScheduledActivity (projectStepObject.activity);
	// 	}
	// }

	// public void UnFadeScheduledObjects(){
	// 	if (lastScheduledStep != null) {
	// 		foreach (ProjectStepObject projectStepObject in lastScheduledStep.projectObjects) {
	// 			ProjectObjects.instance.GetObjectById(projectStepObject.objectId).RemoveScheduledActivity (projectStepObject.activity);
	// 		}
	// 		lastScheduledStep = null;
	// 	}
	// }

	// public void ResetMenu(){
	// 	// crewSize = 0;
	// 	// crewSizeTextInput.ReceiveInput("");
	// 	// foreach(RadioButtonUI button in crewRadioButtons){
	// 	// 	button.SetActive(false);
	// 	// }
	// 	// durationHours = 0;
	// 	// durationTextInput.ReceiveInput("");
	// 	// foreach(RadioButtonUI button in hourRadioButtons){
	// 	// 	button.SetActive(false);
	// 	// }
	// 	// durationMins = 0;
	// 	// foreach(RadioButtonUI button in minRadioButtons){
	// 	// 	button.SetActive(false);
	// 	// }
	// 	// leadTime = 0;
	// 	// leadTimeTextInput.ReceiveInput("");
	// 	// lagTime = 0;
	// 	// lagTimeTextInput.ReceiveInput("");
	// }

	// List<ProjectStepObject> CreateStepObjects(List<ProjectObject> projectObjects, bool prereq = false){
	// 	List<string> addedIds = new List<string> ();
	// 	List<ProjectStepObject> projectStepObjects = new List<ProjectStepObject> ();
	// 	for(int i = 0; i < projectObjects.Count; i++) {
	// 		if (!addedIds.Contains (projectObjects[i].id)) {
	// 			if (!prereq) {
	// 				projectStepObjects.Add (new ProjectStepObject (projectObjects [i].id, ActivityMenu.instance.GetSelectedActivity().activity));
	// 			} else {
	// 				projectStepObjects.Add (new ProjectStepObject(projectObjects [i].id, ObjectSelection.instance.prereqActivities[i]));
	// 			}
	// 			addedIds.Add (projectObjects [i].id);
	// 		}
	// 	}
	// 	return projectStepObjects;
	// }

	// public void DisplaySelectedActivity(){
	// 	if (ActivityMenu.instance.GetSelectedActivity() != null) {
	// 		selectedActivityText.text = ActivityMenu.instance.GetSelectedActivity().name;
	// 		PlanMenu.instance.SetStepNameObject (ObjectSelection.instance.selectedObjects[0].name + " - " + ActivityMenu.instance.GetSelectedActivity().name);
    //     } else {
	// 		selectedActivityText.text = "";
    //         // nameInput.ReceiveInput("");
    //     }
	// }

	// public void SetStepNameObject(string input){
	// 	// nameInput.ReceiveInput (input);
	// }

}
