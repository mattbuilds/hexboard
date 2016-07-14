
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Config : MonoBehaviour {
	public static Config instance;
	public string username;
	public string password;
	public int game_id, move_count, player_id;
	public bool turn;
	public Transform cardPrefab;
	public Transform cardMovementPrefab;
	public GameObject you_score;
	public GameObject them_score;
	public GameObject turn_label;
	public GameObject waiting; 
	public GameObject hand;
	public bool game_waiting;
	private Dictionary<string, Dictionary<int,int>> cardMovementDict;

	// Use this for initialization
	void Start () {
		username = PlayerPreferencs.username;
		password = PlayerPreferencs.password;
		player_id = PlayerPreferencs.player_id;
		game_id = PlayerPreferencs.game_id;
		turn = true;
		move_count = 0;
		game_waiting = true;
		cardMovementDict = new Dictionary<string, Dictionary<int,int>> ();
		GameState ();
		InvokeRepeating ("GameState", 2.0f, 10.0f);
	}

	//Call to get main game info
	void GameState(){
		string url = "/game/"+game_id;
		StartCoroutine(HttpRequest.SendRequest(url, HandleGameState, "GET", "body", username, password));
	}

	public void MoveSent(){
		move_count++;
		if (move_count > 1) {
			this.move_count = 0;
			this.turn = false;
			this.SwitchTurnLabel ();
		}
	}

	public void SwitchTurnLabel(){
		// Update turn label
		if (turn)
			turn_label.GetComponent<Text> ().text = "It is your turn.";
		else
			turn_label.GetComponent<Text> ().text = "Waiting for opponent";
	}

	void HandleGameState(string response){
		Debug.Log (response);
		CurrentGame game = JsonUtility.FromJson<CurrentGame> (response);

		//Check if game started yet
		if(game.status.Equals("starting") && game_waiting){
			return;
		}else{
			waiting.SetActive(false);
			game_waiting = false;
			hand.GetComponent<InitialHand>().GetInitialHand();
		}

		//Check turn
		if (game.turn.id == player_id)
			turn = true;
		else
			turn = false;

		// Update turn label
		SwitchTurnLabel();

		//Update opponents's location
		for (int i = 0; i < game.board_played.meeples.Length; i++) {
			GameObject meeple = GameObject.Find ("meeple_" + game.board_played.meeples [i].id);
			Debug.Log (game.board_played.meeples [i].finished);
			if (game.board_played.meeples [i].finished && meeple != null) {
				Destroy (meeple);
			} else if (game.board_played.meeples [i].finished && meeple == null) {
				continue;
			} else {
				meeple.GetComponent<SelectPlayer> ().Move (game.board_played.meeples [i].board_space.x_loc, game.board_played.meeples [i].board_space.y_loc);
			}
		}

		//Update cards on board
		for (int i = 0; i < game.board_played.cards.Length; i++) {
			GameObject card = GameObject.Find ("card_" + game.board_played.cards [i].id);
			if (game.board_played.cards [i].finished && card != null) {
				Destroy (card);
			} else if (game.board_played.cards [i].finished && card == null) {
				continue;
			} else if (card == null) {
				card = CreateCard(game.board_played.cards[i]);
			}
				
			if (game.board_played.cards [i].value == "P") {
				card.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("moving_" + game.board_played.cards [i].direction);
				SetCardColor (card, game.board_played.cards [i].color);
			}

			card.GetComponent<SelectCard> ().Move (game.board_played.cards [i].board_space.x_loc, game.board_played.cards [i].board_space.y_loc);
		}
			

		//Set the score for each player
		if (game.hosting.id == player_id) {
			you_score.GetComponent<Text> ().text = game.hosting.score.ToString();
			them_score.GetComponent<Text> ().text = game.joining.score.ToString();
		} else {
			you_score.GetComponent<Text> ().text = game.joining.score.ToString();
			them_score.GetComponent<Text> ().text = game.hosting.score.ToString();
		}
	}

	public void SetCardColor (GameObject card, int setColor){
		Color color;
		if (setColor == 0)
			color = new Color (155 / 255f, 89 / 255f, 182 / 255f, 1f);
		else if(setColor == 1)
			color = new Color (241 / 255f, 196 / 255f, 15 / 255f, 1f);
		else if(setColor == 2)
			color = new Color (46 / 255f, 204 / 255f, 113 / 255f, 1f);
		else if(setColor == 3)
			color = new Color (230 / 255f, 126 / 255f, 34 / 255f, 1f);
		else if(setColor == 4)
			color = new Color (52 / 255f, 152 / 255f, 219 / 255f, 1f);
		else
			color = new Color (231 / 255f, 76 / 255f, 60 / 255f, 1f);	
		
		card.GetComponent<SpriteRenderer> ().color = color;
	}

	public GameObject CreateCard(Card card){
		Vector2 postion = PositionCoordinates.CoordiatesToPosition (card.board_space.x_loc, card.board_space.y_loc);
		Transform clone;
		clone = (Transform)Instantiate (cardPrefab, new Vector3 (postion.x, postion.y, -5f), cardPrefab.rotation);
		if (card.value.Equals("P")){
			clone.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>("moving_" + card.direction);
		}else{
			clone.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>("card_" + card.value);
		}
		clone.GetComponent<SelectCard> ().id = card.id;
		clone.name = "card_" + card.id;

		return clone.gameObject;
	}

	void Awake(){
		instance = this;
	}
}