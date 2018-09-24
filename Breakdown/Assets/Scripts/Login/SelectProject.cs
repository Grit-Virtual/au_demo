using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ControllerSelection;
using TMPro;

public class SelectProject : MonoBehaviour {

	public static SelectProject instance;

	public GameObject projectListScreen;
	public Transform projectSelectorsParent;
	public GameObject projectSelectorPrefab;

	public List<Project> projectList = new List<Project>();

	public TextMeshProUGUI nameText;

	public List<GameObject> projectSelectors = new List<GameObject>();

	// Use this for initialization
	void Awake () {
		instance = this;
	}

	void Start(){
		HideProjectSelectScreen ();
	}

	public void PopulateProjectSelectionList(){
		ClearProjectSelectors();
		ProjectService.PopulateProjectSelectionList ();
	}

	void SortProjects(){
		projectList.Sort ((p1, p2) => p1.name.CompareTo(p2.name));
	}

	public void AddProjectToList (Project project){
		projectList.Add (project);
	}

	public void PopulateSelectors (){
		SortProjects ();
		foreach (Project project in projectList) {
			GameObject projectEntry = (GameObject)Instantiate (projectSelectorPrefab);
			projectSelectors.Add(projectEntry);
			projectEntry.transform.SetParent (projectSelectorsParent, false);
			ProjectSelector projectSelector = projectEntry.GetComponent<ProjectSelector> ();
			projectSelector.Initialize (project);
		}
	}

	public void ShowProjectSelectScreen(){
		projectListScreen.SetActive (true);
	}

	public void HideProjectSelectScreen(){
		projectListScreen.SetActive (false);
	}

	public void LogoutButtonClick(){
		if (File.Exists (Application.persistentDataPath + "/auth.grit")) {
			File.Delete (Application.persistentDataPath + "/auth.grit");
		}
		Utils_Prefs.ClearPrefs ();
		HideProjectSelectScreen ();
		Login.instance.ShowLoginScreen ();
	}

	public void SetNameText(string name){
		nameText.text = name;
	}

	void ClearProjectSelectors(){
		foreach (GameObject projectSelector in projectSelectors) {
			Destroy(projectSelector);
		}
		projectSelectors.Clear();
		projectList.Clear();
	}
}
