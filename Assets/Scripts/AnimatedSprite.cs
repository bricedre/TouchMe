using UnityEngine;
using System.Collections;

public class AnimatedSprite : MonoBehaviour {

	public Animator anim;
	public StretchScript stretchScript;
	public float relativeTime;

	void Start () {

		anim = gameObject.GetComponent<Animator> ();
		stretchScript = GameObject.Find(this.name + "Stretch").GetComponent<StretchScript> ();

	}

	void Update () {

		//Calculate Value
		relativeTime = Normalize (stretchScript.maxCompress , stretchScript.maxStretch, stretchScript.stretchRatio);

		//Show right frame
		anim.Play("displacement", -1, relativeTime);

	}

	public float Normalize(float OldMin, float OldMax, float OldValue){
	
		float OldRange = (OldMax - OldMin);
		float NewValue = (((OldValue - OldMin) * 1.0f) / OldRange);
	
		return(NewValue);
	}
}

