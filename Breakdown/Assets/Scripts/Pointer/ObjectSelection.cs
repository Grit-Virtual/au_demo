using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectSelection: MonoBehaviour {

	public static ObjectSelection instance;
	public List<ProjectObject> selectedObjects = new List<ProjectObject>();
	public List<ProjectObject> selectedPrereqs = new List<ProjectObject>();
	public List<string> prereqActivities = new List<string> ();

	public Animator animator;

	[HideInInspector]
	public bool selectionEnabled;
	public bool selectingPrereqs;
	public bool selectingKit;

	public Transform selectedObjectsList, selectedPrereqsList;
	public GameObject selectedObjectUIPrefab;
	List<SelectedObjectUI> selectedObjectUIs = new List<SelectedObjectUI>();
	List<SelectedObjectUI> selectedPrereqUIs = new List<SelectedObjectUI>();

    ProjectObject lastSelectedObject;

	public ToggleButtonUI planMenuButton, infoMenuButton;
	public ButtonUI selectPrereqsButton;

    void Awake(){
		instance = this;
	}

	public List<ProjectObject> GetSelectedObjects(){
		return selectedObjects;
	}

	public List<ProjectObject> GetPrerequisiteObjects(){
		return selectedPrereqs;
	}

	public void ObjectSelectionEnabled(bool enabled){
		selectionEnabled = enabled;
	}

	public void AddSelectedObject(ProjectObject projectObject){
		if(ProjectObjects.instance.CheckIfObjectIsSelectable(projectObject)){
			if(!selectingKit){
				DeselectSelection();
			}
			selectedObjects.Add (projectObject);
			projectObject.interactiveSelect.HighlightObject();
			lastSelectedObject = projectObject;
			InstatiateSelectedObjectUI();

			bool selectionPlannable = true;
			for(int i = 0; i < selectedObjects.Count; i++){
				selectionPlannable = ProjectObjects.instance.CheckIfObjectIsPlannable(selectedObjects[i]);
				if(!selectionPlannable) break;
			}
			planMenuButton.SetDisabled(!selectionPlannable);
			infoMenuButton.SetDisabled(false);
			selectPrereqsButton.SetDisabled(false);
		}
	}

	public void ConfirmKitSelection(){
		Confirmation.instance.ShowConfirmation(StartSelectingKit, StartSelectingPrereqs, "Add objects to kit?", "Yes", "No");
	}

	void StartSelectingKit(){
		selectingKit = true;
		animator.SetInteger("State", 1);
	}

	public void StartSelectingPrereqs(){
		selectingKit = false;
		selectingPrereqs = true;
		Raycaster.instance.ApplyPrereqMask();
		FilterMenu.instance.RevertToFilter ();
		animator.SetInteger("State", 2);
	}

	public void EndSelectingPrereqs(){
		selectingKit = false;
		selectingPrereqs = false;
		animator.SetInteger("State", 0);
	}

	public void AddSelectedPrereq(ProjectObject projectObject){
        lastSelectedObject = projectObject;
        selectedPrereqs.Add(projectObject);
		projectObject.interactiveSelect.HighlightObjectAsPrereq();
		Raycaster.instance.SetPointerEnabled(false);
	}

	public void AddPrereqActivity(string activity){
		while (prereqActivities.Count < selectedPrereqs.Count) {
			prereqActivities.Add (activity);
		}
	}

	public void DeselectSelection(){
		for(int i = selectedObjects.Count - 1; i >= 0; i--){
			RemoveSelectedObject(selectedObjects[i]);
		}
	}

	public void RemoveSelectedObject(ProjectObject projectObject){
		projectObject.interactiveSelect.UnHighlightObject ();
		selectedObjects.Remove (projectObject);
		if (selectedObjects.Count <= 0) {
			ActivityMenu.instance.DeselectActivity ();
		}
		ProjectObjects.instance.ShowCircularObjects ();
		SelectedObjectUI selectedObjectUI = selectedObjectUIs.Find (obj => obj.projectObject == projectObject);
		if (selectedObjectUI != null) {
			selectedObjectUIs.Remove (selectedObjectUI);
			Destroy (selectedObjectUI.gameObject);
		}
		if(selectedObjects.Count <= 0){
			planMenuButton.SetDisabled(true);
			infoMenuButton.SetDisabled(true);
			selectPrereqsButton.SetDisabled(true);
		}
	}

    public void RemoveLastSelectedPrereq() {
		ProjectObject prereq = selectedPrereqs[selectedPrereqs.Count - 1];
		selectedPrereqs.RemoveAt (selectedPrereqs.Count - 1);
		if(!selectedPrereqs.Contains(prereq)){
			prereq.interactiveSelect.UnHighlightObjectAsPrereq ();
		}
		ReHighlightSelection ();
    }

	public void DeselectPrereqs(){
		for(int i = selectedPrereqs.Count - 1; i >= 0; i--){
			RemoveSelectedPrereq(selectedPrereqs[i], prereqActivities[i]);
		}
	}

	public void RemoveSelectedPrereq(ProjectObject projectObject, string activity){
        for (int i = selectedPrereqs.Count - 1; i >= prereqActivities.Count; i--) {
            selectedPrereqs.RemoveAt(i);
        }
        for (int i = 0; i < selectedPrereqs.Count; i++) {
			if (selectedPrereqs[i] == projectObject && prereqActivities[i] == activity) {                        
                selectedPrereqs.RemoveAt(i);
                prereqActivities.RemoveAt(i);
                break;
            }
        }
		if (!selectedPrereqs.Contains(projectObject)) {
			projectObject.interactiveSelect.UnHighlightObjectAsPrereq();
        }

		SelectedObjectUI selectedPrereqUI = selectedPrereqUIs.Find (obj => obj.projectObject == projectObject && obj.activity == activity);
		if (selectedPrereqUI != null) {
			selectedPrereqUIs.Remove (selectedPrereqUI);
			Destroy (selectedPrereqUI.gameObject);
		}
		ReHighlightSelection (); // <-- MOVE INTO UnHighlightObjectAsPrereq()
	}

	public void RemoveInvalidPrereq(ProjectObject projectObject){
        Alert.instance.ShowAlert("Tasks cannot be dependent on one another, creating a loop. Please verify your prerequisites.");
        projectObject.interactiveSelect.UnHighlightObjectAsPrereq ();
		int index = selectedPrereqs.IndexOf (projectObject);
		selectedPrereqs.Remove (projectObject);
		ReHighlightSelection ();
	}

	public void DeselectObjectsWithoutSelectedActivity(){
		List<ProjectObject> objectsToDeselect = new List<ProjectObject> ();
		foreach (ProjectObject projectObject in selectedObjects) {
			bool remove = false;

			if (projectObject.activities.Count < 1) {
				remove = true;
			}
			if (!remove) {
				if (ActivityMenu.instance.GetSelectedActivity().activity != null) {
					if (!projectObject.activities.Contains (ActivityMenu.instance.GetSelectedActivity().activity)) {
						remove = true;
					}
				}
			}
			if (!remove) {
				remove = true;
				foreach (string activity in projectObject.activities) {
					if (Activities.instance.permittedActivities.Contains (activity)) {
						remove = false;
						break;
					}
				}
			}
			if (remove) {
				objectsToDeselect.Add (projectObject);
			}
		}
		foreach (ProjectObject projectObject in objectsToDeselect) {
			if (selectedObjects.Contains (projectObject)) {
				RemoveSelectedObject (projectObject);
			}
		}

		ReHighlightSelection ();
	}

	public void ReHighlightSelection(){
		//Rehighlight the objects that are selected in case of adding prereq and then removing on itself
		if (selectedObjects.Count > 0) {
			foreach (ProjectObject projectObject in selectedObjects) {
				projectObject.interactiveSelect.HighlightObject ();
			}
		}
	}

	public void UnHighlightPrereqs(){
		foreach (ProjectObject projectObject in selectedPrereqs) {
			projectObject.interactiveSelect.UnHighlightObjectAsPrereq ();
		}
	}

	public void UnHighlightSelectedObjects(){
		foreach (ProjectObject projectObject in selectedObjects) {
			projectObject.interactiveSelect.UnHighlightObject ();
		}
	}

	public bool IsObjectSelected(ProjectObject projectObject){
		return selectedObjects.Contains (projectObject);
	}

	public bool IsObjectSelectedById(string objectId){
		ProjectObject selected = selectedObjects.Find (obj => obj.id == objectId);
		return selected != null;
	}

	public bool IsPrereqObjSelected(ProjectObject projectObject){
		return selectedPrereqs.Contains (projectObject);
	}

	// public int GetSelectionCount(){
	// 	List<string> selectedObjectIds = new List<string>();
	// 	foreach (ProjectObject projectObject in selectedObjects) {
	// 		if (!selectedObjectIds.Contains (projectObject.id)) {
	// 			selectedObjectIds.Add (projectObject.id);
	// 		}
	// 	}
	// 	return selectedObjectIds.Count;
	// }

	// public int GetPrereqCount(){
	// 	List<string> selectedPrereqIds = new List<string>();
	// 	foreach (ProjectObject projectObject in selectedPrereqs) {
	// 		if (!selectedPrereqIds.Contains (projectObject.id)) {
	// 			selectedPrereqIds.Add (projectObject.id);
	// 		}
	// 	}
	// 	return selectedPrereqIds.Count;
	// }

	public void InstatiateSelectedObjectUI(Activity activity = null, bool prereq = false){
		GameObject selectedObjectUIGo = (GameObject)Instantiate (selectedObjectUIPrefab);
		SelectedObjectUI selectedObjectUI = selectedObjectUIGo.GetComponent<SelectedObjectUI> ();
		if (!prereq) {
			selectedObjectUI.SetUp (lastSelectedObject, false);
			selectedObjectUIGo.transform.SetParent (selectedObjectsList, false);
			selectedObjectUIs.Add (selectedObjectUI);
		} else {
			selectedObjectUI.SetUp (lastSelectedObject, true, activity);
			selectedObjectUIGo.transform.SetParent (selectedPrereqsList, false);
			selectedPrereqUIs.Add (selectedObjectUI);
        }
	}
}


