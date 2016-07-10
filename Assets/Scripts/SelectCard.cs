using UnityEngine;
using System;
using System.Collections;

public class SelectCard : MonoBehaviour {
	public bool on_board, selected;
	public int id;
	public string direction;

	// Use this for initialization
	void Start () {
		on_board = false;
		selected = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		if (!Config.instance.turn)
			return;
		
		if (on_board)
			return;

		if (selected) {
			LoopPlayers (Color.white);
			selected = false;
			return;
		}

		//If a player or card is already selected, return and don't allow selection
		foreach (Transform child in transform.parent.parent) {
			if (child.name == "Hand") {
				foreach (Transform granchild in child) {
					if (granchild.GetComponent<SelectCard>().selected)
						return;
				}
			}

			if (child.name == "Players") {
				foreach (Transform grandchild in child) {
					if (grandchild.GetComponent<SelectPlayer> ().selected)
						return;
				}
			}
		}

		LoopPlayers (Color.cyan);
		selected = true;
	}

	public void Move(int x, int y){
		transform.position = PositionCoordinates.CoordiatesToPosition (x, y);
		transform.position = new Vector3(transform.position.x, transform.position.y, -2.0f);
	}

	public void ResetPlayers(){
		LoopPlayers (Color.white);
		selected = false;
	}

	public void SendPlayCard(){
		Vector2 player = PositionCoordinates.PositionToCoordinates(transform.position);
		BoardSpace board_space = new BoardSpace();
		board_space.x_loc = Convert.ToInt32(player.x);
		board_space.y_loc = Convert.ToInt32(player.y);

		//Convert board_space to JSON
		string json = JsonUtility.ToJson(board_space);

		//Change HTTP Request to be able to receive a POST body
		string url = "/game/"+Config.instance.game_id+"/hand/" + id;
		//httpGet (url, username, password);
		Debug.Log(json);
		StartCoroutine(HttpRequest.SendRequest(url, HandleSendPlayCard, "POST", json, Config.instance.username, Config.instance.password));
	}

	public void HandleSendPlayCard(string response){
		Debug.Log (response);
	}

	private void LoopPlayers(Color color){
		foreach (Transform child in transform.parent.parent) {
			if (child.name == "Players") {
				SelectPlayers (child, color);
			}
		}
	}

	private void SelectPlayers(Transform players, Color color){
		foreach (Transform child in players) {
			child.GetComponent<SelectPlayer> ().SetMoveColor (color);
		}
	}
}
