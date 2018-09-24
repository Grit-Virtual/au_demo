using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmailInput : MonoBehaviour, KeyboardTextInput {

	public TextMeshProUGUI text;
	public GameObject placeholderText;
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
			placeholderText.SetActive (true);
		} else {
			placeholderText.SetActive (false);
			text.text = input;
		}
	}

	public void ShowKeyboard(){
		keyboardManager.ShowKeyboard (this, text.text, 50, KeyboardType.EMAIL);
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
