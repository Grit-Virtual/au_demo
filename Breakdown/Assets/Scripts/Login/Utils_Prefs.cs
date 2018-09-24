using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils_Prefs {

	public static void StoreAuthKey(string token){
		PlayerPrefs.SetString ("token", token);
	}

	public static string GetAuthKey(){
		if (!PlayerPrefs.HasKey ("token")) {
			PlayerPrefs.SetString ("token", "");
		}

		return PlayerPrefs.GetString ("token");
	}

	public static void StoreEnvironment(int environment){
		PlayerPrefs.SetInt ("environment", environment);
	}

	public static int GetEnvironment(){
		if (!PlayerPrefs.HasKey ("environment")) {
			PlayerPrefs.SetInt ("environment", -1);
		}

		return PlayerPrefs.GetInt ("environment");
	}

	public static void StoreProjectId(string projectId){
		PlayerPrefs.SetString ("projectId", projectId);
	}

	public static string GetProjectId(){
		if (!PlayerPrefs.HasKey ("projectId")) {
			PlayerPrefs.SetString ("projectId", "");
		}

		return PlayerPrefs.GetString ("projectId");
	}

	public static void StoreUserId(string userId){
		PlayerPrefs.SetString ("userId", userId);
	}

	public static string GetUserId(){
		if (!PlayerPrefs.HasKey ("userId")) {
			PlayerPrefs.SetString ("userId", "");
		}

		return PlayerPrefs.GetString ("userId");
	}

	public static void StoreUserEmail(string userEmail){
		PlayerPrefs.SetString ("userEmail", userEmail);
	}

	public static string GetUserEmail(){
		if (!PlayerPrefs.HasKey ("userEmail")) {
			PlayerPrefs.SetString ("userEmail", "");
		}

		return PlayerPrefs.GetString ("userEmail");
	}

	public static void StoreUserName(string userName){
		PlayerPrefs.SetString ("userName", userName);
	}

	public static string GetUserName(){
		if (!PlayerPrefs.HasKey ("userName")) {
			PlayerPrefs.SetString ("userName", "");
		}

		return PlayerPrefs.GetString ("userName");
	}

    public static void ClearPrefs() {
        PlayerPrefs.DeleteAll();
    }
}
