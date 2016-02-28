using UnityEngine;
using System.Collections;

public class InitialPlayer : MonoBehaviour {
	public Transform tilePrefab;
	public static float x_off = .8f;
	public static float y_off = 1.05f;
	// Use this for initialization
	void Start () {
		int y = 1;	
		for (int x=0; x < 6; x++){

			Transform clone;
			clone = (Transform)Instantiate(tilePrefab, new Vector2(x*x_off, (y*y_off) - (x*.5f*y_off) ), tilePrefab.rotation);
			clone.parent = transform;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
