using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

	private Text info;
	private Text infoFromServer;

	void Start () {
		Debug.Log("Lobby");

		infoFromServer = GameObject.Find ("InfoTxt").GetComponent<Text> ();

		GameObject.Find ("DisconnectBtn").GetComponent<Button> ().onClick.AddListener(Disconnect);
		GameObject.Find ("FreerideBtn").GetComponent<Button> ().onClick.AddListener(ToFreeride);
		GameObject.Find ("ReadyBtn").GetComponent<Button> ().onClick.AddListener(Ready);

		infoFromServer.text = "No msg from server yet";
	}

	void Update () {
		if (GAMEMANAGER.GM.GetSocket ()) {
			
			string fromServer = GAMEMANAGER.GM.Receive ();

			if (fromServer != "") {
				infoFromServer.text = fromServer;
			}

		} else {
			Disconnect ();
		}
	}

	void Disconnect(){

		GAMEMANAGER.GM.Disconnect ();
		GAMEMANAGER.GM.SceneLoader ("Menu");
	}

	void ToFreeride(){

		GAMEMANAGER.GM.SceneLoader ("View");
	}

	void Ready(){

		Debug.Log("# Ready");
	}
}
