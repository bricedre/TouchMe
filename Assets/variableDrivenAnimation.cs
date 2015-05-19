using UnityEngine;
using System.Collections;

public class variableDrivenAnimation : MonoBehaviour {

	public float min;
	public float max;
	public Animator anim;
	public float frame;
	
	void Start () {

		anim = gameObject.GetComponent<Animator> ();
		min = GameObject.Find ("GameManager").GetComponent<GameManager> ().minHappiness;
		max = GameObject.Find ("GameManager").GetComponent<GameManager> ().maxHappiness;
	}

	void Update () {

		//Calculate Value
		frame = GameObject.Find("GameManager").GetComponent<GameManager>().happiness / max;
		
		//Show right frame
		anim.Play("displacement", -1, frame);
	
	}
}
