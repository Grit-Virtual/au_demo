using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour {

	public static SettingsMenu instance;

	public Animator animator;
	// public GameObject settingsMenu;
	void Awake () {
		instance = this;
	}

	void Start(){
		// settingsMenu.SetActive (false);
		animator.SetBool("MenuShowing", false);
	}

	// void Update(){
	// 	if (Controller.instance.BackButtonPress()) {
	// 		if (animator.GetBool("MenuShowing")) {
	// 			BackButtonPress();
	// 		}
	// 	}
	// }

	// void BackButtonPress() {
	// 	HideSettingsMenu();
	// }

	public void ShowSettingsMenu(){
		// settingsMenu.SetActive (true);
		animator.SetBool("MenuShowing", true);
	}

	public void HideSettingsMenu(){
		// settingsMenu.SetActive(false);
		animator.SetBool("MenuShowing", false);
	}
	
	public void LogoutButtonClick(){
		Confirmation.instance.ShowConfirmation (Logout, BackToSettingMenu, "Are you sure you want to logout?", "Logout");
	}

	void Logout(){
		if (File.Exists (Application.persistentDataPath + "/auth.grit")) {
			File.Delete (Application.persistentDataPath + "/auth.grit");
		}
		Utils_Prefs.ClearPrefs ();
		SceneManager.LoadSceneAsync ("Login");
	}

	public void ProjectSelectButtonClick(){
		Confirmation.instance.ShowConfirmation (GoToProjectSelect, BackToSettingMenu, "Are you sure you want to select a new project?");
	}

	void GoToProjectSelect(){
		SceneManager.LoadSceneAsync ("Login");
	}

	void BackToSettingMenu(){
		MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.SETTINGS);
	}
}
