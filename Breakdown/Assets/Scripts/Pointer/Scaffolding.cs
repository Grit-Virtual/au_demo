using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffolding : MonoBehaviour {

	public static Scaffolding instance;

	public bool showingScaffolding;

	public Transform userTransform;
	public GameObject scaffolding;
	public ToggleButtonUI scaffoldingButton;

	public Transform groundPlane;

	void Awake(){
		instance = this;
	}

	void Start(){
		scaffolding.SetActive(false);
	}

	public void ScaffoldingButtonClick(){
		if(!showingScaffolding){
			ShowScaffolding();
		} else {
			if(scaffolding.transform.position.y < groundPlane.position.y){
				LockGroundPlane();
			}
			HideScaffolding();
		}
	}

	public void ShowScaffolding(){
		showingScaffolding = true;
		scaffolding.transform.position = userTransform.position + Vector3.down * ArcTeleporter.instance.height;
		scaffolding.SetActive(true);
		scaffoldingButton.SetActive(true);
	}

	public void HideScaffolding(){
		showingScaffolding = false;
		scaffolding.SetActive(false);
		scaffoldingButton.SetActive(false);
	}

	void LockGroundPlane(){
		groundPlane.position = scaffolding.transform.position;
	}
}
