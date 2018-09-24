using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	public static LoadingScreen instance;
	public GameObject loadingScreen;

	// Use this for initialization
	void Awake () {
		instance = this;
	}

	public void ShowLoadingScreen () {
		if (!loadingScreen.activeInHierarchy) {
			loadingScreen.SetActive (true);
		}
	}

	public void HideLoadingScreen () {
		loadingScreen.SetActive (false);
	}
}
