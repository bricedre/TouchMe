using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class TickleScript: TouchObject {
	
	public float threshold;
	public float value;
	public float touchyTrigger;
	public float pointsInfluence;
	public bool regenerative;
	public string parameter;
	private Animator animBody;
	private Animator animHead;
	public bool isHead;
	public string warning;
	public string sentence;

	private bool warningDone;
	private bool sentenceSaid;

	private AudioManager audioManager;
	private GameManager gameManager;
	
	void Start () {

		value = threshold;
		warningDone = false;
		sentenceSaid = false;

		animBody = GameObject.Find ("ToyBody").GetComponent<Animator>();
		animHead = GameObject.Find ("ToyHead").GetComponent<Animator>();
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager>();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		transform.parent = GameObject.Find ("TickleZones").transform;
	}

	void NDrag(GestureEvent gEvent){

		//Getting right animation
		if (gEvent.Phase == GesturePhase.GESTURE_BEGIN){
			//Changing animation parameters
			PlayAnimation(true);
		}

		if(!gameManager.speaking)
			value -= 0.1f;

		//Getting right animation
		if (gEvent.Phase == GesturePhase.GESTURE_END || gEvent.Phase == GesturePhase.GESTURE_RELEASE){
			PlayAnimation(false);
		}
	}
	
	void Update () {

		if(value < 0.0f){

			//in both cases, sentence is played
			if(sentence != "" && !gameManager.speaking && !sentenceSaid){
				audioManager.playEvent(sentence);
				audioManager.wait (5);
				sentenceSaid = true;

				//it adds points to the points' pool
				GameObject.Find("GameManager").GetComponent<GameManager>().pointsPool += pointsInfluence;

				//Stop animation
				PlayAnimation(false);

				//If it's regenerative, a new one is done
				if(regenerative)
					RegenerateTickleZone();

				else
					Destroy(this.gameObject);

			}

		}

		else if(value < touchyTrigger && !warningDone){
			if(warning != "" && !gameManager.speaking)
				audioManager.playEvent(warning);
				warningDone = true;
				audioManager.wait (3);
		}
	}

	void PlayAnimation(bool state){

		//Changing animation parameters
		if(isHead){
			animHead.SetBool(parameter, state);
		}
		
		else{
			animBody.SetBool(parameter, state);
		}
	}

	void RegenerateTickleZone(){

		value = threshold;
		warningDone = false;
		sentenceSaid = false;

	}
}