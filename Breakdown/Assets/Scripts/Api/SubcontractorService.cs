using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Restifizer;
using UnityEngine.Networking;
using System.Threading;
using SimpleJSON;

public static class SubcontractorService {

    // Thread data locker
    static readonly object dataLock = new object();

    // For getting project model
    static bool awaitingSubcontractors = false;
    static bool subsDone;

    public static void Update() {
        if (awaitingSubcontractors) {
            lock (dataLock) {
                if (subsDone) {
                    awaitingSubcontractors = false;
                    Subcontractors.instance.projectSubs.Sort((a, b) => { return a.name.CompareTo(b.name); });
                    Initialization.instance.InitializeProjectSubsDone();
                }
            }
        }
    }

    public static IEnumerator GetProjectSubs() {
		UnityWebRequest subcontractorsRequest = UnityWebRequest.Get(Restifizer.RestifizerManager.Instance.baseUrl + "project/" + Utils_Prefs.GetProjectId() + "/subContractors");
        subcontractorsRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
        BackendManager.LogRequest(subcontractorsRequest);
        yield return subcontractorsRequest.SendWebRequest();
        if(!subcontractorsRequest.isHttpError){
            awaitingSubcontractors = true;
            lock (dataLock) {
                subsDone = false;
            }
            ThreadPool.QueueUserWorkItem ((object obj) => {
                try {
                    SubcontractorsParse (obj);
                } catch (System.Exception e) {
                    Debug.Log (e);
                }
            }, subcontractorsRequest.downloadHandler.text);
        } else {
            BackendManager.LogError(subcontractorsRequest);
            Alert.instance.ShowAlert("Failed to get project subcontractors");
        }
    }

    static void SubcontractorsParse(object paramObject) {
        string data = (string)paramObject;
        JSONNode res = SimpleJSON.JSON.Parse(data);
        lock (dataLock) {
            JSONArray subs = res["response"].AsArray;
            for (int i = 0; i < subs.Count; i++) {
                Subcontractors.instance.AddProjectSub(subs[i]);
            }
            subsDone = true;
        }
    }

}
