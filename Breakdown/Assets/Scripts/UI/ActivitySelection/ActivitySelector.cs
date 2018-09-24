using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivitySelector : MonoBehaviour {

	public TextMeshProUGUI buttonText;

	Activity activity;
    ProjectObject projectObject;
	bool selected;
	public Animator animator;
	public ActivitySelector[] activitySelectors;

	public void SetUp(ProjectObject projectObject, Activity activity, bool activityAlreadySelected){
        this.projectObject = projectObject;
        this.selected = activityAlreadySelected;
		this.activity = activity;
        buttonText.text = activity.name + (activityAlreadySelected ? " - Selected" : "");
		if(this.selected){
			animator.SetBool ("Active", true);
		}
	}

	public void ActivitySelectorClick(){
		if (selected) {
			DeselectActivity ();
            ObjectSelection.instance.RemoveSelectedPrereq(projectObject, activity.activity);
        	MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.NONE);
        } else {
            ActivityMenu.instance.SelectActivity(activity);
			DeactivateActivitySelectors ();
			animator.SetBool ("Active", true);
        }
	}

	void DeactivateActivitySelectors (){
		foreach (ActivitySelector tcs in activitySelectors) {
			tcs.DeselectActivity ();
		}
	}

	public void DeselectActivity(){
		selected = false;
		animator.SetBool ("Active", false);
	}
}
