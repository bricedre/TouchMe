using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class HandleScript: TouchObject {

	private StretchScript parentScript;
	bool active = true;
	public string phase;

	void Start () {
		parentScript = transform.parent.GetComponent<StretchScript> ();
	}

	void Update(){

	}
	
	public void NDrag(GestureEvent gEvent){

		//Debug Log
		phase = "* " + gEvent.Phase.ToString () + " *";

		//According to cases
		switch(gEvent.Phase) {

		case GesturePhase.GESTURE_ACTIVE:

			//Get touch Infos
			float dX = gEvent.Values ["drag_dx"];
			
			//Formatting touch Infos
			if (!parentScript.moveableLeft && dX < 0)
				dX = 0.0f;
			if (!parentScript.moveableRight && dX > 0)
				dX = 0.0f;
			
			//Joint at full stretch, reduce movement a lot!
			if (!parentScript.withinBounds) {
				dX = Mathf.Clamp (dX, -0.2f, 0.2f);
			}
			
			//Turn the joint Kinematic / freeze X gravity
			rigidbody.isKinematic = true;
			
			//Move object according to mousePosition
			Camera cam = Camera.main;
			
			Vector3 previousPosition = cam.WorldToScreenPoint (transform.position);
			Vector3 nextPosition = new Vector3 (dX, 0.0f, 0.0f);
			Vector3 newPosition = previousPosition + nextPosition;

			transform.position = cam.ScreenToWorldPoint (newPosition);

			break;

		case GesturePhase.GESTURE_PASSIVE:
			rigidbody.isKinematic = false;
			break;

		case GesturePhase.GESTURE_END:
			rigidbody.isKinematic = false;
			break;

		case GesturePhase.GESTURE_RELEASE:
			rigidbody.isKinematic = false;
			break;

		default:
			break;
		}
	}
	
}
