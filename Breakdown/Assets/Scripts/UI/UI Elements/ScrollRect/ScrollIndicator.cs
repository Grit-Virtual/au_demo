using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollIndicator : MonoBehaviour {

	public Image image;
	public RectTransform rectTransform;
	public Color activeColor, inactiveColor;

	public void SetActive(){
		image.color = Color.red;
		rectTransform.sizeDelta = new Vector2 (12, 12);
	}

	public void SetInactive(){
		image.color = Color.gray;
		rectTransform.sizeDelta = new Vector2 (10, 10);
	}

}
