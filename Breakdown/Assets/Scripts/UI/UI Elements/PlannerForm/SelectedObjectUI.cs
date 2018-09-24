using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedObjectUI : MonoBehaviour {

	public TextMeshProUGUI nameText;
	[HideInInspector]
	public ProjectObject projectObject;
	[HideInInspector]
    public string activity;
	bool prereq;

	public void SetUp(ProjectObject projectObject, bool prereq, Activity activity = null){
		this.projectObject = projectObject;
		nameText.text = projectObject.name;
		this.prereq = prereq;
		if (prereq) {
			nameText.text = nameText.text + " - " + activity.name;
			this.activity = activity.activity;
		}
	}

	public void DeselectButtonClick(){
		if(!prereq){
			ObjectSelection.instance.RemoveSelectedObject (projectObject);
		} else {
			ObjectSelection.instance.RemoveSelectedPrereq (projectObject, activity);
		}
	}
}
