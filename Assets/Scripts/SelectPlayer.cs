using UnityEngine;
using System;
using System.Collections;

public static class  PositionCoordinates{
	public static float x_off = .8f;
	public static float y_off = 1.05f;

	public static Vector2 CoordiatesToPosition (int x, int y){
		return new Vector2 (x * x_off, (y * y_off) - (x * .5f * y_off));
	}

	public static Vector2 PositionToCoordinates(Vector3 position){
		float x_result = (position.x / x_off);
		float y_result = ((position.y / y_off) + .5f * position.x / x_off);
		return new Vector2 (Convert.ToInt32(x_result), Convert.ToInt32(y_result));
	}
}

public class SelectPlayer : MonoBehaviour {
	public GameObject board;
	public CreateGrid createGridScript;
	public bool selected, my_player;
	public int meeple_id;

	// Use this for initialization
	void Start () {
		board = GameObject.Find ("Board");
		createGridScript = board.GetComponent<CreateGrid> ();
		selected = false;
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		//This is the other players, stop trying to do shit with it
		if (!my_player)
			return;

		//If not your turn do nothing
		if (!Config.instance.turn)
			return; 
		
		//If already selected, unselect
		if (selected) {
			ResetColors ();
			return;
		} 	

		//If a player is already selected, return and don't allow selection
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
			
		//Selects the player
		SetMoveColor (Color.green);
		selected = true;
	}

	public void ResetColors(){
		selected = false;
		SetMoveColor (Color.white);
	}

	public void Move(int x, int y){
		transform.position = PositionCoordinates.CoordiatesToPosition (x, y);
		transform.position = new Vector3(transform.position.x, transform.position.y, -2.0f);
	}

	public void SendMove(){
		Vector2 player = PositionCoordinates.PositionToCoordinates(transform.position);
		BoardSpace board_space = new BoardSpace();
		board_space.x_loc = Convert.ToInt32(player.x);
		board_space.y_loc = Convert.ToInt32(player.y);

		//Convert board_space to JSON
		string json = JsonUtility.ToJson(board_space);

		//Change HTTP Request to be able to receive a POST body
		string url = "/game/"+Config.instance.game_id+"/move/" + meeple_id;
		//httpGet (url, username, password);
		Debug.Log(json);
		StartCoroutine(HttpRequest.SendRequest(url, HandleMoveMeep, "POST", json, Config.instance.username, Config.instance.password));
	}

	public void HandleMoveMeep(string response){
		Debug.Log (response);
	}

	public void SetMoveColor(Color color){
		Vector2 player = PositionCoordinates.PositionToCoordinates(transform.position);
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if ((x == 0 && y == 0) || (Mathf.Abs(y-x)==2)) {
					//Do nothing
				}else{
					try
					{
						Vector2 space = PositionCoordinates.CoordiatesToPosition(Convert.ToInt32(player.x + x), Convert.ToInt32(player.y + y));
						Collider2D[] hitCollider = Physics2D.OverlapPointAll(space);
						if (hitCollider.Length > 1)
							continue;
						
						GameObject spot = createGridScript.GetGridFromMap (Convert.ToInt32(player.x + x), Convert.ToInt32(player.y + y));
						spot.gameObject.GetComponent<SelectHex>().move_player = transform.gameObject;
						SpriteRenderer sr = spot.GetComponent<SpriteRenderer> ();
						sr.color = color;
					} 
					catch(Exception ex){
						//
					}
				}
			}
		}
	}
}
