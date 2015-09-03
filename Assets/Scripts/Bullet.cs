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
		if (!gameCtrl.IsOutsideBounds(transform.position)){

		}
	}


	public void Init(Vector2 goalPos, BulletStats stats){
		this.goalPos = goalPos;
		this.stats = stats;

		Vector2 dir = (goalPos - (Vector2)transform.position).normalized;
		Vector2 vel = dir * stats.speed;
		rb.AddForce(vel, ForceMode2D.Impulse);


	}

	void OnTriggerEnter2D(Collider2D coll){
//		print ("OnTriggerEnter2D - bullet collided with: " + coll);

		if (coll.GetComponent<Enemy>() != null){
			coll.GetComponent<Enemy>().GotHit();
			GameObject.Destroy(gameObject);
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