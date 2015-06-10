using UnityEngine;
using System.Collections;

public class TickleScript: MonoBehaviour {
	
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
	private RaycastHit hit;
	private Ray ray;
	
	void Start () {

		value = threshold;
		warningDone = false;
		sentenceSaid = false;

		animBody = GameObject.Find ("ToyBody").GetComponent<Animator>();
		animHead = GameObject.Find ("ToyHead").GetComponent<Animator>();
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager>();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
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

				//Booleans are reinitialised
				if(isHead){
					animHead.SetBool(parameter, false);
				}
				
				else{
					animBody.SetBool(parameter, false);
				}

				//If it's regenerative, a new one is done
				if(regenerative){
					GameObject newTickleZone = Instantiate(this, transform.position, Quaternion.identity) as GameObject;
				}

				//it is destroyed 
				GameObject.Destroy(this.gameObject);
			}

		}

		else if(value < touchyTrigger && value > touchyTrigger - 2.0f && warningDone == false){
			if(warning != "")
				if(!gameManager.speaking)
					audioManager.playEvent(warning);
			warningDone = true;
			audioManager.wait (3);
		}

		else {
			//Detects MousePointer
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Input.GetMouseButton(0)){

				//If it hits this instance, it decreases threshold
				if (Physics.Raycast(ray, out hit)){
					if(hit.collider.gameObject == this.gameObject && !gameManager.speaking){

						//decreases value
						value -= 0.1f;

						//Changing animation parameters
						if(isHead){
							animHead.SetBool(parameter, true);
						}

						else{
							animBody.SetBool(parameter, true);
						}
					}
				}
			}
		}
	}
}