using UnityEngine;
using System.Collections;

public class PatternScript : MonoBehaviour {

	public int turnsLeft; //nb of turns left to complete
	private int turnsMax;
	private int steps;
	public int currentStep;
	public GameObject marker;
	public int errors;
	public float points;
	private AudioManager audioManager;
	public GameObject particleGuide;
	public int target = 0;
	public float speed;
	public float minDistance;

	// get nb of steps
	void Start () {
		currentStep = 0;
		steps = transform.childCount-1;
		turnsMax = turnsLeft;

		audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		particleGuide = transform.FindChild ("GuideParticles").gameObject;
	}

	void Update () {

		//If it's the first turn
		if(turnsLeft == turnsMax){

			target = GuideThePlayer(target);
		}
		else{
			particleGuide.SetActive(false);
		}

		//When a turn is completed
		if(this.currentStep == this.steps){
			this.turnsLeft --;
			this.currentStep = 0;
			Debug.Log ("Pattern Validated");

			//creating particules because it's beautiful
			GameObject part = Instantiate(marker, transform.position, transform.rotation) as GameObject;

			//Modifying character happiness
			GameObject.Find ("GameManager").GetComponent<GameManager>().pointsPool += points;

			//Triggering audio response
			//audioManager.playEvent("positifs");

		}

		// If numbers of turnsLeft is done
		if(this.turnsLeft == 0){
			Destroy(this.gameObject);
			Debug.Log ("Pattern End");

			if(errors < 5){
				GameObject.Find ("GameManager").GetComponent<GameManager>().pointsPool += points*2;
			}
			else{}
		}
	}

	public int GuideThePlayer(int target){

		particleGuide.transform.position = Vector3.Lerp(particleGuide.transform.position, transform.GetChild(target).transform.position, speed);

		if(Vector3.Distance(particleGuide.transform.position, transform.GetChild(target).transform.position) < minDistance)
			target = (target + 1) % (transform.childCount-1);

		return target;
	}
}
