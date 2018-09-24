using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Alert : MonoBehaviour {

	public static Alert instance;

	public GameObject alertBox;
	public TextMeshProUGUI messageText;
	float alertDuration;

	bool showingAlert;
	float timer;

	void Awake(){
		instance = this;
	}

	public void ShowAlert(string message, float duration = 3f){
		messageText.text = message;
		alertBox.SetActive (true);
		alertDuration = duration;
		showingAlert = true;
	}

	void Update(){
		if (showingAlert) {
			if (timer <= alertDuration) {
				timer += Time.deltaTime;
			} else {
				alertBox.SetActive (false);
				showingAlert = false;
				timer = 0f;
			}
		}
	}
}
