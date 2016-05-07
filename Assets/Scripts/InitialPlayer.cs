using UnityEngine;
using System.Collections;

public class InitialPlayer : MonoBehaviour {
	public Transform tilePrefab;
	// Use this for initialization
	void Start () {
		CreatePlayer (-4, -1);
		CreatePlayer(-4,-3);
	}

	void CreatePlayer(int x, int y){
		Transform clone;
		clone = (Transform)Instantiate(tilePrefab, PositionCoordinates.CoordiatesToPosition(x,y), tilePrefab.rotation);
		clone.position = new Vector3 (clone.position.x, clone.position.y, -1);
		clone.parent = transform;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
