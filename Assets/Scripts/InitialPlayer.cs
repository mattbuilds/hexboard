using UnityEngine;
using System.Collections;

[System.Serializable]
public class BoardSpace{
	public int x_loc;
	public int y_loc;
}

[System.Serializable]
public class Meeple{
	public int id;
	public BoardSpace board_space;
}

[System.Serializable]
public class Meeples{
	public Meeple[] meeples;
}

public class InitialPlayer : MonoBehaviour {
	public Transform tilePrefab;
	// Use this for initialization
	void Start () {
		GetInitialPositions();
	}

	void CreatePlayer(int id, int x, int y){
		Transform clone;
		clone = (Transform)Instantiate(tilePrefab, PositionCoordinates.CoordiatesToPosition(x,y), tilePrefab.rotation);
		clone.GetComponent<SelectPlayer>().meeple_id = id;
		clone.position = new Vector3 (clone.position.x, clone.position.y, -1);
		clone.parent = transform;
	}

	public void GetInitialPositions(){
		string url = "/game/1/meeples";
		string username = "matt";
		string password = "test";
		//httpGet (url, username, password);
		StartCoroutine(HttpRequest.SendRequest(url, HandleGetInitialPosition, "GET", "body", username, password));
	}

	public void HandleGetInitialPosition(string response){
		Meeples collection = JsonUtility.FromJson<Meeples> (response);

		for(int i=0; i < collection.meeples.Length; i++){
			CreatePlayer(
				collection.meeples[i].id,
				collection.meeples[i].board_space.x_loc,
				collection.meeples[i].board_space.y_loc
			);
		}
	}
}
