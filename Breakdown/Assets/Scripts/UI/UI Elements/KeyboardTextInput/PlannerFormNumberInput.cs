using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlannerFormNumberInput : MonoBehaviour, KeyboardTextInput {

	public PlannerFormInputType plannerFormInputType;
	public TextMeshProUGUI text;
	public int maxCharacters;
	public Image backgroundImage;
	public Image accentImage;
	bool hovered;
	public Animator animator;
	KeyboardManager keyboardManager;

	void Start(){
		keyboardManager = GetComponentInParent<KeyboardManager> ();
	}

	public void ReceiveInput(string input){
		if (input == "") {
			text.text = input;
		} else {
			text.text = input;
			switch (plannerFormInputType) {
			case PlannerFormInputType.CREW_SIZE:
				CrewSizeMenu.instance.OverrideInputCrewSize (input);
				break;
			case PlannerFormInputType.DURATION:
				float duration = float.Parse (input);
				DurationMenu.instance.OverrideInputHour (Mathf.Floor (duration));
				DurationMenu.instance.OverrideInputMin (duration % Mathf.Floor(duration));
				break;
			case PlannerFormInputType.LEAD:
				LeadLagMenu.instance.SelectLeadTime (float.Parse(input));
				break;
			case PlannerFormInputType.LAG:
				LeadLagMenu.instance.selectLagTime (float.Parse(input));
				break;
			}
		}
	}

	public void ReceiveInputFromForm(string input){
		text.text = input;
	}

	public void ShowKeyboard(){
		keyboardManager.ShowKeyboard (this, text.text, maxCharacters, KeyboardType.NUMBER);
	}

	public void Hover(){
		hovered = true;
		animator.SetBool ("Hover", true);
	}

	public void UnHover(){
		hovered = false;
		animator.SetBool ("Hover", false);
	}
}

public enum PlannerFormInputType {
	CREW_SIZE,
	DURATION,
	LEAD,
	LAG
}
