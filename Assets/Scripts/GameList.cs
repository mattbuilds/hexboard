﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game{
	public int id;
	public string status;
}

[System.Serializable]
public class Games{
	public Game[] games;
}


public class GameList : MonoBehaviour {
	public GameObject button_game_prefab;
	public Games games;
	// Use this for initialization
	void Start () {
		games = new Games ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void getGames(){
		string url = "/game";
		string username = "matt";
		string password = "test";
		//httpGet (url, username, password);
		StartCoroutine(HttpRequest.SendRequest(url, handleGetGames, "GET", "body", username, password));
	}

	public void handleGetGames(string response){
		Games collection = JsonUtility.FromJson<Games> (response);

		foreach (Transform child in transform) {
			Destroy (child.gameObject);
		}

		for (int i = 0; i < collection.games.Length; i++) {
			float y = -20f + i * -40f;

			GameObject clone;
			clone = (GameObject)Instantiate(button_game_prefab, new Vector3(0,0,0), Quaternion.identity);
			clone.transform.parent = transform;
			clone.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			clone.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f,y,0.0f);
			clone.transform.Find ("Id").GetComponent<UnityEngine.UI.Text>().text = collection.games [i].id.ToString();
			clone.transform.Find ("Status").GetComponent<UnityEngine.UI.Text>().text = collection.games [i].status;
		} 

		Debug.Log (collection.games[0].status);
	}
}
