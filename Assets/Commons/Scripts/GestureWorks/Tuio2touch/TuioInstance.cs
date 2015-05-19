/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		TuioInstance.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		1.0
 * @Description :	Assure la connexion TUIO tout au long de la session
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 * 
 * Ce script permet de n'avoir qu'une seule connexion TUIO, réutilisée
 * dans chaque scène. La déconnexion de TUIO n'est requise QUE lorsque l'application est quittée.
 *
 */
 
using UnityEngine;
using System.Collections;
using TUIO;
using Swifty;

public sealed class TuioInstance {

		// Instance
		public static readonly TuioInstance	instance = new TuioInstance();

		// Client & Listener
		public	SwTuioClient			client;
		public	Sw_TouchListener		listener;

		
	public TuioInstance() {
		
		// Création du client
		client = new SwTuioClient(3333);
		// Création d'un listener
		listener = new Sw_TouchListener();
		// Attribution du listener au client
		client.addTuioListener(listener);
		// Connection du client
		client.connect();
	}
 
	public static TuioInstance Instance {
		get {
			return instance;
		}
	}

	public void Disconnect() {
		if(instance!=null && instance.client!=null && instance.listener!=null) {
			instance.client.removeTuioListener(instance.listener);
			instance.client.disconnect();
			instance.listener = null;
			instance.client = null;
		}
	}

}