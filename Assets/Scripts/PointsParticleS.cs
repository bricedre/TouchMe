using UnityEngine;
using System.Collections;

public class PointsParticleS : MonoBehaviour {

	public Transform destination;
	public int timer;

	void Setup (){
		destination = GameObject.Find ("PointsTarget").transform;

	}

	void Update () {

		timer--;

		transform.position = Vector3.Lerp (transform.position, destination.position, 0.02f);

		if(timer < 0)
		{
			Destroy(this.gameObject);
		}
	}
}
