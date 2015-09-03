using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	Vector2 vel = Vector2.zero;

	float speed = 5;


	void Update () {
		transform.Translate(vel * Time.deltaTime);
	}



	public void Init(EnemyType type, Vector2 goalPos){

		vel = (goalPos - (Vector2)transform.position).normalized * speed;
	}


	public void GotHit(){
		//Debug
		GameObject.Destroy(gameObject);
	}

//	void OnTriggerEnter2D(Collider2D coll){
//		print ("OnTriggerEnter2D - enemy collided with: " + coll);
//	}
}
