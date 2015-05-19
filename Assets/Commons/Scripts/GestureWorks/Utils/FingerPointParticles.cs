using UnityEngine;
using System.Collections;

public class FingerPointParticles : MonoBehaviour {
	
	private	float	startTrailAt = 0.2f;
	
	// Use this for initialization
	void Awake() {
		startTrailAt+=Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if(startTrailAt>0 && Time.timeSinceLevelLoad>=startTrailAt)
			this.transform.GetChild(0).GetComponent<TrailRenderer>().enabled = true;
		
		Vector3 Where = Camera.main.ViewportToWorldPoint(this.transform.position);
		this.transform.GetChild (0).position = Where + Camera.main.transform.forward * 0.5f;
	}
}