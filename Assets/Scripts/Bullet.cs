using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	ProjectileType type;

	Vector2 goalPos;

	Rigidbody2D rb;

	GameController gameCtrl;

	void Awake(){
		rb = GetComponent<Rigidbody2D>();

		gameCtrl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	void Update () {
		//Check for outside screen
		if (gameCtrl.IsOutsideBounds(transform.position)){
			GameObject.Destroy(gameObject);
		}

		//Exploding
		if (type == ProjectileType.EXPLODING){
			Vector2 dir = GetComponent<Rigidbody2D>().velocity.normalized;

			//Is postion further than goal pos?
			if (dir.x > 0 && transform.position.x > goalPos.x ||
			    dir.x < 0 && transform.position.x < goalPos.x ||
			    dir.y > 0 && transform.position.y > goalPos.y ||
			    dir.y < 0 && transform.position.y < goalPos.y)
			{
				gameCtrl.MakeExplosionAt(goalPos);
				GameObject.Destroy(gameObject);
			}
		}
	}


	public void Init(Vector2 goalPos, ProjectileType type, float speed){
		this.goalPos = goalPos;
		this.type = type;

		//Set velocity
		Vector2 dir = (goalPos - (Vector2)transform.position).normalized;
		Vector2 vel = dir * speed;
		rb.AddForce(vel, ForceMode2D.Impulse);

		//Set rotation

		transform.rotation = Tools.DirectionToQuaternion(dir);

	}

	private void BulletHitTarget(){

		if (type == ProjectileType.EXPLODING){
			gameCtrl.MakeExplosionAt(transform.position);
		}

		GameObject.Destroy(gameObject);
	}


	void OnTriggerEnter2D(Collider2D coll){
//		print ("OnTriggerEnter2D - bullet collided with: " + coll);

		if (coll.GetComponent<Enemy>() != null){
			coll.GetComponent<Enemy>().GotHit();
			BulletHitTarget();
		}
	}
}

