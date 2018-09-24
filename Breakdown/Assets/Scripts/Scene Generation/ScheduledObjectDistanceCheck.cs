using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScheduledObjectDistanceCheck : MonoBehaviour {

	public static ScheduledObjectDistanceCheck instance;

	void Awake(){
		instance = this;
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "UID Object" && collider.gameObject.layer == LayerMask.NameToLayer("Scheduled")) {
			ProjectObject projectObject = collider.GetComponent<ProjectObject> ();
			projectObject.EnableAndLoad (true);
		}
	}

	void OnTriggerExit(Collider collider){
		if (collider.tag == "UID Object" && collider.gameObject.layer == LayerMask.NameToLayer("Scheduled")) {
			ProjectObject projectObject = collider.GetComponent<ProjectObject> ();
			if(!projectObject.wallFloorOrRoof) {
				projectObject.Disable ();
			}
		}
	}

	public void Initialize(){
		List<ProjectObject> scheduledObjects = ProjectObjects.instance.GetScheduledObjects();
		List<Collider> collidersInRange = Physics.OverlapSphere (transform.position, 20f).ToList();
		foreach(ProjectObject projectObject in scheduledObjects){
			if(collidersInRange.Contains(projectObject.loadingCollider)){
				// projectObject.EnableAndLoad (true);
			} else {
				if (projectObject.wallFloorOrRoof) {
					// projectObject.EnableAndLoad (false);
				} else {
					projectObject.Disable ();
				}
			}
		}
	}
}
