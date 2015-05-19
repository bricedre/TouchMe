using UnityEngine;
using System.Collections;

//classe regroupant les informations globales utiles au jeu

[System.Serializable]
public class HUD{

	//HUD Info
	public GameObject pauseBar;
	public float onPosX;
	public float offPosX;
	public float outFactor;
	public float speed;

}

public class GameManager : MonoBehaviour {

	public HUD hud;

	//Game Generics
	public bool gameRunning;
	public float pointsPool;

	//Level Generics
	public float minHappiness;
	public float happiness;
	public float maxHappiness;
	public float flowRate;
	public int scenarioAdvancement;
	private bool scenarioPartLoaded;

	void Start () {

		gameRunning = true;
		scenarioPartLoaded = false;

		hud.offPosX = hud.pauseBar.transform.position.x;
		hud.onPosX = hud.offPosX + hud.outFactor;
	
	}

	void Update () {

		//Game is running
		if(gameRunning) {

			//Changing stuff according to Scenario
			if(!scenarioPartLoaded){
				switch(scenarioAdvancement){
					case 0: //Travail DOS
						
						scenarioPartLoaded = true;
						break;

					case 1: //travail BRAS
						
						scenarioPartLoaded = true;
						break;
				}
			}

			//If there are points in the points pool, add them slowly to the counter
			if(pointsPool > 0.1f || pointsPool < -0.1f){
				happiness += Mathf.Sign(pointsPool)*flowRate;
				pointsPool -= Mathf.Sign(pointsPool)*flowRate;
			}

			//else if almost 0, back to zero
			else pointsPool = 0.0f;

			//Up-Limit
			if(happiness > maxHappiness)
				happiness = maxHappiness;

			if(happiness < minHappiness)
				happiness = minHappiness;

			//Pause Bar is folded
			hud.pauseBar.transform.position = new Vector3(Mathf.Lerp(hud.pauseBar.transform.position.x, hud.offPosX, hud.speed),
			                                              hud.pauseBar.transform.position.y,
			                                              hud.pauseBar.transform.position.z);
		}

		//Game is in pause
		else{
			
			//PutPauseBar
			hud.pauseBar.transform.position = new Vector3(Mathf.Lerp(hud.pauseBar.transform.position.x, hud.onPosX, hud.speed),
			                                              hud.pauseBar.transform.position.y, 
			                                              hud.pauseBar.transform.position.z);
		}

	}

	void makeProgressInScenario(){
		scenarioPartLoaded = false;
		scenarioAdvancement++;
	}
}
