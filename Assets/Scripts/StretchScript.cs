﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class ParentLimbInfluence{

	//if there's parent influence over this stretch
	public AnimatedSprite parentLimb;
	public Vector2 parentPosOffset;
	public float parentRotOffset;
}

[System.Serializable]
public class SelfInfluence{

	//if there's self influence over this stretch
	public Vector2 ownPosOffset;
	public float ownRotOffset;
}

public class StretchScript : MonoBehaviour {

	public ParentLimbInfluence parentInfluence;
	public SelfInfluence selfInfluence;

	public Transform anchor;
	public Transform handle;
	public Transform limb;
	
	public bool moveableRight;
	public bool moveableLeft;

	private float initialLimbScale;
	private float initialLimbDistance;
	public float stretchRatio;
	public float maxStretch;
	public float maxCompress;
	
	public bool withinBounds;

	private float parentRelativeTime;
	

	void Start () {

		initialLimbScale = limb.transform.localScale.x;
		initialLimbDistance = anchor.position.x - handle.position.x;
		anchor.GetComponent<SpringJoint> ().spring = 500; //default : 1000
		anchor.GetComponent<SpringJoint> ().damper = 0;

		stretchRatio = 1.0f;

		withinBounds = true;

	}

	void Update () {

		//Get how many strain limb gets
		stretchRatio = (anchor.position.x - handle.position.x) / initialLimbDistance;

		//Is stretch completely stretched or completely compressed?
		if(stretchRatio < maxCompress || stretchRatio > maxStretch){
			withinBounds = false;
		}
		else withinBounds = true;

		parentRelativeTime = 0.0f;
		//If parent animation influences child's values
		if(parentInfluence.parentLimb != null){
			parentRelativeTime = parentInfluence.parentLimb.relativeTime;
		}

		//Change position & rotation according to stretches
		if(withinBounds){

			limb.transform.position = new Vector3 ((anchor.position.x + handle.position.x) / 2.0f
			                                       + parentInfluence.parentPosOffset.x * parentRelativeTime
			                                       + selfInfluence.ownPosOffset.x * limb.gameObject.GetComponent<AnimatedSprite>().relativeTime,

			                                       (anchor.position.y + handle.position.y) / 2.0f
			                                       + parentInfluence.parentPosOffset.y * parentRelativeTime
			                                       + selfInfluence.ownPosOffset.y * limb.gameObject.GetComponent<AnimatedSprite>().relativeTime,

			                                       limb.transform.position.z);

			limb.transform.eulerAngles = new Vector3 (limb.transform.rotation.x,
			                                          
			                                          limb.transform.rotation.y,
			                                          
			                                          parentInfluence.parentRotOffset * parentRelativeTime
			                                          + selfInfluence.ownRotOffset * limb.gameObject.GetComponent<AnimatedSprite>().relativeTime);
		}

		else{

			limb.transform.position = new Vector3 ((anchor.position.x + handle.position.x) / 2.0f,
			                                       (anchor.position.y + handle.position.y) / 2.0f,
			                                       limb.transform.position.z);

			limb.transform.eulerAngles = new Vector3 (limb.transform.rotation.x,
			                                          
			                                          limb.transform.rotation.y,
			                                          
			                                          parentInfluence.parentRotOffset * parentRelativeTime);
		}

		//Change scale according to stretches
		limb.transform.localScale = new Vector3((anchor.position.x - handle.position.x) / initialLimbDistance * initialLimbScale,
		                                        limb.transform.localScale.y,
		                                        limb.transform.localScale.z);
	}
}