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
	public bool selected;
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
		if (selected) {
			ResetColors ();
		} else {
			SetMoveColor (Color.green);
			selected = true;
		}
	}

	public void ResetColors(){
		selected = false;
		SetMoveColor (Color.white);
	}

	private void SetMoveColor(Color color){
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
