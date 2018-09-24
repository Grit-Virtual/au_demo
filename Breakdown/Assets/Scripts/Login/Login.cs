using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Restifizer;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class Login : MonoBehaviour {

	public static Login instance;

	public EmailInput emailInput;
	public PasswordInput p;
	public AuthCodeInput authCodeInput;

	public TextMeshProUGUI envLabel;

	public GameObject loginScreen;

	public GameObject defaultObject, easterEggObject, loginCanvases;

	void Awake () {
		instance = this;
		if(Random.value < 0f){
			defaultObject.SetActive(false);
			easterEggObject.SetActive(true);
		}
	}

	void Start() {
		loginScreen.SetActive(false);
		if (File.Exists (Application.persistentDataPath + "/auth.grit")) {
			LoadingScreen.instance.ShowLoadingScreen ();
			try {
				LoadAuthData ();
			} catch (System.Exception e) {
				Debug.LogError (e);
				ShowLoginScreen ();
			}
		} else {
			ShowLoginScreen ();
		}
	}

	void Update(){
		if (Controller.instance.BackButtonPress()) {
			loginCanvases.SetActive(false);
			Confirmation.instance.ShowConfirmation(Application.Quit, ShowLoginCanvases, "Exit Application?", "Quit");
        }
    }

	public void ShowLoginCanvases(){
		loginCanvases.SetActive(true);
	}

	void LoadAuthData(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = new FileStream (Application.persistentDataPath + "/auth.grit", FileMode.Open);
		AuthInfo authInfo = bf.Deserialize (fs) as AuthInfo;
		fs.Close ();
		AuthService.CheckToken (authInfo);
	}

	public void OnLoginSubmit(){
		HideLoginScreen ();
        if (authCodeInput.shortCode != "") {
			AuthService.ShortCodeLogin (authCodeInput.shortCode);
		} else {
			AuthService.UserLogin (emailInput.text.text, p.p);
		}
		emailInput.ReceiveInput("");
		p.ReceiveInput ("");
		authCodeInput.ReceiveInput ("");
	}

	public void ShowLoginScreen (){
		loginScreen.SetActive(true);
		LoadingScreen.instance.HideLoadingScreen();
	}

	public void HideLoginScreen(){
		loginScreen.SetActive(false);
		LoadingScreen.instance.ShowLoadingScreen();
	}

	public void SetEnvironmentLabel(){
		if (RestifizerManager.Instance.environment == RestifizerManager.Environment.LOCAL) {
			envLabel.text = "LOCAL";
		} else if(RestifizerManager.Instance.environment == RestifizerManager.Environment.DEVELOP){
			envLabel.text = "DEV";
		} else if(RestifizerManager.Instance.environment == RestifizerManager.Environment.STAGING){
			envLabel.text = "STAGING";
		} else if(RestifizerManager.Instance.environment == RestifizerManager.Environment.PRODUCTION){
			envLabel.text = "";
		}
	}
}
