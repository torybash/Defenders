using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {


	float speed = 5;

	Vector2 vel = Vector2.zero;

	void Update () {
//		transform.Translate(vel * Time.deltaTime);
	}



	public void Init(EnemyType type, Vector2 goalPos){

		vel = (goalPos - (Vector2)transform.position).normalized * speed;
		GetComponent<Rigidbody2D>().AddForce(vel, ForceMode2D.Impulse);
	}


	public void GotHit(){
		//Debug
		GameObject.Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D coll){
		print ("OnTriggerEnter2D - enemy collided with: " + coll);

		if (coll.GetComponent<Building>() != null){
			Building building = coll.GetComponent<Building>();

			building.GotHit();

			GameObject.Destroy(gameObject);


		}else if (coll.GetComponent<Explosion>() != null){
			GameObject.Destroy(gameObject);
		}
	}
}
