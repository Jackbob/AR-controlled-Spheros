using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GAMEMANAGER: MonoBehaviour
{

	public static GAMEMANAGER GM;

	public bool Socket;
	public bool Tracking;
	public bool IsChosen;
	public int WhichSpehro;

	public bool onTargetpos;

	public float ShowChosenX;
	public float ShowChosenZ;

	public float CountDownHeight;

	private TcpClient client;
	private NetworkStream stream;

	private string Seq = "";

	void Awake ()
	{
		if (GM != null && GM != this) {
			DestroyImmediate (gameObject);
			return;
		}
		GM = this;
		DontDestroyOnLoad (gameObject);
	}

	public void TEST(){
		CountDownHeight = 1.0f;
	}

	void Start ()
	{
		InvokeRepeating("SocketConnected", 2.0f, 2.0f);
		Socket = false;
		Tracking = false;
		IsChosen = false;
		onTargetpos = false;
	}

	public void AddToSeq(float x, float z){

		if(x > -0.7 && x < 0.7 && z > -1 && z < 1){
			Seq += x.ToString() + " " + z.ToString() + " ";
			CancelInvoke ("SendSeq");
			Invoke ("SendSeq", 2);
			CountDownHeight = 1.0f;
		}
	}

	public void SendSeq(){
		SendString(Seq);
		Seq = "";
		IsChosen = false;

	}

	public void SendString (string s)
	{
		Byte[] data = System.Text.Encoding.ASCII.GetBytes (s);

		try {
			stream.Write (data, 0, data.Length);
		} catch (Exception e) {
			Debug.Log ("# " + s);
			Debug.Log ("Socket error: " + e.Message);
		}
	}

	public void Connect (string host)
	{
		if (Socket)
			return;

		int port = 6321;

		try {
			client = new TcpClient (host, port);
			stream = client.GetStream ();
			Socket = true;
		} catch (Exception e) {
			Debug.Log ("Socket error: " + e.Message);
		}
	}

	public void Disconnect ()
	{
		try {
			stream.Close ();
			client.Close ();
			Socket = false;
		} catch (Exception e) {
			Debug.Log ("DC error: " + e.Message);
		}
	}

	public string Receive ()
	{
		if (stream.DataAvailable) {

			try {
				Byte[] data = new Byte[256];
				String responseData = String.Empty;
				Int32 bytes = stream.Read (data, 0, data.Length);
				responseData = System.Text.Encoding.ASCII.GetString (data, 0, bytes);
				return responseData;
			} catch (Exception e) {
				Debug.Log ("Receive error: " + e.Message);
				return "";
			}
		}

		return "";
	}

	public void SocketConnected ()
	{
		if (Socket) {
			bool part1 = client.Client.Poll (1000, SelectMode.SelectRead);
			bool part2 = (client.Client.Available == 0);
			if (part1 && part2) {
				Socket = false;
			}
		}
	}

	public void SceneLoader (string s)
	{
		SceneManager.LoadScene (s);
	}
}
