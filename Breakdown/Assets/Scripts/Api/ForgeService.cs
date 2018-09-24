using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ForgeService: MonoBehaviour {
	
	public static ForgeService instance;

	List<MeshThreadParams> threads = new List<MeshThreadParams>();
	public LinkedList<ProjectObject> toGenerate = new LinkedList<ProjectObject>();
	public LinkedList<ProjectObject> toApply = new LinkedList<ProjectObject>();
	public LinkedList<ProjectObject> applying = new LinkedList<ProjectObject>();
	const int THREAD_COUNT = 16;

	void Awake(){
		instance = this;
	}

	void Update(){
		for (int i = 0; i < threads.Count; i++) {
			if (threads [i].done) {
				threads.RemoveAt (i);
			}
		}
		lock (toApply) {
			while (toApply.Count > 0) {
				applying.AddLast (toApply.First.Value);
				toApply.RemoveFirst ();
			}
		}
		while (applying.Count > 0) {
			ApplyMesh (applying.First.Value);
			applying.RemoveFirst ();
			Menu.instance.UpdateLoadingText();
		}
	}

	public void GetMeshFrag(ProjectObject projectObject, bool priority){
		lock (toGenerate) {
			if (priority) {
				toGenerate.AddFirst (projectObject);
			} else {
				toGenerate.AddLast (projectObject);
			}
		}
		if (threads.Count < THREAD_COUNT) {
			MeshThreadParams newThread = new MeshThreadParams ();
			newThread.done = false;
			threads.Add (newThread);
			ThreadPool.QueueUserWorkItem (DecodeMesh, newThread);
		}
	}

	void DecodeMesh(object threadParams){
		while (true) {
			ProjectObject projectObject = null;
			lock (toGenerate) {
				if (toGenerate.Count > 0) {
					projectObject = toGenerate.First.Value;
					toGenerate.RemoveFirst ();
				}
			}
			if (projectObject == null) {
				break;
			}
			try {
				int index = projectObject.meshes == null ? 0 : ObjectMesh.getLod (projectObject.lods.Count, projectObject.lod, projectObject.meshes.Length);
				if (projectObject.meshes == null || projectObject.meshes [index] == null || !projectObject.meshes[index].loaded) {
					ObjectMesh.Decode (projectObject,
						Projects.instance.projectModels.Find (model => model.id == projectObject.modelId).data, projectObject.fragments, false);
					lock(toApply) {
						toApply.AddFirst(projectObject);
					}
				}
			} catch (System.Exception error) {
				Debug.LogError (error);
			}
		}
		((MeshThreadParams)threadParams).done = true;
	}

	void ApplyMesh(ProjectObject projectObject){
		for (int i = 0; i < projectObject.lods.Count; i++) {
			int lod = ObjectMesh.getLod (projectObject.lods.Count, i, projectObject.meshes.Length);
			ObjectMesh mesh = projectObject.meshes[lod];
			if (mesh == null) {
				GetMeshFrag (projectObject, true);
			} else if (mesh.triangles.Length > 0 && !mesh.loaded) {
				GenerateMesh (mesh.vertices, mesh.triangles, mesh.colors, mesh.colorIndexes, projectObject, i);
				mesh.loaded = true;
			}
		}
		if(projectObject.isScheduled){
			projectObject.SetLayer ("Scheduled");
		} else {
			projectObject.SetLayer ("Unscheduled");
		}
		projectObject.loaded = true;
		projectObject.EnableAndLoad (true);
	}

	void GenerateMesh(Vector3[] vertices, int[] triangles, Color32[] colors, int[] colorIndexes, ProjectObject projectObject, int index){
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;


		Color32[] vertexColors = new Color32[vertices.Length];
		for (int i = 0; i < colorIndexes.Length; i++) {
			vertexColors [i] = colors [colorIndexes [i]];
		}
		mesh.colors32 = vertexColors;

		var lod = projectObject.lods [index];
		MeshFilter filter = lod.MeshFilter;
		MeshRenderer renderer = lod.meshRenderer;
		if (filter.mesh.vertices.Length > 0) {
			CombineInstance[] combineInstances = new CombineInstance[2];
			combineInstances [0].mesh = mesh;
			combineInstances [1].mesh = filter.mesh;
			Mesh newMesh = new Mesh ();
			newMesh.CombineMeshes (combineInstances, true, false);
			filter.mesh = newMesh;
		} else {
			filter.mesh = mesh;
		}
		if (index == 1) {
			projectObject.meshCollider.sharedMesh = filter.mesh;
		}
		filter.mesh.RecalculateNormals ();
		filter.mesh.RecalculateBounds ();
		for(int i = 0; i < projectObject.categories.Count; i++){
			if(projectObject.categories[i].ToLower().Contains("door") || projectObject.categories[i].ToLower().Contains("glass") || projectObject.categories[i].ToLower().Contains("window")){
				projectObject.gameObject.SetActive(false);
				break;
			}
		}
	}
}

public class MeshThreadParams {
	public int index;
	public ProjectObject projectObject;
	public bool done;
}
