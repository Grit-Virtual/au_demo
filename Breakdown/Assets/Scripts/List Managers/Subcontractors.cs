using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subcontractors : MonoBehaviour {

	public static Subcontractors instance;
    
    public List<Subcontractor> projectSubs = new List<Subcontractor>();

    void Awake(){
		instance = this;
	}

    public void AddProjectSub(JSONNode sub) {
        Subcontractor subToAdd = new Subcontractor(sub);
        projectSubs.Add(subToAdd);
    }

    public List<Subcontractor> FilterSubcontractors(string input) {
        if (input == "") {
            return projectSubs;
        } else {
            return projectSubs.FindAll(sub => {
                return sub.name.ToLower().Contains(input.ToLower());
            });
        }
    }

    public List<string> GetSubcontractorActivities(string id) {
        List<string> retActivities = new List<string>();
        Subcontractor mySub = projectSubs.Find(sub => sub.id == id);
        if (mySub != null) {
            foreach(string tc in mySub.activities) {
                retActivities.Add(tc);
            }
        }
        return retActivities;
    }
}
