using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AuthCodeInput : MonoBehaviour, KeyboardTextInput {

	public TextMeshProUGUI text;
	public Animator animator;
	public AuthCodeInput[] authCodeInputs;
	public Image backgroundImage;
	public Image accentImage;
	public string shortCode = "";
	KeyboardManager keyboardManager;

	void Start(){
		keyboardManager = GetComponentInParent<KeyboardManager> ();
	}

	public void ReceiveInput(string input){
		if (input == "") {
			foreach (AuthCodeInput authInput in authCodeInputs) {
				authInput.text.text = input;
			}
		} else {
			input.Replace (".", "");
			for (int i = 0; i < input.Length; i++) {
				authCodeInputs [i].text.text = input [i].ToString ().ToUpper ();
				authCodeInputs [i].shortCode = input.Insert (3, "-");
			}
			if(SceneManager.GetActiveScene().name == "Login"){
			} else {
			}
		}
	}

	public void ShowKeyboard(){

		keyboardManager.ShowKeyboard (this, shortCode.Replace("-","").Replace(".",""), 6, KeyboardType.NUMBER);
	}

	public void Hover(){
		foreach (AuthCodeInput codeInput in authCodeInputs) {
			codeInput.animator.SetBool ("Hover", true);
		}
	}

	public void UnHover(){
		foreach (AuthCodeInput codeInput in authCodeInputs) {
			codeInput.animator.SetBool ("Hover", false);
		}
	}
}