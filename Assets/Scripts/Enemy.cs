using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {


	float speed = 5;

	Vector2 vel = Vector2.zero;

	GameController gameCtrl;

	Building currTarget;

	Rigidbody2D rb;

	void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		rb = GetComponent<Rigidbody2D>();
	}

	void Update () {
//		transform.Translate(vel * Time.deltaTime);

		if (currTarget != null && currTarget.isDestroyed){
			UpdateGoalPos();
		}
	}

	private void UpdateGoalPos(){
		currTarget = gameCtrl.buildingCtrl.GetClosestBuilding(transform.position);
		Vector2 goalPos = Vector2.zero;
		if (currTarget == null){
			goalPos = transform.position;
		}else{
			goalPos = currTarget.transform.position;
		}
		


		GetComponent<Rigidbody2D>().AddForce(-rb.velocity, ForceMode2D.Impulse); //stop velocity
		vel = (goalPos - (Vector2)transform.position).normalized * speed;
		GetComponent<Rigidbody2D>().AddForce(vel, ForceMode2D.Impulse);
	}


	public void Init(EnemyType type){
		UpdateGoalPos();
	}


	public void GotHit(){
		//Debug

		gameCtrl.waveCtrl.EnemyKilled(this);

		GameObject.Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D coll){
		print ("OnTriggerEnter2D - enemy collided with: " + coll);

		if (coll.GetComponent<Building>() != null){
			Building building = coll.GetComponent<Building>();

			building.GotHit();

			GotHit();


		}else if (coll.GetComponent<Explosion>() != null){
			GotHit();
		}
	}
}
