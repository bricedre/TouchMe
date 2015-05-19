////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    Main.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;
using Swifty;
//using TUIO;

public class Main : MonoBehaviour {

	// Accessibles dans l'inspecteur

	// Accessibles dans les autres scripts
	public string windowName_inPlayer { get; set; }
	public Camera cameraUsedByGestures { get; set; }
	public bool tryAnotherCam { get; set; }
	public bool showPoint { get; set; }
	public enum pLayoutsList { XY, XZ };
	public pLayoutsList pointsLayout2D { get; set; }
	public bool verboseMode { get; set; }
	public bool PQLab { get; set; }
	public bool typeCadreSet { get; set; }

	public GestureworksInstance Core { get; set; }

	private bool _proceedTheEvents;
	public bool proceedTheEvents {
		get { return _proceedTheEvents; }
		set {
			if (value == false && _proceedTheEvents == true) {
				_proceedTheEvents = value;
				stopGestureWorks();
			} else {
				_proceedTheEvents = value;
			}
		}
	}

	// Strictement privées
	private string windowName;
	private string dllFileName;
	private string dllFilePath;
	private string gmlFileName;
	private string gmlFilePath;
	private MainConfig GW_config;
	private bool canDoUpdates = false;
	private GestureWorksCoreNET.Unity.HitManager HitManager;


	// ### INITIALISATION ###############################################################
	//void Start() {
	void Awake() {

		this.name = "GestureWorks";

		// Delete all artefacts
		removeArtefacts();

		// GET PARAMETERS FROM CONFIG
		getSceneParameters();

		Core = GestureworksInstance.Instance;

		Core.initOrCleanDatas();

		HitManager = new HitManager(cameraUsedByGestures);

		// Autorisation de traiter les events
		proceedTheEvents = true;

		// Initialisation des objets tactiles
		InitializeGestureObjects();

		// Autorise les updates
		canDoUpdates = true;
	}

	// GET PARAMETERS FROM CONFIG
	public void getSceneParameters() {
		// Connect to config
		GameObject GWconfig = GameObject.Find("/GestureWorks_Config");
		if (GWconfig != null) {
			GW_config = GWconfig.GetComponent<MainConfig>();
			// get config
			windowName_inPlayer = GW_config.windowNameInPlayer;
			cameraUsedByGestures = GameObject.Find((GW_config.cameraUsedByGestures.ToString() == "SwiftyCam_GUI" ? "/SwiftyCam_GUI" : "/SwiftyCam_WORLD/SwiftyCam_WORLD")).camera;
			showPoint = GW_config.showPoint;
			pointsLayout2D = GW_config.pointsLayout2D.ToString() == "XZ" ? pLayoutsList.XZ : pLayoutsList.XY;
			verboseMode = GW_config.verboseMode;
			if (!typeCadreSet) {
				PQLab = GW_config.typeDeCadre.ToString() == "PQLab" ? true : false;
				typeCadreSet = true;
			}
			// edit TuioClient Camera
			this.gameObject.GetComponent<GW_touchClient>().cameraOfMouseSimulation = cameraUsedByGestures;
		}
	}

	void onDestroy() {
		stopGestureWorks();
	}

	void OnApplicationQuit() {
		stopGestureWorks();
	}

	// Bloque les updates / Vide les variables / Détruit les artefactes
	public void stopGestureWorks() {

		// Stop the updates
		canDoUpdates = false;

		// Clean GW datas
		if (Core != null)
			Core.initOrCleanDatas();

		// Remove Artefacts
		removeArtefacts();
	}

	/*private bool jot=false;
	private void justOneTime() {
		foreach(TouchObject obj in Core.GestureObjects) {
			Debug.Log(obj.name);
		}
		jot=true;
	}*/

	// ### UPDATE #######################################################################
	void LateUpdate() {

		/*if(!jot)
			justOneTime();*/

		if (!canDoUpdates && !proceedTheEvents)
			return;

		if (Application.isLoadingLevel)
			return;

		if (Core.core == null)
			return;

		if (!Core.core.IsInitialized || !Core.core.IsLoaded || !Core.GmlLoaded || !Core.WindowLoaded) {
			return;
		}

		Core.core.ProcessFrame();

		Core.pEvents = Core.core.ConsumePointEvents();

		if (verboseMode) {
			foreach (PointEvent pEvent in Core.pEvents) {
				if (pEvent.Status == TouchStatus.TOUCHADDED) {
					string output = "TOUCHADDED-----------------------------\r\n";
					output += "Point ID:  " + pEvent.PointId.ToString();
					output += "\r\n X: " + pEvent.Position.X.ToString();
					output += "\r\n Y: " + pEvent.Position.Y.ToString();
					output += "\r\n W: " + pEvent.Position.W.ToString();
					output += "\r\n H: " + pEvent.Position.H.ToString();
					output += "\r\n Z: " + pEvent.Position.Z.ToString();
					output += "\r\n Touch Status: " + pEvent.Status.ToString();
					output += "\r\n Timestamp: \r\n" + pEvent.Timestamp.ToString();
					Debug.Log(output);
				}
			}
		}

		if (Core.pEvents != null && Core.pEvents.Count > 0) {

			foreach (PointEvent pEvent in Core.pEvents) {

				// === ADD ===
				if (pEvent.Status == TouchStatus.TOUCHADDED) {

					if (!Core.TouchPoints.ContainsKey(pEvent.PointId) && showPoint) {

						Core.TouchPoints.Add(
							pEvent.PointId,
							new TouchCircle(
								pEvent.PointId,
								pEvent.Position.X,
								pEvent.Position.Y,
								(pointsLayout2D == pLayoutsList.XY ? new Vector3(0, 0, 0) : new Vector3(90, 0, 0)),
								cameraUsedByGestures,
								(verboseMode ? true : false)
							)
						);
					}
					bool touchPointHitSomething = false;
					int i = 0;
					foreach (TouchObject obj in Core.GestureObjects) {

						// Default Camera
						if (obj != null && HitManager.DetectHit(
								pEvent.Position.X,
							//cameraUsedByGestures.GetScreenHeight()-pEvent.Position.Y,
								Screen.height - pEvent.Position.Y,
								obj.gameObject)
						) {
							if (obj.name == "GestureWorks_SwipePlane") {
								string newColumnName = "SwipeColumn_" + Common.GenerateID();
								createColumn(newColumnName, pEvent.Position.X, pEvent.Position.Y);
								Core.core.AddTouchPoint(newColumnName, pEvent.PointId);
								touchPointHitSomething = true;
								if (verboseMode) Debug.Log("(defaultCam) I hit the SwipePlane (column: " + newColumnName + ") / pEvent.PointId" + pEvent.PointId);
							} else {
								Core.core.AddTouchPoint(obj.GetObjectName(), pEvent.PointId);
								touchPointHitSomething = true;
								obj.SendMessage("set_pEvent", pEvent.PointId, SendMessageOptions.DontRequireReceiver);
								if (verboseMode) Debug.Log("(defaultCam) I hit " + obj.GetObjectName() + " pEvent.PointId" + pEvent.PointId);
							}
							break;
						}
						i++;
					}
					if (touchPointHitSomething == false) {
						Core.core.AddTouchPoint(cameraUsedByGestures.name, pEvent.PointId);
					}

				}
				// === REMOVE ===
				if (pEvent.Status == TouchStatus.TOUCHREMOVED) {
					if (Core.TouchPoints.ContainsKey(pEvent.PointId)) {
						Core.TouchPoints[pEvent.PointId].RemoveRing();
						Core.TouchPoints.Remove(pEvent.PointId);
					}

				}
				// === UPDATE ===
				if (pEvent.Status == TouchStatus.TOUCHUPDATE) {
					if (Core.TouchPoints.ContainsKey(pEvent.PointId)) {
						Core.TouchPoints[pEvent.PointId].Update(pEvent.Position.X, pEvent.Position.Y);
					}
				}
			}
		}

		if (Core.core == null)
			return;

		Core.gEvents = Core.core.ConsumeGestureEvents();

		if (Core.gEvents != null) {

			if (Core.gEvents.Count > 0) {

				//string previousObject="";
				foreach (GestureEvent gEvent in Core.gEvents) {

					if (verboseMode) {
						string o;
						o = "-----------------------------\r\n";
						o += gEvent.ToString();
						o += "EventID: " + gEvent.EventID;
						o += "\r\n GestureID: " + gEvent.GestureID;
						o += "\r\n Target: " + gEvent.Target;
						o += "\r\n N: " + gEvent.N.ToString();
						o += "\r\n X: " + gEvent.X.ToString();
						o += "\r\n Y: " + gEvent.Y.ToString();
						o += "\r\n Timestamp: " + gEvent.Timestamp.ToString();
						o += "\r\n Locked Points: " + gEvent.LockedPoints.GetLength(0).ToString();
						o += "\r\n";
						Debug.Log(o);
					}

					// send gesture events to all subscribers
					foreach (TouchObject obj in Core.GestureObjects) {

						//send events only to corresponding registered touch objects
						if (obj != null && obj.name == gEvent.Target) {
							if (verboseMode)
								Debug.Log("sendGesture \"" + gEvent.GestureID + "\" to " + obj.gameObject.name + " (" + gEvent.Target + ")");
							obj.gameObject.SendMessage(gEvent.GestureID, gEvent, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}
		} else {
			if (verboseMode)
				Debug.Log("gEvents null");
		}

	}

	// Get all TouchObjects (already placed on scene)
	private void InitializeGestureObjects() {
		GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in objects) {
			RegisterNewTouchObject(obj);
		}
	}

	// Register a new TouchObject
	public void RegisterNewTouchObject(GameObject cible) {

		// Stop si partie terminée
		if (!proceedTheEvents) return;

		// get the touchObject script
		TouchObject script = cible.GetComponent<TouchObject>();
		if (script != null) {
			if (verboseMode) Debug.Log("You give me the GameObject \"" + cible.name + "\" as a TouchObject.");
			// register object with core
			Core.core.RegisterTouchObject(cible.name);
			// and register gestures
			foreach (string gesture in script.SupportedGestures) {
				Core.core.AddGesture(cible.name, gesture);
				Core.core.EnableGesture(cible.name, gesture);
				if (verboseMode) Debug.Log("gestures trouvées sur l'objet \"" + cible.name + "\" ré-attribué: " + gesture);
			}
			// keep references to TouchObjects in scene
			Core.GestureObjects.Add(script);
		}
	}

	// UnRegister a TouchObject
	public void UnRegisterTouchObject(GameObject cible) {
		TouchObject script = cible.GetComponent<TouchObject>();
		if (script != null) {
			foreach (string gesture in script.SupportedGestures) {
				Core.core.DisableGesture(cible.name, gesture);
			}
			//Core.GestureObjects.Remove(script);//(commenté car fausse la collection, ce qui provoque des erreurs dans la boucle de traitement des objets)
			Core.core.DeregisterTouchObject(cible.name);
		} else
			//if(verboseMode)
				Debug.Log("[GestureWorks/Main.cs] L'objet à retirer ne porte pas de script de type \"TouchObject\".");
	}

	// Unregister/Register a TouchObject
	public void restartTouchObject(GameObject cible) {
		UnRegisterTouchObject(cible);
		RegisterNewTouchObject(cible);
	}

	// Delete all artefacts
	private void removeArtefacts() {
		foreach (Transform t in this.transform) {
			Destroy(t.gameObject);
		}
	}

	// Creation d'une nouvelle colonne de détection
	public void createColumn(string columnName, float ePosX, float ePosY) {

		// Calcul de la position initiale
		Vector3 screenPos = new Vector3(ePosX, ePosY, 0.0f);
		Vector3 worldPosition = cameraUsedByGestures.ScreenToWorldPoint(screenPos);
		worldPosition = Common.EditAxisV3(worldPosition, "y", 0.0f);
		worldPosition = Common.EditAxisV3(worldPosition, "z", -worldPosition.z);

		// On crée une colonne de détection
		GameObject column = GameObject.Instantiate(Resources.Load("SwiftyTouch_SwipeColumn"), worldPosition/*new Vector3(100.0f,0.0f,100.0f)*/, Quaternion.Euler(0, 0, 0)) as GameObject;

		// Nom
		column.name = columnName;

		// On change la parenté
		column.transform.parent = GameObject.Find("/GestureWorks_SwipeColumns").transform;

		// Inscription
		RegisterNewTouchObject(column);
	}
}