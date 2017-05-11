using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchInput : MonoBehaviour {

	public Text vecText;
	private Color orColor = new Color (1.0f, 0f, 0f, 1.0f);
	private Color posColor = new Color(0f, 1.0f, 0f, 1.0f);
	private Material orColored;
	private Material posColored;

	void Start(){
		vecText = GameObject.Find ("Vec").GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update ()
	{
		// Attach this script to a trackable object
		// Create a plane that matches the target plane
		Plane targetPlane = new Plane(transform.up, transform.position);

		// Creates a cylinder with 0 height and places it in origo to mark out (0,0,0)
		GameObject origo = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
		orColored = new Material(Shader.Find("Diffuse"));
		orColored.color = orColor;
		origo.GetComponent<Renderer>().material = orColored;
		origo.transform.parent = transform;
		origo.transform.localScale = new Vector3(0.1f, 0.001f, 0.1f);
		origo.transform.position = new Vector3(0, 0, 0);

		// When user touch the screen
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				//Creates ray and send to the target plane where the user touch the screen
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				float dist = 0.0f;
				targetPlane.Raycast(ray, out dist);
				Vector3 planePoint = ray.GetPoint(dist);

				// Creates and gameobject (cylinder) and makes it green, used to mark out the user touch position
				GameObject pos = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				posColored = new Material(Shader.Find("Diffuse"));
				posColored.color = posColor;
				pos.GetComponent<Renderer>().material = posColored;
				pos.transform.parent = transform;
				pos.transform.localScale = new Vector3(0.1f, 0.001f, 0.1f);
				pos.transform.position = planePoint;

				// Just to write out the coords of the touch input on the target plane
				float vX = planePoint.x;
				float vZ = planePoint.z;
				vecText.text = "X: " + vX.ToString() + " Z: " + vZ.ToString();

				GAMEMANAGER.GM.AddToSeq (vX, vZ);

				Destroy (pos, 3.0f);
			}
		}
	}
}
