using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Restifizer;
using UnityEngine.Networking;
using System.Linq;
using SimpleJSON;
using System.Threading;

public static class ProjectStepService {

    // Thread data locker
    static readonly object dataLock = new object();

    // For getting project steps
    static bool[] stepThreadsDone;
    static bool awaitingRemainingSteps = false;
    static bool firstPageStepsDone;
    static bool awaitingFirstPageSteps = false;
    static int firstPageStepsResCount = 0;

    public static void Update() {
        if (awaitingFirstPageSteps) {
            lock (dataLock) {
                if (firstPageStepsDone) {
                    awaitingFirstPageSteps = false;
                    RemainingStepsCheck();
                }
            }
        }
        
        if (awaitingRemainingSteps) {
            lock (dataLock) {
                bool allPagesReceived = true;
                for (int i = 0; i < stepThreadsDone.Length; i++) {
                    if (!stepThreadsDone[i]) {
                        allPagesReceived = false;
                        break;
                    }
                }
                if (allPagesReceived) {
                    awaitingRemainingSteps = false;
                    Initialization.instance.InitializeProjectStepsDone();
                }
            }
        }
    }

    public static IEnumerator GetFirstPageOfSteps() {
        int pageSize = BackendManager.instance.projectStepPageSize;
		UnityWebRequest projectStepsRequest = UnityWebRequest.Get(Restifizer.RestifizerManager.Instance.baseUrl + "project/" + Utils_Prefs.GetProjectId() + "/steps?pageSize=" + pageSize + "&pageStart=0");
        projectStepsRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
        BackendManager.LogRequest(projectStepsRequest);
        yield return projectStepsRequest.SendWebRequest();
        if(!projectStepsRequest.isHttpError){
            awaitingFirstPageSteps = true;
            lock (dataLock) {
                firstPageStepsDone = false;
            }
		    ThreadPool.QueueUserWorkItem((object obj) => {
			    try {
			    	FirstStepsParse(obj);
			    } catch (System.Exception e) {
			    	Debug.Log(e);
			    }
		    }, projectStepsRequest.downloadHandler.text);
        } else {
            BackendManager.LogError(projectStepsRequest);
            Alert.instance.ShowAlert("Failed to get project activities");
        }
    }

    static void FirstStepsParse(object paramObject) {
        string data = (string)paramObject;
        JSONNode res = SimpleJSON.JSON.Parse(data);
        lock (dataLock) {
            firstPageStepsResCount = res["count"];
            JSONArray steps = res["response"].AsArray;
            for (int i = 0; i < steps.Count; i++) {
                ProjectSteps.instance.AddProjectStep(new ProjectStep(steps[i]));
            }
            firstPageStepsDone = true;
        }
    }

    static void RemainingStepsCheck() {
        int pageSize = BackendManager.instance.projectStepPageSize;
        int numberOfPages = ((firstPageStepsResCount) / pageSize);

        if(numberOfPages == 0) {
            Initialization.instance.InitializeProjectStepsDone();
        } else {
            lock (dataLock) {
                awaitingRemainingSteps = true;
                stepThreadsDone = new bool[numberOfPages];
            }
            for(int s = 1; s <= numberOfPages; s++) {
                Initialization.instance.RemainingStepsCoroutine(pageSize, s * pageSize, s - 1);
            }
        }
    }

    public static IEnumerator GetRemainingSteps(int pageSize, int pageStart, int threadNum) {
		UnityWebRequest projectStepsRequest = UnityWebRequest.Get(Restifizer.RestifizerManager.Instance.baseUrl + "project/" + Utils_Prefs.GetProjectId() + "/steps?pageSize=" + pageSize + "&pageStart=" + pageStart);
        projectStepsRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
        BackendManager.LogRequest(projectStepsRequest);
        yield return projectStepsRequest.SendWebRequest();
        if(!projectStepsRequest.isHttpError){
            StepParamObject myParamobj = new StepParamObject {
                data = projectStepsRequest.downloadHandler.text,
                threadIndex = threadNum
            };
		    ThreadPool.QueueUserWorkItem ((object obj) => {
			    try {
			    	RemainingStepsParse (obj);
			    } catch (System.Exception e) {
			    	Debug.Log (e);
		    	}
		    }, myParamobj);
        } else {
            BackendManager.LogError(projectStepsRequest);
            Alert.instance.ShowAlert("Failed to get project activities");
        }
    }

    static void RemainingStepsParse(object paramObject) {
        StepParamObject input = (StepParamObject)paramObject;
        string data = input.data;
        int index = input.threadIndex;
        JSONNode res = SimpleJSON.JSON.Parse(data);
        lock (dataLock) {
            JSONArray steps = res["response"].AsArray;
            for (int i = 0; i < steps.Count; i++) {
                ProjectSteps.instance.AddProjectStep(new ProjectStep(steps[i]));
            }
            stepThreadsDone[index] = true;
        }
    }

    public static void SendProjectStep(ProjectStep projectStep){
        ProjectSteps.instance.lastScheduledStep = projectStep;
        ProjectSteps.instance.FadeScheduledObjects(projectStep);
		RestifizerManager.Instance.ConfigBearerAuth(Utils_Prefs.GetAuthKey());

		Hashtable projectStepParameters = new Hashtable();
		projectStepParameters.Add ("projectId", projectStep.projectId);
		ArrayList objects = new ArrayList ();
		foreach (ProjectStepObject projectStepObject in projectStep.projectObjects) {
			Hashtable stepObject = new Hashtable ();
			stepObject.Add ("objectId", projectStepObject.objectId);
			stepObject.Add ("activity", projectStepObject.activity);
			objects.Add (stepObject);
		}
		projectStepParameters.Add ("objects", objects);
		projectStepParameters.Add ("durationHours", projectStep.durationHours);
		projectStepParameters.Add ("crewSize", projectStep.crewSize);
		projectStepParameters.Add ("lead", projectStep.lead);
		projectStepParameters.Add ("lag", projectStep.lag);
		projectStepParameters.Add ("name", projectStep.name);
		ArrayList prereqObjects = new ArrayList ();
		foreach (ProjectStepObject projectStepObject in projectStep.prerequisiteObjects) {
			Hashtable stepObject = new Hashtable ();
			stepObject.Add ("objectId", projectStepObject.objectId);
			stepObject.Add ("activity", projectStepObject.activity);
			prereqObjects.Add(stepObject);
		}
		projectStepParameters.Add ("prerequisiteObjects", prereqObjects);
		RestifizerManager.Instance.ResourceAt ("project/step").WithBearerAuth().Post (projectStepParameters, (response) => {
			if(response == null){
                Alert.instance.ShowAlert ("Failed to plan task");
				ProjectSteps.instance.UnFadeScheduledObjects();
			} else {
				if(response.HasError){
					Alert.instance.ShowAlert ("Failed to plan task");
					ProjectSteps.instance.UnFadeScheduledObjects();
				} else {
					projectStep.stepId = ((Hashtable)response.Resource["response"])["id"].ToString();
                    Hashtable segmentParameters = new Hashtable();
                    segmentParameters.Add("projectId", Utils_Prefs.GetProjectId());
                    segmentParameters.Add("taskId", projectStep.stepId);
                    SegmentService.Track("Task Planned", segmentParameters);
					ProjectSteps.instance.AddProjectStep(projectStep, true);
				}
			}
		});
	}

	public static IEnumerator DeleteProjectStep(ProjectStep step){
		string address = RestifizerManager.Instance.baseUrl + "project/step/" + step.stepId;
		UnityWebRequest deleteRequest = UnityWebRequest.Delete (address);
		deleteRequest.downloadHandler = new DownloadHandlerBuffer ();
		deleteRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
        BackendManager.LogRequest(deleteRequest);
		yield return deleteRequest.SendWebRequest ();
		if (!deleteRequest.isHttpError) {
			ProjectSteps.instance.projectSteps.Remove (step);
			UndoMenu.instance.undoSteps.Remove(step);
			foreach (ProjectStepObject projectStepObject in step.projectObjects) {
				ProjectObjects.instance.GetObjectById(projectStepObject.objectId).RemoveScheduledActivity (projectStepObject.activity);
			}
            Hashtable segmentParameters = new Hashtable();
            segmentParameters.Add("projectId", Utils_Prefs.GetProjectId());
            segmentParameters.Add("taskId", step.stepId);
            SegmentService.Track("Task Deleted", segmentParameters);
		} else {
            BackendManager.LogError(deleteRequest);
			Alert.instance.ShowAlert ("Failed to undo planned task");
		}
	}
}

class StepParamObject {
    public string data;
    public int threadIndex;
}
