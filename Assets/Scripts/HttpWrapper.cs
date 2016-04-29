using UnityEngine;
using UnityEngine.Experimental.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class HttpWrapper : MonoBehaviour {

	private string getBasic(string username, string password){
		string result = "Basic " + System.Convert.ToBase64String(
			System.Text.Encoding.ASCII.GetBytes(username+":"+password));
		return result;
	}

	public void createGame(){
		string url = "/game";
		string body = "{\"status\":\"starting\"}";
		string username = "matt";
		string password = "test";
		//httpPost (url, body, username, password);
	}

	public void getGames(){
		string url = "/game";
		string username = "matt";
		string password = "test";
		//httpGet (url, username, password);
		StartCoroutine(SendRequest(url, handleGetGames, "GET", "body", username, password));
	}

	public void handleGetGames(string response){
		Games collection = JsonUtility.FromJson<Games> (response);

		Debug.Log (collection.games[0].status);
	}

	IEnumerator SendRequest(string url, System.Action<string> onSuccess, string method, string body = null, string username = null, string password =null){
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
	/*
	private void httpGet(string url, string username = null, string password = null){
		Dictionary<string, string> headers = new Dictionary<string,string>();;
		headers.Add("Content-Type", "application/json");
		if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			headers.Add ("Authorization", getBasic(username, password));
		WWW www = new WWW("http://127.0.0.1:5000" + url, null, headers); 
		StartCoroutine (SendRequest (www));
	}

	private void httpPost(string url,  string body, string username = null, string password = null){
		Debug.Log ("username" + username);
		Dictionary<string, string> headers = new Dictionary<string,string>();;
		headers.Add("Content-Type", "application/json");
		if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			headers.Add ("Authorization", getBasic(username, password));
		Debug.Log (headers);
		byte[] input = Encoding.UTF8.GetBytes (body);
		WWW www = new WWW("http://127.0.0.1:5000" +url, input, headers); 
		StartCoroutine (SendRequest (www));
	}
		
	IEnumerator SendRequest(WWW www){
		yield return www;

		Debug.Log (www.text);
		Games collection = JsonUtility.FromJson<Games> (www.text);

		Debug.Log (collection.games[0].status);

		string test = "{\"id\": 1, \"status\": \"In Progress\"}";
		Game game = JsonUtility.FromJson<Game> (test);
		Debug.Log(game.id);
	}
	*/
}
