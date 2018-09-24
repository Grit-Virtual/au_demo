using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerLogin : MonoBehaviour {

	public static PlannerLogin instance;

	public Animator animator;
	public EmailInput emailInput;
	public PasswordInput p;
	public AuthCodeInput authCodeInput;
	public GameObject mainMenu;

	void Awake () {
		instance = this;
	}

	void Start(){
		HideLoginScreen();
	}

	public void ShowLoginScreen (){
		mainMenu.SetActive(false);
		animator.SetBool("MenuShowing", true);
		MenuManager.instance.DeactivateButtons();
	}

	public void HideLoginScreen(){
		mainMenu.SetActive(true);
		animator.SetBool("MenuShowing", false);
		MenuManager.instance.ReactivateButtons();
	}

	public void Submit(){
        if (authCodeInput.shortCode != "") {
			AuthService.ShortCodeLogin (authCodeInput.shortCode, true);
		} else {
			AuthService.UserLogin (emailInput.text.text, p.p, true);
		}
		emailInput.ReceiveInput("");
		p.ReceiveInput ("");
		authCodeInput.ReceiveInput ("");
	}

}
