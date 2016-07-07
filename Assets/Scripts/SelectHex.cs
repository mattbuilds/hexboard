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

			//If fine, move shit
			move_player.GetComponent<SelectPlayer> ().ResetColors ();
			move_player.transform.position = transform.position;

			//Send Move to Server
			move_player.GetComponent<SelectPlayer>().SendMove();

			//End Turn
			Config.instance.turn = false;
			Config.instance.SwitchTurnLabel();
		}

		//For playing a card
		if (sr.color == Color.cyan) {
			//Place card at space
			foreach (Transform child in me.transform) {
				if (child.name == "Hand") {
					foreach (Transform grandchild in child) {
						if (grandchild.GetComponent<SelectCard> ().selected) {
							grandchild.GetComponent<SelectCard> ().ResetPlayers ();
							grandchild.transform.position = new Vector3 (transform.position.x, transform.position.y, child.transform.position.z);
							grandchild.GetComponent<SelectCard> ().on_board = true;
							grandchild.GetComponent<SelectCard> ().SendPlayCard ();
							grandchild.transform.parent = null;
							child.GetComponent<InitialHand> ().RemoveFromHand (grandchild.GetComponent<SelectCard> ().id);
							child.GetComponent<InitialHand> ().RedrawCardsinHand ();
						}
					}
				}
			}

			//End Turn
			Config.instance.turn = false;
			Config.instance.SwitchTurnLabel();
		}
	}
}
