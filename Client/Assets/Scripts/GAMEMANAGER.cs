using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GAMEMANAGER: MonoBehaviour
{

	public static GAMEMANAGER GM;

	private bool socket;
	private bool tracking;
	private bool sphero;

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

	void Start ()
	{
		InvokeRepeating("SocketConnected", 2.0f, 2.0f);
		socket = false;
		tracking = false;
		sphero = false;
	}

	public void Switch(){
		SendSeq ();
		sphero = !sphero;
	}

	public void AddToSeq(float x, float z){

		if(x > -0.7 && x < 0.7 && z > -1 && z < 1){
			Seq += x.ToString() + " " + z.ToString() + " ";
			CancelInvoke ("SendSeq");
			Invoke ("SendSeq", 2);
		}
	}

	public void SendSeq(){

		if(!sphero){
			Seq = "0 " + Seq;
		} else {
			Seq = "1 " + Seq;
		}

		SendString(Seq);
		Seq = "";
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
		
		if (socket)
			return;

		//Default IP/port
		int port = 6321;

		//Create socket
		try {
			client = new TcpClient (host, port);
			stream = client.GetStream ();
			socket = true;
		} catch (Exception e) {
			Debug.Log ("Socket error: " + e.Message);
		}
	}

	public void Disconnect ()
	{
		try {
			stream.Close ();
			client.Close ();
			socket = false;
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
		if (socket) {
			bool part1 = client.Client.Poll (1000, SelectMode.SelectRead);
			bool part2 = (client.Client.Available == 0);
			if (part1 && part2) {
				socket = false;
			}
		}
	}

	public void SceneLoader (string s)
	{
		SceneManager.LoadScene (s);
	}

	public bool GetSocket ()
	{
		if (socket) {
			return true;
		} else {
			return false;
		}
	}

	public bool GetTracking ()
	{
		if (tracking) {
			return true;
		} else {
			return false;
		}
	}

	public void SetTracking (bool c)
	{
		if (c) {
			tracking = true;
		} else {
			tracking = false;
		}
	}
}
