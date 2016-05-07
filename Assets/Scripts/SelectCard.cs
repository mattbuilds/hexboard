using UnityEngine;
using System.Collections;

public class SelectCard : MonoBehaviour {
	public bool on_board, selected;
	private MyTurnScript me;

	// Use this for initialization
	void Start () {
		on_board = false;
		selected = false;
		me = transform.parent.GetComponent<MyTurnScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		if (!me.my_turn)
			return;
		
		if (on_board)
			return;

		if (selected) {
			LoopPlayers (Color.white);
			selected = false;
			return;
		}

		//If a player is already selected, return and don't allow selection
		foreach (Transform child in transform.parent) {
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

	public void ResetPlayers(){
		LoopPlayers (Color.white);
		selected = false;
	}

	private void LoopPlayers(Color color){
		foreach (Transform child in transform.parent) {
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
