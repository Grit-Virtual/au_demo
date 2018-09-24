using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoader : MonoBehaviour {

	public static ObjectLoader instance;

	public Dictionary<Collider, ProjectObject> objectsToLoad = new Dictionary<Collider, ProjectObject>();
	public Dictionary<Collider, ProjectObject> priorityObjectsToLoad = new Dictionary<Collider, ProjectObject>();
	public Dictionary<Collider, ProjectObject> proximityObjectsToLoad = new Dictionary<Collider, ProjectObject>();
	public Dictionary<Collider, ProjectObject> currentObjects = new Dictionary<Collider, ProjectObject>();

	void Awake(){
		instance = this;
    }

	public void Initialize(){
		Collider[] colliders = Physics.OverlapSphere (transform.position, 20f);
		foreach (Collider collider in colliders) {
			if (collider.tag == "UID Object") {
				if(collider is BoxCollider){
					ProjectObject projectObject = null;
					if(objectsToLoad.ContainsKey(collider)){
						projectObject = objectsToLoad[collider];
						objectsToLoad.Remove(collider);
					}
					if(projectObject == null){
						projectObject = collider.GetComponent<ProjectObject>();
					}
					if(!priorityObjectsToLoad.ContainsKey(collider)){
						priorityObjectsToLoad.Add(collider, projectObject);
					}
				}
			}
		}
	}

	private int iteration = 0;
	void Update(){
		if ((iteration++) % 15 == 0 && Camera.main != null) {
			Vector3 position = Camera.main.transform.position;
			foreach (ProjectObject obj in currentObjects.Values) {
				obj.CheckLod (position, false);
			}
		}
		if(Initialization.instance.fragmentsReceived){
			if (proximityObjectsToLoad.Count > 0) {
				Collider collider = null;
				int objectsLoaded = 0;
				foreach (KeyValuePair<Collider, ProjectObject> entry in proximityObjectsToLoad) {
					collider = entry.Key;
					ProjectObject objectToLoad = entry.Value;
					objectToLoad.MarkAsLoaded();
					// ForgeService.instance.GetMeshFrag (objectToLoad);
					objectToLoad.CheckLod (true);
					objectsLoaded++;
					if(objectsLoaded  >= 1){
						// break;
					}
				}
				proximityObjectsToLoad.Clear ();
			} else if (priorityObjectsToLoad.Count > 0) {
				Collider collider = null;
				int objectsLoaded = 0;
				foreach (KeyValuePair<Collider, ProjectObject> entry in priorityObjectsToLoad) {
					collider = entry.Key;
					ProjectObject objectToLoad = entry.Value;
					objectToLoad.MarkAsLoaded();
					// ForgeService.instance.GetMeshFrag (objectToLoad);
					objectToLoad.CheckLod (true);
					objectsLoaded++;
					if(objectsLoaded  >= 1){
						// break;
					}
				}
				priorityObjectsToLoad.Clear ();
			} else if (objectsToLoad.Count > 0) {
				Collider collider = null;
				int objectsLoaded = 0;
				foreach (KeyValuePair<Collider, ProjectObject> entry in objectsToLoad) {
					collider = entry.Key;
					ProjectObject objectToLoad = entry.Value;
					objectToLoad.MarkAsLoaded();
					// ForgeService.instance.GetMeshFrag (objectToLoad);
					objectToLoad.CheckLod (false);
					objectsLoaded++;
					if(objectsLoaded >= 1){
						// break;
					}
				}
				objectsToLoad.Clear ();
			}
		}
	}

	public void LoadDefaultObjects(){
		foreach (ProjectObject projectObject in ProjectObjects.instance.projectObjects) {
			if(projectObject.wallFloorOrRoof){
				if(!priorityObjectsToLoad.ContainsKey(projectObject.loadingCollider)){
					priorityObjectsToLoad.Add(projectObject.loadingCollider, projectObject);
				}
			} else {
				if(!objectsToLoad.ContainsKey(projectObject.loadingCollider)){
					objectsToLoad.Add(projectObject.loadingCollider, projectObject);
				}
			}
    	}
		Menu.instance.UpdateLoadingText();
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "UID Object") {
			if(collider is BoxCollider){
				ProjectObject projectObject = null;
				if(objectsToLoad.ContainsKey(collider)){
					projectObject = objectsToLoad[collider];
					objectsToLoad.Remove(collider);
				}
				if(priorityObjectsToLoad.ContainsKey(collider)){
					projectObject = priorityObjectsToLoad[collider];
					priorityObjectsToLoad.Remove(collider);
				}
				if(projectObject == null){
					projectObject = collider.GetComponent<ProjectObject>();
				}
				if(!proximityObjectsToLoad.ContainsKey(collider)){
					proximityObjectsToLoad.Add(collider, projectObject);
				}
			}
		}
	}

	void OnTriggerExit(Collider collider){
		if (collider.tag == "UID Object") {
			if(collider is BoxCollider){
				if(proximityObjectsToLoad.ContainsKey(collider)){
					ProjectObject projectObject = proximityObjectsToLoad[collider];
					proximityObjectsToLoad.Remove(collider);
					if(projectObject.wallFloorOrRoof){
						if(!priorityObjectsToLoad.ContainsKey(collider)){
							priorityObjectsToLoad.Add(collider, projectObject);
						}
					} else {
						if(!objectsToLoad.ContainsKey(collider)){
							objectsToLoad.Add(collider, projectObject);
						}
					}
				}
			}
		}
	}
}
