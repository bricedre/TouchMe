////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    TouchCube.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;
using Swifty;

public class TouchCube : TouchObject {

	public	enum			layoutsList{XY,XZ};
	public	layoutsList		layout2D;
	
	private Camera 				cam;
	private	Vector3				worldPosition=Vector3.zero;
	
	public	string				currentStatus {get; set;}
	public  bool				Drag{get; set;}
	private	bool				AllowedDrag = true;		//autorise ou invalide le drag
	public 	int					FramesReleased {get;set;}
	
	public	int					pointID{get;set;}
	public	void				set_pEvent(int p) {if(pointID == -1){pointID=p; currentStatus="TOUCHADDED";}}
	
	// Variables pour le retour du cube à sa position initiale
	public float 				speedLerp = 1.0f;
	public bool					isLerping{get; set;}
	private float 				startTime;
	private Vector3				positionToAttempt;
	private Vector3				startPosition;
	private float				journeyLength;
	
	void Awake(){
		SupportedGestures = new string[1];
		SupportedGestures[0] = "NDrag";
		pointID = -1;
	}
	
	void Start() {
		currentStatus = "";
		// Obtention du script GW
		CommonGW.GWmain.RegisterNewTouchObject(this.gameObject);
		
		// Obtention de la caméra
		cam = CommonGW.GWmain.cameraUsedByGestures;
		Drag = false;
		FramesReleased = 3;
	}

	void Update(){
		if(FramesReleased <= 2)
			FramesReleased++;
		else if (currentStatus == "TOUCHREMOVED"){
			currentStatus = "";
			Drag = false;
		}
		if(isLerping){
			float distCovered = (Time.timeSinceLevelLoad - startTime) * speedLerp;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (startPosition, positionToAttempt, fracJourney);
			if(fracJourney >= 1.0f)
				isLerping = false;
		}
	}
	
	public void NDrag(GestureEvent gEvent){
		
		//Debug.Log(pointID);
		// Calcule des position QUE si les coordonnées ne sont pas nulles
		if(gEvent.X+gEvent.Y != 0 ) {
			Vector3	screenPos = new Vector3(gEvent.X, gEvent.Y, (cam!=Camera.main ? cam.transform.position.y - this.transform.position.y : 0.0f));
			//Debug.Log (screenPos);
			//screenPos.y += 30;
			worldPosition = cam.ScreenToWorldPoint(screenPos);
			worldPosition = Common.EditAxisV3( worldPosition, "y", transform.position.y);
			worldPosition = Common.EditAxisV3( worldPosition, "z", -worldPosition.z);
		}
		
		// Récupération du Status via pEvent, à partir de l'ID du point utilisé
		if(currentStatus!="TOUCHADDED") {
			foreach(PointEvent pEvent in CommonGW.GWmain.Core.pEvents) {
				if(pEvent.PointId==pointID)
					currentStatus = pEvent.Status.ToString();
			}
		}
		
		if(currentStatus=="")
			return;
		
		/* DOWN */
		if(currentStatus=="TOUCHADDED") {
			SetDragAllowed(true);
			isLerping = false;
			currentStatus="";
			positionToAttempt = transform.position;
			// SoundFx
		}
		
		
		/* UP */
		
		if(currentStatus=="TOUCHREMOVED" && Drag) {
			Drag = false;
			isLerping = true;
			FramesReleased = 0;
			startPosition = worldPosition;
			journeyLength = Vector3.Distance(startPosition, positionToAttempt);
			startTime = Time.timeSinceLevelLoad;
			pointID=-1;
			//Debug.Log ("Touch Removed");
		}
		/*else if(currentStatus == "TOUCHREMOVED"){
			currentStatus = "";
		}*/
		
		
		/* DRAG */
		if(currentStatus=="TOUCHUPDATE" && AllowedDrag) {
			
			Drag = true;
			isLerping = false;
			
			if(layout2D == layoutsList.XY)
				this.transform.position = new Vector3(worldPosition.x, worldPosition.z , worldPosition.y);
			else
				this.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
		}
	}
	
	// Active le drag sur le cube
	void SetDragAllowed(bool sd) {
		AllowedDrag = sd;
	}

}