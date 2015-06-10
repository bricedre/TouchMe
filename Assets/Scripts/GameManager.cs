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
	public float GAMETIMER;

	//Level Generics
	public float minHappiness;
	public float happiness;
	public float maxHappiness;
	public float flowRate;
	public int scenarioAdvancement;
	private bool scenarioPartLoaded;
	public bool speaking;

	public bool scriptedDialog1Said;
	public bool scriptedDialog2Said;
	public bool scriptedDialog3Said;

	public GameObject stretches;
	public GameObject stretchSprites;
	public GameObject tickles;
	public GameObject toySprites;
	public GameObject patternCW;


	private AudioManager audioManager;
	private DialogsManager dialogManager;
	public bool musicOn;

	void Start () {

		gameRunning = true;
		scenarioPartLoaded = false;

		hud.offPosX = hud.pauseBar.transform.position.x;
		hud.onPosX = hud.offPosX + hud.outFactor;

		//Music
		audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		dialogManager = GameObject.Find ("DialogsManager").GetComponent<DialogsManager> ();
		speaking = false;

		//Background Music
		if(musicOn)
		{
			audioManager.playEvent("music_play_01");
		}
	
	}

	void Update () {

		GAMETIMER += Time.deltaTime;

		//Game is running
		if(gameRunning) {

			//pre-phase events according to Scenario
			if(!scenarioPartLoaded){
				switch(scenarioAdvancement){
					case 0: //Phase jouet + patterns
						GAMETIMER = 0.0f;
						audioManager.playEvent("ARI_RP2");
						audioManager.wait (4);

						stretches.transform.position = new Vector3(-100.0f, 0.0f, stretches.transform.position.z);
						stretchSprites.SetActive(false);

						tickles.transform.position = new Vector3(0.0f, 0.0f, stretches.transform.position.z);
						toySprites.SetActive(true);


						scenarioPartLoaded = true;
						break;

					case 1: //travail DOS : stretches
						GAMETIMER = 0.0f;
						audioManager.playEvent("ARI_RI1");

						stretches.transform.position = new Vector3(0.0f, 0.0f, stretches.transform.position.z);
						stretchSprites.SetActive(true);
						
						tickles.transform.position = new Vector3(-100.0f, 0.0f, stretches.transform.position.z);
						toySprites.SetActive(false);
						
						scenarioPartLoaded = true;
						break;

					case 2: //travail BRAS : patterns
						GAMETIMER = 0.0f;
						//audioManager.playEvent("Play_ARI_RA_04");
						scenarioPartLoaded = true;
						break;

					case 3: //travail BRAS : stretches
						GAMETIMER = 0.0f;
						//audioManager.playEvent("Play_ARI_RA_04");
						scenarioPartLoaded = true;
						break;
				}
			}

			//In-game triggered events
			switch(scenarioAdvancement){
			case 0: //Phase jouet + patterns
				if((int)GAMETIMER < 40 && (int)GAMETIMER > 20 && !scriptedDialog1Said && !speaking){
					dialogManager.startDialog(0);
					scriptedDialog1Said = true;
				}

				if((int)GAMETIMER == 60){
					makeProgressInScenario();
				}
			
				break;
				
			case 1: //travail DOS : stretches
				if((int)GAMETIMER < 40 && (int)GAMETIMER > 20 && !scriptedDialog2Said && !speaking){
					dialogManager.startDialog(1);
					scriptedDialog2Said = true;
				}

				if((int)GAMETIMER == 10 && !scriptedDialog3Said && !speaking){
					patternCW.transform.position = new Vector3(-4.712f, 0.0f, patternCW.transform.position.z);
					audioManager.playEvent("ARI_RS2_2");
					scriptedDialog3Said = true;

				}

				break;
				
			case 2: //travail BRAS : patterns
				if((int)GAMETIMER < 40 && (int)GAMETIMER > 20 && !scriptedDialog3Said && !speaking){
					dialogManager.startDialog(2);
					scriptedDialog1Said = true;
				}
				break;
				
			case 3: //travail BRAS : stretches

				break;
			}

			//If there are points in the points pool, add them slowly to the counter
			if(pointsPool > 0.1f || pointsPool < -0.1f){
				happiness += Mathf.Sign(pointsPool)*flowRate;
				pointsPool -= Mathf.Sign(pointsPool)*flowRate;
			}

			//else if almost 0, back to zero
			else pointsPool = 0.0f;

			//Up-Limit
			if(happiness > maxHappiness){
				happiness = maxHappiness;
				pointsPool = 0.0f;
			}

			///Down-Limit
			if(happiness < minHappiness){
				happiness = minHappiness;
				pointsPool = 0.0f;
			}

			//Pause Bar is folded
			hud.pauseBar.transform.position = new Vector3(Mathf.Lerp(hud.pauseBar.transform.position.x, hud.offPosX, hud.speed),
			                                              hud.pauseBar.transform.position.y,
			                                              hud.pauseBar.transform.position.z);
		}

		//Game is in pause
		else{
			
			//Pause Bar is unfolded
			hud.pauseBar.transform.position = new Vector3(Mathf.Lerp(hud.pauseBar.transform.position.x, hud.onPosX, hud.speed),
			                                              hud.pauseBar.transform.position.y, 
			                                              hud.pauseBar.transform.position.z);
		}

	}

	public void makeProgressInScenario(){
		scenarioPartLoaded = false;
		scenarioAdvancement++;
	}
}
