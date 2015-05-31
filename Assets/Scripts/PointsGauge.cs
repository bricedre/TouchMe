using UnityEngine;
using System.Collections;

public class PointsGauge : MonoBehaviour {

	public float min;
	public float max;
	public Animator anim;
	public float frame;

	private float sizeX;
	private float sizeY;
	public float magnitude;
	public float vitesse;
	
	void Start () {

		anim = gameObject.GetComponent<Animator> ();
		min = GameObject.Find ("GameManager").GetComponent<GameManager> ().minHappiness;
		max = GameObject.Find ("GameManager").GetComponent<GameManager> ().maxHappiness;

		sizeX = transform.localScale.x;
		sizeY = transform.localScale.y;
	}

	void Update () {

		//Oscillating if having points
		if(GameObject.Find("GameManager").GetComponent<GameManager>().pointsPool != 0.0f){
			transform.localScale = new Vector3(sizeX + Mathf.Sin(Time.time * vitesse) * magnitude,
			                                   sizeY + Mathf.Sin(Time.time * vitesse) * magnitude,
			                                   transform.localScale.z);
		}

		//Calculate Value
		frame = GameObject.Find("GameManager").GetComponent<GameManager>().happiness / max;
		
		//Show right frame
		anim.Play("displacement", -1, frame);
	
	}
}
