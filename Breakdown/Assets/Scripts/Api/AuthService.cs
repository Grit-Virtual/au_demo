using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Restifizer;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class AuthService {

	static AuthInfo authInfo;

	public static void UserLogin(string email, string password, bool planner = false){
		Hashtable parameters = new Hashtable();
		parameters.Add ("email", email);
		parameters.Add("password", password);
		parameters.Add ("remeberMe", true);
		Utils_Prefs.StoreEnvironment(-1);
		RestifizerManager.Instance.ResourceAt("authenticate").Post(parameters, (response) => {
            if (response == null){
				Alert.instance.ShowAlert("Server connection failed");
				if(!planner){
					Login.instance.loginScreen.SetActive (true);
					LoadingScreen.instance.HideLoadingScreen();
				}
            } else {
				if (response.HasError){
					Alert.instance.ShowAlert("Login failed");
					if(!planner){
						Login.instance.loginScreen.SetActive (true);
                    	LoadingScreen.instance.HideLoadingScreen();
					}
				} else {
					authInfo = new AuthInfo(((Hashtable)response.Resource["response"])["token"].ToString(), ((Hashtable)response.Resource["response"])["userId"].ToString());
					Utils_Prefs.StoreEnvironment((int) RestifizerManager.Instance.environment);
					Utils_Prefs.StoreAuthKey(authInfo.token);
					Utils_Prefs.StoreUserId(authInfo.userId);
					SegmentService.Identify(authInfo.userId);
					Hashtable segmentParameters = new Hashtable();
					segmentParameters.Add("userId", authInfo.userId);
					SegmentService.Track("Log In", segmentParameters);
					if(planner){
						MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.NONE);
					}
					GetAuthenticatedUser(planner);
                }
            }
		});
    }

	public static void GetAuthenticatedUser(bool planner){
		RestifizerManager.Instance.ConfigBearerAuth(Utils_Prefs.GetAuthKey());
		RestifizerManager.Instance.ResourceAt ("authenticatedUser").WithBearerAuth ().Get ((response) => {
			if(!response.HasError){
				authInfo.userEmail = ((Hashtable)response.Resource["response"])["email"].ToString();
				if(((Hashtable)response.Resource["response"])["firstName"] != null){
					authInfo.userName = ((Hashtable)response.Resource["response"])["firstName"].ToString();
				}
				if(((Hashtable)response.Resource["response"])["lastName"] != null){
					authInfo.userName =authInfo.userName != "" ? authInfo.userName + " " + ((Hashtable)response.Resource["response"])["lastName"].ToString() : ((Hashtable)response.Resource["response"])["lastName"].ToString();
				}
			}
			Utils_Prefs.StoreUserEmail(authInfo.userEmail);
			Utils_Prefs.StoreUserName(authInfo.userName);
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = new FileStream(Application.persistentDataPath + "/auth.grit", FileMode.Create);
			bf.Serialize(fs, authInfo);
			fs.Close();
			if(!planner){
				if(authInfo.userName != ""){
					SelectProject.instance.SetNameText(authInfo.userName);
				} else {
					SelectProject.instance.SetNameText(authInfo.userEmail);
				}
				SelectProject.instance.PopulateProjectSelectionList();
			}
		});
	}

	public static void ShortCodeLogin(string shortCode, bool planner = false){
		int environment = 2;
		switch (shortCode.Substring(0, 2)) {
		case "99":
			environment = 0;
			break;
		case "98":
			environment = 1;
			break;
		case "97":
			environment = 3;
			break;
		}
		Utils_Prefs.StoreEnvironment(environment);
		RestifizerManager.Instance.ResourceAt("authenticate/shortCode/" + shortCode).Get((response) => {
			if(response == null){
				Alert.instance.ShowAlert("Server connection failed");
				if(!planner){
					Login.instance.loginScreen.SetActive (true);
					LoadingScreen.instance.HideLoadingScreen();
				}
			} else {
				if (response.HasError){
					Alert.instance.ShowAlert("Login failed");
					if(!planner){
						Login.instance.loginScreen.SetActive (true);
						LoadingScreen.instance.HideLoadingScreen();
					}
				} else {
					authInfo = new AuthInfo(((Hashtable)response.Resource["response"])["token"].ToString(), ((Hashtable)response.Resource["response"])["userId"].ToString());
					authInfo.environment = environment;
					Utils_Prefs.StoreEnvironment(authInfo.environment);
					Utils_Prefs.StoreAuthKey(authInfo.token);
					Utils_Prefs.StoreUserId(authInfo.userId);
					SegmentService.Identify(authInfo.userId);
					Hashtable segmentParameters = new Hashtable();
					segmentParameters.Add("userId", authInfo.userId);
					SegmentService.Track("Log In", segmentParameters);
					if(planner){
						MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.NONE);
					}
					GetAuthenticatedUser(planner);
				}
			}
		});
	}

	public static void CheckToken(AuthInfo authInfo){
		RestifizerManager.Instance.ConfigBearerAuth(authInfo.token);
		Utils_Prefs.StoreEnvironment(authInfo.environment);
		RestifizerManager.Instance.ResourceAt("isAuthenticated").WithBearerAuth().Get((response) => {
			if(response == null){
				Login.instance.ShowLoginScreen();
			} else {
				if (response.HasError){
					Login.instance.ShowLoginScreen();
					File.Delete(Application.persistentDataPath + "/auth.grit");
				} else {
					Utils_Prefs.StoreAuthKey(authInfo.token);
					Utils_Prefs.StoreUserId(authInfo.userId);
					Utils_Prefs.StoreUserEmail(authInfo.userEmail);
					Utils_Prefs.StoreUserName(authInfo.userName);
					SegmentService.Identify(authInfo.userId);
					Hashtable segmentParameters = new Hashtable();
					segmentParameters.Add("userId", authInfo.userId);
					SegmentService.Track("Log In", segmentParameters);
					if(authInfo.userName != ""){
						SelectProject.instance.SetNameText(authInfo.userName);
					} else {
						SelectProject.instance.SetNameText(authInfo.userEmail);
					}
					Login.instance.HideLoginScreen();
					SelectProject.instance.PopulateProjectSelectionList();
				}
			}
		});
	}

	public static void CheckToken(){
		RestifizerManager.Instance.ConfigBearerAuth(Utils_Prefs.GetAuthKey());
		RestifizerManager.Instance.ResourceAt("isAuthenticated").WithBearerAuth().Get((response) => {
			if(response == null){
				MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.LOGIN);
			} else {
				if (response.HasError){
					MenuManager.instance.MenuButtonClick((int)MenuManager.MenuType.LOGIN);
				}
			}
		});
	}
}
