using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour {

	public static MaterialManager instance;
	List<Material> objectMaterials = new List<Material>();

	public Material baseMaterial;
    public Material plannedMaterial;
	public Material hoverMaterial;
	public Material selectionMaterial;
	public Material prereqMaterial;

	void Awake(){
		instance = this;
	}

	public int RegisterMaterial(Color color){
		int existingMaterialIndex = objectMaterials.FindIndex(mat => {
			return mat.color == color;
		});
		if (existingMaterialIndex < 0) {
			Material newMaterial = new Material (baseMaterial);
			newMaterial.color = color;
			objectMaterials.Add (newMaterial);
			existingMaterialIndex = objectMaterials.Count - 1;
		}
		return existingMaterialIndex;
	}

	public void FadeProjectObject(ProjectObject projectObject){
        projectObject.AssignMaterial(plannedMaterial);
	}

	public void UnFadeProjectObject(ProjectObject projectObject){
		projectObject.AssignMaterial(projectObject.defaultMaterial);
	}

	public Material GetMaterial(int index){
		return objectMaterials[index];
	}
}
