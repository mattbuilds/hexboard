using UnityEngine;
using System.Collections;

public class SelectHex : MonoBehaviour {
	public GameObject move_player;
	private GameObject me;

	// Use this for initialization
	void Start () {
		me = GameObject.Find("Me");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		SpriteRenderer sr = transform.gameObject.GetComponent<SpriteRenderer> ();

		//For moving a player
		if (sr.color == Color.green){
			
			move_player.GetComponent<SelectPlayer> ().ResetColors ();
			move_player.transform.position = transform.position;

			//Send Move to Server
			move_player.GetComponent<SelectPlayer>().SendMove();

			//End Turn
			me.GetComponent<MyTurnScript> ().my_turn = false;
		}

		//For playing a card
		if (sr.color == Color.cyan) {
			//Place card at space
			foreach (Transform child in me.transform) {
				if (child.name == "card") {
					child.GetComponent<SelectCard> ().ResetPlayers ();
					child.transform.position = new Vector3(transform.position.x, transform.position.y, child.transform.position.z);
					child.GetComponent<SelectCard> ().on_board = true;
				}
			}

			//End Turn
			me.GetComponent<MyTurnScript> ().my_turn = false;
		}
	}
}
