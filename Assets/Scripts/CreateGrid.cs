using UnityEngine;
using System;
using System.Collections;

public class CreateGrid : MonoBehaviour {
	public Transform tilePrefab;
	public Transform[,] map = new Transform[13,11];

	// Use this for initialization
	void Start () {
		for (int x = -6; x <= 6; x++){
			for (int y = -5; y <= 5; y++){
				if (Mathf.Abs(y-x) < 6){
					Transform clone;
					clone = (Transform)Instantiate(tilePrefab, PositionCoordinates.CoordiatesToPosition(x,y), tilePrefab.rotation);
					clone.parent = transform;
					map[x+6,y+5] = clone;
				}
			}
		}


	}

	// Update is called once per frame
	void Update () {
	
	}

	public GameObject GetGridFromMap(int x, int y){
		try{
			return map [x + 6, y + 5].gameObject;
		}catch(Exception e){
			return null;
		}
	}
}
