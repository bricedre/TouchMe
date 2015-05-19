using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Swifty;

public class SwiftyCam_WORLD : MonoBehaviour {
	
	// ### PROPERTIES ####################################################################
	
	// ACCESSIBLES
	public	ArrayList		listOfParallaxObjects = new ArrayList();
	public	enum			movementList{noMove, circle, randomCircle};
	public	movementList	mouvementDeCamera;
	
	// NON-ACCESSIBLES
	private	int				vitesseDuRoulis = 50;
	private	bool			_utiliserLeBarycentreEtZoom = false;
	private	GameObject		ObjetContenantLaListeDesAvatars;
	private	ArrayList		_avatarList = new ArrayList();
	/*propriétés du mouvement circulaire de camera*/
	private	float			_rayonDuRoulis = 1.5f;
	private	Quaternion 		circle_rotation;
	private	Vector3			circle_radius;
	private	float			circle_currentRotation = 0.0f;
	/*propriétés du mouvement circulaire aléatoire de camera*/
	private	float			aleaRoll_end = 0.0f;
	private	float			aleaRoll_p = 1.0f;
	private	float			aleaRoll_pp = 1.0f;
	private	float			aleaRoll_r = 1.0f;
	// Ecarts maximums utilisés en X et en Z pour le barycentre
	private	float			_ecartXMin = 40; // 40
	private	float			_ecartXMax = 90; // 70
	private	float			_ecartZMax = 60; // 40
	private	float			defaultCamY;
	private	Dictionary
							<GameObject,float>		defaultParallaxScale = new Dictionary<GameObject,float>();
	// Ecarts effectifs
	private	float			_ecartX;
	private	float			_ecartZ;
	// Zoom max de la caméra
	private	float			_zoomMax = 40f;
	private	float			_zoomMin;
	private	float			prevPercentX;
	private	float			prevPercentZ;
	public	Dictionary
							<string,string>		updatePublics = new Dictionary<string,string>();
	
	
	// ### INITIALISATION ################################################################
	void Awake() {
		
		// Configuration des elements de la scène à partir du fichier XML du jeu.
		foreach( var up in updatePublics) {
			if(up.Key == "mouvementDeCamera") {
				switch(up.Value){
					case "noMove":			mouvementDeCamera = movementList.noMove;		break;
					case "circle":			mouvementDeCamera = movementList.circle;		break;
					case "randomCircle":	mouvementDeCamera = movementList.randomCircle;	break;
				}
			}
			if(up.Key == "vitesseDuRoulis")						vitesseDuRoulis = int.Parse(up.Value);
			if(up.Key == "rayonDuRoulis")						rayonDuRoulis = float.Parse(up.Value);
			if(up.Key == "ObjetContenantLaListeDesAvatars")		ObjetContenantLaListeDesAvatars = GameObject.Find("/"+up.Value) as GameObject;
			if(up.Key == "utiliserLeBarycentreEtZoom")			_utiliserLeBarycentreEtZoom = bool.Parse(up.Value);
			if(up.Key == "objectsEnParallaxSurCameraWorld")		listOfParallaxObjects = Common.String2ObjectsList(up.Value, ";");
		}
		
		// Initialise le roulis
		refreshRoll();
		
		// Stock la position originelle de la camera et des objets en parallax
		defaultCamY = this.transform.position.y;
		if( listOfParallaxObjects.Count > 0 )
			foreach(GameObject parallax in listOfParallaxObjects)
				if(parallax!=null)
					defaultParallaxScale[parallax] = (float)parallax.transform.localScale.x;
	}
	
	
	// ### 1ST FRAME #####################################################################
	void Start() {
		
		// Si le conteneur de la liste des avatars est renseigné...
		if(ObjetContenantLaListeDesAvatars!=null) {
			// ...on s'en sert pour obtenir la liste des avatars
			ObjetContenantLaListeDesAvatars.SendMessage("setAvatarList");
			_zoomMin = this.transform.position.y;
		}
	}
	
	
	// ### UPDATE #######################################################################
	void Update() {
		
		// ROULIS simple (circle)
		if(mouvementDeCamera == movementList.circle){
			// position de la camera
			circle_currentRotation += Time.deltaTime*vitesseDuRoulis;
			circle_rotation.eulerAngles = new Vector3(0.0f, circle_currentRotation, 0.0f);
			this.transform.position = circle_rotation * circle_radius;
			
			// cible de la camera : 0,0,0
			this.transform.rotation = Quaternion.Euler(
													this.transform.position.z * 1.0f,
													0.0f,
													this.transform.position.x * -1.0f
												);
		}
		
		// ROULIS aléatoire (randomCircle) // WIP
		/*if(mouvementDeCamera == movementList.randomCircle){
			// roulis aleatoire
			rayonDuRoulis = getRandomRoll();
			// position de la camera
			circle_currentRotation += Time.deltaTime*vitesseDuRoulis*rayonDuRoulis;
			circle_rotation.eulerAngles = new Vector3(0.0f, circle_currentRotation, 0.0f);
			this.transform.position = circle_rotation * circle_radius;
			
			// cible de la camera : 0,0,0
			this.transform.rotation = Quaternion.Euler(
													this.transform.position.z * 1.0f,
													0.0f,
													this.transform.position.x * -1.0f
												);
		}*/
		
		// Utilisation du barycentre
		if(_utiliserLeBarycentreEtZoom){
			if(ObjetContenantLaListeDesAvatars==null){
				Debug.Log("-------> /!\\ Attention ! Pour utiliser le zoom de la camera, il faut renseigner le gameObject dans l'inspecteur !");
			}else {
				if( _avatarList.Count > 0) {
					CalculateEcart();
					SetPositionCameraToBarycentre();
				}
			}
		}
	}
	
	// Permet de position la caméra au barycentre des positions des avatars
	private	void SetPositionCameraToBarycentre() {
		
		float x = 0;
		float y = this.transform.position.y;
		float z = 0;
		
		// On ajoute tous les x et les z
		foreach(GameObject g in _avatarList) {
			/*x += Common.GetChildrenByName(g, "avatar").transform.position.x;
			z += Common.GetChildrenByName(g, "avatar").transform.position.z;*/
			x += g.transform.position.x;
			z += g.transform.position.z;
		}
		
		// On fait la moyenne
		x =  x / _avatarList.Count;
		z =  z / _avatarList.Count;
		
		// On détermine le % de l'écart par rapport a l'écart max
		float percentX = 100.0f - ( ( _ecartX /  ( _ecartXMax - _ecartXMin ) ) * 100.0f);
		float percentZ = 100.0f - ( ( _ecartZ / _ecartZMax ) * 100.0f);
		
		// Si les pourcentages sont négatifs, on les bloque a 0 %
		if(percentX < 0.0f)		percentX = 0.0f;
		if(percentZ < 0.0f)		percentZ = 0.0f;
		
		// On prend le pourcentage le plus bas pour placer la caméra 
		if(percentX < percentZ){
			if(mouvementDeCamera == movementList.noMove)
				this.transform.position =  new Vector3(  ( x * percentX ) / 100.0f, y, ( z * percentX ) / 100.0f);
			else
				this.transform.position +=  new Vector3(  ( x * percentX ) / 100.0f, y, ( z * percentX ) / 100.0f);
		}
		else {
			if(mouvementDeCamera == movementList.noMove)
				this.transform.position =  new Vector3(  ( x * percentZ ) / 100.0f, y, ( z * percentZ ) / 100.0f);
			else
				this.transform.position +=  new Vector3(  ( x * percentZ ) / 100.0f, y, ( z * percentZ ) / 100.0f);
		}
		
		// Refresh zoom
		if(prevPercentX != percentX || prevPercentZ != percentZ)
			Zoom (percentX, percentZ);
		prevPercentX = percentX;
		prevPercentZ = percentZ;
		
	}
	
	// Mets à jour le zoom de la caméra
	public void Zoom(float percentX, float percentZ) {	
		// On prend le pourcentage le plus bas pour effectuer le zoom
		if(percentX < percentZ)
			this.transform.position =  new Vector3(  this.transform.position.x , _zoomMin - ( ( _zoomMax * percentX ) / 100.0f), this.transform.position.z);
		else
			this.transform.position =  new Vector3(  this.transform.position.x , _zoomMin - ( ( _zoomMax * percentZ ) / 100.0f), this.transform.position.z);
		// Application du barycentre aux parallax
		foreach(GameObject parallax in listOfParallaxObjects) {
			float pScale = defaultParallaxScale[parallax] / this.transform.position.y * defaultCamY;
			//pScale = pScale / 10;
			parallax.transform.localScale = new Vector3(pScale, pScale, pScale);
		}
	}	
	
	// Calcul d'écart (?)
	private	void CalculateEcart() {
		float xMin = 0;
		float xMax = 0;
		
		float zMin = 0;
		float zMax = 0;
		
		// On définit les minimums a la position du premier avatar
		/*xMin = Common.GetChildrenByName((GameObject)_avatarList[0], "avatar").transform.position.x;
		zMin = Common.GetChildrenByName((GameObject)_avatarList[0], "avatar").transform.position.z;
		zMax = Common.GetChildrenByName((GameObject)_avatarList[0], "avatar").transform.position.z;*/
		xMin = ((GameObject)_avatarList[0]).transform.position.x;
		zMin = ((GameObject)_avatarList[0]).transform.position.z;
		zMax = ((GameObject)_avatarList[0]).transform.position.z;
		
		foreach(GameObject g in _avatarList) {
			/*if(Common.GetChildrenByName(g, "avatar").transform.position.x < xMin)	xMin = Common.GetChildrenByName(g, "avatar").transform.position.x;
			if(Common.GetChildrenByName(g, "avatar").transform.position.x > xMax)	xMax = Common.GetChildrenByName(g, "avatar").transform.position.x;
			if(Common.GetChildrenByName(g, "avatar").transform.position.z < zMin)	zMin = Common.GetChildrenByName(g, "avatar").transform.position.z;
			if(Common.GetChildrenByName(g, "avatar").transform.position.z > zMax)	zMax = Common.GetChildrenByName(g, "avatar").transform.position.z;*/
			if(g.transform.position.x < xMin)	xMin = g.transform.position.x;
			if(g.transform.position.x > xMax)	xMax = g.transform.position.x;
			if(g.transform.position.z < zMin)	zMin = g.transform.position.z;
			if(g.transform.position.z > zMax)	zMax = g.transform.position.z;
		}
		
		_ecartX = xMax - xMin;
		_ecartX = Mathf.Abs(_ecartX);
		
		_ecartX = _ecartX - _ecartXMin;
		if(_ecartX < 0.0f)
			_ecartX = 0.0f;
		
		_ecartZ = zMax - zMin;
		_ecartZ = Mathf.Abs(_ecartZ);
	}
	
	// ### TOOLS ########################################################################
	
	// Genere un roulis aleatoire
	public float getRandomRoll(){
		float res;
		// si P seconde, random des R
		if(Time.timeSinceLevelLoad >= aleaRoll_end){
			aleaRoll_pp = aleaRoll_p;//durée precedente
			aleaRoll_p = Random.Range(1.0f, 5.0f);//durée du cycle
			aleaRoll_r = 10.0f;//radius du roulis (amplitude)
			aleaRoll_end = Time.timeSinceLevelLoad + aleaRoll_p;
		}
		// Retourne le roll à l'instant T
		res = aleaRoll_r*Mathf.Sin(aleaRoll_p * Time.timeSinceLevelLoad + Mathf.Abs( (Time.timeSinceLevelLoad/aleaRoll_pp) - (Time.timeSinceLevelLoad/aleaRoll_p) ) );
		//Debug.Log(res);
		return res;
	}
	
	// rafraichit le radius du roulis
	public void refreshRoll() {
		circle_radius= new Vector3(rayonDuRoulis, this.transform.position.y, rayonDuRoulis);
	}
	
	
	// ### GET / SET ######################################################################
	
	public float rayonDuRoulis {
		get { return _rayonDuRoulis; }
		set {
				_rayonDuRoulis = value;
				refreshRoll();
			}
	}
	
	public ArrayList avatarList {
		get { return _avatarList; }
		set {
				_avatarList = value;
		}
	}
	
}