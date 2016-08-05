using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BoardSpace{
	public int x_loc;
	public int y_loc;
}

[System.Serializable]
public class Meeple{
	public int id;
	public bool finished;
	public BoardSpace board_space;
	public Player player;
}

[System.Serializable]
public class Meeples{
	public Meeple[] meeples;
}

[System.Serializable]
public class Card{
	public int id;
	public string value;
	public string direction;
	public bool finished;
	public BoardSpace board_space;
	public int color;
}

[System.Serializable]
public class CardMovement{
	public int id;
	public BoardSpace board_space;
	public Card card;
}

[System.Serializable]
public class Player{
	public int id;
	public int score;
	public string username;
	public string password;
}


[System.Serializable]
public class Cards{
	public List<Card> cards;
}

[System.Serializable]
public class BoardPlayed{
	public Card[] cards;
	public Meeple[] meeples;
	public CardMovement[] card_movement;
}

[System.Serializable]
public class CurrentGame{
	public int id;
	public string status;
	public BoardPlayed board_played;
	public Player hosting;
	public Player joining;
	public Player turn;
	public Player winner;
}


public class InitialPlayer : MonoBehaviour {
	public Transform tilePrefab;
	public bool me;
	public bool cancel;
	// Use this for initialization
	void Start () {
		cancel = false;
		if (me)
			InvokeRepeating ("GetInitialPositions", 0.0f, 10.0f);
		else
			InvokeRepeating ("GetOtherInitalPositions", 0.0f, 10.0f);
	}
		
	public void CreatePlayer(int id, int x, int y, bool my_player = true){
		Transform clone;
		clone = (Transform)Instantiate(tilePrefab, PositionCoordinates.CoordiatesToPosition(x,y), tilePrefab.rotation);
		clone.name = "meeple_" + id;
		clone.GetComponent<SelectPlayer>().meeple_id = id;
		clone.GetComponent<SelectPlayer> ().my_player = my_player;

		if (!my_player)
			clone.GetComponent<SpriteRenderer> ().material.color = Color.red;
		else
			clone.GetComponent<SpriteRenderer> ().material.color = Color.blue;

		clone.position = new Vector3 (clone.position.x, clone.position.y, -1);
		clone.parent = transform;
	}

	public void GetInitialPositions(){
		string url = "/game/"+Config.instance.game_id+"/meeples";
		//httpGet (url, username, password);
		StartCoroutine(HttpRequest.SendRequest(url, HandleGetInitialPosition, "GET", "body", Config.instance.username, Config.instance.password));
	}

	public void GetOtherInitalPositions(){
		string url = "/game/" + Config.instance.game_id;
		StartCoroutine(HttpRequest.SendRequest(url, HandleGetOtherInitialPosition, "GET", "body", Config.instance.username, Config.instance.password));
	}

	public void HandleGetInitialPosition(string response){
		Meeples collection = JsonUtility.FromJson<Meeples> (response);

		for(int i=0; i < collection.meeples.Length; i++){
			cancel = true;
			CreatePlayer(
				collection.meeples[i].id,
				collection.meeples[i].board_space.x_loc,
				collection.meeples[i].board_space.y_loc
			);
		}

		if (cancel)
			CancelInvoke ();
	}

	public void HandleGetOtherInitialPosition(string response){
		CurrentGame game = JsonUtility.FromJson<CurrentGame> (response);

		for (int i = 0; i < game.board_played.meeples.Length; i++) {
			cancel = true;
			if (game.board_played.meeples[i].player.id != Config.instance.player_id) {
				CreatePlayer (
					game.board_played.meeples [i].id,
					game.board_played.meeples [i].board_space.x_loc,
					game.board_played.meeples [i].board_space.y_loc,
					false
				);
			}
		}

		if (cancel)
			CancelInvoke ();
	}
}
