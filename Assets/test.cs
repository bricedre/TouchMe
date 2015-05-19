using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	public HandleScript[] stretches;
	public string text = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		text = "";
			for(int i = 0 ; i < stretches.Length ; i++){
			text = text + stretches[i].phase;
			}
		GetComponent<TextMesh> ().text = text;
	}
}
