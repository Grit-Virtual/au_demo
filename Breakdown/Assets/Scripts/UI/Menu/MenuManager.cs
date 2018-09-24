using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	public static MenuManager instance;

	public enum MenuType {
		PLANNER,
		OBJECT_INFO,
		ACTIVITY_FILTER,
		SUBCONTRACTOR_FILTER,
		CATEGORY_FILTER,
		ACTIVITY,
		SETTINGS,
		LOGIN,
		NONE
	}

	MenuType currentMenu;

	public ToggleButtonUI[] toggleMenuButtons;

	public ButtonUI[] allMenuButtons;
	List<bool> menuButtonStates = new List<bool>();

	void Awake(){
		instance = this;
	}

	void Start(){
		currentMenu = MenuType.NONE;
	}

	void Update(){
		if (Controller.instance.BackButtonPress()) {
			switch(currentMenu){
				case MenuType.PLANNER:
					PlanMenuManager.instance.BackButtonPress();
					break;
				case MenuType.OBJECT_INFO:
					InfoMenu.instance.BackButtonPress();
					break;
				case MenuType.ACTIVITY:
					ActivityMenu.instance.BackButtonPress ();
					break;
			}
			if(currentMenu != MenuType.LOGIN){
				if(currentMenu != MenuType.NONE){
					MenuButtonClick((int)MenuType.NONE);
				} else {
					Confirmation.instance.ShowConfirmation(Application.Quit, null, "Exit Application?", "Quit");
				}
			}
        }
    }

	public void MenuButtonClick(int menu){
		if(currentMenu != (MenuType)menu){
			HideMenu(currentMenu);
			ShowMenu((MenuType)menu);
		} else {
			if(menu != (int)MenuType.LOGIN){
				HideMenu((MenuType)menu);
			}
		}
	}

	void ShowMenu(MenuType menu){
		ObjectSelection.instance.ObjectSelectionEnabled(false);
		switch((MenuType)menu){
			case MenuType.PLANNER:
				PlanMenuManager.instance.ShowPlanMenu();
				currentMenu = (MenuType)menu;
				break;
			case MenuType.OBJECT_INFO:
				InfoMenu.instance.ShowInfoMenu();
				break;
			case MenuType.ACTIVITY_FILTER:
				FilterMenu.instance.ShowFilterMenu(0);
				break;
			case MenuType.SUBCONTRACTOR_FILTER:
				FilterMenu.instance.ShowFilterMenu(1);
				break;
			case MenuType.CATEGORY_FILTER:
				FilterMenu.instance.ShowFilterMenu(2);
				break;
			case MenuType.ACTIVITY:
				ActivityMenu.instance.ShowActivityMenu();
				break;
			case MenuType.SETTINGS:
				SettingsMenu.instance.ShowSettingsMenu();
				break;
			case MenuType.LOGIN:
				PlannerLogin.instance.ShowLoginScreen();
				break;
			case MenuType.NONE:
				ObjectSelection.instance.ObjectSelectionEnabled(true);
				break;
		}
		if((int)menu <= 5){
			if(!toggleMenuButtons[(int)menu].active){
				toggleMenuButtons[(int)menu].SetActive(true);
			}
		}
		currentMenu = (MenuType)menu;
	}

	void HideMenu(MenuType menu){
		switch((MenuType)menu){
			case MenuType.PLANNER:
				PlanMenuManager.instance.HidePlannerMenu();
				break;
			case MenuType.OBJECT_INFO:
				InfoMenu.instance.HideInfoMenu();
				break;
			case MenuType.ACTIVITY_FILTER:
				FilterMenu.instance.HideFilterMenu();
				break;
			case MenuType.SUBCONTRACTOR_FILTER:
				FilterMenu.instance.HideFilterMenu();
				break;
			case MenuType.CATEGORY_FILTER:
				FilterMenu.instance.HideFilterMenu();
				break;
			case MenuType.ACTIVITY:
				ActivityMenu.instance.HideActivityMenu();
				break;
			case MenuType.SETTINGS:
				SettingsMenu.instance.HideSettingsMenu();
				break;
			case MenuType.LOGIN:
				PlannerLogin.instance.HideLoginScreen();
				break;
			case MenuType.NONE:
				break;
		}
		if((int)menu <= 4){
			if(toggleMenuButtons[(int)menu].active){
				toggleMenuButtons[(int)menu].SetActive(false);
			}
		}
		currentMenu = MenuType.NONE;
		ObjectSelection.instance.ObjectSelectionEnabled(true);
	}

	public void DeactivateButtons(){
		menuButtonStates.Clear();
		foreach(ButtonUI button in allMenuButtons){
			menuButtonStates.Add(button.disabled);
			button.SetDisabled(true);
		}
	}

	public void ReactivateButtons(){
		if(menuButtonStates.Count > 0){
			for(int i = 0; i < allMenuButtons.Length; i++){
				allMenuButtons[i].SetDisabled(menuButtonStates[i]);
			}
			menuButtonStates.Clear();
		}
	}
}
