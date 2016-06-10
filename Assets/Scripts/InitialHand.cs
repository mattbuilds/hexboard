using UnityEngine;
using System.Collections;

public class InitialHand : MonoBehaviour {
	public Transform tilePrefab;
	private Hand cards;
	// Use this for initialization
	void Start () {
		GetInitialHand ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public void RemoveFromHand(int id){
		for (int i = 0; i< cards.hand.Count; i++){
			if (cards.hand [i].id == id) {
				cards.hand.RemoveAt (i);
			}
		}
	}

	public void RedrawCardsinHand(){
		for (int i = 0; i < cards.hand.Count; i++) {
			Transform clone;
			GameObject card_object;
			float y = 3f + (-1.2f * i);
			card_object = GameObject.Find ("card_" + cards.hand [i].id);
			if (card_object) {
				clone = card_object.transform;
				clone.position = new Vector3 (-7f, y, -5f);
			} else {
				clone = (Transform)Instantiate (tilePrefab, new Vector3 (-7f, y, -5f), tilePrefab.rotation);
				if ( cards.hand[i].value.Equals("P")){
					clone.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>("moving_" + cards.hand[i].direction);
					Config.instance.SetCardColor (clone.gameObject, cards.hand [i].color);
				}else{
					clone.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>("card_" + cards.hand [i].value);
				}
				clone.GetComponent<SelectCard> ().id = cards.hand [i].id;
				clone.name = "card_" + cards.hand [i].id;
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
		Config.instance.turn = false;
	} 

	public void HandleGetInitialHand(string response){
		cards = JsonUtility.FromJson<Hand> (response);

		RedrawCardsinHand ();
	}

	public void HandleDrawCard(string response){
		Card card = JsonUtility.FromJson<Card> (response);

		cards.hand.Add (card);
		RedrawCardsinHand ();
	}
}
