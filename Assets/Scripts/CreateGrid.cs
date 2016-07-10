using UnityEngine;
using System;
using System.Collections;

public class CreateGrid : MonoBehaviour {
	public Transform tilePrefab;
	private int x_set = 5;
	private int y_set = 4;
	public Transform[,] map; 

	// Use this for initialization
	void Start () {
		map = new Transform[2*x_set+1,2*y_set+1];
		for (int x = -x_set; x <= x_set; x++){
			for (int y = -y_set; y <= y_set; y++){
				if (Mathf.Abs(y-x) < x_set){
					Transform clone;
					clone = (Transform)Instantiate(tilePrefab, PositionCoordinates.CoordiatesToPosition(x,y), tilePrefab.rotation);
					clone.parent = transform;
					map[x+x_set,y+y_set] = clone;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public GameObject GetGridFromMap(int x, int y){
		try{
			return map [x + x_set, y + y_set].gameObject;
		}catch(Exception e){
			return null;
		}
	}
}
