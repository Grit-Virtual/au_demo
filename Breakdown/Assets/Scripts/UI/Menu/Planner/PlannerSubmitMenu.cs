using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlannerSubmitMenu : MonoBehaviour {

	public static PlannerSubmitMenu instance;

	public Animator animator;

	public DefaultInput nameInput;

	public TextMeshProUGUI crewSizeText, durationText, leadText, lagText;


	void Awake(){
		instance = this;
	}

	void Start () {
		animator.SetInteger("State", 0);
	}

	public void ShowSubmitMenu(){
		crewSizeText.text = CrewSizeMenu.instance.GetCrewSizeInput().ToString();
		durationText.text = DurationMenu.instance.GetDurationInput().ToString();
		leadText.text = LeadLagMenu.instance.GetLeadInput().ToString();
		lagText.text = LeadLagMenu.instance.GetLagInput().ToString();
		animator.SetInteger("State", 3);
	}

	public void HideSubmitMenu(){
		animator.SetInteger("State", 0);
	}

	public void NextMenu(){
		animator.SetInteger("State", 2);
	}

	public string GetNameInput(){
		return nameInput.text.text;
	}
	public void SetStepName(string stepName){
		nameInput.ReceiveInput (stepName);
	}
}
