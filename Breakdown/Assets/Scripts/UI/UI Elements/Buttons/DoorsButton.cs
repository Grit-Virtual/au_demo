using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorsButton : MonoBehaviour {

	public Animator animator;
	public Sprite showDoorsSprite, hideDoorsSprite;
	public Image buttonImage;
	public TextMeshProUGUI buttonText;

	bool showingDoors;

	public void Hover () {
		animator.SetBool ("Hover", true);
	}
	
	public void UnHover () {
		animator.SetBool ("Hover", false);
    }
	
	public void Click() {
		animator.SetTrigger ("Click");
		showingDoors = !showingDoors;
		FilterMenu.instance.ShowHideDoors(showingDoors);
		if(showingDoors){
			buttonImage.sprite = hideDoorsSprite;
			buttonText.text = "Hide Doors";
		} else {
			buttonImage.sprite = showDoorsSprite;
			buttonText.text = "Show Doors";
		}
	}
}
