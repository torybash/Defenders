using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	const float gameWidth = 4f;


	GameController gameCtrl;

	public Camera cam;
	
	void Awake(){
		gameCtrl = GetComponent<GameController>();
		cam = Camera.main;
	}


	public void Init(){

	}

}
