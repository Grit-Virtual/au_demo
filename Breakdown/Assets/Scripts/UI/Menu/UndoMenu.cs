using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoMenu : MonoBehaviour {

	public static UndoMenu instance;

	public List<ProjectStep> undoSteps = new List<ProjectStep>();

	void Awake () {
		instance = this;
	}

	public void UndoButtonClick(){
		if(undoSteps.Count > 0){
			Confirmation.instance.ShowConfirmation (UndoLastStep, null, "Are you sure you want to undo the last planned task?\n" + UndoMenu.instance.undoSteps[0].name, "Undo");
		}
	}

	public void AddUndoStep(ProjectStep step){
		undoSteps.Insert (0, step);
	}

	public void UndoLastStep(){
		if(undoSteps.Count > 0){
			StartCoroutine(ProjectStepService.DeleteProjectStep(undoSteps[0]));
			Menu.instance.DeselectButtonClick();
		}
	}
}
