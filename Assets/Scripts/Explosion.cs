using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	float goalSize;

	float timeFull;
	float timeDisappear;


	float timeToFull = 1.0f;
	float timeToDisappear = 0.25f;
	
	// Update is called once per frame
	void Update () {
		if (Time.time < timeFull){
			float progress = 1 - (timeFull - Time.time)/timeToFull; // 0 -> 1
			float currSize = progress * goalSize;
			transform.localScale = new Vector3(currSize, currSize, currSize);
		}else if (Time.time < timeDisappear){

		}else if (Time.time >= timeDisappear){
			GameObject.Destroy(gameObject);

		}
	}


	public void Init(){




		goalSize = 3.2f;
		timeFull = Time.time + timeToFull;
		timeDisappear = Time.time + timeToFull + timeToDisappear;
	}
}
