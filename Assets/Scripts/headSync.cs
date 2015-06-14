using UnityEngine;
using System.Collections;

public class headSync : MonoBehaviour {

	public Animator anim;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//if(anim.GetCurrentAnimatorStateInfo (0).IsName("Idle")){
		anim.Play("Idle", -1, transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo (0).normalizedTime);
		//}
	}
}
