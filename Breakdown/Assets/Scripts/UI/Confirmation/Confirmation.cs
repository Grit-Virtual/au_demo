using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Confirmation : MonoBehaviour {

	public static Confirmation instance;

	public GameObject confirmationBox;
	public TextMeshProUGUI messageText;
	public TextMeshProUGUI confirmText;
	public TextMeshProUGUI cancelText;

	public delegate void Action();
	Action confirmAction;
	Action cancelAction;

	void Awake(){
		instance = this;
	}

	void Start(){
		confirmationBox.SetActive (false);
	}

	void Update(){
		if(confirmationBox.activeInHierarchy){
			if(Controller.instance.BackButtonPress()){
				CancelButtonClick();
			}
		}
	}

	public void ShowConfirmation(Action confirmAction, Action cancelAction, string message, string confirm = "Confirm", string cancel ="Cancel"){
		if(MenuManager.instance != null){
			MenuManager.instance.MenuButtonClick ((int)MenuManager.MenuType.NONE);
			MenuManager.instance.DeactivateButtons();
		}
		this.confirmAction = confirmAction;
		this.cancelAction = cancelAction;
		messageText.text = message;
		confirmText.text = confirm;
		cancelText.text = cancel;
		confirmationBox.SetActive (true);
		if(ObjectSelection.instance != null){
			ObjectSelection.instance.ObjectSelectionEnabled(false);
		}
	}

	public void CancelButtonClick(){
		HideConfirmation();
		if(cancelAction != null){
			cancelAction ();
		}
	}
	
	public void ConfirmButtonClick(){
		HideConfirmation ();
		confirmAction ();
	}

	public void HideConfirmation(){
		confirmationBox.SetActive (false);
		if(ObjectSelection.instance != null){
			ObjectSelection.instance.ObjectSelectionEnabled(true);
		}
		if(MenuManager.instance != null){
			MenuManager.instance.ReactivateButtons();
		}
	}
}