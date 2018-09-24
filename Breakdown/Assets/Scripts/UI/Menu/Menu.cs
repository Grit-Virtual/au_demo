using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour {

	public static Menu instance;

	bool loading = true;

	public TextMeshProUGUI userNameText;
	public TextMeshProUGUI timeText;
	public TextMeshProUGUI loadingText;
	public Transform loadingImage;

	void Awake () {
		instance = this;
	}

	void Start(){
		if(Utils_Prefs.GetUserName() != ""){
			userNameText.text = Utils_Prefs.GetUserName();
		} else {
			userNameText.text = Utils_Prefs.GetUserEmail();
		}

		timeText.text = System.DateTime.Now.ToShortTimeString();
	}

	void Update(){
		if (timeText.text != System.DateTime.Now.ToShortTimeString ()) {
			timeText.text = System.DateTime.Now.ToShortTimeString ();
		}
		if(loading){
			loadingImage.Rotate(Vector3.back * 500 * Time.deltaTime);
		}
	}

	// public void PlanButtonClick(){
	// 	PlanMenu.instance.ShowPlanMenu();
	// }

	public void DeselectButtonClick(){
		PlanMenuManager.instance.planning = false;
		ObjectSelection.instance.EndSelectingPrereqs();
		ObjectSelection.instance.DeselectSelection ();
		ObjectSelection.instance.DeselectPrereqs();
		FilterMenu.instance.RevertToFilter ();
		ActivityMenu.instance.DeselectActivity ();
		MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.NONE);
	}
	
	// public void HideAllMenus(){
	// 	ActivityMenu.instance.HideActivityMenu ();
	// 	PlanMenu.instance.HidePlanMenu ();
	// 	FilterMenu.instance.HideFilterMenu ();
	// 	InfoMenu.instance.HideInfoMenu();
	// 	SettingsMenu.instance.HideSettingsMenu ();
	// 	Confirmation.instance.HideConfirmation ();
	// 	ObjectSelection.instance.ObjectSelectionEnabled(true);
	// }

	public void UpdateLoadingText(){
		// float progress = ((float)(ProjectObjects.instance.projectObjects.Count - (ObjectLoader.instance.objectsToLoad.Count + ObjectLoader.instance.priorityObjectsToLoad.Count)) / ProjectObjects.instance.projectObjects.Count);
		float progress1 = ((float)(ProjectObjects.instance.projectObjects.Count - (ForgeService.instance.toGenerate.Count)) / ProjectObjects.instance.projectObjects.Count);
		float progress2 = ((float)(ProjectObjects.instance.projectObjects.Count - (ForgeService.instance.toGenerate.Count + ForgeService.instance.toApply.Count + ForgeService.instance.applying.Count)) / ProjectObjects.instance.projectObjects.Count);
		int percent = Mathf.RoundToInt(progress2 * 100);
		int percent2 = Mathf.RoundToInt (progress1 * 100);
		if(percent < 100){
			loading = true;
			loadingText.text = percent + "%";
			loadingText.gameObject.SetActive(true);
			loadingImage.gameObject.SetActive(true);
		} else {
			loading = false;
			loadingText.gameObject.SetActive(false);
			loadingImage.gameObject.SetActive(false);
		}
	}

	public void ResetHeightButtonClick(){
		ArcTeleporter.instance.ResetHeight();
		Controller.instance.ResetElevation();
	}
}
