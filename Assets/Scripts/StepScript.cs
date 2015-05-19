using UnityEngine;
using System.Collections;

public class StepScript : MonoBehaviour {

	private int ID;
	private PatternScript parentPattern;

	void Start () {

		// Get parent Pattern
		this.parentPattern = this.transform.parent.GetComponent<PatternScript>();

		// Get specific ID
		this.ID = (int)this.gameObject.name[0] - 48;
		//Debug.Log (this.name + " : " + this.ID);

	}

	void OnTriggerEnter(Collider other){

		if(this.ID == parentPattern.currentStep){
			parentPattern.currentStep++;
		}

		else {
			parentPattern.errors++;
		}
	
	}

	void Update () {

	}

}
