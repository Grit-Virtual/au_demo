using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface KeyboardTextInput {
	void ReceiveInput(string input);
	void ShowKeyboard ();
	void Hover ();
	void UnHover();
}
