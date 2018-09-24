using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivityMenu : MonoBehaviour {

	public static ActivityMenu instance;

	public Animator animator;

	public GameObject activitySelectorPrefab;
	public Transform activitySelectorParent;

	List<ActivitySelector> activitySelectors = new List<ActivitySelector>();

	Activity selectedActivity;

	public GameObject selectedActivityDisplay;
	public TextMeshProUGUI selectedActivityText;

	void Awake () {
		instance = this;
	}

	void Start(){
		DeselectActivity ();
		HideActivityMenu();
	}

	public void InitializeActivityMenu(){
		ClearActivitySelectors ();
		GenerateActivitySelectors();
		MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.ACTIVITY);
		if(!MenuManager.instance.toggleMenuButtons[(int)MenuManager.MenuType.PLANNER].active){
			MenuManager.instance.toggleMenuButtons[(int)MenuManager.MenuType.PLANNER].SetActive(true);
		}
	}

	public void ShowActivityMenu(){
		animator.SetBool("MenuShowing", true);
		MenuManager.instance.DeactivateButtons();
    }

	public void HideActivityMenu(){
		animator.SetBool("MenuShowing", false);
		MenuManager.instance.ReactivateButtons();
    }

    public void BackButtonPress() {
		if(ObjectSelection.instance.selectingPrereqs){
			ObjectSelection.instance.RemoveLastSelectedPrereq ();
    	}
		PlanMenuManager.instance.planning = false;
	}

    public void GenerateActivitySelectors(){
		if (!ObjectSelection.instance.selectingPrereqs) {
			int activitiesAdded = 0;
			ProjectObject firstSelectedObject = ObjectSelection.instance.GetSelectedObjects () [0];
			for (int i = 0; i < firstSelectedObject.activities.Count; i++) {
				if (Activities.instance.CheckActivityPermission (firstSelectedObject.activities [i]) && !firstSelectedObject.scheduledActivities.Contains (firstSelectedObject.activities [i])) {
					if (activitiesAdded > 6) {
						break;
					}
					GameObject activitySelectorGo = (GameObject)Instantiate (activitySelectorPrefab);
					activitySelectorGo.transform.SetParent (activitySelectorParent, false);
					ActivitySelector activitySelector = activitySelectorGo.GetComponent<ActivitySelector> ();
					Activity activity = Activities.instance.GetActivityByCode (firstSelectedObject.activities [i]);
					activitySelector.SetUp (firstSelectedObject, activity, false);
					activitySelectors.Add (activitySelector);
					activitiesAdded++;
				}
			}
		} else {
			List<string> circularActivities = new List<string> ();
			ProjectObject lastSelectedPrereq = ObjectSelection.instance.GetPrerequisiteObjects () [ObjectSelection.instance.GetPrerequisiteObjects ().Count - 1];
			for (int i = 0; i < lastSelectedPrereq.circularObjects.Count; i++) {
				for (int e = 0; e < lastSelectedPrereq.circularObjects [i].projectStepObjects.Count; e++) {
					if (ObjectSelection.instance.IsObjectSelectedById (lastSelectedPrereq.circularObjects [i].projectStepObjects [e].objectId)
					    && lastSelectedPrereq.circularObjects [i].projectStepObjects [e].activity == ActivityMenu.instance.selectedActivity.activity) {
						circularActivities.Add (lastSelectedPrereq.circularObjects [i].activity);
						break;
					}
				}
			}

            // Remove selected obj activity as an option because thats invalid
			List<string> validPrereqActivities = new List<string> (lastSelectedPrereq.activities);
			if (ObjectSelection.instance.IsObjectSelected (lastSelectedPrereq)) {
				validPrereqActivities.Remove (ActivityMenu.instance.selectedActivity.activity);
			}
            // Remove any codes that would cause circular
			foreach (string tc in circularActivities) {
				if (validPrereqActivities.Contains (tc)) {
					validPrereqActivities.Remove (tc);
				}
			}

			int activitiesAdded = 0;
			for (int i = 0; i < validPrereqActivities.Count; i++) {
				if (activitiesAdded > 6) {
					break;
				}
				GameObject activitySelectorGo = (GameObject)Instantiate (activitySelectorPrefab);
				activitySelectorGo.transform.SetParent (activitySelectorParent, false);
				ActivitySelector activitySelector = activitySelectorGo.GetComponent<ActivitySelector> ();
				Activity activity = Activities.instance.GetActivityByCode (validPrereqActivities[i]);
				bool activitySelected = false;
				for (int x = 0; x < ObjectSelection.instance.selectedPrereqs.Count; x++) {
					if(ObjectSelection.instance.selectedPrereqs[x] == lastSelectedPrereq) {
						if(ObjectSelection.instance.prereqActivities.Count > x) {
							if(ObjectSelection.instance.prereqActivities[x] == validPrereqActivities[i]) {
								activitySelected = true;
							}
						}
					}
				}
				activitySelector.SetUp(lastSelectedPrereq, activity, activitySelected);
				activitySelectors.Add (activitySelector);
				activitiesAdded++;
			}
		}
	}

	public bool ActivitySelectionRequired(bool prereqSelection = false){
		if(!prereqSelection){
			ProjectObject projectObject = ObjectSelection.instance.GetSelectedObjects()[0];
			List<string> activities = new List<string>(projectObject.activities);
			// REMOVE SCHEDULED TRADE CODES
			foreach (string scheduledActivity in projectObject.scheduledActivities) {
				if (activities.Contains(scheduledActivity)) {
					activities.Remove(scheduledActivity);
				}
			}
			// REMOVE FORBIDDEN TRADE CODES IF NOT GC
			if(!Activities.instance.userIsGC){
				for (int i = activities.Count - 1; i >= 0; i--) {
					if (!Activities.instance.permittedActivities.Contains(activities[i])) {
						activities.RemoveAt(i);
					}
				}
			}

			if (activities.Count > 1) {
				return true;
			} else {
				SelectActivity(Activities.instance.GetActivityByCode(activities[0]));
				return false;
			}
		} else {
			List<string> circularActivities = new List<string> ();
			ProjectObject lastSelectedPrereq = ObjectSelection.instance.GetPrerequisiteObjects () [ObjectSelection.instance.GetPrerequisiteObjects ().Count - 1];
			for (int i = 0; i < lastSelectedPrereq.circularObjects.Count; i++) {
				for (int e = 0; e < lastSelectedPrereq.circularObjects [i].projectStepObjects.Count; e++) {
					if (ObjectSelection.instance.IsObjectSelectedById (lastSelectedPrereq.circularObjects [i].projectStepObjects [e].objectId)
						&& lastSelectedPrereq.circularObjects [i].projectStepObjects [e].activity == ActivityMenu.instance.selectedActivity.activity) {
						circularActivities.Add (lastSelectedPrereq.circularObjects [i].activity);
						break;
					}
				}
			}

			// Remove selected obj activity as an option because thats invalid
			List<string> validPrereqActivities = new List<string> (lastSelectedPrereq.activities);
			if (ObjectSelection.instance.IsObjectSelected (lastSelectedPrereq)) {
				validPrereqActivities.Remove (ActivityMenu.instance.selectedActivity.activity);
			}
			// Remove any codes that would cause circular
			foreach (string tc in circularActivities) {
				if (validPrereqActivities.Contains (tc)) {
					validPrereqActivities.Remove (tc);
				}
			}

			if (validPrereqActivities.Count > 0) {
				if (validPrereqActivities.Count > 1) {
					// int activitiesAdded = 0;
					// for (int i = 0; i < validPrereqActivities.Count; i++) {
					// 	if (activitiesAdded > 6) {
					// 		break;
					// 	}
					// 	GameObject activitySelectorGo = (GameObject)Instantiate (activitySelectorPrefab);
					// 	activitySelectorGo.transform.SetParent (activitySelectorParent, false);
					// 	ActivitySelector activitySelector = activitySelectorGo.GetComponent<ActivitySelector> ();
					// 	Activity activity = Activities.instance.GetActivityByCode (validPrereqActivities[i]);
					// 	bool activitySelected = false;
					// 	for (int x = 0; x < ObjectSelection.instance.selectedPrereqs.Count; x++) {
					// 		if(ObjectSelection.instance.selectedPrereqs[x] == lastSelectedPrereq) {
					// 			if(ObjectSelection.instance.prereqActivities.Count > x) {
					// 				if(ObjectSelection.instance.prereqActivities[x] == validPrereqActivities[i]) {
					// 					activitySelected = true;
					// 				}
					// 			}
					// 		}
					// 	}
					// 	activitySelector.SetUp(lastSelectedPrereq, activity, activitySelected);
					// 	activitySelectors.Add (activitySelector);
					// 	activitiesAdded++;
					// }
					// MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.ACTIVITY);
					return true;
				} else {
					bool prereqSelected = false;
					for(int i = 0; i < ObjectSelection.instance.selectedPrereqs.Count; i++) {
						if(ObjectSelection.instance.selectedPrereqs[i] == lastSelectedPrereq) {
							if (ObjectSelection.instance.prereqActivities.Count > i) {
								if (ObjectSelection.instance.prereqActivities[i] == validPrereqActivities[0]) {
									prereqSelected = true;
									break;
								}
							}
						}
					}
					if (!prereqSelected) {
						SelectActivity(Activities.instance.GetActivityByCode(validPrereqActivities[0]));
						return false;
					} else {
						ObjectSelection.instance.RemoveSelectedPrereq(lastSelectedPrereq, validPrereqActivities[0]);
						return false;
					}
				}
			} else {
				ObjectSelection.instance.RemoveInvalidPrereq (lastSelectedPrereq);
				return false;
			}
		}
	}

	void ClearActivitySelectors(){
		if (activitySelectorParent.childCount > 0 || activitySelectors.Count > 0) {
			activitySelectors.Clear ();
			foreach (Transform child in activitySelectorParent) {
				Destroy (child.gameObject);
			}
		}
	}

	public void SelectActivity(Activity selectedActivity){
		if (!ObjectSelection.instance.selectingPrereqs) {
			this.selectedActivity = selectedActivity;
			DisplaySelectedActivity ();
			ObjectSelection.instance.ConfirmKitSelection();
		} else {
			ObjectSelection.instance.AddPrereqActivity (selectedActivity.activity);
			ObjectSelection.instance.InstatiateSelectedObjectUI(selectedActivity, true);
        }
		ProjectObjects.instance.HideCircularObjects ();
        MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.NONE);
	}

	public void DeselectActivity(){
		selectedActivity = null;
		DisplaySelectedActivity ();
	}

	public Activity GetSelectedActivity(){
		return selectedActivity;
	}

	public void DisplaySelectedActivity(){
		if (ActivityMenu.instance.GetSelectedActivity() != null) {
			selectedActivityDisplay.SetActive(true);
			selectedActivityText.text = "Planning " + ActivityMenu.instance.GetSelectedActivity().name;
			PlannerSubmitMenu.instance.SetStepName (ObjectSelection.instance.selectedObjects[0].name + " - " + ActivityMenu.instance.GetSelectedActivity().name);
			if(!MenuManager.instance.toggleMenuButtons[(int)MenuManager.MenuType.PLANNER].active){
				MenuManager.instance.toggleMenuButtons[(int)MenuManager.MenuType.PLANNER].SetActive(true);
			}
        } else {
			selectedActivityDisplay.SetActive(false);
			selectedActivityText.text = "";
            PlannerSubmitMenu.instance.SetStepName("");
			if(MenuManager.instance.toggleMenuButtons[(int)MenuManager.MenuType.PLANNER].active){
				MenuManager.instance.toggleMenuButtons[(int)MenuManager.MenuType.PLANNER].SetActive(false);
			}
        }
	}
}
