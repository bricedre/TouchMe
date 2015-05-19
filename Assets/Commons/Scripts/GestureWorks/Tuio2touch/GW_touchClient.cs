using UnityEngine;
using System;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;
using TUIO;
using Swifty;

public class GW_touchClient : MonoBehaviour {

	public	Camera					cameraOfMouseSimulation{get;set;}

	private	TuioInstance			tuioInstance;
	
	private	int						mouseEventID = 9000;//Index dédié aux clics souris
	
	private	bool					canUpdate=false;
	
	// Use this for initialization
	void Start() {
		
		// Camera
		cameraOfMouseSimulation = CommonGW.GWmain.cameraUsedByGestures;
		
		// Instance TUIO
		tuioInstance = TuioInstance.Instance;
		
		// Permet les Updates
		canUpdate=true;
	}
	
	void OnApplicationQuit() {
		
		// Stop les Updates
		canUpdate=false;
		
		// Deconnexion de l'instance
		tuioInstance.Disconnect();
	}
	
	void OnDestroy() {
		
		// Stop les Updates
		canUpdate=false;
	}

	public void debugTuio() {
		Debug.Log(tuioInstance);
	}
	
	
	void LateUpdate() {
		
		if(!canUpdate)
			return;
		
		// Mouse DOWN
		if(Input.GetMouseButtonDown(0)) {
			Vector2 mousePos = cameraOfMouseSimulation.ScreenToViewportPoint(Input.mousePosition);
			TouchEvent new_mouseEvent = new TouchEvent();
			new_mouseEvent.TouchEventID = mouseEventID;
			new_mouseEvent.Status = TouchStatus.TOUCHADDED;
			new_mouseEvent.X = (float)mousePos.x;
			new_mouseEvent.Y = (float)(1.0f-mousePos.y);
			new_mouseEvent.Z = 0.0f;
			new_mouseEvent.W = 1.0f;
			new_mouseEvent.H = 1.0f;
			new_mouseEvent.R = 0.0f;
			
			CommonGW.Core.core.AddEvent(new_mouseEvent);
		}
		// Mouse DRAG
		if(Input.GetMouseButton(0)) {
			Vector2 mousePos = cameraOfMouseSimulation.ScreenToViewportPoint(Input.mousePosition);
			TouchEvent updt_mouseEvent = new TouchEvent();
			updt_mouseEvent.TouchEventID = mouseEventID;
			updt_mouseEvent.Status = TouchStatus.TOUCHUPDATE;
			updt_mouseEvent.X = (float)mousePos.x;
			updt_mouseEvent.Y = (float)(1.0f-mousePos.y);
			updt_mouseEvent.Z = 0.0f;
			updt_mouseEvent.W = 1.0f;
			updt_mouseEvent.H = 1.0f;
			updt_mouseEvent.R = 0.0f;
			
			CommonGW.Core.core.AddEvent(updt_mouseEvent);
		}
		// Mouse UP
		if(Input.GetMouseButtonUp(0)) {
			Vector2 mousePos = cameraOfMouseSimulation.ScreenToViewportPoint(Input.mousePosition);
			TouchEvent rem_mouseEvent = new TouchEvent();
			rem_mouseEvent.TouchEventID = mouseEventID;
			rem_mouseEvent.Status = TouchStatus.TOUCHREMOVED;
			rem_mouseEvent.X = (float)mousePos.x;
			rem_mouseEvent.Y = (float)(1.0f-mousePos.y);
			rem_mouseEvent.Z = 0.0f;
			rem_mouseEvent.W = 1.0f;
			rem_mouseEvent.H = 1.0f;
			rem_mouseEvent.R = 0.0f;
			
			CommonGW.Core.core.AddEvent(rem_mouseEvent);
			mouseEventID++;
		}
	}
}