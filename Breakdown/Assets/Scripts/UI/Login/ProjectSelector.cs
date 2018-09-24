using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;
using TMPro;

public class ProjectSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[HideInInspector]
	public string projectId;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI addressText;

    public Image image;
	public GameObject hoverOutline;

	public void Initialize(Project project){
		projectId = project.id;
		addressText.text = project.address;
		nameText.text = project.name;
    }

	public void OnButtonClick(){
		LoadingScreen.instance.ShowLoadingScreen ();
		SelectProject.instance.HideProjectSelectScreen ();
		Utils_Prefs.StoreProjectId (projectId);
		SceneManager.LoadSceneAsync("Planner");
	}

	public void OnPointerEnter (PointerEventData eventData){
		hoverOutline.SetActive(true);
	}

	public void OnPointerExit (PointerEventData eventData){
		hoverOutline.SetActive(false);
	}
}
