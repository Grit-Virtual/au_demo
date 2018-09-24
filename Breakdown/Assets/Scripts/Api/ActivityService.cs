using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public static class ActivityService {

    static readonly object dataLock = new object();

    static bool awaitingActivities = false;
    static bool activitiesDone;
    static bool permittedActivitiesDone;

    public static void Update() {
        if (awaitingActivities) {
            lock (dataLock) {
                if (activitiesDone && permittedActivitiesDone) {
                    awaitingActivities = false;
                    Initialization.instance.ActivityInitFinish();
                }
            }
        }
    }

    public static IEnumerator GetPermittedActivities() {
		UnityWebRequest permittedActivitiesRequest = UnityWebRequest.Get(Restifizer.RestifizerManager.Instance.baseUrl + "project/" + Utils_Prefs.GetProjectId() + "/user/" + Utils_Prefs.GetUserId());
        permittedActivitiesRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
        BackendManager.LogRequest(permittedActivitiesRequest);
        yield return permittedActivitiesRequest.SendWebRequest();
        if(!permittedActivitiesRequest.isHttpError){
            lock (dataLock) {
                permittedActivitiesDone = false;
            }
            ThreadPool.QueueUserWorkItem ((object obj) => {
                try {
                    PermittedActivitiesParse (obj);
                } catch (System.Exception e) {
                    UnityEngine.Debug.Log (e);
                }
            }, permittedActivitiesRequest.downloadHandler.text);
        } else {
            BackendManager.LogError(permittedActivitiesRequest);
            Alert.instance.ShowAlert("Failed to get user activities");
        }
    }

    static void PermittedActivitiesParse(object paramObject) {
        string data = (string)paramObject;
        JSONNode res = SimpleJSON.JSON.Parse(data);
        lock (dataLock) {
            JSONNode activityRes = res["response"].AsObject;
            if (activityRes != null) {
                for (int i = 0; i < activityRes["activities"].Count; i++) {
                    string activity = activityRes["activities"][i];
                    Activities.instance.AddPermittedActivity(activity);
                }
            }
            permittedActivitiesDone = true;
        }
    }

    public static IEnumerator GetActivities() {
		UnityWebRequest activitiesRequest = UnityWebRequest.Get(Restifizer.RestifizerManager.Instance.baseUrl + "project/" + Utils_Prefs.GetProjectId() + "/trades");
        activitiesRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
        BackendManager.LogRequest(activitiesRequest);
        yield return activitiesRequest.SendWebRequest();
        if(!activitiesRequest.isHttpError){
            awaitingActivities = true;
            lock (dataLock) {
                activitiesDone = false;
            }
            ThreadPool.QueueUserWorkItem ((object obj) => {
                try {
                    ActivitiesParse (obj);
                } catch (System.Exception e) {
                    UnityEngine.Debug.Log (e);
                }
            }, activitiesRequest.downloadHandler.text);
        } else {
            BackendManager.LogError(activitiesRequest);
            Alert.instance.ShowAlert("Failed to get project activities");
        }
    }

    static void ActivitiesParse(object paramObject) {
        string data = (string)paramObject;
        JSONNode res = SimpleJSON.JSON.Parse(data);
        lock (dataLock) {
            JSONArray activities = res["response"].AsArray;
            for (int i = 0; i < activities.Count; i++) {
                Activities.instance.AddActivity(new Activity(activities[i]));
            }
            activitiesDone = true;
        }
    }
}
