using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

	private Text InfoFromServer;
	public GameObject Panel;

	void Start () {
		Debug.Log("Lobby");

		InfoFromServer = GameObject.Find ("InfoFromServerText").GetComponent<Text> ();

		GameObject.Find ("PlayBtn").GetComponent<Button> ().onClick.AddListener(Play);
		GameObject.Find ("HowToPlayBtn").GetComponent<Button> ().onClick.AddListener(HowToPlay);
		GameObject.Find ("DisconnectBtn").GetComponent<Button> ().onClick.AddListener(Disconnect);
		GameObject.Find ("ClosePanelBtn").GetComponent<Button> ().onClick.AddListener(ClosePanel);

		Panel.SetActive (false);
		InfoFromServer.text = "";
	}
		
	void HowToPlay(){
		Panel.SetActive (true);
	}

	void ClosePanel(){
		Panel.SetActive (false);
	}

	void Update () {
		if (GAMEMANAGER.GM.Socket) {
			string fromServer = GAMEMANAGER.GM.Receive ();
			if (fromServer != "") {
				InfoFromServer.text = fromServer;
			}
		} else {
			Disconnect ();
		}
	}

	void Disconnect(){
		GAMEMANAGER.GM.Disconnect ();
		GAMEMANAGER.GM.SceneLoader ("Menu");
	}

	void Play(){
		GAMEMANAGER.GM.SceneLoader ("View");
	}
}
