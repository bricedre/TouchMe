using UnityEngine;
using System.Collections;

public class DialogsManager : MonoBehaviour {

	private AudioManager audioManager;
	private GameManager gameManager;

	public TextMesh answer1;
	public TextMesh answer2;
	public TextMesh answer3;

	public int currentDialog = 0;
	public GameObject[] dialogs;
	public int dialogStatus = 0;

	public int scenarioProgress;

	public bool asking;
	public int answerChoice;
	public bool answered;



	void Start () {

		//Get actuators
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		answer1 = GameObject.Find("answer1").GetComponent<TextMesh>();
		answer2 = GameObject.Find("answer2").GetComponent<TextMesh>();
		answer3 = GameObject.Find("answer3").GetComponent<TextMesh>();

		// Dialogue d'introduction ("vous pouvez me masser les hanches") : currentDialog = 0;
		//asking = true;

	}

	void Update () {

		//Narrative advancement
		if((int)Time.time % 30 == 29 && !asking){
			
			currentDialog = (int)Random.Range(0, 11);
			asking = true;
			
		}

		//If Character is asking something
		if(asking){

			// START OF THE DIALOG => Character Cue
			if(dialogStatus == 0){

				//Triggering first cue audio
				audioManager.playEvent(dialogs[currentDialog].GetComponent<DialogScript>().firstCue);

				// Go on with the dialog
				dialogStatus = 1;
			}

			// THEN THE PLAYER ANSWERS => Players Choices
			else if(dialogStatus == 1){

				//Show possible answers
				answer1.text = dialogs[currentDialog].GetComponent<DialogScript>().playerAnswers[0];
				answer2.text = dialogs[currentDialog].GetComponent<DialogScript>().playerAnswers[1];
				answer3.text = dialogs[currentDialog].GetComponent<DialogScript>().playerAnswers[2];

				//Activate triggers for answering on HUD
				answer1.gameObject.SetActive(true);
				answer2.gameObject.SetActive(true);
				answer3.gameObject.SetActive(true);
		

				if(answered || dialogs[currentDialog].GetComponent<DialogScript>().autoAnswering)
					dialogStatus = 2;

			}

			// FINALLY THE CHARACTER RESPONDS ACCORDINGLY
			else if(dialogStatus == 2){
				
				//Triggering last cue audio
				if(dialogs[currentDialog].GetComponent<DialogScript>().characterAnswers[answerChoice] != "")
					audioManager.playEvent(dialogs[currentDialog].GetComponent<DialogScript>().characterAnswers[answerChoice]);

				//Modifying character happiness
				gameManager.pointsPool += dialogs[currentDialog].GetComponent<DialogScript>().answersValues[answerChoice];

				//Going back to normal
				asking = false;
				answered = false;
				dialogStatus = 0;
				answerChoice = 0;
						
			}

		}

		//Nothing ... !
		else{

			answer1.gameObject.SetActive(false);
			answer2.gameObject.SetActive(false);
			answer3.gameObject.SetActive(false);

		}
		
	}


}
