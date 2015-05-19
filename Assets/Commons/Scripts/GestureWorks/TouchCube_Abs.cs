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

public class TouchCube_Abs : TouchObject {

	public	enum			layoutsList{XY,XZ};
	public	layoutsList		layout2D;
	
	private	Vector3			startPosition;
	
	void Start() {
		startPosition = this.transform.position;
	}

	//void Update(){}
	
	public void NDrag(GestureEvent gEvent){
		
		Camera cam = Camera.main;
		
		/*
		float multiplier = 0.0008f;
		
		float dX = gEvent.Values["drag_dx"]*multiplier*Flipped;
		float dY = gEvent.Values["drag_dy"]*multiplier*Flipped;
		
		Vector3 previousPosition = cam.WorldToScreenPoint(transform.position);
		Vector3 nextPosition = new Vector3(dX, dY, 0.0f);
		
		Vector3 newPosition = previousPosition + nextPosition;
		
		float c1 = Mathf.Clamp(
					cam.ScreenToWorldPoint(newPosition).x+dX,
					-0.54f, 0.54f);
		float c2 = Mathf.Clamp(
					(layout2D==layoutsList.XY ?
						cam.ScreenToWorldPoint(newPosition).y+dY
						:
						cam.ScreenToWorldPoint(newPosition).z-dY
					),
					-0.25f, 0.25f);
		float c3 = (layout2D==layoutsList.XY ? startPosition.z : startPosition.y);
		*/
		
		Vector3 screenPos = new Vector3(gEvent.X, gEvent.Y, 0.0f);
		if(screenPos==Vector3.zero)
			return;
		
		float c1 = Mathf.Clamp(
					cam.ScreenToWorldPoint(screenPos).x,
					-0.54f, 0.54f);
		float c2 = Mathf.Clamp(
					(layout2D==layoutsList.XY ?
						cam.ScreenToWorldPoint(screenPos).y
						:
						-cam.ScreenToWorldPoint(screenPos).z
					),
					-0.25f, 0.25f);
		float c3 = (layout2D==layoutsList.XY ? startPosition.z : startPosition.y);
		
		transform.position = ( layout2D==layoutsList.XY ? new Vector3(c1, c2, c3) : new Vector3(c1, c3, c2) );
		
	}

}