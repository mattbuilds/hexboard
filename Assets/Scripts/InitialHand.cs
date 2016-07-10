using UnityEngine;
using System.Collections;

public class InitialHand : MonoBehaviour {
	public Transform tilePrefab;
	private Cards hand;
	private int row_length =7;
	// Use this for initialization
	void Start () {
		GetInitialHand ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public void RemoveFromHand(int id){
		for (int i = 0; i< hand.cards.Count; i++){
			if (hand.cards [i].id == id) {
				hand.cards.RemoveAt (i);
			}
		}
	}

	public void RedrawCardsinHand(){
		for (int i = 0; i < hand.cards.Count; i++) {
			Transform clone;
			GameObject card_object;
			int x_section = i / row_length;
			float y = 0;
			float x = 0;
			y = 4f + (-1.2f * (i-x_section*row_length));
			x = -9f + (x_section);
			card_object = GameObject.Find ("card_" + hand.cards [i].id);
			if (card_object) {
				clone = card_object.transform;
				clone.position = new Vector3 (x, y, -5f);
			} else {
				clone = (Transform)Instantiate (tilePrefab, new Vector3 (x, y, -5f), tilePrefab.rotation);
				if ( hand.cards[i].value.Equals("P")){
					clone.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>("moving_" + hand.cards[i].direction);
					Config.instance.SetCardColor (clone.gameObject, hand.cards [i].color);
				}else{
					clone.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>("card_" + hand.cards [i].value);
				}
				clone.GetComponent<SelectCard> ().id = hand.cards [i].id;
				clone.name = "card_" + hand.cards [i].id;
				clone.parent = transform;
			}
		}
	}

	public void GetInitialHand(){
		string url = "/game/" + Config.instance.game_id+"/hand";
		StartCoroutine(HttpRequest.SendRequest(url, HandleGetInitialHand, "GET", "body", Config.instance.username, Config.instance.password));
	}

	public void DrawCard(){
		if (!Config.instance.turn)
			return;

		string url = "/game/" + Config.instance.game_id + "/draw";
		StartCoroutine(HttpRequest.SendRequest(url, HandleDrawCard, "GET", "body", Config.instance.username, Config.instance.password));
	} 

	public void HandleGetInitialHand(string response){
		Debug.Log (response);
		hand = JsonUtility.FromJson<Cards> (response);

		RedrawCardsinHand ();
	}

	public void HandleDrawCard(string response){
		Card card = JsonUtility.FromJson<Card> (response);

		hand.cards.Add (card);
		Config.instance.turn = false;
		Config.instance.SwitchTurnLabel ();
		RedrawCardsinHand ();
	}
}
