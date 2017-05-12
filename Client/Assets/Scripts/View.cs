using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

	private InputField input;
	private float prev = 0;

	void Start () {
		Debug.Log("View");

		input = GameObject.Find ("InputField").GetComponent<InputField> ();

		GameObject.Find ("ToLobbyBtn").GetComponent<Button> ().onClick.AddListener(ToLobby);
		GameObject.Find ("SendStringBtn").GetComponent<Button> ().onClick.AddListener(SendStringInput);
		GameObject.Find ("SendCoordBtn").GetComponent<Button> ().onClick.AddListener(SendCoordInput);

		DoneWithSeq ();
	}

	void Update(){
		if (GAMEMANAGER.GM.GetSocketReady ()) {

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

	public void SendCoordInput(){
		GAMEMANAGER.GM.SendSeq();
	}

	public void DoneWithSeq(){

		float cur = GAMEMANAGER.GM.GetSeqLength ();

		if(prev != 0 && cur == prev){
			SendCoordInput ();
			prev = 0;
		} else {
			prev = cur;
		}
		Invoke ("DoneWithSeq", 2);
	}
}