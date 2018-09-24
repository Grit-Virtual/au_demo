using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectObjectInfo {
	public string modelId;
	public int forgeId;
	public List<CircularObject> circularObjects = new List<CircularObject> ();
    public List<ObjectProperty> properties = new List<ObjectProperty>();
	public List<string> activities = new List<string>();
	public string id;
	public bool hidden;

	public ProjectObjectInfo(Hashtable projectObject){
		this.modelId = projectObject ["projectModelId"].ToString ();
		if (projectObject ["activities"] != null) {
			foreach (string activity in (ArrayList)projectObject["activities"]) {
				this.activities.Add(activity);
			}
		}
		this.forgeId = (int)projectObject ["forgeObjectId"];
		this.id = projectObject ["id"].ToString();
		if (projectObject ["scheduledPrerequisites"] != null) {
			foreach (DictionaryEntry circular in (Hashtable)projectObject ["scheduledPrerequisites"]) {
				circularObjects.Add(new CircularObject(circular.Key.ToString(), (ArrayList)circular.Value));
			}
		}
		if (projectObject ["hidden"] != null) {
			if ((int)projectObject ["hidden"] == 0) {
				hidden = false;
			} else {
				hidden = true;
			}
		} else {
			hidden = false;
		}
        DictionaryEntry curProp;
        string header = "";
        string curKey = "";
        string curVal = "";
        if (projectObject["properties"] != null) {
            foreach (DictionaryEntry propHeader in (Hashtable)projectObject["properties"]) {
                curProp = propHeader;
                header = propHeader.Key.ToString();
                foreach(DictionaryEntry prop in (Hashtable)propHeader.Value) {
                    curKey = prop.Key.ToString();
                    curVal = prop.Value.ToString();
                    properties.Add(new ObjectProperty(curKey, curVal));
                }
            }
        }
	}

}
