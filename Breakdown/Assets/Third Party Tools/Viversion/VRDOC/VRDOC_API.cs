using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRDOC_API {
	/*
		This class contains the API calls that can be used to interact with VRDOC 
		while your game is running. As the class is static, it does not need to be
		assigned to in your scripts.

		Example usage:
			VRDOC_API.SetRaycastCount(1500);
	 */

	private static VRDOC_Camera VRDOC_Cam;

	public static void _InitializeMainCamera()
	{
		/*
			Called automatically in the VRDOC_Camera script.
		 */
		VRDOC_Cam = Object.FindObjectOfType<VRDOC_Camera>();

		if(!VRDOC_Cam)
		{
			Debug.Log("VRDOC Error: No VRDOC_Camera script found attached to the main camera!");
		}
	}
	
	public static void API_SetTargetLayers(LayerMask targetLayers)
	{
		/*
			Changes the VRDOC target layers
		 */
		 VRDOC_Cam.VRDOC_Layers = targetLayers;
	}

	public static void API_SetRaycastFOV(float fov)
	{
		/*
			Changes the field of view which will be used to fire culling rays.
			This value should always be about 5-10 degrees higher than the field of vision
			of the player.

			For the HTC Vive and Oculus Rift, this value should be around 110.
		*/

		VRDOC_Cam.raycastFieldOfView = fov;

		if(fov < 90)
		{
			Debug.Log("VRDOC Warning: Raycast field of view should be about 10 degrees higher than the HMD FOV.");
		}
	}

	public static void API_SetRaycastCount(int count)
	{
		/*
			Sets the amount of raycasts to be fired per frame.

			A value of 500-600 usually works great on mobile and small rooms and corridors.
			A value greater than 1000 is not recommended on mobile platforms.
		*/

		if(count > 0)
		{
			VRDOC_Cam.raycastCount = count;
		}
		else
		{
			Debug.Log("VRDOC Warning: Setting raycast count to zero!");
		}
	}

	public static void API_UseRealtimeShadows(bool state)
	{
		VRDOC_Cam.UseRealtimeShadows = state;
	}

	public static void API_UseRecasting(bool state)
	{
		VRDOC_Cam.UseRecasting = state;
	}

	public static void API_SetRecastProximityRange(float range)
	{
		/*
			Changes the recasting proximity range.

			When a ray hits a gameobject, all other objects inside this range (metres) will
			be considered visible. A value of < 0.5f is not recommended as it only impacts performance
			with near-zero culling benefit.
		*/
		if(range > 0.5f)
		{
			VRDOC_Cam.recastProximityRange = range;
		}
	}

	public static void API_SetMaxFrameTime(int maxFrameTime)
	{
		/*
			Sets the delay in frames after an object is considered hidden from vision
			and will be culled.
		*/

		if(maxFrameTime > 0)
		{
			VRDOC_Cam.maxFrameTime = maxFrameTime;
		}
		else
		{
			Debug.Log("VRDOC Warning: Max frame time cannot be zero! The minimum should be your target framerate.");
		}
	}
}