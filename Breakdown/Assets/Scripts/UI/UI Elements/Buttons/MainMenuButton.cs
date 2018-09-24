using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour {
	public MainMenuButton[] mainMenuButtons;
	public Animator animator;
	public bool active;

	void Start(){
		if(active){
			Click();
		}
		animator.SetBool("Interactable", true);
	}

	public void Click(){
		DeactivateButtons();
		Activate ();
	}

	void DeactivateButtons(){
		foreach (MainMenuButton button in mainMenuButtons) {
			button.Deactivate ();
		}
	}

	public void Deactivate(){
		active = false;
		animator.SetBool ("Toggle", false);
	}

	void Activate(){
		active = true;
		animator.SetBool ("Toggle", true);
	}
}
