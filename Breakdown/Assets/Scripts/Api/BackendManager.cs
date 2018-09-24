using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class BackendManager : MonoBehaviour {
	
    [HideInInspector]
    public int projectObjectPageSize, projectStepPageSize, activityPageSize;
    public static BackendManager instance;

    void Awake() {
        instance = this;
    }

    void Start () {
		if (Login.instance != null) {
			Login.instance.SetEnvironmentLabel ();
		}
    }
	
	void Update () {
        ActivityService.Update();
        ProjectModelService.Update();
        ProjectObjectService.Update();
        ProjectStepService.Update();
        SubcontractorService.Update();
    }

    public static void LogError(UnityWebRequest request){
        Debug.LogError(JSONNode.Parse(request.downloadHandler.text)["response"] + " (" + request.url + ")");
        if(request.responseCode == 401){
            AuthService.CheckToken();
        }
    }

    public static void LogRequest(UnityWebRequest request){
        Debug.Log("(" + request.method + ") " + request.url);
    }
}
