using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadLagMenu : MonoBehaviour {

	public static LeadLagMenu instance;

	public Animator animator;

	public PlannerFormNumberInput leadTimeTextInput;
	public PlannerFormNumberInput lagTimeTextInput;
	
	float leadTime = 0;
	float lagTime = 0;

	void Awake(){
		instance = this;
	}

	void Start () {
		animator.SetInteger("State", 0);
	}

	public void ShowLeadLagMenu(){
		animator.SetInteger("State", 3);
	}

	public void HideLeadLagMenu(){
		animator.SetInteger("State", 0);
	}

	public void NextMenu(){
		animator.SetInteger("State", 2);
	}

	public void SelectLeadTime(float lead) {
		leadTime = lead;
	}

	public void selectLagTime(float lag) {
		lagTime = lag;
	}

	public void ResetForm(){
		leadTime = 0;
		leadTimeTextInput.ReceiveInput("");
		lagTime = 0;
		lagTimeTextInput.ReceiveInput("");
	}

	public float GetLeadInput(){
		return leadTime;
	}

	public float GetLagInput(){
		return lagTime;
	}
}
