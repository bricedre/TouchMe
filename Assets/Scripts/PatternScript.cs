using UnityEngine;
using System.Collections;

public class PatternScript : MonoBehaviour {

	public int turnsLeft; //nb of turns left to complete
	private int steps;
	public int currentStep;
	public GameObject marker;
	public int errors;
	public float points;

	// get nb of steps
	void Start () {
		this.currentStep = 0;
		this.steps = transform.childCount;
		//marker = GameObject.Find ("PointsParticles");
	}

	void Update () {

		//When a turn is completed
		if(this.currentStep == this.steps){
			this.turnsLeft --;
			this.currentStep = 0;
			Debug.Log ("Pattern Validated");

			//creating particules because it's beautiful
			GameObject part = Instantiate(marker, transform.position, transform.rotation) as GameObject;

			//Modifying character happiness
			GameObject.Find ("GameManager").GetComponent<GameManager>().pointsPool += points;
		}

		// If numbers of turnsLeft is done
		if(this.turnsLeft == 0){
			Destroy(this.gameObject);
			Debug.Log ("Pattern End");
		}
	}
}
