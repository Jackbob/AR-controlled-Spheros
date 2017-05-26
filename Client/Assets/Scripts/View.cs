using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

	public static View Vw;

	private Text TrackText;
	public Image redFeedback;
	public Image blueFeedback;

	void Start () {
		Debug.Log("View");

		GameObject.Find ("ToLobbyBtn").GetComponent<Button> ().onClick.AddListener(ToLobby);
		TrackText = GameObject.Find ("OnTrack").GetComponent<Text> ();

		redFeedback = GameObject.Find("Red").GetComponent<Image>();
		blueFeedback = GameObject.Find("Blue").GetComponent<Image>();

		redFeedback.enabled = false;
		blueFeedback.enabled = false;

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


		if (cmd == 0) {

			int sph = int.Parse (V [1]);
			GAMEMANAGER.GM.SpheroX = float.Parse (V [3])/100;
			GAMEMANAGER.GM.SpheroZ = -float.Parse (V [2])/100;

			GAMEMANAGER.GM.chosen = true;
			GAMEMANAGER.GM.ShowChosen = true;
			if (sph == 1) {
				redFeedback.enabled = true;
				blueFeedback.enabled = false;
			} else if (sph == 2) {
				blueFeedback.enabled = true;
				redFeedback.enabled = false;
			}
		} else if (cmd == 1) {
			GAMEMANAGER.GM.onTargetpos = true;
		} else if (cmd == 2) {
			blueFeedback.enabled = false;
			redFeedback.enabled = false;
			GAMEMANAGER.GM.ShowChosen = false;
		}
	}

}