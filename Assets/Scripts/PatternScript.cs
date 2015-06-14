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
	public float speed = 0.15f;
	public float minDistance;
	private float emission = 40;

	// get nb of steps
	void Start () {
		currentStep = 0;
		steps = transform.childCount-1;
		turnsMax = turnsLeft;

		audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		particleGuide = transform.FindChild ("GuideParticles").gameObject;

		//emission = particleGuide.GetComponent<ParticleSystem> ().emissionRate;

		for(int i = 0; i<transform.childCount; i++){
			transform.GetChild(i).renderer.material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		}
	}

	void Update () {

		//If it's the first turn
		if(turnsLeft == turnsMax){

			target = GuideThePlayer(target);
		}
		else{
			target = GuideThePlayer(target);
			//particleGuide.SetActive(false);
		}

		//When a turn is completed
		if(this.currentStep == this.steps){
			this.turnsLeft --;
			this.currentStep = 0;
			Debug.Log ("Pattern Validated");

			//creating particules because it's beautiful
			GameObject emitter = transform.GetChild(transform.childCount-1).gameObject;
			GameObject part = Instantiate(marker, emitter.transform.position, emitter.transform.rotation) as GameObject;

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
		if (target == 0 || target == transform.childCount-2) {
			particleGuide.GetComponent<ParticleSystem>().emissionRate = 0;
		}
		else{
			particleGuide.GetComponent<ParticleSystem>().emissionRate = emission;
		}

		if(Vector3.Distance(particleGuide.transform.position, transform.GetChild(target).transform.position) < minDistance)
			target = (target + 1) % (transform.childCount-1);

		return target;
	}
}
