using UnityEngine;
using System.Collections;

public class DialogsManager : MonoBehaviour {

	private AudioManager audioManager;
	private GameManager gameManager;

	public TextMesh answer1;
	public TextMesh answer2;
	public TextMesh answer3;
	public GameObject answer1Trigger;
	public GameObject answer2Trigger;
	public GameObject answer3Trigger;
	public float timer;

	public int currentDialog = 0;
	public GameObject[] dialogs;
	public int dialogStatus = 0;

	public int scenarioProgress;


	public bool askingCue;
	public bool speaking;
	public int answerChoice;
	public bool answered;


	void Start () {

		//Get actuators
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		try{
			answer1 = GameObject.Find("answer1").GetComponent<TextMesh>();
			answer2 = GameObject.Find("answer2").GetComponent<TextMesh>();
			answer3 = GameObject.Find("answer3").GetComponent<TextMesh>();
			answer1Trigger = GameObject.Find("answer1Trigger");
			answer2Trigger = GameObject.Find("answer2Trigger");
			answer3Trigger = GameObject.Find("answer3Trigger");
		}
		catch {}

		answer1.color = new Color(answer1.color.r, answer1.color.g, answer1.color.b, 0.0f);
		answer2.color = new Color(answer2.color.r, answer2.color.g, answer2.color.b, 0.0f);
		answer3.color = new Color(answer3.color.r, answer3.color.g, answer3.color.b, 0.0f);
		
		timer = 0.0f;

	}

	void Update () {

		speaking = gameManager.speaking;

		//If Character is askingCue something
		if(askingCue){

			// START OF THE DIALOG => Character Cue
			if(dialogStatus == 0){

				//Triggering first cue audio
				audioManager.playEvent(dialogs[currentDialog].GetComponent<DialogScript>().firstCue);

				// Go on with the dialog
				dialogStatus = 1;
			}

			// THEN THE PLAYER ANSWERS => Players Choices
			else if(dialogStatus == 1){

				//Fading bubbles in
				answer1.color = new Color(answer1.color.r, answer1.color.g, answer1.color.b, Mathf.Lerp(answer1.color.a, 1f, 0.02f));
				answer2.color = new Color(answer2.color.r, answer2.color.g, answer2.color.b, Mathf.Lerp(answer2.color.a, 1f, 0.02f));
				answer3.color = new Color(answer3.color.r, answer3.color.g, answer3.color.b, Mathf.Lerp(answer3.color.a, 1f, 0.02f));

				//Show possible answers
				answer1.text = dialogs[currentDialog].GetComponent<DialogScript>().playerAnswers[0];
				answer2.text = dialogs[currentDialog].GetComponent<DialogScript>().playerAnswers[1];
				answer3.text = dialogs[currentDialog].GetComponent<DialogScript>().playerAnswers[2];

				//Activate triggers for answering on HUD
				answer1Trigger.gameObject.SetActive(true);
				answer2Trigger.gameObject.SetActive(true);
				answer3Trigger.gameObject.SetActive(true);

				//Wait for cue to end
				timer--;

				//If the players chooses one option or auto-answering is on
				if(answered || dialogs[currentDialog].GetComponent<DialogScript>().autoAnswering)
				{
					dialogStatus = 2;
				}

			}

			else if(dialogStatus == 2){

				//Triggering last cue audio
				if(dialogs[currentDialog].GetComponent<DialogScript>().characterAnswers[answerChoice] != "")
					audioManager.playEvent(dialogs[currentDialog].GetComponent<DialogScript>().characterAnswers[answerChoice]);

				//Modifying character happiness accordingly
				gameManager.pointsPool += dialogs[currentDialog].GetComponent<DialogScript>().answersValues[answerChoice];

				//Make bubbles pop out
				answer1Trigger.GetComponent<Animator>().SetTrigger("ComeOut");
				answer2Trigger.GetComponent<Animator>().SetTrigger("ComeOut");
				answer3Trigger.GetComponent<Animator>().SetTrigger("ComeOut");

				//Start Timer for listening response
				timer = 150f; //dialogs [currentDialog].GetComponent<DialogScript> ().cuesDuration;

				//Going on with the dialog
				dialogStatus = 3;
			}

			// Last bit, reinitializing stuff, waiting for cue to end
			else if(dialogStatus == 3){

				timer--;

				//Fading bubbles away
				answer1.color = new Color(answer1.color.r, answer1.color.g, answer1.color.b, Mathf.Lerp(answer1.color.a, 0.0f, 0.1f));
				answer2.color = new Color(answer2.color.r, answer2.color.g, answer2.color.b, Mathf.Lerp(answer2.color.a, 0.0f, 0.1f));
				answer3.color = new Color(answer3.color.r, answer3.color.g, answer3.color.b, Mathf.Lerp(answer3.color.a, 0.0f, 0.1f));

				if(timer < 0.0f){
					//Going back to normal
					askingCue = false;
					gameManager.speaking = false; 
					answered = false;
					dialogStatus = 0;
					answerChoice = 0;
				}
						
			}

		}

		//Nothing ... !
		else{
			answer1Trigger.gameObject.SetActive(false);
			answer2Trigger.gameObject.SetActive(false);
			answer3Trigger.gameObject.SetActive(false);
			
		}
		
	}

	public void startDialog(int dialog){

		currentDialog = dialog;
		askingCue = true;
		gameManager.speaking = true;
		timer = dialogs [currentDialog].GetComponent<DialogScript> ().cuesDuration;

	}


}
