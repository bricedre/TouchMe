using UnityEngine;
using System.Collections;

public class PointsGauge : MonoBehaviour {

	private float min;
	private float max;
	private float sizeX;
	private float sizeY;
	public float magnitude;
	public float vitesse;
	public float value;
	
	
	void Start () {
		
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

		else{

			transform.localScale = new Vector3(sizeX, sizeY, transform.localScale.z);

		}
		
		//Calculate Value
		value = GameObject.Find("GameManager").GetComponent<GameManager>().happiness / max;
		
		//Place goodly
		transform.localPosition = new Vector3 (Mathf.Lerp(-12.0f, 12.0f, value), transform.localPosition.y, transform.localPosition.z);
		
	}
}
