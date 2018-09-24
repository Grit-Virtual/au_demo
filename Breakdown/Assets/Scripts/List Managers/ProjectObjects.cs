using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectObjects : MonoBehaviour {

	public static ProjectObjects instance;
	// Change to HashSet for better performance
	[HideInInspector]
	public List<ProjectObject> projectObjects = new List<ProjectObject>();

	void Awake(){
		instance = this;
	}

	public void RegisterObjects(){
		foreach (GameObject go in GameObject.FindGameObjectsWithTag ("UID Object")) {
			projectObjects.Add(go.GetComponent<ProjectObject>());
		}
	}

	public void AddProjectObject(ProjectObject projectObject){
//		if (!projectObjects.Contains (projectObject) && !projectObject.hidden) {
			projectObjects.Add (projectObject);
//		}
	}

	public void RemoveHiddenObjects(){
		List<ProjectObject> hiddenObjects = projectObjects.FindAll (obj => obj.hidden);
		foreach (ProjectObject projectObject in hiddenObjects) {
			projectObjects.Remove (projectObject);
		}
	}

	public ProjectObject GetObjectById(string objectId){
		return projectObjects.Find(obj => {
			return obj.id == objectId;
		});
	}

	public List<ProjectObject> GetAllObjects(){
		return projectObjects;
	}

//	public void SetUpUidsForSelection(){
//		foreach (ProjectObject projectObject in projectObjects) {
//			//projectObject.interactiveSelect.pointerActivatesUseAction = true;
//		}
//	}
//
//	public void SetUpUidsForNavigation(){
//		foreach (ProjectObject projectObject in projectObjects) {
//			//projectObject.interactiveSelect.pointerActivatesUseAction = false;
//		}
//	}
//
//	public void SetUpScheduledObjects(){
//		foreach (ProjectStep projectStep in ProjectSteps.instance.projectSteps) {
//			foreach (string objectId in projectStep.objectIds) {
//					ProjectObject projectObject = GetObjectById(objectId);
//					SetUidToScheduled (projectObject);
//					MaterialManager.instance.FadeProjectObject (projectObject);
//			}
//		}
//	}

	public void SetObjectToUnscheduled(ProjectObject projectObject){
		projectObject.SetLayer("Unscheduled");
	}

	public void SetObjectToCircular(ProjectObject projectObject){
		projectObject.SetLayer("Circular");
	}

	public void CheckIfObjectIsScheduled(ProjectObject projectObject){
        if (projectObject.activities.Count > 0) {
            if (!Activities.instance.userIsGC) {
                List<string> activities = projectObject.activities.FindAll(activity => Activities.instance.permittedActivities.Contains(activity));
                bool scheduled = true;
                for (int i = 0; i < activities.Count; i++) {
                    if (!projectObject.scheduledActivities.Contains(activities[i])) {
                        scheduled = false;
                        break;
                    }
                }
                if (scheduled) {
                    projectObject.Fade();
                    projectObject.isScheduled = true;
                } else {
                    projectObject.UnFade();
                    projectObject.isScheduled = false;
                }
            } else {
                if (projectObject.scheduledActivities.Count >= projectObject.activities.Count) {
                    projectObject.Fade();
                    projectObject.isScheduled = true;
                } else {
                    projectObject.UnFade();
                    projectObject.isScheduled = false;
                }
            }
        }
		if(projectObject.loaded){
			if(projectObject.isScheduled){
				projectObject.SetLayer("Scheduled");
			} else {
				projectObject.SetLayer("Unscheduled");
			}
		}
	}

	public void AddScheduledActivity(){
		foreach (ProjectObject projectObject in ObjectSelection.instance.GetSelectedObjects()) {
			projectObject.AddScheduledActivity(ActivityMenu.instance.GetSelectedActivity().activity);
		}
	}

	public void AssignObjectInfo(ProjectObjectInfo projectObjectInfo){
		GetObjectById (projectObjectInfo.id).AssignInfo (projectObjectInfo);
	}

	public void HideCircularObjects(){
		Dictionary<string, List<string>> circularRefs = new Dictionary<string, List<string>> ();
		foreach (ProjectObject projectObject in ObjectSelection.instance.GetSelectedObjects()) {
			CircularObject circularObject = projectObject.circularObjects.Find (circ => circ.activity == ActivityMenu.instance.GetSelectedActivity().activity);
			if (circularObject != null) {
				foreach (ProjectStepObject circularRef in circularObject.projectStepObjects) {
					if (circularRefs.ContainsKey (circularRef.objectId)) {
						if (!circularRefs [circularRef.objectId].Contains (circularRef.activity)) {
							circularRefs [circularRef.objectId].Add (circularRef.activity);
						}
					} else {
						circularRefs.Add (circularRef.objectId, new List<string> ());
						circularRefs [circularRef.objectId].Add (circularRef.activity);
					}
				}
			}
		}
		foreach (KeyValuePair<string, List<string>> circularRef in circularRefs) {
			ProjectObject projectObject = GetObjectById (circularRef.Key);
			if (circularRef.Value.Count >= projectObject.activities.Count) {
				if (projectObject.gameObject.layer != LayerMask.NameToLayer ("Circular")) {
					SetObjectToCircular (projectObject);
				}
			}
		}
	}

	public void ShowCircularObjects(){
		foreach (ProjectObject projectObject in projectObjects.FindAll(obj => obj.gameObject.layer == LayerMask.NameToLayer("Circular"))) {
			CheckIfObjectIsScheduled (projectObject);
		}
	}

	public void FilterObjectsByActivity(List<string> activities, bool showAll = false){
        bool active;
        foreach (ProjectObject projectObject in projectObjects) {
			active = false;
			if (showAll) {
				if (!projectObject.gameObject.activeInHierarchy) {
					projectObject.gameObject.SetActive (true);
				}
			} else {
                for(int t = 0; t<activities.Count; t++) {
                    if (projectObject.activities.Contains(activities[t])) {
                        active = true;
                        break;
                    }
                }

				if (active) {
					if (!projectObject.gameObject.activeInHierarchy) {
						projectObject.gameObject.SetActive (true);
					}
				} else {
					if (projectObject.gameObject.activeInHierarchy) {
						projectObject.gameObject.SetActive (false);
					}
				}
			}
		}
	}

    public void FilterObjectsBySubcontractor(List<string> subIds) {
        List<string> activitiesToFilter = new List<string>();
        foreach(string subId in subIds) {
            activitiesToFilter.AddRange(Subcontractors.instance.GetSubcontractorActivities(subId));
        }
        bool active;
        foreach (ProjectObject projectObject in projectObjects) {
            active = false;
            for (int t = 0; t < activitiesToFilter.Count; t++) {
                if (projectObject.activities.Contains(activitiesToFilter[t])) {
                    active = true;
                    break;
                }
            }

            if (active) {
                if (!projectObject.gameObject.activeInHierarchy) {
                    projectObject.gameObject.SetActive(true);
                }
            } else {
                if (projectObject.gameObject.activeInHierarchy) {
                    projectObject.gameObject.SetActive(false);
                }
            }
        }
    }

    public void FilterObjectsByCategory(List<string> categories) {
        bool active;
        foreach (ProjectObject projectObject in projectObjects) {
            active = false;
            for (int t = 0; t < categories.Count; t++) {
                if (projectObject.categories.Contains(categories[t])) {
                    active = true;
                    break;
                }
            }

            if (active) {
                if (!projectObject.gameObject.activeInHierarchy) {
                    projectObject.gameObject.SetActive(true);
                }
            } else {
                if (projectObject.gameObject.activeInHierarchy) {
                    projectObject.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ShowAllObjects() {
        foreach (ProjectObject projectObject in projectObjects) {
            if (!projectObject.gameObject.activeInHierarchy) {
                projectObject.gameObject.SetActive(true);
            }
        }
    }

    public void FilterObjectsForPlanning(string activity){
		foreach (ProjectObject projectObject in projectObjects) {
			bool active = false;
			active = projectObject.activities.Contains (activity);
			if (active) {
				if (!projectObject.gameObject.activeInHierarchy) {
					projectObject.gameObject.SetActive (true);
				}
			} else {
				if (projectObject.gameObject.activeInHierarchy) {
					projectObject.gameObject.SetActive (false);
				}
			}
		}
	}

	public void FilterObjectsWithoutActivities(){
		foreach (ProjectObject projectObject in projectObjects) {
			if (projectObject.activities.Count > 0) {
				if (!projectObject.gameObject.activeInHierarchy) {
					projectObject.gameObject.SetActive (true);
				}
			} else {
				if (projectObject.gameObject.activeInHierarchy) {
					projectObject.gameObject.SetActive (false);
				}
			}
		}
	}

	public List<string> GetValidActivities(){
		List<string> validActivities = new List<string> ();
		foreach (ProjectObject projectObject in projectObjects) {
			List<string> objectActivities = new List<string> ();
			foreach (string activity in projectObject.activities) {
				objectActivities.Add (activity);
				objectActivities.Add (activity.Substring (0, 6) + "00");
				objectActivities.Add (activity.Substring (0, 3) + "00 00");
				foreach (string ObjectActivity in objectActivities) {
					if (!validActivities.Contains (ObjectActivity)) {
						validActivities.Add (ObjectActivity);
					}
				}
			}
		}
		return validActivities;
	}

    public bool CheckIfObjectIsSelectable(ProjectObject projectObject){
		if(!ObjectSelection.instance.selectingPrereqs){
			if(!ObjectSelection.instance.selectingKit){
				return true;
			} else {
				List<string> activities = new List<string>(projectObject.activities);
				if (activities.Count > 0) {
					// REMOVE SCHEDULED TRADE CODES
					foreach (string scheduledActivity in projectObject.scheduledActivities) {
						if (activities.Contains(scheduledActivity)) {
							activities.Remove(scheduledActivity);
						}
					}
					
					// REMOVE FORBIDDEN TRADE CODES IF NOT GC
					if(!Activities.instance.userIsGC){
						for (int i = activities.Count - 1; i >= 0; i--) {
							if (!Activities.instance.permittedActivities.Contains (activities [i])) {
								activities.RemoveAt (i);
							}
						}
					}

					if (activities.Count > 0) {
						if (ActivityMenu.instance.GetSelectedActivity() == null) {
							return true;
						} else {
							if (activities.Contains (ActivityMenu.instance.GetSelectedActivity().activity)) {
								return true;
							} else {
								return false;
							}
						}
					} else {
						return false;
					}
				} else {
					if(ActivityMenu.instance.GetSelectedActivity() == null){
						return true;
					} else {
						return false;
					}
				}
			}
		} else {
			if (projectObject.activities.Count > 0) {
				return true;
			} else {
				return false;
			}
		}
	}

	public bool CheckIfObjectIsPlannable(ProjectObject projectObject){
		bool plannable = true;
		if (projectObject.activities.Count < 1) {
			plannable = false;
		} else {
			if (ActivityMenu.instance.GetSelectedActivity() != null) {
				if (!projectObject.activities.Contains (ActivityMenu.instance.GetSelectedActivity().activity)) {
					plannable = false;
				} else {
					if (projectObject.scheduledActivities.Contains(ActivityMenu.instance.GetSelectedActivity().activity)) {
						plannable = false;
					} else {
						plannable = true;
					}
				}
			} else {
				plannable = true;
			}
		}
		return plannable;
	}

	public List<ProjectObject> GetScheduledObjects(){
		return projectObjects.FindAll(projectObject => projectObject.gameObject.layer == LayerMask.NameToLayer("Scheduled"));
	}

	public List<ProjectObject> GetDoorsAndWindows(){
		return projectObjects.FindAll(obj => {
			bool door = false;
			for(int i = 0; i < obj.categories.Count; i++){
				door = obj.categories[i].ToLower().Contains("door") || obj.categories[i].ToLower().Contains("glass") || obj.categories[i].ToLower().Contains("window");
				if(door) break;
			}
			return door;
		});
	}
}
