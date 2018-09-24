/*
	--------------------------------
	VRDOC Dynamic Occlusion Culling
	Copyright (C) 2016 Anton Korhonen
	--------------------------------
*/
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class VRDOC_Camera : MonoBehaviour {

	public static VRDOC_Camera instance;
	bool initialized;

	[HeaderAttribute("VRDOC_Object will be auto-added to gameobjects on these layers.")]
	public LayerMask VRDOC_Layers;
	[HeaderAttribute("Transparent gameobjects will use this layer. NOTE: Do not put any gameobjects on this layer.")]
	public string VRDOC_TranparencyLayer;

	[HeaderAttribute("Raycast FOV - set it to about ten units larger than your camera FOV.")]
	[RangeAttribute(60, 200)]
	public float raycastFieldOfView = 90f;
	private int transparencyLayer;

	[HeaderAttribute("How many rays to fire per frame - larger value for higher accuracy.")]
	[RangeAttribute(100, 3000)]
	public int raycastCount;
	[HeaderAttribute("How many frames will it take after an object is hidden?")]
	[RangeAttribute(30, 300)]
	public int maxFrameTime = 80;
	[HeaderAttribute("Does your game have realtime shadows?")]
	public bool UseRealtimeShadows;

	[SpaceAttribute(10)]
	[HeaderAttribute("Massively reduces popping for far away objects. See VRDOC_Readme for more.")]
	public bool UseRecasting;
	[HeaderAttribute("Set the recast proximity range for objects.")]
	[RangeAttribute(0.1f, 5f)]
	public float recastProximityRange;

	public static VRDOC_Camera _VRDOC_Camera;
	private Ray VRDOC_Ray;
	private Camera VR_CAMERA;
	public Camera VRDOC_RaycastCamera;

	private RaycastHit hit;
	private int pointCount;
	private int currentPointIndex;
	private Vector2[] sp;
	private int baseOne = 2;
	private int baseTwo = 3;

    // Generate points thread
    object generatePointsLock = new object();
    bool threadIsDone;
    bool awaitingResponse;

	private List<VRDOC_Object> objects = new List<VRDOC_Object>();
	private Dictionary<Transform, VRDOC_Object> objectsDictionary = new Dictionary<Transform, VRDOC_Object>();
    List<VRDOC_Object> notHiddenObjects = new List<VRDOC_Object>();

    private void Awake() {
        instance = this;
        _VRDOC_Camera = this;
        VR_CAMERA = GetComponent<Camera>();

        if (VR_CAMERA == null)
            Debug.LogError("VRDOC Error: The VRDOC_Camera component should be attached to your main camera!");

        VerifyTransparentLayer();
        VRDOC_API._InitializeMainCamera();
    }

    public void Initialize() {
        InitializeRaycastCamera();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("UID Object");

        for (int i = 0; i < gameObjects.Length; i++) {
			VRDOC_Object vrdocObject = gameObjects[i].GetComponent<VRDOC_Object>();
			if (vrdocObject == null) {
				vrdocObject = gameObjects[i].AddComponent<VRDOC_Object>();
			}
			objects.Add(vrdocObject);
			objectsDictionary.Add(gameObjects[i].transform, vrdocObject);
        }
        
        awaitingResponse = true;
        lock (generatePointsLock) {
            threadIsDone = false;
        }
		ThreadPool.QueueUserWorkItem ((object obj) => {
			try {
				GenerateRaycastPoints(obj);
			} catch (System.Exception e) {
				UnityEngine.Debug.Log (e);
			}
		}, null);
    }

    private void Update() {
        if (awaitingResponse) {
            lock (generatePointsLock) {
                if (threadIsDone) {
                    awaitingResponse = false;
                    initialized = true;
                    Initialization.instance.InitCameraOccDone();
                }
            }
        }

        if (initialized) {
            for (int i = 0; i < raycastCount; i++)
                Raycast();

            if (Time.frameCount % 12 == 0) {
                notHiddenObjects = objects.FindAll(o => {
                    return !o.objectHidden;
                });

                foreach (VRDOC_Object obj in notHiddenObjects) {
                    if (Time.frameCount > obj.frameCountOnVisible) {
                        obj.VRDOC_DisableRenderer();
                    }
                }
            }
        }
    }

    private void GenerateRaycastPoints(object paramObject) {
        pointCount = Mathf.FloorToInt(Screen.width * Screen.height / 5f);
        sp = new Vector2[pointCount];

        for (int p = 0; p < pointCount; p++)
            sp[p] = new Vector2(GetRandomValue(baseOne, p), GetRandomValue(baseTwo, p));

        lock (generatePointsLock) {
            threadIsDone = true;
        }
    }

    public float GetCameraFarClipPlane()
	{
		return VR_CAMERA.farClipPlane;
	}

	private void VerifyTransparentLayer()
	{
		LayerMask transparentLayer = LayerMask.NameToLayer(VRDOC_TranparencyLayer);

		if(transparentLayer.value == -1)
			Debug.LogError("VRDOC Error: The transparent layer " + VRDOC_TranparencyLayer + " does not exist.");
		else
			transparencyLayer = transparentLayer.value;
	}

	public int GetTransparentLayer()
	{
		return transparencyLayer;
	}
    
	public static float GetRandomValue(int baseValue, int setIndex)
	{
		float f_target = 1f / baseValue, ci = setIndex, result = 0f;

		while(ci > 0)
		{
			result += f_target * (ci % baseValue);
			ci = Mathf.FloorToInt(ci / baseValue);
			f_target = f_target / baseValue;
		}
		
		return result;
	}

    private void FinishedThreadTask() {
        initialized = true;
        // Initialization.instance.statusText.text = "Status: Loading default object meshes";
        // DistanceCheck.instance.LoadDefaultObjects();
    }

    private void InitializeRaycastCamera()
	{
		GameObject VRDOC_RayCastCamera_GameObject = new GameObject("VRDOC_RaycastCamera");
		VRDOC_RayCastCamera_GameObject.transform.position = this.transform.position;
		VRDOC_RayCastCamera_GameObject.transform.rotation = this.transform.rotation;
		VRDOC_RayCastCamera_GameObject.transform.parent = this.transform;

		VRDOC_RaycastCamera = VRDOC_RayCastCamera_GameObject.AddComponent<Camera>();
		VRDOC_RaycastCamera.nearClipPlane = VR_CAMERA.nearClipPlane;
		VRDOC_RaycastCamera.farClipPlane = VR_CAMERA.farClipPlane;
		VRDOC_RaycastCamera.cullingMask = 0;
		VRDOC_RaycastCamera.aspect = VR_CAMERA.aspect;
		VRDOC_RaycastCamera.clearFlags = CameraClearFlags.Nothing;
		VRDOC_RaycastCamera.enabled = false;
	}

    Collider[] sphereHits = new Collider[10];
	int numberOfHits;

	private void Raycast()
	{

		VRDOC_Ray = VRDOC_RaycastCamera.ViewportPointToRay(new Vector3(sp[currentPointIndex].x, sp[currentPointIndex].y, 0f));

		currentPointIndex += 1;

		if(currentPointIndex >= pointCount) 
			currentPointIndex = 0;
		
		if(Physics.Raycast(VRDOC_Ray, out hit, VR_CAMERA.farClipPlane, VRDOC_Layers.value, QueryTriggerInteraction.Collide))
		{
			VRDOC_EnableRenderer(hit.transform);

			if(UseRecasting)
			{
				numberOfHits = Physics.OverlapSphereNonAlloc(hit.point, recastProximityRange, sphereHits, VRDOC_Camera._VRDOC_Camera.VRDOC_Layers.value, QueryTriggerInteraction.Collide);

				for(int i = 0; i < numberOfHits; i++)
				{
					VRDOC_EnableRenderer(sphereHits[i].transform);
				}
			}
		}
	}

	private VRDOC_Object vo;
	
	public void VRDOC_EnableRenderer(Transform target){

		// vo = target.GetComponent<VRDOC_Object>();
		vo = objectsDictionary.ContainsKey(target) ? objectsDictionary[target] : null;

		if(vo != null)
			vo.VRDOC_EnableRenderer();
		else if(target.parent != null)
			VRDOC_EnableRenderer(target.parent);
	}

}