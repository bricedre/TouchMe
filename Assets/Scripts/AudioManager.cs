using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	uint mainBankID;
	uint arietteBankID;

	void Start () {

		AkSoundEngine.LoadBank ("Ariette", AkSoundEngine.AK_DEFAULT_POOL_ID, out arietteBankID);
		AkSoundEngine.LoadBank ("Main", AkSoundEngine.AK_DEFAULT_POOL_ID, out mainBankID);
	
	}

	//play a specific event
	public void playEvent(string eventName){

		AkSoundEngine.PostEvent (eventName, gameObject);

	}

	//stop a specific event
	public void stopEvent(string eventName, int fadeout){

		uint eventID;
		eventID = AkSoundEngine.GetIDFromString (eventName);
		AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, fadeout * 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
		
	}
}
