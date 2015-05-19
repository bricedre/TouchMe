/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		SwiftyCam_GUI.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		2.0
 * @Description :	Script principal de toutes les scènes, qui gére l'initialisation, les caméras, les GUI.
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * Ce script doit être présent une seule fois dans la scène, sur l'objet <SwiftyCam_GUI>
 * et paramétré pour obtenir le fonctionnement souhaité :
 * 
 * Paramètres disponibles :
 * X
 *
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Swifty;
//using DB;

public class SwiftyCam_GUI : MonoBehaviour {
/*
	// ### PROPERTIES ####################################################################
	
	// Accessibles dans l'inspecteur

	// Accessibles par autres scripts
		public	Camera 			CameraPrincipale{get;set;}
		public	bool 			ActiverInterfaces{get;set;}
		public	Dictionary<int,Player_GUI>
								PlayerGUIscripts = new Dictionary<int,Player_GUI>();
		public	Dictionary
								<string,string>	updatePublics = new Dictionary<string,string>();
		public	int				guiID{get;set;}
	
	// Privées
		private	int				maxPlayers=8;
		private	bool			OuvertureAutomatiqueDesInterfaces = false;
		private	bool 			ActiverLeCollider = false;
		private	bool 			ActiverLeBoutonAction = false;
		private	bool			ActiverLeCompteur = false;
		private	bool			ActiverLeJoystick=false;
		private	bool			ActiverLaJauge=false;
		private	bool			ActiverLaJauge_Icone=false;
		private	float 			ProfondeurDesInterfaces = 4.0f;
		private	Rect 			TexturePosition;
		private	Vector3 		pos;
		private	Vector2 		GUI_pivot;
		private	Vector3 		GUIspace;
		private	Player_GUI 		newUIScript = null;
		private	float			delayBeforeStartingGame;
		private	Navette			nScript;
		private	GestionDesAcces	gestion;


	// ### INITIALISATION ################################################################
	void Awake() {
		
		// 0) Masque le curseur
		if(!Application.isEditor)
			Screen.showCursor = false;
		
		// 1) Obtention de la camera utilisée
		CameraPrincipale = CommonSwifty.GetCamera("SwiftyCam_GUI", "GUIcam", false);
		
		// 2) Configuration des elements de la scène à partir du fichier XML du jeu.
		foreach( var up in updatePublics) {
			if(up.Key == "ActiverInterfaces")					ActiverInterfaces = bool.Parse(up.Value);
			if(up.Key == "OuvertureAutomatiqueDesInterfaces")	OuvertureAutomatiqueDesInterfaces = bool.Parse(up.Value);
			if(up.Key == "ActiverLeCollider")					ActiverLeCollider = bool.Parse(up.Value);
			if(up.Key == "ActiverLeBoutonAction")				ActiverLeBoutonAction = bool.Parse(up.Value);
			if(up.Key == "ActiverLeCompteur")					ActiverLeCompteur = bool.Parse(up.Value);
			if(up.Key == "ActiverLeJoystick")					ActiverLeJoystick = bool.Parse(up.Value);
			if(up.Key == "ActiverLaJauge")						ActiverLaJauge = bool.Parse(up.Value);
			if(up.Key == "ActiverLaJauge_Icone")				ActiverLaJauge_Icone = bool.Parse(up.Value);
			if(up.Key == "ProfondeurDesInterfaces")				ProfondeurDesInterfaces = float.Parse(up.Value);
		}
		
		// 3) Force ou Bloque l'affichage des GUI suivant le mode de simulation choisi sur </Access> dans <Swifty_Menu_Games>
		GameObject gestionGame = GameObject.Find("/Access");
		if(gestionGame != null) {
			gestion = gestionGame.GetComponent<GestionDesAcces>();
			if(gestion!=null) {
				switch( gestion.getSimulationMode() ) {
					case "Carrousel" :	ActiverInterfaces = false;	break;
					case "Access" :		ActiverInterfaces = true;	break;
				}
			}
		}
		
		// 4) Obtention du script de la Navette
		GameObject navette = GameObject.Find("/Navette");
		if(navette != null)
			nScript = navette.GetComponent<Navette>();
			
		// 5) Creation des GUI
		if(ActiverInterfaces)
			createGUIs();
	}
	
	// ### AFFICHAGE DU DEBUG ############################################################
	void OnGUI() {
		if(Common.debugOutput.Count>0) {
			int incr = 0;
			GUI.Label(new Rect(10, 0, 1000, 25), "=== Informations de debug ===");
			foreach(var debug in Common.debugOutput) {
				if(debug.Key<Common.debugOutput.Count-51)
					continue;
				float deb_time = debug.Key;
				string deb_tex = debug.Value;
				GUI.Label(new Rect(10, 16+16*incr, 1000, 25), "["+deb_time+"] "+deb_tex);
				incr++;
			}
		}
	}

	// ### 1ST FRAME #####################################################################
	void Start() {
		
		// 2) Mise à jour des avatars
		nScript.updateAvatars();
		
		// Test d'obtention du ProductID de l'OS
		//Common.GetProductID();
	}
	
	
	// ### UPDATE ########################################################################
	//void Update() {}
	
	
	// ### TOOLS ########################################################################
	
	// Création des GUI
	public void createGUIs() {
		
		// Ne fait pas poper les coffres dans la partie Admin
		if(Application.loadedLevelName!="Swifty_Menu_Boot") {
			
			// Boucle d'instanciation des [Player_GUI]
			for(int pq=1; pq<=maxPlayers; pq++) {
				
				// 0) Vérification : Faut t-il créer ce Player_GUI ?
				if( !CommonSwifty.IsPlayerGuiIsUsed(pq) && nScript.get_GUIAuth_BasedOn_AccessMode() ) {
					continue;
				}
				
				// 1) Positionnement
				float posX=0;
				float posZ=0;
				float posR=0;
				switch(pq) {
					case 1 :	posX=0.3050f;	posZ=0.0000f;	posR=0.000f;	break;
					case 2 :	posX=0.7000f;	posZ=0.0000f;	posR=0.000f;	break;
					case 3 :	posX=1.0000f;	posZ=0.2550f;	posR=270.0f;	break;
					case 4 :	posX=1.0000f;	posZ=0.7550f;	posR=270.0f;	break;
					case 5 :	posX=0.6950f;	posZ=1.0000f;	posR=180.0f;	break;
					case 6 :	posX=0.3000f;	posZ=1.0000f;	posR=180.0f;	break;
					case 7 :	posX=0.0000f;	posZ=0.7400f;	posR=90.00f;	break;
					case 8 :	posX=0.0000f;	posZ=0.2400f;	posR=90.00f;	break;
				}
				Vector3 newGUIposition = CameraPrincipale.ViewportToWorldPoint( new Vector3 (posX,posZ, 0) );
				Quaternion quat = Quaternion.Euler(0,posR,0);
				
				// 2) Instanciation, Taille, Nommage, Layer, Hauteur
				GameObject newUI = Instantiate(Resources.Load("Player_GUI"), newGUIposition, quat) as GameObject;
				newUI.transform.localPosition = Common.EditAxisV3(newUI.transform.localPosition, "y", ProfondeurDesInterfaces);
				newUI.transform.localScale = new Vector3(5,5,5);
				newUI.layer = 8;
				
				// 3) Recuperation & Stockage du script GUI instancié
				newUIScript = newUI.GetComponent<Player_GUI>();
				PlayerGUIscripts.Add(pq-1, newUIScript);
			
				// 4) Activation / Desactivation des éléments du GUI, au lancement
				if(ActiverLeCollider)			newUIScript.guiCollider = true;
				if(ActiverLeBoutonAction)		newUIScript.GUI_Button = true;
				if(ActiverLeCompteur)			newUIScript.GUI_Compteur = true;
				if(ActiverLeJoystick)			newUIScript.GUI_Joystick = true;
				if(ActiverLaJauge)				newUIScript.GUI_Jauge = true;
				if(ActiverLaJauge_Icone)		newUIScript.GUI_JaugeIcon = true;
				
				// 5) Score initial
				newUIScript.guiScore = 0;
				
				// 6) Renseigne le nom du joueur
				newUIScript.playerName = "";
				
				// 7) Ouvre les coffres automatiquement
				int accessMode = nScript.getAccessMode();
				bool isThereMonnayeur = (gestion!=null ? (accessMode==2 || accessMode==3 ? true : false) : false);
				newUIScript.tableMode=nScript.tableMode;
				if(OuvertureAutomatiqueDesInterfaces == true)
					newUIScript.actionChest("open", (accessMode==1 ? true : false), "", isThereMonnayeur, ActiverLeCollider);
				else
					newUIScript.actionChest("close", false, "", isThereMonnayeur, ActiverLeCollider);
				
				// 8) EDITOR : Enregistre la liste des players dans la navette
				if(Application.isEditor && CommonSwifty.IsGame()) {
					nScript.AddPlayer(pq, "", "", 0);
				}
			}
			
			// 9) Masque les GUI si mode veille
			if(gestion!=null && gestion.hibernating && gestion.modeDeSimulation.ToString()=="Aucun") {
				hideAllChest();
			}
			
		}
	}
	
	// Masquer tous les GUI
	public void hideAllChest() {
		foreach(var p in PlayerGUIscripts) {
			if(p.Value!=null)
				((Player_GUI)p.Value).actionChest("hide", false, "", false);
		}
	}
	
	// Réafficher tous les GUI
	public void showAllChest(string anim="show") {
		foreach(var p in PlayerGUIscripts) {
			if(p.Value!=null)
				((Player_GUI)p.Value).actionChest(anim, false, "", false);
		}
	}
	
	// Appel de fondu
	public void fadeEffect(string type="fadeIn", bool shouldRewind=true, GameObject gameObjectCalledAtEnd=null, string functionCalledAtEnd="") {
		GameObject fondu;
		FonduAuBlanc fonduScript;
		// S'il n'existe pas encore de fondu au blanc
		if(GameObject.Find("/SwiftyCam_GUI/FonduAuBlanc")==null) {
			// On le crée
			fondu = Instantiate(Resources.Load("FonduAuBlanc"), Vector3.zero, Quaternion.Euler(0,0,0) ) as GameObject;
			fondu.name="FonduAuBlanc";
			fondu.transform.parent = this.transform;
			fondu.transform.localPosition = -1*Vector3.up;
			fondu.transform.localEulerAngles = Vector3.zero;
			fonduScript = fondu.GetComponent<FonduAuBlanc>();
		}
		// S'il existe
		else {
			// On l'utilise
			fondu = GameObject.Find("/SwiftyCam_GUI/FonduAuBlanc");
			fonduScript = fondu.GetComponent<FonduAuBlanc>();
		}
		// On configure les actions de fin de fondu
		fonduScript.gameObjectCalledAtEnd = gameObjectCalledAtEnd;
		fonduScript.functionCalledAtEnd = functionCalledAtEnd;
		// On le lance dans le mode souhaité
		switch(type) {
			case "fadeIn" :		fondu.GetComponent<FonduAuBlanc>().fadeIn(shouldRewind);		break;
			case "fadeOut" :	fondu.GetComponent<FonduAuBlanc>().fadeOut(shouldRewind);		break;
		}
	}
*/
}