using UnityEngine;
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
		UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000"+url);
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


