using UnityEngine;
using System.Collections;

public class SelectHex : MonoBehaviour {
	public GameObject move_player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		SpriteRenderer sr = transform.gameObject.GetComponent<SpriteRenderer> ();
		if (sr.color == Color.green){
			move_player.GetComponent<SelectPlayer> ().ResetColors ();
			move_player.transform.position = transform.position;
		}


	}
}
