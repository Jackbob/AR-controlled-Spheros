using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchInput : MonoBehaviour
{
	public static TouchInput TI;
	private Color posColor = new Color (0f, 1.0f, 0f, 1.0f);
	private Material posColored;
	private Color sphero1 = new Color(1.0f, 0f, 0f, 1.0f);
	private Color sphero2 = new Color(0.7f, 0.1f, 0.6f, 1.0f);
	private float Rotate = 0;

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

				if (GAMEMANAGER.GM.WhichSpehro == 1) {
					posColored.color = sphero1;
				} else {
					posColored.color = sphero2;
				}

				pos.GetComponent<Renderer> ().material = posColored;
				pos.transform.parent = transform;
				pos.transform.localScale = new Vector3 (0.1f, 0.001f, 0.1f);
				pos.transform.position = planePoint;

				// Just to write out the coords of the touch input on the target plane
				float vX = planePoint.x;
				float vZ = planePoint.z;

				if (GAMEMANAGER.GM.IsChosen) {
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

		if(GAMEMANAGER.GM.IsChosen){
			
				
			Rotate += 1;

			// Which Sphero is used at the moment
			GameObject onSpheroChosen = Instantiate(Resources.Load("arrow", typeof(GameObject))) as GameObject;

			posColored.color = posColor;
			onSpheroChosen.GetComponent<Renderer> ().material = posColored;
			onSpheroChosen.transform.parent = transform;
			onSpheroChosen.transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
			onSpheroChosen.transform.position = new Vector3(GAMEMANAGER.GM.ShowChosenX,0.2f,GAMEMANAGER.GM.ShowChosenZ);

			Destroy (onSpheroChosen, 0.1f);
		}
	}
}
