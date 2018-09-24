using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationMenu : MonoBehaviour {

	public static DurationMenu instance;

	public Animator animator;
	
	public GameObject defaultForm;
	public GameObject overrideForm;

	public RadioButtonUI[] hourRadioButtons;
	public RadioButtonUI[] minRadioButtons;
	public PlannerFormNumberInput durationTextInput;
	public ButtonUI nextButton;
	
	float durationHours = 0;
	float durationMins = 0;

	void Awake(){
		instance = this;
	}

	void Start () {
		animator.SetInteger("State", 0);
	}

	public void ShowDurationMenu(){
		animator.SetInteger("State", 3);
	}

	public void HideDurationMenu(){
		animator.SetInteger("State", 0);
	}

	public void NextMenu(){
		animator.SetInteger("State", 2);
	}

	public void SelectHour(int hour) {
		if (!hourRadioButtons [hour - 1].active) {
			durationHours = hour;
		} else {
			durationHours = 0;
		}
		durationTextInput.ReceiveInputFromForm ((durationHours + durationMins).ToString ());
		nextButton.SetDisabled(durationHours + durationMins <= 0);
	}

	public void OverrideInputHour(float input){
		durationHours = input;
		if (durationHours >= 1 && durationHours <= 4) {
			if (!hourRadioButtons [(int)durationHours - 1].active) {
				hourRadioButtons [(int)durationHours - 1].Click ();
			}
		} else {
			foreach (RadioButtonUI hourButton in hourRadioButtons) {
				hourButton.SetActive (false);
			}
		}
		nextButton.SetDisabled(durationHours + durationMins <= 0);
	}

	public void SelectMin(float min) {
		int index = GetMinButtonIndex (min / 100);
		if (index != -1) {
			if (!minRadioButtons [index].active) {
				durationMins = min / 100;
			} else {
				durationMins = 0;
			}
		}
		durationTextInput.ReceiveInputFromForm ((durationHours + durationMins).ToString());
		nextButton.SetDisabled(durationHours + durationMins <= 0);
	}

	public void OverrideInputMin(float input){
		durationMins = RoundMinInput(input);
		if (durationMins >= 1) {
			OverrideInputHour (durationHours + 1);
			durationMins = 0;
			foreach (RadioButtonUI minButton in minRadioButtons) {
				minButton.SetActive (false);
			}
		} else if(durationMins <= 0){
			foreach (RadioButtonUI minButton in minRadioButtons) {
				minButton.SetActive (false);
			}
		} else {
			int index = 0;
			if (durationMins == 0.5f) {
				index = 1;
			} else if(durationMins == 0.75f){
				index = 2;
			}
			if (!minRadioButtons [index].active) {
				minRadioButtons [index].Click ();
			}
		}
		durationTextInput.ReceiveInputFromForm ((durationHours + durationMins).ToString());
		nextButton.SetDisabled(durationHours + durationMins <= 0);
	}

	float RoundMinInput(float input){
		if (input <= 0) {
			return 0;
		} else if (input <= 0.25f) {
			return 0.25f;
		} else if (input <= 0.5f) {
			return 0.5f;
		} else if (input <= 0.75f) {
			return 0.75f;
		} else {
			return 1;
		}
	}

	int GetMinButtonIndex(float mins){
		if (mins == 0.25f) {
			return 0;
		} else if (mins == 0.5f) {
			return 1;
		} else if (mins == 0.75f) {
			return 2;
		} else {
			return -1;
		}
	}

	public void ShowDefaultForm(){
		defaultForm.SetActive (true);
		overrideForm.SetActive (false);
	}

	public void ShowOverrideForm(){
		defaultForm.SetActive (false);
		overrideForm.SetActive (true);
	}

	public void ResetForm(){
		durationHours = 0;
		durationTextInput.ReceiveInput("");
		foreach(RadioButtonUI button in hourRadioButtons){
			button.SetActive(false);
		}
		durationMins = 0;
		foreach(RadioButtonUI button in minRadioButtons){
			button.SetActive(false);
		}
		nextButton.SetDisabled(true);
	}

	public float GetDurationInput(){
		return durationHours + durationMins;
	}
}
