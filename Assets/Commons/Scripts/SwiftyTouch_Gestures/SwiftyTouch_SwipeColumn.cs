/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		SwiftyTouch_SwipeColumn.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		1.0
 * @Description :	Script lié aux colonnes de détection de la gesture Swipe.
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * Ce script doit être présent sur chaque instance du gameObject <SwiftyTouch_SwipeColumn>.
 * 
 * Paramètres disponibles :
 * - X
 * 
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;
using Swifty;

public class SwiftyTouch_SwipeColumn : TouchObject {

	// ### PROPERTIES ###################################################################
	
	// Accessibles dans l'inspecteur
		/* Propriétés publiques, visibles dans l'inspecteur */
	
	// Accessibles dans d'autres scripts
		/* Propriétés publiques, mais avec {get/set}, et donc non visibles en inspecteur */
	
	// Privées
		/* Propriétés strictement privées */
		private	Camera				cam;
		private	Vector3				startPosition=Vector3.zero;
		private	Vector3				worldPosition=Vector3.zero;
		private	SwiftyTouch_Swipe	SwipeScript;
	
	
	// ### INITIALISATION ###############################################################
	void Awake() {
		
		// Obtention du script Swipe
		SwipeScript = GameObject.Find("/GestureWorks_SwipePlane").GetComponent<SwiftyTouch_Swipe>();
	}
	
	
	// ### 1ST FRAME ####################################################################
	void Start() {
		
		// Obtention de la caméra
		cam = CommonGW.GWmain.cameraUsedByGestures;
		
		// Obtention de la position initiale
		startPosition = this.transform.position;
	}
	
	
	// ### UPDATE #######################################################################
	//void Update() {}
	
	
	// ### MULTITOUCH ###################################################################
	
	// Si un doigt glisse sur ce gameObject
	public void Drag(GestureEvent gEvent) {
		
		// Calcule des position QUE si les coordonnées ne sont pas nulles
		if(gEvent.X+gEvent.Y != 0 ) {
			Vector3	screenPos = new Vector3(gEvent.X, gEvent.Y, (cam!=Camera.main ? cam.transform.position.y : 0.0f));
			worldPosition = cam.ScreenToWorldPoint(screenPos);
			worldPosition = Common.EditAxisV3( worldPosition, "y", startPosition.y);
			worldPosition = Common.EditAxisV3( worldPosition, "z", -worldPosition.z);
		}
		
		/* DOWN */
		if(gEvent.Phase.ToString()=="GESTURE_BEGIN") {
			setColumn();
		}

		/* DRAG */
		if(gEvent.Phase.ToString()=="GESTURE_ACTIVE") {
			setColumn();
		}
		
		/* UP */
		if(gEvent.Phase.ToString()=="GESTURE_END" || gEvent.Phase.ToString()=="GESTURE_RELEASE") {
			CommonGW.GWmain.UnRegisterTouchObject(this.gameObject);
			Destroy(this.gameObject);
		}
	}
	
	
	// ### TOOLS ########################################################################
	
	// Règle la position et la rotation de la colonne de détection
	public void setColumn() {
		this.GetComponent<CapsuleCollider>().enabled = false;
		this.transform.position = worldPosition;
		this.transform.transform.LookAt( SwipeScript.CameraTrans.position );
		this.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
		this.GetComponent<CapsuleCollider>().enabled = true;
	}
}