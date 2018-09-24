using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRectUI : MonoBehaviour, IEndDragHandler, IBeginDragHandler {

	public ScrollRect scrollRect;
	int selectedItem;
	int numberOfItems;

	Vector2 targetPosition;
	bool dragging;

	public Transform scrollBar;
	public GameObject scrollIndicatorPrefab;
	List<ScrollIndicator> scrollIndicators = new List<ScrollIndicator>();

	void Awake(){
		if(scrollRect == null) {
			scrollRect = GetComponent<ScrollRect>();
		}
	}

	// Use this for initialization
	public void Initialize () {
		numberOfItems = scrollRect.content.childCount;
		selectedItem = 0;
		for (int i = 0; i < numberOfItems; i++) {
			GameObject scrollIndicatorGo = (GameObject)Instantiate (scrollIndicatorPrefab);
			scrollIndicatorGo.transform.SetParent (scrollBar, false);
			ScrollIndicator scrollIndicator = scrollIndicatorGo.GetComponent<ScrollIndicator> ();
			scrollIndicators.Add (scrollIndicator);
			if (i == 0) {
				scrollIndicator.SetActive ();
			}
		}
	}

	public void OnBeginDrag(PointerEventData data){
		dragging = true;
	}

	public void OnEndDrag(PointerEventData data){
		dragging = false;
		scrollIndicators [selectedItem].SetInactive ();
		selectedItem = Mathf.Min((int)Mathf.Round(scrollRect.horizontalNormalizedPosition * numberOfItems), scrollRect.content.childCount - 1);
		targetPosition = new Vector2(
			((Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position) -
				(Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.GetChild(selectedItem).position)).x + 150f, scrollRect.content.anchoredPosition.y);
		scrollIndicators [selectedItem].SetActive ();
	}

	// Update is called once per frame
	void Update () {
		if (!dragging) {
			if (Vector2.Distance(scrollRect.content.anchoredPosition, targetPosition) > 1) {
				scrollRect.content.anchoredPosition = Vector2.Lerp (scrollRect.content.anchoredPosition, targetPosition, Time.deltaTime * 5);
			} else {
				scrollRect.content.anchoredPosition = targetPosition;
			}
		}
	}
}
