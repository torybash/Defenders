using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	float timeFull;
	float timeDisappear;


	[SerializeField] float timeToFull = 0.56f;
	[SerializeField] float timeToDisappear = 0.25f;
    [SerializeField] float goalSize = 2.5f;
	
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

		timeFull = Time.time + timeToFull;
		timeDisappear = Time.time + timeToFull + timeToDisappear;
	}
}
