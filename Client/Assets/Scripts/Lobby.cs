using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

	private Text info;
	private Text infoFromServer;

	public GameObject panel;

	void Start () {
		Debug.Log("Lobby");

		infoFromServer = GameObject.Find ("InfoTxt").GetComponent<Text> ();

		GameObject.Find ("DisconnectBtn").GetComponent<Button> ().onClick.AddListener(Disconnect);
		GameObject.Find ("FreerideBtn").GetComponent<Button> ().onClick.AddListener(ToFreeride);

		GameObject.Find ("HowToBtn").GetComponent<Button> ().onClick.AddListener(HowTo);
		GameObject.Find ("ClosePanelBtn").GetComponent<Button> ().onClick.AddListener(ClosePanel);

		panel.SetActive (false);
		infoFromServer.text = "";
	}
		
	void HowTo(){
		panel.SetActive (true);
	}

	void ClosePanel(){
		panel.SetActive (false);
	}

	void Update () {
		if (GAMEMANAGER.GM.socket) {
			
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
}
