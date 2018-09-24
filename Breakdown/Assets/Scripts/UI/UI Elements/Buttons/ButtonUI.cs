using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonUI : MonoBehaviour {

	public Animator animator;
	EventTrigger eventTrigger;
	public bool disabled;
	// public bool animDisabled;

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
		SetDisabled(disabled);
	}

	public void SetDisabled(bool disabled){
		EventTrigger.Entry clickTriggerEntry = eventTrigger.triggers.Find(t => t.eventID == EventTriggerType.PointerClick);
		if(clickTriggerEntry != null && clickTriggerEntry.callback.GetPersistentEventCount() > 0){
			if(disabled){
				for(int i = 0; i < clickTriggerEntry.callback.GetPersistentEventCount(); i++){
					clickTriggerEntry.callback.SetPersistentListenerState(i, UnityEventCallState.Off);
				}
			} else {
				for(int i = 0; i < clickTriggerEntry.callback.GetPersistentEventCount(); i++){
					clickTriggerEntry.callback.SetPersistentListenerState(i, UnityEventCallState.RuntimeOnly);
				}
			}
		}
		this.disabled = disabled;
		animator.SetBool("Disabled", disabled);
	}

	public void Hover () {
		if(!disabled){
			animator.SetBool ("Hover", true);
		}
	}
	
	public void UnHover () {
		animator.SetBool ("Hover", false);
    }
	
	public virtual void Click() {
		if(!disabled){
			animator.SetTrigger ("Click");
		}
	}

	public virtual void OnEnable() {
		animator.SetBool("Disabled", disabled);
	}
}
