using System.Collections;
using System.Threading;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections.Generic;

class ModelRead {
    public string path;
    public int index;
}
class ModelWrite {
    public string path;
    public int index;
    public byte[] data;
}
public class ModelRequest {
    public string id;
    public string appPath;
    public int index;
}

public static class ProjectModelService {

    // Thread data locker
    static readonly object dataLock = new object();

    // For getting project model
    static bool awaitingProjectModels = false;
    static bool projectModelsDone;

    // For getting fragment data
    static bool[] fragThreadsDone;
    static bool awaitingFragsReadWrite = false;


    public static void Update() {
        if (awaitingProjectModels) {
            lock (dataLock) {
                if (projectModelsDone) {
                    awaitingProjectModels = false;
                    Initialization.instance.InitializeProjectModelsDone();
                }
            }
        }
        if (awaitingFragsReadWrite) {
            lock (dataLock) {
                bool fragsReceived = true;
                for (int i = 0; i < fragThreadsDone.Length; i++) {
                    if (!fragThreadsDone[i]) {
                        fragsReceived = false;
                        break;
                    }
                }
                if (fragsReceived) {
                    awaitingFragsReadWrite = false;
                    Initialization.instance.InitializeFragmentDataDone();
                }
            }
        }
    }

    public static IEnumerator GetProjectModels() {
		UnityWebRequest projectModelsRequest = UnityWebRequest.Get(Restifizer.RestifizerManager.Instance.baseUrl + "project/" + Utils_Prefs.GetProjectId() + "/models");
        projectModelsRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
        BackendManager.LogRequest(projectModelsRequest);
        yield return projectModelsRequest.SendWebRequest();
        if(!projectModelsRequest.isHttpError){
            awaitingProjectModels = true;
            lock (dataLock) {
                projectModelsDone = false;
            }
            ThreadPool.QueueUserWorkItem ((object obj) => {
			    try {
			    	ProjectModelsParse(obj);
			    } catch (System.Exception e) {
			    	UnityEngine.Debug.Log (e);
			    }
		    }, projectModelsRequest.downloadHandler.text);
        } else {
            BackendManager.LogError(projectModelsRequest);
            Alert.instance.ShowAlert("Error getting project models");
        }
    }

    static void ProjectModelsParse(object paramObject) {
        string data = (string)paramObject;
        JSONNode res = SimpleJSON.JSON.Parse(data);
        lock (dataLock) {
            JSONArray models = res["response"].AsArray;
            for (int i = 0; i < models.Count; i++) {
                Projects.instance.projectModels.Add(new ProjectModel(models[i]["id"], models[i]["urn"]));
            }
            projectModelsDone = true;
        }
    }

    public static IEnumerator GetModelFragments() {
        List<ModelRequest> modelsNeedingRequests = new List<ModelRequest>();
        lock (dataLock) {
            fragThreadsDone = new bool[Projects.instance.projectModels.Count];
        }
        awaitingFragsReadWrite = true;
        for (int m = 0; m < Projects.instance.projectModels.Count; m++) {
            string path = Application.persistentDataPath + "/" + Projects.instance.projectModels[m].id + ".grit";
            //if (File.Exists(path)) {
            //    Initialization.instance.statusText.text = "Status: Reading fragments from file";
            //    ModelRead paramObject = new ModelRead {
            //        path = path,
            //        index = m
            //    };
            //    ThreadPool.QueueUserWorkItem(ReadFragData, paramObject);
            //} else {
                ModelRequest modelReq = new ModelRequest {
                    id = Projects.instance.projectModels[m].id,
                    appPath = path,
                    index = m
                };
                modelsNeedingRequests.Add(modelReq);
            //}
        }
        foreach(ModelRequest req in modelsNeedingRequests) {
			UnityWebRequest fragRequest = UnityWebRequest.Get(Restifizer.RestifizerManager.Instance.baseUrl + "/project/model/" + req.id + "/fragment");
            fragRequest.SetRequestHeader("Authorization", Utils_Prefs.GetAuthKey());
            BackendManager.LogRequest(fragRequest);
            yield return fragRequest.SendWebRequest();
            if(!fragRequest.isHttpError){
                if (fragRequest.downloadHandler.data.Length > 0) {
                    ModelWrite paramObject = new ModelWrite {
                        path = req.appPath,
                        index = req.index,
                        data = fragRequest.downloadHandler.data
                    };
                    ThreadPool.QueueUserWorkItem(WriteFragData, paramObject);
                }
            } else {
                BackendManager.LogError(fragRequest);
                Alert.instance.ShowAlert("Error getting model fragment");
            }
        }
    }

    static void ReadFragData(object paramObject) {
        ModelRead model = (ModelRead)paramObject;
        string path = model.path;
        int m = model.index;

        lock (dataLock) {
            Projects.instance.projectModels[m].data = File.ReadAllBytes(path);
            fragThreadsDone[m] = true;
        }
    }

    static void WriteFragData(object paramObject) {
        ModelWrite model = (ModelWrite)paramObject;
        string path = model.path;
        int m = model.index;
        byte[] data = model.data;

        lock (dataLock) {
            File.WriteAllBytes(path, data);
            Projects.instance.projectModels[m].data = data;
            fragThreadsDone[m] = true;
        }
    }
}
