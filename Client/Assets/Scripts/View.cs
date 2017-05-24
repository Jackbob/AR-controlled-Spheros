using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

	private Text TrackText;

	void Start () {
		Debug.Log("View");

		GameObject.Find ("ToLobbyBtn").GetComponent<Button> ().onClick.AddListener(ToLobby);
		TrackText = GameObject.Find ("OnTrack").GetComponent<Text> ();
	}

	void Update(){
		if (GAMEMANAGER.GM.GetSocket ()) {

			string inp = GAMEMANAGER.GM.Receive ();

			if (inp != "") {
				inputData (inp);
			}
				
			if (GAMEMANAGER.GM.GetTracking ()) {
				TrackText.text = "";
			} else {
				TrackText.text = "NO TRACKING FOUND";
			}
		} else {
			Disconnect ();
		}
	}

	void Disconnect(){

		GAMEMANAGER.GM.Disconnect ();
		GAMEMANAGER.GM.SceneLoader ("Menu");
	}

	void ToLobby(){

		GAMEMANAGER.GM.SceneLoader ("Lobby");
	}

	void inputData(string inp){
		
		string[] V;
		V = inp.Split(null);

		int cmd = int.Parse (V [0]);

		if (cmd == 0) {
			GAMEMANAGER.GM.chosen = true;
		} else if (cmd == 1) {
			GAMEMANAGER.GM.onTargetpos = true;
		}

	}
}