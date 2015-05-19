using UnityEngine;
using System.Collections;

// GestureWorks WON'T WORK without using those namespace
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;
using Swifty;

//TouchObject allows the object to recieve gestures
public class Gesture_Cube: TouchObject {

	/*
	 * In order to make an object working with GestureWork, it has to have an UNIQUE name
	 * You won't be able to move two objects named "Cube" if they're both registered on GestureWorks
	 * 
	 * To do so use the following method :
	 * 
	 * CommonGW.GWmain.UnRegisterTouchObject (this.gameObject);
	 * CommonGW.GWmain.this.name = this.name+"_"+Common.GenerateID();
	 * CommonGW.GWmain.RegisterNewTouchObject(this.gameObject);
	 * 
	 * An array should appear on the public listed variable in the editor
	 * It should be named "Supported Gestures"
	 * For this exemple, set it to 1 and write NDrag [Name of the following gesture]
 	*/

	//Camera used by gestures
	private Camera cam;

	//World position of the selected object
	private Vector3 worldPosition;

	//World position when the gesture begin
	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
	
		//Get the camera used by GestureWorks --> GestureWorks_Config <MainConfig> value : Camera used By Gestures
		if( GameObject.Find("/GestureWorks") !=null)
			cam = GameObject.Find("/GestureWorks").GetComponent<Main>().cameraUsedByGestures;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Gesture : Drag multi-touch.
	//Is called each time you click on a object which does not have any gesture active
	public void NDrag(GestureEvent gEvent) {

		//Convert click to world position
		Vector3	screenPos = new Vector3(gEvent.X, gEvent.Y, (cam!=Camera.main ? cam.transform.position.y : 0.0f));
		worldPosition = cam.ScreenToWorldPoint(screenPos) * 0.95f;
		worldPosition = Common.EditAxisV3( worldPosition, "y", startPosition.y);
		worldPosition = Common.EditAxisV3( worldPosition, "z", -worldPosition.z);

		switch (gEvent.Phase) {
			//Called when the gesture begins : First click on the object
		case GesturePhase.GESTURE_BEGIN:

			//Register position
			if(startPosition==Vector3.zero) {
				startPosition = this.transform.position;
			}

			//Starts updating
			this.transform.position = worldPosition;

			break;

			//Is called once per frame as long as their is still a finger executing the gesture
		case GesturePhase.GESTURE_ACTIVE:

			//Updates position
			this.transform.position = worldPosition;

			break;

			//Is called each time a finger is released
		case GesturePhase.GESTURE_RELEASE:

			break;

			//Is called once there is no more fingers on the touch object
		case GesturePhase.GESTURE_END:


			break;

			//Other cases, if a gesture isn't specified
		default:

			break;
		}
	}
}
