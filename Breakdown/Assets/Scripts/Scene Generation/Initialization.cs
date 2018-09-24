using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Restifizer;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.VR;
using TMPro;

public class Initialization : MonoBehaviour {

	public static Initialization instance;

    public GameObject projectObjectPrefab;
    public GameObject loadingCanvas;
    public GameObject[] hideDuringInitialization;
    public GameObject[] hideAfterInitialization;
    public Transform player;
    public Transform grid;
    public Transform origin;
    [HideInInspector]
	public List<ObjectGenerationInfo> objectsToGenerate = new List<ObjectGenerationInfo>();

	bool objectGenerationStarted;
	bool objectGenerationDone;
    public int objectGenerationsPerFrame;
    float minX = Mathf.Infinity, minY = Mathf.Infinity, minZ = Mathf.Infinity;

    // Initilization bools
    bool allInitDone;
    bool readyToGenerate;
    bool activityInitDone;
    bool pStepsInitStarted;
    bool defaultLoadStarted;
    bool cameraOccInitDone;
    bool stepsInitDone;
    bool subsInitDone;
    [HideInInspector]
    public bool fragmentsReceived;

    void Awake() {
		instance = this;
    }

	void Start () {
        foreach(GameObject go in hideDuringInitialization){
            go.SetActive(false);
        }
        StartCoroutine(ActivityService.GetActivities());
        StartCoroutine(ActivityService.GetPermittedActivities());
        StartCoroutine(ProjectModelService.GetProjectModels());
        StartCoroutine(SubcontractorService.GetProjectSubs());
    }

	void Update() {
        if (allInitDone) return;
        if (!readyToGenerate) return;
		if (objectsToGenerate.Count > 0) {
			objectGenerationStarted = true;
            int initialCount = objectsToGenerate.Count;
			for (int i = objectsToGenerate.Count - 1; i >= 0 && i > initialCount - 1 - objectGenerationsPerFrame; i--) {
				if (objectsToGenerate [i].boundingBox.Count > 0) {
					GenerateProjectObject (objectsToGenerate [i]);
				}
				objectsToGenerate.RemoveAt (i);
			}
		}

		if (objectGenerationStarted && !objectGenerationDone &&  fragmentsReceived) {
            // Check to make sure we've generated all project objects
			if (objectsToGenerate.Count == 0) {
                VRDOC_Camera.instance.Initialize();
                objectGenerationDone = true;
            }
        }
        
        if (!pStepsInitStarted && activityInitDone && objectGenerationDone) {
            pStepsInitStarted = true;
            FilterMenu.instance.InitializeMyWorkButton();
            StartCoroutine(ProjectStepService.GetFirstPageOfSteps());
        }

        if (!defaultLoadStarted && stepsInitDone && cameraOccInitDone) {
            defaultLoadStarted = true;
            ObjectLoader.instance.LoadDefaultObjects();
            FinishLoad();
        }
	}

#region Project Models and Objects Init
    public void InitializeProjectModelsDone() {
        StartCoroutine(ProjectObjectService.GetObjects());
        StartCoroutine(ProjectModelService.GetModelFragments());
    }
    
    public void InitializeObjectsDone() {
        readyToGenerate = true;
    }

    public void InitializeFragmentDataDone() {
        fragmentsReceived = true;
    }
#endregion

#region Activity Init
    public void ActivityInitFinish() {
        activityInitDone = true;
    }
#endregion

#region Camera OCC
    public void InitCameraOccDone() {
        cameraOccInitDone = true;
    }
#endregion

#region Steps Init
    public void RemainingStepsCoroutine(int pageSize, int pageStart, int threadNum) {
        StartCoroutine(ProjectStepService.GetRemainingSteps(pageSize, pageStart, threadNum));
    }
    public void InitializeProjectStepsDone() {
        ProjectSteps.instance.AddScheduledActivitiesToObjects();
        stepsInitDone = true;
    }
#endregion

#region Subcontractor Init
    public void InitializeProjectSubsDone() {
        subsInitDone = true;
    }
#endregion

    public void FinishLoad() {
		player.position = new Vector3(minX, minY + 1.66f, minZ);
		grid.position = new Vector3(minX, minY, minZ);
        foreach(GameObject go in hideAfterInitialization){
            go.SetActive(false);
        }
        foreach(GameObject go in hideDuringInitialization){
            go.SetActive(true);
        }
        Controller.instance.SetPointerMode(PointerMode.MOVE);
        ObjectLoader.instance.Initialize();
        ScheduledObjectDistanceCheck.instance.Initialize();
        allInitDone = true;
    }

    public void GenerateProjectObject(ObjectGenerationInfo objectToGenerate){
        if (objectToGenerate.hidden) return;
        Vector3 position = new Vector3 (
            (objectToGenerate.boundingBox[0] + objectToGenerate.boundingBox[3]) / 2,
            (objectToGenerate.boundingBox[1] + objectToGenerate.boundingBox[4]) / 2,
            (objectToGenerate.boundingBox[2] + objectToGenerate.boundingBox[5]) / 2
        );
		if (Mathf.Min(objectToGenerate.boundingBox[0], objectToGenerate.boundingBox[3]) < minX) {
			minX = Mathf.Min(objectToGenerate.boundingBox[0], objectToGenerate.boundingBox [3]) - 1f;
        }
		if (Mathf.Min(objectToGenerate.boundingBox[1], objectToGenerate.boundingBox[4]) < minY) {
			minY = Mathf.Min(objectToGenerate.boundingBox[1], objectToGenerate.boundingBox[4]);
        }
		if (Mathf.Min(objectToGenerate.boundingBox[2], objectToGenerate.boundingBox[5]) < minZ) {
			minZ = Mathf.Min(objectToGenerate.boundingBox[2], objectToGenerate.boundingBox[5]) - 1f;
        }
		Vector3 size = new Vector3 (Mathf.Abs(objectToGenerate.boundingBox[0] - objectToGenerate.boundingBox[3]), Mathf.Abs(objectToGenerate.boundingBox[1] - objectToGenerate.boundingBox[4]), Mathf.Abs(objectToGenerate.boundingBox[2] - objectToGenerate.boundingBox[5]));
		GameObject projectObjectGo = (GameObject)Instantiate (projectObjectPrefab, origin);
		ProjectObject projectObject = projectObjectGo.GetComponent<ProjectObject> ();
        BoxCollider boxCollider = projectObject.loadingCollider;
		boxCollider.size = size;
		boxCollider.center = position;
		projectObjectGo.transform.position = Vector3.zero;
		projectObjectGo.transform.rotation = Quaternion.identity;
        projectObjectGo.name = objectToGenerate.name;
        projectObject.Initialize(objectToGenerate);
        Categories.instance.AddCategoryList(projectObject.categories);
		ProjectObjects.instance.AddProjectObject (projectObject);
	}

}