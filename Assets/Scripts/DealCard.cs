using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class DealCard : MonoBehaviour {

	// Use this for initialization
	IEnumerator  Start () {
		Player player = new Player ();
		player.id = 5;

		string input = JsonUtility.ToJson(player);
		Dictionary<string, string> headers = new Dictionary<string,string>();;
		headers.Add("Content-Type", "application/json");
		byte[] body = Encoding.UTF8.GetBytes (input);
		WWW www = new WWW("http://127.0.0.1:5000/", body, headers); 
		yield return www;

		Debug.Log (player.id);
		//Debug.Log(www.text);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
