﻿using UnityEngine;
using UnityEngine.Experimental.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;
	
public static class HttpRequest
{

	private static string getBasic(string username, string password){
		string result = "Basic " + System.Convert.ToBase64String(
			System.Text.Encoding.ASCII.GetBytes(username+":"+password));
		return result;
	}

	public static IEnumerator SendRequest(string url, System.Action<string> onSuccess, string method, string body = null, string username = null, string password =null){
		UnityWebRequest www;
		if (method.Equals ("GET")) {
			www = UnityWebRequest.Get ("http://127.0.0.1:5000" + url);
		} else {
			WWWForm form = new WWWForm ();
			form.AddField ("bs", "bs");
			www = UnityWebRequest.Post ("http://127.0.0.1:5000" + url, form);

			byte[] rawJson = System.Text.Encoding.UTF8.GetBytes(body);
			UploadHandler upload = new UploadHandlerRaw(rawJson);
			upload.contentType = "application/json";
			www.uploadHandler = upload;
		} 
			
		www.SetRequestHeader ("Content-Type", "application/json");
		www.SetRequestHeader ("Authorization", getBasic (username, password));

		yield return www.Send();

		if(www.isError) {
			Debug.Log(www.error);
		}
		else {
			onSuccess(www.downloadHandler.text);
		}

	}
}


