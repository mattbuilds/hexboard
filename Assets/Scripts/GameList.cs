using UnityEngine;
using UnityEngine.UI;
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
		getGames ();
		InvokeRepeating ("getGames", 2.0f, 10.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void getGames(){
		string url = "/game";
		//httpGet (url, username, password);
		StartCoroutine(HttpRequest.SendRequest(url, handleGetGames, "GET", "body","matt", "test"));
	}

	public void handleGetGames(string response){
		Debug.Log(response);
		Games collection = JsonUtility.FromJson<Games> (response);

		foreach (Transform child in transform) {
			Destroy (child.gameObject);
		}

		for (int i = 0; i < collection.games.Length; i++) {
			float y = -63.5f + i * -120f;

			GameObject clone;
			clone = (GameObject)Instantiate(button_game_prefab, new Vector3(0,0,0), Quaternion.identity);
			clone.transform.SetParent(transform);
			clone.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			clone.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f,y,0.0f);
			clone.transform.Find ("Id").GetComponent<UnityEngine.UI.Text>().text = "Join Game " + collection.games [i].id.ToString();
			Button b = clone.GetComponent<Button>();
			AddListener(b, collection.games [i].id);
		} 

		Debug.Log (collection.games[0].status);
	}
	void AddListener(Button b, int value) 
	{
		b.onClick.AddListener(() => joinGame(value));
	}

	public void createGame(){
		string url = "/player/generate";
		StartCoroutine(HttpRequest.SendRequest(url, handleCreateGame, "POST", "body"));
	}

	public void handleCreateGame(string response){
		Player player = JsonUtility.FromJson<Player> (response);

		PlayerPreferencs.username = player.username;
		PlayerPreferencs.password = player.password;
		PlayerPreferencs.player_id = player.id;

		string url = "/game";
		StartCoroutine(HttpRequest.SendRequest(url, handleCreateSwitch, "POST", "body", player.username, player.password));
	}

	public void handleCreateSwitch(string response){
		// Parse game info
		Debug.Log(response);
		Game game = JsonUtility.FromJson<Game>(response);

		// Store credentials and game_id in global object
		PlayerPreferencs.game_id = game.id;

		// Switch to new scene
		Application.LoadLevel(1);
	}

	public void joinGame(int game_id){
		string url = "/player/generate";
		PlayerPreferencs.game_id = game_id;
		StartCoroutine(HttpRequest.SendRequest(url, handleJoinGame, "POST", "body"));
	}

	public void handleJoinGame(string response){
		Player player = JsonUtility.FromJson<Player> (response);

		PlayerPreferencs.username = player.username;
		PlayerPreferencs.password = player.password;
		PlayerPreferencs.player_id = player.id;

		string url = "/game/"+PlayerPreferencs.game_id;
		StartCoroutine(HttpRequest.SendRequest(url, handleJoinSwitch, "POST", "body", player.username, player.password));

	}

	public void handleJoinSwitch(string response){
		// Switch to new scene
		Application.LoadLevel(1);
	}
}
