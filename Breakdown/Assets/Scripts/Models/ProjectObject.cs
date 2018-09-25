using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using cakeslice;

public class ProjectObject : MonoBehaviour {

	public string id;
	public int forgeId;
	public string modelId;

	public List<string> categories = new List<string> ();

	public List<string> activities = new List<string> ();
	public List<string> scheduledActivities = new List<string> ();

	public string name;
	public List<ObjectProperty> properties = new List<ObjectProperty> ();
	public List<CircularObject> circularObjects = new List<CircularObject>();
	public bool hidden;
	public bool loaded;

	public MeshCollider meshCollider;
	public List<Lod> lods = new List<Lod>();

	public InteractiveSelect interactiveSelect;

	public Material defaultMaterial;

    public bool isScheduled = false;
	public bool isLoaded = false;

	public BoxCollider loadingCollider;
	public int lod = -1;
	public int activeLod = -1;

	public List<int> fragments;

	public bool wallFloorOrRoof;
	public List<float> box;

#if UNITY_EDITOR
    void OnMouseUpAsButton() {
		if(Raycaster.instance.pointerEnabled && !EventSystem.current.IsPointerOverGameObject()){
            interactiveSelect.Select();
    	}
	}
#endif

	public void Initialize(){
	}

    public void AssignInfo(ProjectObjectInfo projectObjectInfo) {
        id = projectObjectInfo.id;
        forgeId = projectObjectInfo.forgeId;
        modelId = projectObjectInfo.modelId;
        activities = projectObjectInfo.activities;
        circularObjects = projectObjectInfo.circularObjects;
        properties = projectObjectInfo.properties;
        hidden = projectObjectInfo.hidden;
        if (hidden) {
            gameObject.SetActive(false);
        }
    }

    public void Fade(){
		EnableAndLoad (true);
		MaterialManager.instance.FadeProjectObject (this);
	}

	public void EnableAndLoad(bool priority) {
		CheckLod (priority);
		CheckActiveLod ();
	}

	public void CheckLod(bool priority) {
		if (Camera.main != null) {
			CheckLod (Camera.main.transform.position, priority);
		}
	}

	public void CheckLod(Vector3 position, bool priority) {
		float dx = position.x < box [0] ? box [0] - position.x : position.x > box [3] ? position.x - box [3] : 0;
		float dy = position.y < box [1] ? box [1] - position.y : position.y > box [4] ? position.y - box [4] : 0;
		float dz = position.z < box [2] ? box [2] - position.z : position.z > box [5] ? position.z - box [5] : 0;
		float dist = dx * dx + dy * dy + dz * dz;
		int index = (int) (Mathf.Log (dist) * 0.5f);
		if (index < 0)
			index = 0;
		if (index >= lods.Count)
			index = lods.Count - 1;
		if (index != lod) {
			lod = index;
		}
	}

	private void CheckActiveLod() {
	}

	public void Disable() {
		lod = -1;
		foreach (Lod lod in lods) {
			lod.gameObject.SetActive (false);
		}
	}

	public void UnFade(){
		MaterialManager.instance.UnFadeProjectObject (this);
	}

	public void AssignObjectId(string objectId){
		id = objectId;
	}

	public void AssignMaterial(Material material){
		SetMaterial (material);
	}

	public void AssignDefaultMaterial(Material material){
		SetMaterial (material);
		defaultMaterial = material;
	}

	public void AddScheduledActivity(string activity){
		if (!scheduledActivities.Contains (activity)) {
			scheduledActivities.Add (activity);
		}
        ProjectObjects.instance.CheckIfObjectIsScheduled (this);
	}

	public void RemoveScheduledActivity(string activity){
		if (scheduledActivities.Contains (activity)) {
			scheduledActivities.Remove (activity);
		}
		ProjectObjects.instance.CheckIfObjectIsScheduled (this);
	}

	public bool IsAllScheduled(){
		return scheduledActivities.Count >= activities.Count;
	}

	public void MarkAsLoaded(){
		loadingCollider.enabled = false;
	}

	public void SetMaterial(Material mat) {
		foreach (Lod lod in lods) {
			lod.meshRenderer.sharedMaterial = mat;
		}
	}

	public void SetMaterials(Material[] mat) {
		foreach (Lod lod in lods) {
			lod.meshRenderer.sharedMaterials = mat;
		}
	}

	public Material[] GetMaterials() {
		return lods[0].meshRenderer.sharedMaterials;
	}

	public void SetLayer(string layer) {
		int index = LayerMask.NameToLayer (layer);
		for (int i = 0; i < lods.Count; i++) {
			lods [i].gameObject.layer = index;
			if(layer == "Scheduled"){
				lods[i].meshRenderer.enabled = true;
			}
		}
		gameObject.layer = index;
	}
}
