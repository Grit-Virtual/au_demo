using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AuthInfo {
	public string token;
	public string userId;
	public string userName;
	public string userEmail;
	public int environment = -1;

	public AuthInfo(string token, string userId){
		this.userId = userId;
		this.token = token;
		this.userEmail = "";
		this.userName = "";
	} 
}
