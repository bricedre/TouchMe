using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour {

	private bool showFPS = false;

	void Start(){
		Application.targetFrameRate = 50;
	}

	void LateUpdate(){
		if(Input.GetKeyDown("f"))	showFPS = !showFPS;
	}

	void OnGUI(){
		if( showFPS )
			GUI.Button(new Rect(0, 0, 120, 50), "FPS : " + Mathf.Round(1/Time.deltaTime) );
	}
}
