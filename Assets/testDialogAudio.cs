using UnityEngine;
using System.Collections;

public class testDialogAudio : MonoBehaviour {

	public AudioSource cue;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<AudioSource> ().Play();
		
	}

}
