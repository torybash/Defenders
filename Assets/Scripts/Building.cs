﻿using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void GotHit(){
		GameObject.Destroy(gameObject);
	}
}