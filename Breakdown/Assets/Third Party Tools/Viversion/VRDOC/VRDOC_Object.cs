/*
	--------------------------------
	VRDOC Dynamic Occlusion Culling
	Copyright (C) 2016 Anton Korhonen
	--------------------------------
*/

using UnityEngine;
using System.Collections.Generic;

public class VRDOC_Object : MonoBehaviour {

	public bool isTransparent = false;
	[HideInInspector]
	public bool objectHidden;
	private List<Renderer> objectRenderer = new List<Renderer>();
	[HideInInspector]
	public int frameCountOnVisible;
//	private int startingLayer;
	private UnityEngine.Rendering.ShadowCastingMode SCMode;
	
	private void Start () 
	{
//		startingLayer = gameObject.layer;
		ProjectObject po = GetComponent<ProjectObject> ();
		foreach (Lod lod in po.lods) {
			objectRenderer.Add (lod.meshRenderer);
		}

		if(objectRenderer.Count == 0)
		{
			Destroy(this);
		}
		else
		{
			SCMode = objectRenderer[0].shadowCastingMode;		

			if(!VRDOC_Camera._VRDOC_Camera.enabled)
				this.enabled = false;

//			if(!isTransparent)
//				CheckTransparency();
		}
	}
    
//	private void CheckTransparency()
//	{
//		if (objectRenderer.sharedMaterial.HasProperty("_Mode"))
//		{
//			if(objectRenderer.sharedMaterial.GetFloat("_Mode") == 3f)
//			{
//				isTransparent = true;
//			}
//		}
//		if(!isTransparent)
//		{
//			Renderer[] renderers = GetComponentsInChildren<Renderer>();
//
//			for(int r=0;r<renderers.Length;r++)
//			{
//				for(int m=0;m<renderers[r].sharedMaterials.Length;m++)
//				{
//					if (renderers[r].sharedMaterials[m].HasProperty("_Mode"))
//					{
//						if(renderers[r].sharedMaterials[m].GetFloat("_Mode") == 3f)
//							isTransparent = true;
//					}
//				}
//			}
//		}
//	}
	
	public void VRDOC_DisableRenderer()
	{
		// if(gameObject.GetComponentInParent<ProjectObject>().gameObject.layer == LayerMask.NameToLayer("Unscheduled")){
			objectHidden = true;

		foreach (Renderer renderer in objectRenderer) {
			if (VRDOC_Camera._VRDOC_Camera.UseRealtimeShadows) {
				renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			} else {
				renderer.enabled = false;
			}
		}
		// }
  //      if (!isTransparent)
		//{
		//	objectHidden = true;
			
		//	if (VRDOC_Camera._VRDOC_Camera.UseRealtimeShadows) {
		//		objectRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
		//	} else
		//		objectRenderer.enabled = false;
		//}
		//else
		//{
		//	if(gameObject.layer == VRDOC_Camera._VRDOC_Camera.GetTransparentLayer())
		//	{
		//		//gameObject.layer = startingLayer;
		//	}
		//	else
		//	{
		//		objectHidden = true;
		//		if (VRDOC_Camera._VRDOC_Camera.UseRealtimeShadows)
		//			objectRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
		//		else
		//			objectRenderer.enabled = false;
		//	}
		//}

	}

	public void VRDOC_EnableRenderer()
	{
		frameCountOnVisible = Time.frameCount + VRDOC_Camera._VRDOC_Camera.maxFrameTime;

		if(objectHidden)
		{
			foreach (Renderer renderer in objectRenderer) {
				renderer.shadowCastingMode = SCMode;
				renderer.enabled = true;
			}
			objectHidden = false;
		}
	}
}
