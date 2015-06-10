using UnityEngine;
using System.Collections;

public class AnimationParameters : MonoBehaviour {

	public string[] parameters;

	void Update () {

		//Initializing parameters
		for(int i = 0 ; i<parameters.Length ; i++)
			this.GetComponent<Animator>().SetBool(parameters[i], false);
	}
}
