using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListSelectorUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Animator animator;
	public bool radio;
	bool active;

	public FilterSelector filterSelector;
	
	public void Click(){
		active = !active;
		animator.SetBool ("Active", active);
		if(filterSelector){
			filterSelector.Click();
		}
	}

	public void OnPointerEnter(PointerEventData eventData){
		animator.SetBool ("Hover", true);
	}

	public void OnPointerExit(PointerEventData eventData){
		animator.SetBool ("Hover", false);
	}

	public void SetActive(bool activeState){
		active = activeState;
		animator.SetBool ("Active", active);
	}
}
