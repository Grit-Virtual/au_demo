using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ObjectGenerationInfo {
	public string id;
	public string name;
	public string modelId;
	public int forgeId;
	public List<float> boundingBox = new List<float>();
	public List<string> categories = new List<string>();
	public List<string> activities = new List<string>();
	public List<CircularObject> circularObjects = new List<CircularObject>();
	public bool hidden;
	public List<int> fragments = new List<int>();
    public ObjectGenerationInfo(JSONNode projectObject) {
        this.id = projectObject["id"];
        this.name = projectObject["name"];
        this.modelId = projectObject["projectModelId"];
        this.forgeId = (int)projectObject["forgeObjectId"];

        List<float> bounds = new List<float>();
        JSONArray bbArr = projectObject["boundingBox"].AsArray;
        for(int bb = 0; bb < bbArr.Count; bb++) {
            bounds.Add(float.Parse(bbArr[bb]));
        }
        this.boundingBox = bounds;

        if(projectObject["categories"] != null){
            List<string> cats = new List<string>();
            JSONArray catsArr = projectObject["categories"].AsArray;
            for (int cat = 0; cat < catsArr.Count; cat++) {
                cats.Add(catsArr[cat]);
            }
            this.categories = cats;
        }

        if(projectObject["activities"] != null){
            JSONArray tcArr = projectObject["activities"].AsArray;
            for (int tc = 0; tc < tcArr.Count; tc++) {
                activities.Add(tcArr[tc]);
            }
        }

        int hid = (int)projectObject["hidden"];
        if (hid == 0) {
            hidden = false;
        } else {
            hidden = true;
        }

		if (projectObject ["fragments"] != null) {
			JSONArray fArr = projectObject["fragments"].AsArray;
			for (int f = 0; f < fArr.Count; f++) {
				fragments.Add (fArr [f].AsInt);
			}

		}
    }
}
