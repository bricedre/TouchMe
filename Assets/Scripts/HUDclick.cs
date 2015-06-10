using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

// Classe définissant les comportements des touches d'interface

public class HUDclick : TouchObject {
	
	public int function;
	private GameManager script;
	private DialogsManager scriptDialogs;

	private void Start(){
		gameObject.SetActive (true);
		script = GameObject.Find ("GameManager").GetComponent<GameManager>();
		scriptDialogs = GameObject.Find ("DialogsManager").GetComponent<DialogsManager>();
	}
	
	private void Tap(GestureEvent gEvent){

		// PAUSE BUTTON
		if(this.function == 0){
			script.gameRunning = (script.gameRunning) ? false : true;
		}


		// DIALOG BUTTONS
		if(this.function == 1 && scriptDialogs.timer < 0.0f){
			//Valid 1st answer
			scriptDialogs.answerChoice = 0;
			scriptDialogs.answered = true;
		}
		if(this.function == 2 && scriptDialogs.timer < 0.0f){
			// Valid 2nd answer
			scriptDialogs.answerChoice = 1;
			scriptDialogs.answered = true;
		}
		if(this.function == 3 && scriptDialogs.timer < 0.0f){
			// Valid 3rd answer
			scriptDialogs.answerChoice = 2;
			scriptDialogs.answered = true;
		}

		// PAUSE BAR BUTTONS
		if(this.function == 4 && script.gameRunning){
			// Resume Game
			script.gameRunning = false;
		}
		if(this.function == 5 && script.gameRunning){
			// Options
		}
		if(this.function == 6 && script.gameRunning){
			// Exit to Menu
		}
	}
}