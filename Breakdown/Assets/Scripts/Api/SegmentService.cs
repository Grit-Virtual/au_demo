using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Restifizer;

public static class SegmentService {

	static string writeKey = "HNdhDc21wkzVm6jHABvQVcwXn1xX4Ade";

	public static void Identify(string userId){
		if(RestifizerManager.Instance.environment == RestifizerManager.Environment.PRODUCTION){
			Hashtable parameters = new Hashtable();
			parameters.Add ("userId", userId);
			parameters.Add("traits", new Hashtable());
			RestifizerManager.Instance.ConfigBearerAuth("Basic " + Convert.ToBase64String (Encoding.UTF8.GetBytes (writeKey)));
			RestifizerManager.Instance.baseUrl = "https://api.segment.io/v1/";
			RestifizerManager.Instance.ResourceAt("identify").JSONContent().Post(parameters, (response) => {});
			RestifizerManager.Instance.SetEnvironment ();
		}
	}

	public static void Track(string trackEvent, Hashtable properties){
		if(RestifizerManager.Instance.environment == RestifizerManager.Environment.PRODUCTION){
			Hashtable parameters = new Hashtable();
			parameters.Add ("event", trackEvent);
			parameters.Add("properties", properties);
			RestifizerManager.Instance.ConfigBearerAuth("Basic " + Convert.ToBase64String (Encoding.UTF8.GetBytes (writeKey)));
			RestifizerManager.Instance.baseUrl = "https://api.segment.io/v1/";
			RestifizerManager.Instance.ResourceAt("track").JSONContent().Post(parameters, (response) => {});
			RestifizerManager.Instance.SetEnvironment ();
		}
	}

}
