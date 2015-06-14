using UnityEngine;
using System.Collections;

public class DialogScript : MonoBehaviour {

	public bool autoAnswering;

	public string firstCue;
	public string[] playerAnswers;
	public string[] characterAnswers;
	public float[] answersValues;
	public float cuesDuration;

	void Start(){

		firstCue = "ARI_" + this.name + "_firstCue";
	}

}
