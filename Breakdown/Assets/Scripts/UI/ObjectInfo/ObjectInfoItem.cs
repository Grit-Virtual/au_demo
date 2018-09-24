using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInfoItem: MonoBehaviour {
	public TextMeshProUGUI keyText;
	public TextMeshProUGUI valueText;

    public void SetUpText(string key, string value) {
        keyText.text = key;
        valueText.text = value;
    }
}
