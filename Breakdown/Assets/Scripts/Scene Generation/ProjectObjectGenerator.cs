using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectObjectGenerator : MonoBehaviour {

	public static ProjectObjectGenerator instance;

	public GameObject projectObjectPrefab;

	void Awake(){
		instance = this;
	}
		
//	public void GenerateObject(Hashtable projectObjectResponse){
//		GameObject projectObject = (GameObject)Instantiate (projectObjectPrefab);
//		Uid uid = projectObject.GetComponent<Uid> ();
//		uid.InitializeInfo (new ObjectInitializer(projectObjectResponse));
//		ProjectObjects.instance.AddUid (uid);
//	}
}
