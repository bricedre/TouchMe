using UnityEngine;
using System.Collections;

public class BubbleWiggle : MonoBehaviour {
	
	private Vector3 position;
	public float offset;

	public float positionWiggleMagnitudeX;
	public float positionWiggleMagnitudeY;

	public float vitesseX;
	public float vitesseY;


	void Start () {
		position = transform.position;
		offset = Random.Range (0, 1000);
		positionWiggleMagnitudeX = Random.Range (15f, 20f);
		positionWiggleMagnitudeY = Random.Range (7f, 10f);
		vitesseX = Random.Range (0.8f, 1.2f);
		vitesseY = Random.Range (0.5f, 0.7f);
	}

	void Update () {
		transform.position = new Vector3 (position.x + (Mathf.Sin(Time.time * vitesseX + offset) * positionWiggleMagnitudeX / 100.0f),
		                                  position.y + (Mathf.Sin(Time.time * vitesseY + offset) * positionWiggleMagnitudeY / 100.0f),
		                                 transform.position.z);

	}
}
