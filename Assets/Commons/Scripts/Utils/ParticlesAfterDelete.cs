using UnityEngine;
using System.Collections;

public class ParticlesAfterDelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 Where = GameObject.Find("/SwiftyCam_GUI").camera.ViewportToWorldPoint(this.transform.position);
		burstParticle(Where);
	}
	
	public void burstParticle(Vector3 Where) {
		GameObject p = Instantiate( Resources.Load("FingerPointParticle"), Where, Quaternion.Euler(0,0,0) ) as GameObject;
		p.layer = 8;
		GameObject GO = GameObject.Find("/GestureWorks");
		if(GO!=null)
			p.transform.parent = GO.transform;
	}
	
	// Update is called once per frame
	//void Update () {}
}