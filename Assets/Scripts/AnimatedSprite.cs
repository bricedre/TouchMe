using UnityEngine;
using System.Collections;

public class AnimatedSprite : MonoBehaviour {

	public Animator anim;
	public StretchScript stretchScript;
	public float relativeTime;
	public float progression;

	void Start () {

		anim = gameObject.GetComponent<Animator> ();
		stretchScript = GameObject.Find(this.name + "Stretch").GetComponent<StretchScript> ();

	}

	void Update () {

		progression = stretchScript.stretchProgression;

		//Calculate Value
		relativeTime = Normalize (progression, 1.0f, stretchScript.maxCompress, stretchScript.maxStretch, stretchScript.stretchRatio);

		//Show right frame
		anim.Play("displacement", -1, relativeTime);

	}

	public float Normalize(float NewMin, float newMax, float OldMin, float OldMax, float OldValue){
	
		float OldRange = (OldMax - OldMin);
		float NewRange = (newMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
	
		return(NewValue);
	}
}

