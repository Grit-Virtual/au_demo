using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveFilter : MonoBehaviour {

	[HideInInspector]
    public string id;
	public TextMeshProUGUI buttonText;

	public void SetUp (string id, string label) {
		this.id= id;
        buttonText.text = label;
    }

    public void RemoveButtonClick(){
		FilterMenu.instance.RemoveActiveFilter (this);
	}
}
