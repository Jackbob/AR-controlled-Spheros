using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private Text info;
	private InputField input;

	void Start() {
		Debug.Log("Menu");

		info = GameObject.Find ("IsConnectedTxt").GetComponent<Text> ();
		input = GameObject.Find ("InputField").GetComponent<InputField> ();

		GameObject.Find ("ConnectBtn").GetComponent<Button> ().onClick.AddListener(ToLobby);

		info.text = "Not connected";
	}

	void ToLobby(){

		string ip = input.text;

		GAMEMANAGER.GM.Connect(ip);

		if (GAMEMANAGER.GM.Socket) {
			GAMEMANAGER.GM.SceneLoader ("Lobby");
		} else {
			info.text = "Could not connect";
		}
	}
}
