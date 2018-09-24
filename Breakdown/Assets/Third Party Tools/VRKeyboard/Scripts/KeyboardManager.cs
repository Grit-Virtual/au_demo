/***
 * Author: Yunhan Li 
 * Any issue please contact yunhn.lee@gmail.com
 ***/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class KeyboardManager : MonoBehaviour {

    [Header("User defined")]
    [Tooltip("If the character is uppercase at the initialization")]
    public bool isUppercase = false;
	public bool showingSymbols = false;
    public int maxInputLength;
    
	[Header("Email Keyboard")]
	public GameObject emailKeyboard;
	public TextMeshProUGUI emailInputText;
	public TextMeshProUGUI emailSwapButtonText;
	public Transform emailCharacters;
	public Transform emailSymbols;

	[Header("Number Pad Keyboard")]
	public GameObject numPadKeyboard;
	public TextMeshProUGUI numPadInputText;
	public Transform numPadCharacters;


	// text to 
	KeyboardTextInput keyboardTextInput;

	private Dictionary<GameObject, TextMeshProUGUI> keysDictionary = new Dictionary<GameObject, TextMeshProUGUI>();

    private bool capslockFlag;

	KeyboardType currentType;

    private void Awake() {
		if (emailKeyboard != null) {
			for (int i = 0; i < emailCharacters.childCount; i++) {
				GameObject key = emailCharacters.GetChild(i).gameObject;
				TextMeshProUGUI _text = key.GetComponentInChildren<TextMeshProUGUI>();
				keysDictionary.Add(key, _text);
				key.GetComponent<Button>().onClick.AddListener(() => {
					GenerateInput(_text.text);
				});
			}

			for (int i = 0; i < emailSymbols.childCount; i++) {
				GameObject key = emailSymbols.GetChild(i).gameObject;
				TextMeshProUGUI _text = key.GetComponentInChildren<TextMeshProUGUI>();
				keysDictionary.Add(key, _text);

				key.GetComponent<Button>().onClick.AddListener(() => {
					GenerateInput(_text.text);
				});
			}
		}

		if (numPadKeyboard != null) {
			for (int i = 0; i < numPadCharacters.childCount; i++) {
				GameObject key = numPadCharacters.GetChild (i).gameObject;
				TextMeshProUGUI _text = key.GetComponentInChildren<TextMeshProUGUI> ();
				keysDictionary.Add (key, _text);

				key.GetComponent<Button> ().onClick.AddListener (() => {
					GenerateInput (_text.text);
				});
			}
		}
        capslockFlag = isUppercase;
        CapsLock();
		HideKeyboards ();
    }

    public void Backspace() {
		if (GetInputText().text.Length > 0) {
			GetInputText().text = GetInputText().text.Remove(GetInputText().text.Length - 1);
        } else {
            return;
        }
    }

    public void Clear() {
		if(GetInputText() != null){
			GetInputText().text = "";
		}
    }

    public void CapsLock() {
        if (capslockFlag) {
            foreach (var pair in keysDictionary) {
                pair.Value.text = ToUpperCase(pair.Value.text);
            }
        } else {
            foreach (var pair in keysDictionary) {
                pair.Value.text = ToLowerCase(pair.Value.text);
            }
        }
        capslockFlag = !capslockFlag;
    }

    public void GenerateInput(string s) {
		if (GetInputText().text.Length >= maxInputLength) { return; }
		GetInputText().text += s;
    }

    private string ToLowerCase(string s) {
        return s.ToLower();
    }

    private string ToUpperCase(string s) {
        return s.ToUpper();
    }

	public void SubmitText(){
		keyboardTextInput.ReceiveInput (GetInputText().text);
		HideKeyboards ();
	}

	public void SwapKeyboards(){
		showingSymbols = !showingSymbols;
		switch (currentType) {
		case KeyboardType.EMAIL:
			if (showingSymbols) {
				emailSymbols.gameObject.SetActive (true);
				emailCharacters.gameObject.SetActive (false);
				emailSwapButtonText.text = "ABC";
			} else {
				emailSymbols.gameObject.SetActive (false);
				emailCharacters.gameObject.SetActive (true);
				emailSwapButtonText.text = "#$!&";
			}
			break;
		}
	}

	public void ShowKeyboard(KeyboardTextInput textInput, string initialValue, int maxCharacters, KeyboardType keyboardType){
		currentType = keyboardType;
		GetInputText().text = initialValue;
		showingSymbols = false;
		maxInputLength = maxCharacters;
		keyboardTextInput = textInput;
		if(emailKeyboard != null){
			emailKeyboard.SetActive (false);
		}
		if (numPadKeyboard != null) {
			numPadKeyboard.SetActive (false);
		}
		switch (keyboardType) {
		case KeyboardType.EMAIL:
			emailKeyboard.SetActive (true);
			break;
		case KeyboardType.NUMBER:
			numPadKeyboard.SetActive (true);
			break;
		}
	}

	public void HideKeyboards(){
		keyboardTextInput = null;
		Clear ();
		if(emailKeyboard != null){
			emailKeyboard.SetActive (false);
		}
		if (numPadKeyboard != null) {
			numPadKeyboard.SetActive (false);
		}
	}

	TextMeshProUGUI GetInputText(){
		switch(currentType){
		case KeyboardType.EMAIL:
			return emailInputText;
		case KeyboardType.NUMBER:
			return numPadInputText;
		}
		return emailInputText;
	}
}

public enum KeyboardType {
	EMAIL,
	NUMBER
}