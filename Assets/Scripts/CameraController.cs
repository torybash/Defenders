using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	GameController gameCtrl;

	public Camera cam;


	void Awake(){
		gameCtrl = GetComponent<GameController>();
		cam = Camera.main;

		float aspect = Screen.width / (float)Screen.height;
		
		cam.orthographicSize = gameCtrl.width / aspect / 2f;
	}


	public void Init(){

	}



	public float GetTopY(){
		return cam.orthographicSize;
	}

	public float GetBottomY(){
		return -cam.orthographicSize;
	}
}
