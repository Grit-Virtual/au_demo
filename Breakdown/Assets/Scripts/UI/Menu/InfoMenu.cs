using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour {

	public static InfoMenu instance;

    public Animator animator;
	// public GameObject infoMenu;

	public ToggleButtonUI infoButton;

	public GameObject objectInfoPrefab;
	public Transform objectInfoParent;
    public GameObject prevObjectInfoButton;
    public GameObject nextObjectInfoButton;

    List<List<ObjectInfoHolder>> objectInfoList = new List<List<ObjectInfoHolder>>();
    List<GameObject> currentInfoItems = new List<GameObject>();
    int curIndex = 0;

	void Awake () {
		instance = this;
	}

	void Start () {
		// infoMenu.SetActive (false);
		animator.SetBool("MenuShowing", false);
	}

    // void Update() {
	// 	if (Controller.instance.BackButtonPress()) {
    //         DestroyCurrentInfoItems();
    //         BackButtonPress();
    //     }
    // }

    // public void InfoButtonClick(){
    //     if(ObjectSelection.instance.GetSelectionCount() > 0){
    //         if(infoMenu.activeInHierarchy){
    //             HideInfoMenu();
    //         } else {
    //             ShowInfoMenu();
    //         }
    //     }
    // }

    public void ShowInfoMenu(){
        List<ProjectObject> projectObjects = ObjectSelection.instance.GetSelectedObjects();
        curIndex = 0;
        if (projectObjects.Count > 1) {
            prevObjectInfoButton.SetActive (true);
            nextObjectInfoButton.SetActive(true);
        } else {
            prevObjectInfoButton.SetActive(false);
            nextObjectInfoButton.SetActive (false);
        }
        GenerateObjectInfoList(projectObjects);
        SetIndexObjectInList(curIndex);
        // infoMenu.SetActive(true);
		animator.SetBool("MenuShowing", true);
        // if(!infoButton.active){
        //     infoButton.SetActive(true);
        // }
	}

	public void HideInfoMenu(){
		animator.SetBool("MenuShowing", false);
		// infoMenu.SetActive (false);
        // if(infoButton.active){
		// 	infoButton.SetActive(false);
		// }
    }

    public void BackButtonPress() {
        DestroyCurrentInfoItems();
    }

    void SetIndexObjectInList(int index) {
        DestroyCurrentInfoItems();
        foreach (ObjectInfoHolder objHolder in objectInfoList[index]) {
            GameObject objectInfoItemGO = (GameObject)Instantiate(objectInfoPrefab, objectInfoParent, false);
            ObjectInfoItem objectInfoItem = objectInfoItemGO.GetComponent<ObjectInfoItem>();
            objectInfoItem.SetUpText(objHolder.GetInfoHolderKey(), objHolder.GetInfoHolderValue());
            currentInfoItems.Add(objectInfoItemGO);
        }
    }

    void DestroyCurrentInfoItems() {
        foreach(GameObject infoItem in currentInfoItems) {
            Destroy(infoItem);
        }
    }

    void GenerateObjectInfoList(List<ProjectObject> projectObjects) {
        objectInfoList.Clear();
        foreach(ProjectObject pObj in projectObjects){
            List<ObjectInfoHolder> objectInfos = new List<ObjectInfoHolder>();
            ObjectInfoHolder objectNameHolder = new ObjectInfoHolder("Name", pObj.name);
            objectInfos.Add(objectNameHolder);
            List<ObjectProperty> props = pObj.properties;
            foreach(ObjectProperty prop in props) {
                ObjectInfoHolder objectInfoHolder = new ObjectInfoHolder(prop.label, prop.value);
                objectInfos.Add(objectInfoHolder);
            }
            objectInfoList.Add(objectInfos);
        }
    }

	public void PreviousButtonClick(){
        if(curIndex - 1 >= 0) {
            curIndex--;
        } else {
            curIndex = objectInfoList.Count - 1;
        }
        SetIndexObjectInList(curIndex);
    }

    public void NextButtonClick() {
        if (curIndex + 1 <= objectInfoList.Count - 1) {
            curIndex++;
        } else {
            curIndex = 0;
        }
        SetIndexObjectInList(curIndex);
    }
}
