////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    TouchCircle.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Class for touch point display, depends on Prefabs/FingerPoint
/// </summary>

namespace GestureWorksCoreNET.Unity {
	
	public class TouchCircle {
		
		private	int						Id;
		private float					X,Y,Z;
		
		private	GUIStyle				TextStyle;
		private	GameObject				Ring;
		private	Vector3					ringPosition;
		private	Camera					theCam;
		
		private	string					LabelText;
		
		private	bool					coords;

		private GameObject              parent;
	
		public TouchCircle(int id, float x, float y, Vector3 a, Camera cam, bool showCoords=false, float z=0, GameObject _parent=null) {
			
			if(parent==null) {
				parent = GameObject.Find("/GestureWorks");
			}
			//parent = _parent;

			Id = id;
			
			TextStyle = new GUIStyle();
			TextStyle.normal.textColor = new Color(0/255.0F, 122/255.0F, 157/255.0F);
			
			Ring = GameObject.Instantiate(Resources.Load("FingerPoint"), Vector3.zero, Quaternion.identity) as GameObject;
			ringPosition = new Vector3(x, y, z);
			Ring.transform.eulerAngles = a;
			Ring.transform.parent = parent.transform;
			theCam = cam;
			coords = showCoords;
			
			Ring.guiText.text = LabelText = "";
			
			Update(x,y,z);
		}
		
		public void Update(float x, float y, float z=0) {
			
			X = x;
			Y = Screen.height-y;
			Z = z;
			
			ringPosition.Set(X,Y,Z);
			Ring.transform.position = theCam.ScreenToViewportPoint(ringPosition);
			
			if(coords) {
				LabelText = "ID: " + Id + "\n";
				LabelText += "X: " + Math.Ceiling(X) + " | " + "Y: " + Math.Ceiling(Y);
				Ring.guiText.text = LabelText;
			}
		}
		
		// Call this just before removing
		public void RemoveRing() {
			Transform RingParticle = Ring.transform.GetChild(0);
			RingParticle.GetComponent<ParticleSystem>().enableEmission = false;
			DestroyMeAfter dma = RingParticle.gameObject.AddComponent<DestroyMeAfter>();
			dma.Seconds = 0.5f;
			RingParticle.parent = parent.transform;
			UnityEngine.Object.Destroy(Ring);
	    }
		
		public void Hide() {
			Ring.SetActive(false);
		}
		
		public void Show() {
			Ring.SetActive(true);
		}
		
	}
}