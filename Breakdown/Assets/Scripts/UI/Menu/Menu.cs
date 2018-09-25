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

	public void DeselectButtonClick(){
		PlanMenuManager.instance.planning = false;
		ObjectSelection.instance.EndSelectingPrereqs();
		ObjectSelection.instance.DeselectSelection ();
		ObjectSelection.instance.DeselectPrereqs();
		FilterMenu.instance.RevertToFilter ();
		ActivityMenu.instance.DeselectActivity ();
		MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.NONE);
	}
	
	public void UpdateLoadingText(){
	}

	public void ResetHeightButtonClick(){
		ArcTeleporter.instance.ResetHeight();
		Controller.instance.ResetElevation();
	}
}
