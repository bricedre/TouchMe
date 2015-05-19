using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;
using System.Collections.Generic;

// Classe gérant les touches pour le gameplay Pattern

public class TouchManager : TouchObject {
	
	private GameObject cursor;
	private RaycastHit hit;
	private Ray ray;
	public List<GestureEvent> touches;
	
	void Start () {
		cursor = GameObject.Find ("Cursor");
		cursor.transform.position = new Vector3 (-100.0f, -100.0f, -3.2f);
	}

	void Update(){

		if(transform.childCount == 0){
			initializeCursor();
			Destroy(gameObject);
		}

	}

	public void NDrag(GestureEvent gEvent){

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit)){
			cursor.transform.position = new Vector3 (hit.point.x, hit.point.y, cursor.transform.position.z);
		}


		//At the end of the gesture, put it back to neutral pos
		if (gEvent.Phase == GesturePhase.GESTURE_END || gEvent.Phase == GesturePhase.GESTURE_RELEASE || gEvent.Phase == GesturePhase.GESTURE_PASSIVE) {
			initializeCursor();
		}
	}

	private void initializeCursor(){
		cursor.transform.position = new Vector3 (-100.0f, -100.0f, cursor.transform.position.z);
	}
}
