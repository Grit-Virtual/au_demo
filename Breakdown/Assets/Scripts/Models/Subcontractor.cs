using System.Collections;
using SimpleJSON;


[System.Serializable]
public class Subcontractor {
	public string id;
	public string abbreviation;
	public string name;
	public string hexCode;
    public ArrayList activities;

	public Subcontractor(JSONNode sub){
		id = sub["id"];
        abbreviation = sub["abbreviation"];
        hexCode = sub["hexCode"];
		name = sub["name"];

        JSONArray tcs = sub["activities"].AsArray;
        activities = new ArrayList();
        for(int t = 0; t < tcs.Count; t++) {
            activities.Add(tcs[t].Value);
        }
	}
}
