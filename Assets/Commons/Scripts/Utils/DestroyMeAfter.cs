/*
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * @Filename :		DestroyMeAfter.cs
 * @Copyright :		SARL Dreamtronic 2012 - 52265807900028
 * @Author :		Nicolas Lolmède
 * @Version :		2.0
 * @Description :	Supprime le GameObject après X secondes
 *
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 *
 * Ce script doit être présent une seule fois par gameObject.
 * 
 * Ce script permet par exemple de supprimer un GameObject après un nombre de secondes défini.
 * 
 */

using UnityEngine;
using System.Collections;

public class DestroyMeAfter : MonoBehaviour {

	public	float	Seconds=2.0f;
	private	float	Birth=0.0f;
	
	public void Awake() {
		Birth = Time.timeSinceLevelLoad;
	}
	
	public void Update(){
		if(Time.timeSinceLevelLoad >= Birth+Seconds)
			Destroy(this.gameObject);
	}
}