using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour {
	public StandaloneInputModule standaloneInputModule;
	public GraphicRaycaster[] graphicRaycasters;

	public OVRInputModule ovrInputModule;
	public OVRRaycaster[] ovrRaycasters;
	 

	void Awake () {
		#if UNITY_EDITOR
		standaloneInputModule.enabled = true;
		ovrInputModule.enabled = false;
		foreach(GraphicRaycaster gr in graphicRaycasters){
			gr.enabled = true;
		}
		foreach(OVRRaycaster or in ovrRaycasters){
			or.enabled = false;
		}
		#endif
	}

}
