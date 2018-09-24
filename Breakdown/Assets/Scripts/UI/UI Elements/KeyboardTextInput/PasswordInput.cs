using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PasswordInput : MonoBehaviour, KeyboardTextInput {
	public TextMeshProUGUI text;
	[HideInInspector]
	public string p;
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
			p = input;
			placeholderText.SetActive(true);
		} else {
			placeholderText.SetActive(false);
			text.text = "";
			for (int i = 0; i < input.Length; i++) {
				text.text = text.text + "*";
			}
			p = input;
			if(SceneManager.GetActiveScene().name == "Login"){
				Login.instance.OnLoginSubmit();
			} else {
				PlannerLogin.instance.Submit();
			}
		}
	}

	public void ShowKeyboard(){
		keyboardManager.ShowKeyboard (this, "", 50, KeyboardType.EMAIL);
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
