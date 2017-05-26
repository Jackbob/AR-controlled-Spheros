using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

	public static View Vw;

	private Text TrackText;
	private Image RedFeedback;
	private Image BlueFeedback;

	void Start () {
		Debug.Log("View");

		GameObject.Find ("ToLobbyBtn").GetComponent<Button> ().onClick.AddListener(ToLobby);
		TrackText = GameObject.Find ("NoTrackingText").GetComponent<Text> ();

		RedFeedback = GameObject.Find("Red").GetComponent<Image>();
		BlueFeedback = GameObject.Find("Blue").GetComponent<Image>();

		RedFeedback.enabled = false;
		BlueFeedback.enabled = false;
	}

	void Update(){
		if (GAMEMANAGER.GM.Socket) {

			string inp = GAMEMANAGER.GM.Receive ();

			if (inp != "") {
				inputData (inp);
			}
				
			if (GAMEMANAGER.GM.Tracking) {
				TrackText.text = "";
			} else {
				TrackText.text = "NO TRACKING";
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
			GAMEMANAGER.GM.ShowChosenX = float.Parse (V [3])/100;
			GAMEMANAGER.GM.ShowChosenZ = -float.Parse (V [2])/100;
			GAMEMANAGER.GM.IsChosen = true;
			if (sph == 1) {
				RedFeedback.enabled = true;
				BlueFeedback.enabled = false;
				GAMEMANAGER.GM.WhichSpehro = 1;
			} else if (sph == 2) {
				BlueFeedback.enabled = true;
				RedFeedback.enabled = false;
				GAMEMANAGER.GM.WhichSpehro = 2;
			}
		} else if (cmd == 1) {
			GAMEMANAGER.GM.onTargetpos = true;
		} else if (cmd == 2) {
			BlueFeedback.enabled = false;
			RedFeedback.enabled = false;
			GAMEMANAGER.GM.IsChosen = false;
		}
	}
}