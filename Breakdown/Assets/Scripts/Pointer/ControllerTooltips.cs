using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTooltips : MonoBehaviour {

	public static ControllerTooltips instance;

	public GameObject modeSelect;
	public GameObject modeMove;
	public GameObject select;
	public GameObject holdMove;
	public GameObject releaseJump;

	public Image holdTimer;

	void Awake(){
		instance = this;
        ShowModeSelect();
	}

	public void MoveModeTriggerPressed(bool pressed){
		if (pressed) {
			if (!releaseJump.activeInHierarchy) {
				holdMove.SetActive (false);
				releaseJump.SetActive (true);
				select.SetActive (false);
			}
		} else {
			if (releaseJump.activeInHierarchy) {
				holdMove.SetActive (true);
				releaseJump.SetActive (false);
				select.SetActive (false);
			}
		}
	}

	public void ShowModeMove(){
		modeSelect.SetActive (false);
		modeMove.SetActive (true);
		holdMove.SetActive (false);
		releaseJump.SetActive (false);
		select.SetActive(true);
	}

	public void ShowModeSelect(){
		modeMove.SetActive (false);
		modeSelect.SetActive (true);
		holdMove.SetActive (true);
		releaseJump.SetActive (false);
		select.SetActive(false);
	}

	public void SetHoldTimer(float time){
		holdTimer.fillAmount = time / 0.5f;
	}
}
