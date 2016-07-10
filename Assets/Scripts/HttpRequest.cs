using UnityEngine;
using UnityEngine.Experimental.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;
	
public static class HttpRequest
{

	private static string ip_address = "http://192.168.1.109:5000";

	private static string getBasic(string username, string password){
		string result = "Basic " + System.Convert.ToBase64String(
			System.Text.Encoding.ASCII.GetBytes(username+":"+password));
		return result;
	}

	public static IEnumerator SendRequest(string url, System.Action<string> onSuccess, string method, string body = null, string username = null, string password =null){
		UnityWebRequest www;
		if (method.Equals ("GET")) {
			www = UnityWebRequest.Get (ip_address + url);
		} else {
			WWWForm form = new WWWForm ();
			form.AddField ("bs", "bs");
			www = UnityWebRequest.Post (ip_address + url, form);

			byte[] rawJson = System.Text.Encoding.UTF8.GetBytes(body);
			UploadHandler upload = new UploadHandlerRaw(rawJson);
			upload.contentType = "application/json";
			www.uploadHandler = upload;
		} 
			
		www.SetRequestHeader ("Content-Type", "application/json");
		if (username != null && password != null)
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


