using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

	private InputField input;
	private Text TrackText;


	void Start () {
		Debug.Log("View");

		input = GameObject.Find ("InputField").GetComponent<InputField> ();

		GameObject.Find ("ToLobbyBtn").GetComponent<Button> ().onClick.AddListener(ToLobby);
		GameObject.Find ("SendStringBtn").GetComponent<Button> ().onClick.AddListener(SendStringInput);
		TrackText = GameObject.Find ("OnTrack").GetComponent<Text> ();
		GameObject.Find ("SwitchBtn").GetComponent<Button> ().onClick.AddListener(Switch);
	}

	void Update(){
		if (GAMEMANAGER.GM.GetSocket ()) {
			
			if (GAMEMANAGER.GM.GetTracking ()) {
				TrackText.text = "TRACKING FOUND";
			} else {
				TrackText.text = "TRACKING LOST";
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

	void SendStringInput(){

		string msg = input.text;
		GAMEMANAGER.GM.SendString (msg);
	}

	void Switch(){

		GAMEMANAGER.GM.Switch ();
	}
}