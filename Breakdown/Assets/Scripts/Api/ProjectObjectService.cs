using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Restifizer;
using System.Threading;
using SimpleJSON;

public static class ProjectObjectService {

    static List<ObjectGenerationInfo> objects = new List<ObjectGenerationInfo>();

    // Threading data lock
    static readonly object dataLock = new object();

    static bool awaitingObjects = false;
    static bool objectsDone;

    public static void Update() {
        if (awaitingObjects) {
            lock (dataLock) {
                if (objectsDone) {
                    awaitingObjects = false;
                    Initialization.instance.objectsToGenerate.AddRange(objects);
                    Initialization.instance.InitializeObjectsDone();
                }
            }
        }
    }

    public static IEnumerator GetObjects() {
        objects.Clear();
        awaitingObjects = true;
        objectsDone = false;
		UnityWebRequest objectsRequest = UnityWebRequest.Get(Restifizer.RestifizerManager.Instance.baseUrl + "project/" + Utils_Prefs.GetProjectId() + "/objectMap?boundingBoxes=true");
        objectsRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
        BackendManager.LogRequest(objectsRequest);
        yield return objectsRequest.SendWebRequest();
        if(!objectsRequest.isHttpError){
            lock (dataLock) {
                objectsDone = false;
            }
            ThreadPool.QueueUserWorkItem(ParseObjects, objectsRequest.downloadHandler.text);
        } else {
            BackendManager.LogError(objectsRequest);
            Alert.instance.ShowAlert("Error getting project model objects");
        }
    }

    static void ParseObjects(object paramObject) {
        JSONNode response = SimpleJSON.JSON.Parse((string)paramObject);
        lock (dataLock) {
            JSONArray projectObjects = response["response"].AsArray;
            for(int o = 0; o < projectObjects.Count; o++) {
                LoadObject(projectObjects[o]);
            }
            objectsDone = true;
        }
    }

    private static void LoadObject(JSONNode obj) {
		objects.Add(new ObjectGenerationInfo(obj));
	}

	public static void AssignObjectInfo(ProjectObject projectObject){
		RestifizerManager.Instance.ConfigBearerAuth(Utils_Prefs.GetAuthKey());
		RestifizerManager.Instance.ResourceAt ("project/object/" + projectObject.id).WithBearerAuth ().Get ((response) => {
			if (response.HasError) {
				Alert.instance.ShowAlert ("Error getting object information");
			} else {
				ProjectObjects.instance.AssignObjectInfo (new ProjectObjectInfo ((Hashtable)response.Resource ["response"]));
			}
		});
	}

	public static void AssignPrereqInfo(ProjectObject projectObject){
		RestifizerManager.Instance.ConfigBearerAuth(Utils_Prefs.GetAuthKey());
		RestifizerManager.Instance.ResourceAt ("project/object/" + projectObject.id).WithBearerAuth ().Get ((response) => {
			if (response.HasError) {
                Alert.instance.ShowAlert("Error getting object information");
		        Raycaster.instance.SetPointerEnabled(true);
			} else {
				ProjectObjects.instance.AssignObjectInfo (new ProjectObjectInfo ((Hashtable)response.Resource ["response"]));
                if(ActivityMenu.instance.ActivitySelectionRequired(true)){
                    ActivityMenu.instance.InitializeActivityMenu();
                }
		        Raycaster.instance.SetPointerEnabled(true);
			}
		});
	}
}
