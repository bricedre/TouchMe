using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;
using System.Collections.Generic;

// Classe gérant les touches pour le gameplay Pattern

public class TouchManager : TouchObject {
	
	public GameObject cursor;
	private RaycastHit hit;
	private Ray ray;
	public List<GestureEvent> touches;
	public AudioManager audioManager;
	private float previousX, previousY;
	public float eventMagnitude;
	
	void Start () {
		cursor = GameObject.FindGameObjectWithTag ("Cursor");
		cursor.transform.position = new Vector3 (-100.0f, -100.0f, -18.3f);
		touches = new List<GestureEvent> ();
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager>();

	}

	void Update(){

		//When pattern suite is depleted
		if(transform.childCount == 0){
			initializeCursor();
			GameObject.Find ("GameManager").GetComponent<GameManager>().makeProgressInScenario();
			initializeCursor();
			audioManager.playEvent ("Touch_stop");
			Destroy(gameObject);
		}



	}

	public void NDrag(GestureEvent gEvent){

		if (gEvent.Phase == GesturePhase.GESTURE_BEGIN){
			audioManager.playEvent ("Touch_play");
			previousX = gEvent.X;
			previousY = gEvent.Y;
		}

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit)){
			cursor.transform.position = new Vector3 (hit.point.x, hit.point.y, cursor.transform.position.z);
		}

		eventMagnitude = new Vector2 (gEvent.X - previousX, gEvent.Y - previousY).magnitude;
		eventMagnitude = eventMagnitude;
		// RTCP : AkSoundEngine.SetRTCP(magnitude);

		//At the end of the gesture, put it back to neutral pos
		if (gEvent.Phase == GesturePhase.GESTURE_END || gEvent.Phase == GesturePhase.GESTURE_RELEASE || gEvent.Phase == GesturePhase.GESTURE_PASSIVE) {
			initializeCursor();
			audioManager.playEvent ("Touch_stop");
		}

		//Update last coordinates
		previousX = gEvent.X;
		previousY = gEvent.Y;
	}

	private void initializeCursor(){
		cursor.transform.position = new Vector3 (-100.0f, -100.0f, cursor.transform.position.z);
	}
}
