using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadioButtonUI : MonoBehaviour {

	Animator animator;
	EventTrigger eventTrigger;
	public bool active;
	public Color activeColor, inactiveColor;
	public Image buttonImage;
	public RadioButtonUI[] radioButtons;
	public bool allowDeselect;

	void Awake(){
		if (animator == null) {
			animator = GetComponent<Animator> ();
		}
		if (eventTrigger == null) {
			eventTrigger = GetComponent<EventTrigger> ();
			EventTrigger.Entry hoverTrigger = new EventTrigger.Entry ();
			hoverTrigger.eventID = EventTriggerType.PointerEnter;
			hoverTrigger.callback.AddListener ((eventData) => {Hover();});
			eventTrigger.triggers.Add (hoverTrigger);

			EventTrigger.Entry unHoverTrigger = new EventTrigger.Entry ();
			unHoverTrigger.eventID = EventTriggerType.PointerExit;
			unHoverTrigger.callback.AddListener ((eventData) => {UnHover();});
			eventTrigger.triggers.Add (unHoverTrigger);

			EventTrigger.Entry clickTrigger = new EventTrigger.Entry ();
			clickTrigger.eventID = EventTriggerType.PointerClick;
			clickTrigger.callback.AddListener ((eventData) => {Click();});
			eventTrigger.triggers.Add (clickTrigger);
		}
	}

	public void Hover () {
		animator.SetBool ("Hover", true);
	}
	
	public void UnHover () {
		animator.SetBool ("Hover", false);		
	}

	public void Click() {
		animator.SetTrigger ("Click");
		bool nowActive = active;
		if (active) {
			if (allowDeselect) {
				nowActive = false;
			}
		} else {
			nowActive = true;
		}
		DeactivateSiblings ();
		SetActive (nowActive);
	}


	void DeactivateSiblings (){
		for(int i = 0; i < radioButtons.Length; i++){
			radioButtons[i].SetActive(false);
		}
	}

	public void SetActive(bool isActive){
		active = isActive;
		if (isActive) {
			buttonImage.color = activeColor;
		} else {
			buttonImage.color = inactiveColor;
		}
	}

}
