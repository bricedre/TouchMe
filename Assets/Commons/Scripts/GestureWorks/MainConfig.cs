/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		MainConfig.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		1.0
 * @Description :	Permet de configurer chaque scène (GestureWorks)
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * Ce script doit être présent une seule fois dans la scène, sur le gameObject <GestureWorks_Config>.
 * 
 * Paramètres disponibles :
 * - demmerdez-vous
 *
 */

using UnityEngine;
using System.Collections;

public class MainConfig : MonoBehaviour {
	
	public	string				windowNameInPlayer = "Swifty";
	public	enum				camList{SwiftyCam_GUI,SwiftyCam_WORLD};
	public	camList				cameraUsedByGestures;
	public	bool				tryAnotherCam=true;
	public	bool				showPoint=true;
	public	enum				pLayoutsList{XZ,XY};
	public	pLayoutsList		pointsLayout2D;
	public	bool				verboseMode=false;
	public	enum				frameList{PQLab,ZaagTech};
	public	frameList			typeDeCadre = frameList.PQLab;
	private	GameObject			gWorks;
	public	string				privateFilesPath="D:\\Swifty\\private\\";
	
	
	// Use this for initialization
	void Awake() {
		
		/* POST CONFIG */
		/* Autoriser la configuration de certains paramètres après export */	
		if(!Application.isEditor) {
		
			// Mode PQLab ou ZaagTech
			if( System.IO.File.Exists(privateFilesPath+"\\frame_isZaagTech") )		typeDeCadre = frameList.ZaagTech;
			else																	typeDeCadre = frameList.PQLab;
			
			// Mode Verbose
			if( System.IO.File.Exists(privateFilesPath+"\\verboseMode_enabled") )	verboseMode = true;
			else																	verboseMode = false;
		}
		
		// Récupération de l'objet GestureWorks
		gWorks = GameObject.Find("/GestureWorks");
		
		// S'il n'existe pas, on crée l'objet GestureWorks
		if(gWorks==null) {
			gWorks = Instantiate(Resources.Load("GestureWorks"), new Vector3(0,55,0), Quaternion.Euler(0,0,0) ) as GameObject;
			gWorks.layer = 9;
			gWorks.name = "GestureWorks";
			gWorks = GameObject.Find("/GestureWorks");
		}
	}

}