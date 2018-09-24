using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSelect : MonoBehaviour {

	public ProjectObject projectObject;

	public void Select() {
        if (Controller.instance.pointerMode == PointerMode.SELECT) {
			if (!ObjectSelection.instance.selectingPrereqs) {
				if (!ObjectSelection.instance.IsObjectSelected (projectObject)) {
					ObjectSelection.instance.AddSelectedObject (projectObject);
				} else {
					ObjectSelection.instance.RemoveSelectedObject (projectObject);
				}
			} else {
				if (projectObject.activities.Count > 1) {
					ObjectSelection.instance.AddSelectedPrereq (projectObject);
				} else {
					if (!ObjectSelection.instance.IsPrereqObjSelected (projectObject)) {
						ObjectSelection.instance.AddSelectedPrereq (projectObject);
					} else {
						ObjectSelection.instance.RemoveSelectedPrereq (projectObject, projectObject.activities [0]);
					}
				}
			}
		}
	}

	public void OnHover(){
		projectObject.SetMaterial (MaterialManager.instance.hoverMaterial);
	}

	public void OnHoverExit() {
		if (ObjectSelection.instance.IsObjectSelected (projectObject)) {
			projectObject.SetMaterial (MaterialManager.instance.selectionMaterial);
		} else if (ObjectSelection.instance.IsPrereqObjSelected (projectObject)) {
			projectObject.SetMaterial (MaterialManager.instance.prereqMaterial);
		} else {
			projectObject.SetMaterial (projectObject.defaultMaterial);
			if (projectObject.isScheduled) {
				projectObject.Fade ();
			}
		}
	}

	public void HighlightObject(){
		projectObject.SetMaterial (MaterialManager.instance.selectionMaterial);
	}

	public void UnHighlightObject() {
		projectObject.SetMaterial (projectObject.defaultMaterial);
	}

	public void HighlightObjectAsPrereq(){
		projectObject.SetMaterial (MaterialManager.instance.prereqMaterial);
	}

	public void UnHighlightObjectAsPrereq() {
		projectObject.SetMaterial (projectObject.defaultMaterial);
		if (projectObject.isScheduled) {
			projectObject.Fade ();
		}
	}
}