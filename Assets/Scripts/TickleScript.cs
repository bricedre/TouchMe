using UnityEngine;
using System.Collections;

public class TickleScript: MonoBehaviour {
	
	public float threshold;
	public float value;
	public float touchyTrigger;
	public float pointsInfluence;
	public bool regenerative;

	private bool warningDone;

	private RaycastHit hit;
	private Ray ray;
	
	void Start () {

		value = threshold;
	}
	
	void Update () {

		if(value < 0.0f){

			//If it's regenerative, a new one is done
			if(regenerative){
				GameObject newTickleZone = Instantiate(this, transform.position, Quaternion.identity) as GameObject;
			}

			//in both cases, it is destroyed and it adds points to the points' pool
			GameObject.Destroy(this.gameObject);
			GameObject.Find("GameManager").GetComponent<GameManager>().pointsPool += pointsInfluence;

		}

		if(value < touchyTrigger && value > touchyTrigger - 2.0f){
			Debug.Log (this.name+" triggered!");
		}

		//Detects MousePointer
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//If it hits this instance, it decreases threshold
		if (Physics.Raycast(ray, out hit)){
			if(hit.collider.gameObject == this.gameObject){
				value -= 0.1f;
			}
		}
	}


}