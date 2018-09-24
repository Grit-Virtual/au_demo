using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Raycaster : UnityEngine.EventSystems.BaseRaycaster {

	public LayerMask selectionMask, prereqMask, uiMask;
	LayerMask currentLayerMask;
	public Transform leftController, rightController;
	Transform activeController;
	[HideInInspector]
	public bool pointerEnabled;

	public static Raycaster instance;

	public override Camera eventCamera {
		get {
			return Camera.main;
		}
	}

	protected override void Awake(){
		instance = this;
	}

	protected override void Start(){
		currentLayerMask = selectionMask;
		SetPointerEnabled(true);
		activeController = rightController;
	}

	void Update(){
        for(int i = 0; i < Input.GetJoystickNames().Length; i++){
            if(Input.GetJoystickNames()[i].EndsWith("Left") && activeController == rightController){
                activeController = leftController;
                break;
            } else if(Input.GetJoystickNames()[i].EndsWith("Right") && activeController == leftController){
				activeController = rightController;
				break;
			}
        }
	}

	public override void Raycast(UnityEngine.EventSystems.PointerEventData eventData, List<UnityEngine.EventSystems.RaycastResult> resultAppendList) {
		float dist = 10.0f;
		RaycastHit hit = new RaycastHit();
		if(pointerEnabled){
			if(!Physics.Raycast(activeController.position, activeController.forward, out hit, dist, uiMask)){
				if(ObjectSelection.instance && ObjectSelection.instance.selectionEnabled){
					Physics.Raycast(activeController.position, activeController.forward, out hit, dist, currentLayerMask);
				}
			}
		}
		PointerVisualizer.instance.SetPointer(hit);
		if(hit.collider != null && Controller.instance.pointerMode == PointerMode.SELECT){
			RaycastResult result = new RaycastResult {
				gameObject = hit.collider.gameObject,
				module = this,
				distance = hit.distance,
				index = resultAppendList.Count,
				worldPosition = hit.point,
				worldNormal = hit.normal,
			};
			resultAppendList.Add(result);
		}
	}

	public void ApplyPrereqMask(){
		currentLayerMask = prereqMask;
	}

	public void ApplySelectionMask(){
		currentLayerMask = selectionMask;
	}

	public void SetPointerEnabled(bool enabled){
		pointerEnabled = enabled;
	}
}
