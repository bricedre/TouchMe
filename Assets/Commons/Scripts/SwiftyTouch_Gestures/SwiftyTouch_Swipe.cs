/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		SwiftyTouchGesture_Swipe.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		1.0
 * @Description :	Script de gestion des Swipes "SwiftyTouch"
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * Ce script doit être présent une seule fois dans la scène, sur le gameObject <SwiftyTouch_SwipePlane>
 * et paramétré pour obtenir les interactions multitouch souhaitées (Swipe).
 * 
 * Paramètres disponibles :
 * - X
 * 
 */
 
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class SwiftyTouch_Swipe : TouchObject {

	// ### PROPERTIES ###################################################################
	
	// Accessibles dans l'inspecteur
		/* Propriétés publiques, visibles dans l'inspecteur */
	
	// Accessibles dans d'autres scripts
		/* Propriétés publiques, mais avec {get/set}, et donc non visibles en inspecteur */
		public	Transform		CameraTrans{get;set;}
	
	// Privées
		/* Propriétés strictement privées */
	
	
	// ### INITIALISATION ###############################################################
	void Awake() {
		// Obtention de la hauteur de la caméra
		CameraTrans = GameObject.Find("/SwiftyCam_WORLD/SwiftyCam_WORLD").transform;
	}
	
	
	// ### 1ST FRAME ####################################################################
	//void Start() {}
	
	
	// ### UPDATE #######################################################################
	//void Update() {}
	
	
	// ### MULTITOUCH ###################################################################
	
	
	// ### TOOLS ########################################################################
	
}