using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	BulletStats stats;

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
		if (stats.bulletType == BulletType.EXPLODING){
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


	public void Init(Vector2 goalPos, BulletStats stats){
		this.goalPos = goalPos;
		this.stats = stats;

		//Set velocity
		Vector2 dir = (goalPos - (Vector2)transform.position).normalized;
		Vector2 vel = dir * stats.speed;
		rb.AddForce(vel, ForceMode2D.Impulse);

		//Set rotation

		transform.rotation = Tools.DirectionToQuaternion(dir);

	}

	private void BulletHitTarget(){

		if (stats.bulletType == BulletType.EXPLODING){
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

public class BulletStats{
	public BulletType bulletType;
	public float speed;
}

public enum BulletType{
	NORMAL = 0,
	EXPLODING = 1
}