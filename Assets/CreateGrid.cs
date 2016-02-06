using UnityEngine;
using System.Collections;

public class CreateGrid : MonoBehaviour {
	public Transform tilePrefab;
	private float x_off = .8f;
	private float y_off = 1.05f;
	// Use this for initialization
	void Start () {
		for (int x = -6; x <= 6; x++){
			for (int y = -5; y <= 5; y++){
				if (Mathf.Abs(y-x) < 6){ 
					Transform clone;
					clone = (Transform)Instantiate(tilePrefab, new Vector2(x*x_off, (y*y_off) - (x*.5f*y_off) ), tilePrefab.rotation);
					clone.parent = transform;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
