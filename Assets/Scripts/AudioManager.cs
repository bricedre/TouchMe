using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	uint mainBankID;
	uint arietteBankID;

	void Start () {

		AkSoundEngine.LoadBank ("Ariette", AkSoundEngine.AK_DEFAULT_POOL_ID, out arietteBankID);
		AkSoundEngine.LoadBank ("Main", AkSoundEngine.AK_DEFAULT_POOL_ID, out mainBankID);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playEvent(string eventName){

		AkSoundEngine.PostEvent (eventName, gameObject);

	}
}
