using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class SnapShotCam : MonoBehaviour {

	List<ProjectObject> targetUids = new List<ProjectObject>();
	List<ProjectObject> cloneUids = new List<ProjectObject>();

	int targetOriginalLayer;

	public int snapShotWidth, snapShotHeight;

	public Camera cam;
	public Camera overlayCam;
//	public Camera topDownCam;

	public Material snapShotGreenMaterial;
	public Shader grayShader;

	public int numberOfTestShots;

	Vector3 bestSpot;

	ProjectStep projectStep;

	public void StartProcess(ProjectStep projectStep, List<ProjectObject> projectObjects){
		this.projectStep = projectStep;
		foreach (ProjectObject projectObject in projectObjects) {
			targetUids.Add (projectObject);
		}
		StartCoroutine(FindBestSpot ());
	}

	IEnumerator FindBestSpot(){
		int mostGreenPixels = 0;
		for (int i = 0; i < numberOfTestShots; i++) {
			CreateClones ();
			FrameObject ();
			RenderTexture renderTexture = new RenderTexture (snapShotWidth / 4, snapShotHeight / 4, 24);
			cam.targetTexture = renderTexture;
			Texture2D snapShot = new Texture2D (snapShotWidth / 4, snapShotHeight / 4, TextureFormat.RGB24, false);
			cam.Render ();
			RenderTexture.active = renderTexture;
			snapShot.ReadPixels (new Rect (0, 0, snapShotWidth / 4, snapShotHeight / 4), 0, 0);
			//yield return new WaitForEndOfFrame();
			cam.targetTexture = null;
			RenderTexture.active = null;
			Destroy (renderTexture);
			int greenPixels = 0;
			//yield return new WaitForEndOfFrame();
			for (int x = 0; x < snapShot.width; x++) {
				for (int y = 0; y < snapShot.height; y++) {
					Color pixelColor = (snapShot.GetPixel (x, y));
					if (pixelColor.g == 1 && pixelColor.r == 0 && pixelColor.b == 0) {
						greenPixels++;
					}
				}
			}
			//yield return new WaitForEndOfFrame();
			if (greenPixels > mostGreenPixels) {
				mostGreenPixels = greenPixels;
				bestSpot = cam.transform.position;
			}
			DestroyClones ();
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine(TakeShot ());
	}

	void FrameObject(){
		Bounds bounds = CalculateBounds ();
		Vector3 max = bounds.size;
		float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
		float distance = (radius / (Mathf.Sin (cam.fieldOfView * Mathf.Deg2Rad / 2f))) * 0.7f;
		Vector3 position = Random.onUnitSphere * distance + bounds.center;
		int checks = 0;
		while (Physics.CheckSphere (position, 0.3f) && checks < 10) {
			position = Random.onUnitSphere * distance + bounds.center;
			checks++;
		}
		cam.transform.position = position;
		cam.transform.LookAt (bounds.center);
	}

	Bounds CalculateBounds(){
		Bounds bounds = new Bounds (targetUids[0].transform.position, Vector3.zero);
		foreach (ProjectObject projectObject in targetUids) {
			bounds.Encapsulate (projectObject.meshCollider.bounds);
		}
		return bounds;
	}

	void CreateClones(){
		foreach (ProjectObject projectObject in targetUids) {
			GameObject clone = (GameObject)Instantiate (projectObject.gameObject, projectObject.transform.position, projectObject.transform.rotation);
			clone.layer = LayerMask.NameToLayer ("SnapShot");
			ProjectObject cloneUid = clone.GetComponent<ProjectObject> ();
			cloneUids.Add (cloneUid);
			targetOriginalLayer = projectObject.gameObject.layer;
			projectObject.SetLayer("SnapShotHide");
			List<Material> snapShotMaterials = new List<Material> ();
			var materials = cloneUid.GetMaterials ();
			for (int i = 0; i < materials.Length; i++) {
				snapShotMaterials.Add (snapShotGreenMaterial);
			}
			cloneUid.SetMaterials (snapShotMaterials.ToArray ());
		}
	}

	void DestroyClones(){
		for (int i = 0; i < cloneUids.Count; i++) {
			DestroyImmediate (cloneUids [i].gameObject);
		}
		cloneUids.Clear ();
	}

	IEnumerator TakeShot(){
		Bounds bounds = CalculateBounds ();
		cam.transform.position = bestSpot;
		cam.transform.LookAt (bounds.center);
//		topDownCam.transform.position = new Vector3(bounds.center.x, 30f, bounds.center.z); 
//		topDownCam.transform.LookAt (bounds.center);
		RenderTexture renderTexture = new RenderTexture (snapShotWidth, snapShotHeight, 24);
		cam.targetTexture = renderTexture;
		overlayCam.targetTexture = renderTexture;
		Texture2D snapShot = new Texture2D (snapShotWidth, snapShotHeight, TextureFormat.RGB24, false);
		PrepareForSnapShot ();
		cam.RenderWithShader (grayShader, "");
		overlayCam.Render ();
		UndoSnapShotPrep ();
		RenderTexture.active = renderTexture;
		snapShot.ReadPixels (new Rect (0, 0, snapShotWidth, snapShotHeight), 0, 0);
		snapShot.Apply ();
		//ProjectSteps.instance.AddStepImage (projectStep.stepId, snapShot);
//		ProjectSteps.instance.StashStepImage(snapShot);
		cam.targetTexture = null;
//		topDownCam.targetTexture = renderTexture;
//		Texture2D topDownSnapShot = new Texture2D (snapShotWidth, snapShotHeight, TextureFormat.RGB24, false);
//		overlayCam.transform.position = topDownCam.transform.position;
//		overlayCam.transform.LookAt (bounds.center);
//		PrepareForSnapShot ();
//		topDownCam.RenderWithShader (grayShader, "");
//		overlayCam.Render ();
//		UndoSnapShotPrep ();
//		RenderTexture.active = renderTexture;
//		topDownSnapShot.ReadPixels (new Rect (0, 0, snapShotWidth, snapShotHeight), 0, 0);
//		topDownCam.targetTexture = null;
		overlayCam.targetTexture = null;
//		RenderTexture.active = null;
		Destroy (renderTexture);
//		byte[] snapShotBytes = snapShot.EncodeToPNG ();
//		byte[] topShotBytes = topDownSnapShot.EncodeToPNG ();
//		StartCoroutine(ProjectStepService.SendStepImage(new ImageObject(stepId, snapShotBytes, topShotBytes)));
		yield return new WaitForEndOfFrame ();
		Destroy(this.gameObject);
	}

	public void PrepareForSnapShot(){
		foreach (ProjectObject projectObject in targetUids) {
			projectObject.SetLayer ("SnapShot");
		}
	}

	public void UndoSnapShotPrep(){
		foreach (ProjectStepObject projectStepObject in projectStep.projectObjects) {
			ProjectObjects.instance.GetObjectById(projectStepObject.objectId).AddScheduledActivity (projectStepObject.activity);
		}
	}
}
