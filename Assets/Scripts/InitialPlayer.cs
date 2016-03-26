using UnityEngine;
using System.Collections;

public class InitialPlayer : MonoBehaviour {
	public Transform tilePrefab;
	public static float x_off = .8f;
	public static float y_off = 1.05f;
	// Use this for initialization
	void Start () {
		CreatePlayer (-3, 2);
		CreatePlayer(-6,-3);
		CreatePlayer(-3,-5);
		CreatePlayer (-1, -1);
		CreatePlayer (-6, -5);
		CreatePlayer (5, 4);
	}

	void CreatePlayer(int x, int y){
		Transform clone;
		clone = (Transform)Instantiate(tilePrefab, new Vector2(x*x_off, (y*y_off) - (x*.5f*y_off) ), tilePrefab.rotation);
		clone.parent = transform;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
