using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewSizeMenu : MonoBehaviour {

	public static CrewSizeMenu instance;

	public Animator animator;
	
	public GameObject defaultForm;
	public GameObject overrideForm;

	public RadioButtonUI[] crewRadioButtons;
	public PlannerFormNumberInput crewSizeTextInput;
	public ButtonUI nextButton;

	int crewSize = 0;

	void Awake(){
		instance = this;
	}

	void Start () {
		animator.SetInteger("State", 0);
	}

	public void ShowCrewSizeMenu(){
		animator.SetInteger("State", 1);
	}

	public void HideCrewSizeMenu(){
		animator.SetInteger("State", 0);
	}

	public void NextMenu(){
		animator.SetInteger("State", 2);
	}

	public void SelectCrewSize(int size) {
		crewSize = size;
		crewSizeTextInput.ReceiveInputFromForm (size.ToString());
		nextButton.SetDisabled(crewSize <= 0);
	}

	public void OverrideInputCrewSize(string input){
		crewSize = int.Parse (input);
		if (crewSize >= 1 && crewSize <= 4) {
			if (!crewRadioButtons [crewSize - 1].active) {
				crewRadioButtons [crewSize - 1].Click ();
			}
		} else {
			foreach (RadioButtonUI crewButton in crewRadioButtons) {
				crewButton.SetActive (false);
			}
		}
		nextButton.SetDisabled(crewSize <= 0);
	}

	public void ResetForm(){
		crewSize = 0;
		crewSizeTextInput.ReceiveInput("");
		foreach(RadioButtonUI button in crewRadioButtons){
			button.SetActive(false);
		}
		nextButton.SetDisabled(true);
	}

	public void ShowDefaultForm(){
		defaultForm.SetActive (true);
		overrideForm.SetActive (false);
	}

	public void ShowOverrideForm(){
		defaultForm.SetActive (false);
		overrideForm.SetActive (true);
	}

	public int GetCrewSizeInput(){
		return crewSize;
	}
}
