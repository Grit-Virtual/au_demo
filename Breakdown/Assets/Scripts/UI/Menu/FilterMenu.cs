using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum FilterType {
    ACTIVITY_TYPE,
    SUBCONTRACTOR,
    CATEGORY
}

public class FilterMenu : MonoBehaviour {

	public static FilterMenu instance;

    public Animator animator;
	// public GameObject filterMenu;

    FilterType currentFilterType;

    List<FilterSelector> filterSelectors = new List<FilterSelector>();
    public GameObject filterSelectorPrefab;
    public Transform filterSelectorParent;
    List<ActiveFilter> activeFilters = new List<ActiveFilter>();
    public GameObject activeFilterPrefab;
    public Transform activeFilterParent;
    List<string> filterSelection = new List<string>();
    List<string> filterSelectionStash = new List<string>();
    List<FilterHolder> filteredItems = new List<FilterHolder>();
   
	public TextMeshProUGUI filterSearchText;
	string filterSearch = "";

	List<Activity> filteredActivities = new List<Activity>();
    List<Subcontractor> filteredSubcontractors = new List<Subcontractor>();
    List<string> filteredCategories = new List<string>();

	private bool myWorkFilterActive = false;
    
    public ToggleButtonUI activityButton;
    public ToggleButtonUI subcontractorButton;
    public ToggleButtonUI categoryButton;
    public ToggleButtonUI myWorkButton;

    int currentFilterMenu;

    public TextMeshProUGUI menuLabel;

	void Awake(){
		instance = this;
	}

	void Start () {
		// filterMenu.SetActive (false);
		animator.SetBool("MenuShowing", false);
	}

	// void Update(){
	// 	if (Controller.instance.BackButtonPress()) {
	// 		if (animator.GetBool("MenuShowing")) {
    //             BackButtonPress();
	// 		}
	// 	}
	// }

    // public void FilterButtonClick(int filterType){
    //     if(!filterMenu.activeInHierarchy){
    //         currentFilterMenu = filterType;
    //         ShowFilterMenu(filterType);
    //     } else {
    //         if(currentFilterMenu == filterType){
    //             HideFilterMenu();
    //         } else {
    //             currentFilterMenu = filterType;
    //             ShowFilterMenu(filterType);
    //         }
    //     }
    // }

    public void ShowFilterMenu(int filterType){
        if (currentFilterType != (FilterType)filterType) {
            currentFilterType = (FilterType)filterType;
            ClearActiveFilters();
        }
        switch (currentFilterType) {
            case FilterType.ACTIVITY_TYPE:
                menuLabel.text = "Select Activity Filters";
                GetFilteredActivitySelectors();
                PopulateFilterSelectors();
                // filterMenu.SetActive(true);
		        animator.SetBool("MenuShowing", true);
                // if(!activityButton.active){
                //     activityButton.SetActive(true);
                // }
                break;
            case FilterType.SUBCONTRACTOR:
                menuLabel.text = "Select Subcontractor Filters";
                GetFilteredSubcontractorSelectors();
                PopulateFilterSelectors();
                // filterMenu.SetActive(true);
		        animator.SetBool("MenuShowing", true);
                // if(!subcontractorButton.active){
                //     subcontractorButton.SetActive(true);
                // }
                break;
            case FilterType.CATEGORY:
                menuLabel.text = "Select Category Filters";
                GetFilteredCategorySelectors();
                PopulateFilterSelectors();
                // filterMenu.SetActive(true);
		        animator.SetBool("MenuShowing", true);
                // if(!categoryButton.active){
                //     categoryButton.SetActive(true);
                // }
                break;
        }
    }

	public void HideFilterMenu(){
		// filterMenu.SetActive (false);
        animator.SetBool("MenuShowing", false);
        // if(activityButton.active){
        //     activityButton.SetActive(false);
        // }
        // if(subcontractorButton.active){
        //     subcontractorButton.SetActive(false);
        // }
        // if(categoryButton.active){
        //     categoryButton.SetActive(false);
        // }
    }

    // void BackButtonPress() {
    //     HideFilterMenu();
    // }

    void PopulateFilterSelectors(){
		ClearFilterSelectors ();
		for (int i = 0; i < filteredItems.Count; i++) {
			GameObject filterSelectorGo = (GameObject)Instantiate (filterSelectorPrefab);
			filterSelectorGo.transform.SetParent (filterSelectorParent, false);
			FilterSelector filterSelector = filterSelectorGo.GetComponent<FilterSelector> ();
			filterSelectors.Add (filterSelector);
			bool active = false;
			foreach (ActiveFilter af in activeFilters) {
				if (af.id == filteredItems[i].id) {
					active = true;
				}
			}
			filterSelector.SetUp (filteredItems[i].id, filteredItems[i].label, active);
		}
    }
		
	void GetFilteredActivitySelectors(){
        filteredActivities = Activities.instance.FilterActivities (filterSearch);
		filteredActivities.Sort ((a, b) => {
			return a.activity.CompareTo (b.activity);
		});
        filteredItems.Clear();
        foreach(Activity tc in filteredActivities) {
            filteredItems.Add(new FilterHolder(tc.activity, tc.name));
        }
	}

    void GetFilteredSubcontractorSelectors() {
        filteredSubcontractors = Subcontractors.instance.FilterSubcontractors(filterSearch);
        filteredSubcontractors.Sort((a, b) => {
            return a.name.CompareTo(b.name);
        });
        filteredItems.Clear();
        foreach (Subcontractor sub in filteredSubcontractors) {
            filteredItems.Add(new FilterHolder(sub.id, sub.name));
        }
    }

    void GetFilteredCategorySelectors() {
        filteredCategories = Categories.instance.FilterCategories(filterSearch);
        filteredCategories.Sort((a, b) => {
            return a.CompareTo(b);
        });
        filteredItems.Clear();
        foreach (string cat in filteredCategories) {
            filteredItems.Add(new FilterHolder(cat, cat));
        }
    }

	void ClearFilterSelectors(){
		foreach (FilterSelector filterSelector in filterSelectors) {
			Destroy(filterSelector.gameObject);
		}
		filterSelectors.Clear ();
	}

	public void SetFilterSearch(string searchInput){
		filterSearch = searchInput;

        switch (currentFilterType) {
            case FilterType.ACTIVITY_TYPE:
                GetFilteredActivitySelectors();
                PopulateFilterSelectors();
                break;
            case FilterType.SUBCONTRACTOR:
                GetFilteredSubcontractorSelectors();
                PopulateFilterSelectors();
                break;
            case FilterType.CATEGORY:
                GetFilteredCategorySelectors();
                PopulateFilterSelectors();
                break;
            default:
                break;
        }
	}

	public void SelectFilter(string id, string label){
		GameObject activeFilterGo = (GameObject)Instantiate (activeFilterPrefab);
		activeFilterGo.transform.SetParent (activeFilterParent, false);
		ActiveFilter activeFilter = activeFilterGo.GetComponent<ActiveFilter> ();
		activeFilters.Add (activeFilter);
		activeFilter.SetUp (id, label);
		filterSelection.Add (id);
		FilterObjects ();

        if (myWorkFilterActive) {
            myWorkFilterActive = false;
        }
    }

	public void DeselectFilter(string id){
		ActiveFilter deselectedFilter = activeFilters.Find (af => {
			return af.id == id;
		});
		activeFilters.Remove (deselectedFilter);
        Destroy(deselectedFilter.gameObject);
		filterSelection.Remove (id);
		FilterObjects ();
        if (myWorkFilterActive) {
            myWorkFilterActive = false;
        }
    }

	public void FilterObjects(){
		if (filterSelection.Count <= 0) {
			ProjectObjects.instance.ShowAllObjects();
		} else {
            switch (currentFilterType) {
                case FilterType.ACTIVITY_TYPE:
                    ProjectObjects.instance.FilterObjectsByActivity(filterSelection);
                    break;
                case FilterType.SUBCONTRACTOR:
                    ProjectObjects.instance.FilterObjectsBySubcontractor(filterSelection);
                    break;
                case FilterType.CATEGORY:
                    ProjectObjects.instance.FilterObjectsByCategory(filterSelection);
                    break;
                default:
                    break;
            }
			
		}
	}

	public void RemoveActiveFilter(ActiveFilter activeFilter){
		foreach (FilterSelector fs in filterSelectors) {
			if (fs.id == activeFilter.id) {
				fs.Deactivate ();
			}
		}
		DeselectFilter (activeFilter.id);
	}

	public void ClearActiveFilters(){
		foreach (ActiveFilter af in activeFilters) {
			Destroy (af.gameObject);
		}
		activeFilters.Clear ();
		filterSelection.Clear ();
		foreach (FilterSelector fs in filterSelectors) {
			fs.Deactivate();
		}
		FilterObjects ();
        if (myWorkFilterActive) {
            myWorkFilterActive = false;
        }
    }

	public void RevertToFilter(){
		FilterObjects ();
	}

    public void InitializeMyWorkButton(){
        myWorkButton.SetDisabled(Activities.instance.userIsGC);
    }

	public void MyWorkButtonClick(){
		if (myWorkFilterActive){
			myWorkFilterActive = false;
            myWorkButton.SetActive(false);
			ClearActiveFilters();
		} else {
            myWorkButton.SetActive(true);
            ClearActiveFilters();
            currentFilterType = FilterType.ACTIVITY_TYPE;
			foreach (string permittedActivity in Activities.instance.permittedActivities){
				if (!filterSelection.Contains(permittedActivity)){
                    Activity activity = Activities.instance.GetActivityByCode(permittedActivity);
                    if(activity != null) {
                        FilterMenu.instance.SelectFilter(activity.activity, activity.name);
                    }
			    }
			}
            myWorkFilterActive = true;
        }
	}

    public void ShowHideDoors(bool showDoors){
        foreach(ProjectObject door in ProjectObjects.instance.GetDoorsAndWindows()){
            door.gameObject.SetActive(showDoors);
        }
    }

}
