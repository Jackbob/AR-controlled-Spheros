using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

	private Text TrackText;
	private Image redFeedback;
	private Image blueFeedback;

	void Start () {
		Debug.Log("View");

		GameObject.Find ("ToLobbyBtn").GetComponent<Button> ().onClick.AddListener(ToLobby);
		TrackText = GameObject.Find ("OnTrack").GetComponent<Text> ();

		redFeedback = GameObject.Find("Red").GetComponent<Image>();
		blueFeedback = GameObject.Find("Blue").GetComponent<Image>();

		redFeedback.enabled = false;
		blueFeedback.enabled = false;


		InvokeRepeating("sendEmpty", 0.0f, 0.1f);
	}

	void Update(){
		if (GAMEMANAGER.GM.socket) {

			string inp = GAMEMANAGER.GM.Receive ();

			if (inp != "") {
				inputData (inp);
			}
				
			if (GAMEMANAGER.GM.tracking) {
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
		int sph = int.Parse (V [1]);

		if (cmd == 0) {
			GAMEMANAGER.GM.chosen = true;

			if (sph == 1) {
				redFeedback.enabled = true;
				blueFeedback.enabled = false;
			} else if (sph == 2) {
				blueFeedback.enabled = true;
				redFeedback.enabled = false;
			} else {
				redFeedback.enabled = false;
				blueFeedback.enabled = false;
			}

		} else if (cmd == 1) {
			GAMEMANAGER.GM.onTargetpos = true;
		}
	}

	void sendEmpty(){
		GAMEMANAGER.GM.SendString ("");
	}
}