using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	uint mainBankID;
	uint arietteBankID;
	public float timer;
	private GameManager gameManager;

	void Start () {

		//AkSoundEngine.LoadBank ("Ariette", AkSoundEngine.AK_DEFAULT_POOL_ID, out arietteBankID);
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	
	}

	void Update(){

		if(timer > 0.0f){
			timer--;
			gameManager.speaking = true;
		}

		else
			gameManager.speaking = false;
	}

	//play a specific event
	public void playEvent(string eventName){
		//AkSoundEngine.PostEvent (eventName, gameObject);
	}

	//stop a specific event
	public void stopEvent(string eventName, int fadeout){

		uint eventID;
		//eventID = AkSoundEngine.GetIDFromString (eventName);
		//AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, fadeout * 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
		
	}

	public void wait(float sec){

		timer = sec * 61.5f;

	}
}
