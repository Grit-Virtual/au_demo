using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlatformManager : MonoBehaviour {

	public StandaloneInputModule standaloneInputModule;
	public PointerInputModule ovrInputModule;
	public GameObject[] canvases;

	void Awake () {
		#if UNITY_EDITOR
		standaloneInputModule.enabled = true;
		ovrInputModule.enabled = false;
		foreach(GameObject c in canvases){
			GraphicRaycaster[] grs = c.GetComponents<GraphicRaycaster>();
			foreach(GraphicRaycaster gr in grs){
				if(gr is UnityEngine.UI.GraphicRaycaster){
					gr.enabled = true;
				}
				if(gr is ControllerSelection.OVRRaycaster){
					gr.enabled = false;
				}
			}
			SetInitialTransform sit = c.GetComponent<SetInitialTransform>();
			if(sit != null){
				c.GetComponent<SetInitialTransform>().enabled = false;
			}
		}
		#endif
	}

}
