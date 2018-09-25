using UnityEngine;
using System.Collections;
using System.IO;

namespace Restifizer {
	public class RestifizerManager: MonoBehaviour, RestifizerParams {
		public enum Environment {
			LOCAL,
			DEVELOP,
			PRODUCTION,
			STAGING
		}
		public Environment environment;

		[HideInInspector]
		public string baseUrl {
			get {
				if (overriddenBaseUrl == null) {
					int overriddenEnvironment = Utils_Prefs.GetEnvironment ();
					switch (overriddenEnvironment == -1 ? (int) environment : overriddenEnvironment) {
					case 0:
						return "http://localhost:8080/";
					case 1:
						return "https://api.gritvirtual.info/";
					case 2:
						return "https://api.gritvirtual.com/";
					case 3:
						return "https://api.gritvirtual.xyz/";
					}
				}
				return overriddenBaseUrl;
			}
			set {
				overriddenBaseUrl = value;
			}
		}

		public int projectObjectPageSize, projectStepPageSize, activityPageSize;
		public MonoBehaviour errorHandler;
		public static RestifizerManager Instance;

		private string overriddenBaseUrl = null;
		private string clientId;
		private string clientSecret;
		private string accessToken;

		void Awake() {
			Instance = this;
			if (errorHandler != null && !(errorHandler is IErrorHandler)) {
				Debug.LogError("Wrong ErrorHandler, it should implement IErrorHandler");
			}
		}

		void Start(){
			SetEnvironment ();
		}

		public void SetEnvironment(){
			overriddenBaseUrl = null;
		}

		public RestifizerManager ConfigClientAuth(string clientId, string clientSecret) {
			this.clientId = clientId;
			this.clientSecret = clientSecret;
			return this;
		}

		public RestifizerManager ConfigBearerAuth(string accessToken) {
			this.accessToken = accessToken;
			return this;
		}

		public RestifizerRequest ResourceAt(string resourceName) {
			RestifizerRequest restifizerRequest = new RestifizerRequest(this, (IErrorHandler)errorHandler);
			restifizerRequest.FetchList = true;
			restifizerRequest.Path += baseUrl + "/" + resourceName;
			return restifizerRequest;
		}


		/* RestifizerParams */
		public string GetClientId() {
			return clientId;
		}

		public string GetClientSecret() {
			return clientSecret;
		}

		public string GetAccessToken() {
			return accessToken;
		}
	}
}