using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Restifizer;

public static class ProjectService {

    public static void InitializeProject (){
		RestifizerManager.Instance.ConfigBearerAuth(Utils_Prefs.GetAuthKey());
		RestifizerManager.Instance.ResourceAt ("project/" + Utils_Prefs.GetProjectId()).WithBearerAuth().Get((response) => {
			if(response.HasError){
				Alert.instance.ShowAlert ("Failed to get project");
			} else {
				Projects.instance.SetProject(new Project((Hashtable)response.Resource["response"]));
			}
		});
	}

	public static void PopulateProjectSelectionList (){
		RestifizerManager.Instance.ConfigBearerAuth(Utils_Prefs.GetAuthKey());
		RestifizerManager.Instance.ResourceAt ("projects/ready").WithBearerAuth().Get((response) => {
			if(response.HasError){
				Alert.instance.ShowAlert("Failed to get projects");
			} else {
				ArrayList projectsList = (ArrayList)response.Resource["response"];
				for(int i = 0; i < projectsList.Count; i++){
					SelectProject.instance.AddProjectToList(new Project((Hashtable)projectsList[i]));
				}
				SelectProject.instance.PopulateSelectors();
				SelectProject.instance.ShowProjectSelectScreen();
				LoadingScreen.instance.HideLoadingScreen();
			}
		});
	}
}
