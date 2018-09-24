using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FilterSelector : MonoBehaviour {

	[HideInInspector]
    public string id;
	[HideInInspector]
    bool active;
	public TextMeshProUGUI buttonText;

	public ListSelectorUI listSelector;

	public void SetUp (string id, string label, bool active = false) {
		this.id = id;
        buttonText.text = label;
        if (active){
			this.active = true;
		}
	}

	public void Click(){
		active = !active;
		if (active) {
			FilterMenu.instance.SelectFilter (id, buttonText.text);
		} else {
			FilterMenu.instance.DeselectFilter (id);
		}
	}

	public void Deactivate(){
		active = false;
		listSelector.SetActive (false);
	}
}
