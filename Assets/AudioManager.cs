using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	uint bankID;

	// Use this for initialization
	void Start () {

		AkSoundEngine.LoadBank ("Ariette", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playEvent(string eventName){

		AkSoundEngine.PostEvent (eventName, gameObject);

	}
}
