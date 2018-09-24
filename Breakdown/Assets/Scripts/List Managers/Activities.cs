using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activities : MonoBehaviour {

	public static Activities instance;
	public List<Activity> activities = new List<Activity> ();
	public List<string> permittedActivities = new List<string>();
	public bool userIsGC;

	void Awake () {
		instance = this;
	}

	public void AddActivity (Activity activity) {
		activities.Add (activity);
	}

    public void AddPermittedActivity(string activity){
		if(activity == "00 00 00"){
			userIsGC = true;
		}
        permittedActivities.Add(activity);
    }

	public Activity GetActivityByCode(string code){
		return activities.Find (tc => tc.activity == code);
	}

	public List<Activity> FilterActivities(string input){
		if (input == "") {
            return activities;
		} else {
			return activities.FindAll (tc => {
				return tc.name.ToLower().Contains(input.ToLower());
			});
		}
	}

	public bool CheckActivityPermission(string activity){
		return userIsGC || permittedActivities.Contains(activity);
	}
}