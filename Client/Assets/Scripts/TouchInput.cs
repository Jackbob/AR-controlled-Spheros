using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchInput : MonoBehaviour
{
	public static TouchInput TI;
	public Color posColor = new Color (0f, 1.0f, 0f, 1.0f);
	public Material posColored;

	public float vX;
	public float vZ;

	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		// Attach this script to a trackable object
		// Create a plane that matches the target plane
		Plane targetPlane = new Plane (transform.up, transform.position);

		// When user touch the screen
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				//Creates ray and send to the target plane where the user touch the screen
				Ray ray = Camera.main.ScreenPointToRay (touch.position);
				float dist = 0.0f;
				targetPlane.Raycast (ray, out dist);
				Vector3 planePoint = ray.GetPoint (dist);

				// Creates and gameobject (cylinder) and makes it green, used to mark out the user touch position
				GameObject pos = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
				posColored = new Material (Shader.Find ("Diffuse"));
				posColored.color = posColor;
				pos.GetComponent<Renderer> ().material = posColored;
				pos.transform.parent = transform;
				pos.transform.localScale = new Vector3 (0.1f, 0.001f, 0.1f);
				pos.transform.position = planePoint;

				// Just to write out the coords of the touch input on the target plane
				vX = planePoint.x;
				vZ = planePoint.z;

				if (GAMEMANAGER.GM.chosen) {
					GAMEMANAGER.GM.AddToSeq (vX, vZ);
				} else {
					string s = vX.ToString() + " " + vZ.ToString();
					GAMEMANAGER.GM.SendString (s);
				}

				//if(GAMEMANAGER.GM.onTargetpos){
					Destroy (pos, 3.0f);
					//GAMEMANAGER.GM.onTargetpos = false;
				//}
			}
		}

		if(GAMEMANAGER.GM.ShowChosen){
			// Which Sphero is used at the moment
			GameObject onSpheroChosen = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			posColored = new Material (Shader.Find ("Diffuse"));
			posColored.color = posColor;
			onSpheroChosen.GetComponent<Renderer> ().material = posColored;
			onSpheroChosen.transform.parent = transform;
			onSpheroChosen.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
			onSpheroChosen.transform.position = new Vector3(GAMEMANAGER.GM.SpheroX,0.2f,GAMEMANAGER.GM.SpheroZ);

			Destroy (onSpheroChosen, 1.0f);
		}
	}
}
