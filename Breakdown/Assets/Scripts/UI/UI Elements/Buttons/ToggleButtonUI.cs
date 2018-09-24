using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButtonUI : ButtonUI {
	public ToggleButtonUI[] toggleButtons;
	public bool active;
	// bool animToggle;

	// this is important;
	public override void Click(){}

	public void SetActive(bool activeState){
		if(activeState){
			Activate();
		} else {
			Deactivate();
		}
	}

	void DeactivateButtons(){
		foreach (ToggleButtonUI button in toggleButtons) {
			button.Deactivate ();
		}
	}

	public void Deactivate(){
		active = false;
		// animToggle = false;
		animator.SetBool ("Toggle", false);
	}

	void Activate(){
		DeactivateButtons();
		active = true;
		animator.SetBool ("Toggle", true);
	}

	public override void OnEnable() {
		base.OnEnable();
		animator.SetBool("Toggle", active);
	}
}
