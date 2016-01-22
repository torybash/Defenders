using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Enemy : MonoBehaviour {


	Vector2 vel = Vector2.zero;

	GameController gameCtrl;

	Building currTarget;

	Rigidbody2D rb;

	Vector2 goalPos;

	public EnemyStats stats;

	Tweener currGotHitTween;

	SpriteRenderer sr;


	enum EnemyState{
		ACTIVE,
		DESTROYED
	}

	EnemyState state;

	BaseParticleSys crashParticSys;

	void Awake(){
		gameCtrl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		rb = GetComponent<Rigidbody2D>();

		sr = GetComponent<SpriteRenderer>();
	}

	void Update () {
//		transform.Translate(vel * Time.deltaTime);

		if (state == EnemyState.DESTROYED) return;

		if (currTarget != null && currTarget.isDestroyed){
			UpdateGoalPos();
		}

//		Debug.Log("vel.VectorAngle(): " + vel.VectorAngle());
		transform.rotation = Quaternion.AngleAxis(vel.VectorAngle()+90f, Vector3.forward);
	}

	private void UpdateGoalPos(){
		currTarget = gameCtrl.buildingCtrl.GetClosestBuilding(transform.position);
		goalPos = Vector2.zero;
		if (currTarget == null){
			goalPos = transform.position;
		}else{
			goalPos = currTarget.transform.position;
		}



		
	}

	void FixedUpdate(){ 
		if (state == EnemyState.DESTROYED) return;

//		Debug.Log("stats.def.speed: " +stats.def.speed);
		GetComponent<Rigidbody2D>().AddForce(-rb.velocity, ForceMode2D.Impulse); //stop velocity
		vel = (goalPos - (Vector2)transform.position).normalized * stats.def.speed;
		GetComponent<Rigidbody2D>().AddForce(vel, ForceMode2D.Impulse);
	}


	public void Init(EnemyDefinition def){
		UpdateGoalPos();

		stats.def = def;
		stats.currHp = def.maxHp;

		state = EnemyState.ACTIVE;
//		gameObject.layer = LayerMask.NameToLayer("Default");

		//set sprite TODO Set animator or sumthin
		sr.sprite = SpriteLibrary.I.GetEnemySprite(def.type);

		//Set collider
		GetComponent<BoxCollider2D>().size = sr.sprite.bounds.size;
	}


	public void GotHit(Vector2 forceDir = default(Vector2)){
		//Debug

		stats.currHp -= 1;

		if (state == EnemyState.ACTIVE){
			
			if (stats.currHp <= 0){ //Die
				
				Killed(forceDir);


			}else{ //Hit effect
				Color hitColor = new Color(1f, 0.5f, 0.5f, 1);
				if (currGotHitTween != null) currGotHitTween.Complete();
				currGotHitTween = HOTween.To(GetComponent<SpriteRenderer>(), 0.1f, new TweenParms().Prop("color", hitColor).Ease(EaseType.EaseInOutCirc).Loops(2, LoopType.Yoyo));

				GameController.I.audioCtrl.PlayHit();

		//			if (currGotHitCR != null) StopCoroutine(currGotHitCR);
		//			currGotHitCR = StartCoroutine(GotHitEffect());
			}
		}else if (state == EnemyState.DESTROYED){
//			if (stats.currHp < -stats.def.maxHp / 2){
				Explode();
//			}


		}
	}


	private void Killed(Vector2 forceDir){
		gameCtrl.waveCtrl.EnemyKilled(this);
		state = EnemyState.DESTROYED;



		if (Random.Range(0, 1f) > 0.1f){ //Crash?
			rb.gravityScale = 1f;
			rb.AddForce(forceDir * Random.Range(1.5f, 3.0f), ForceMode2D.Impulse);
			rb.AddTorque(Random.Range(-5f, 5f));


			crashParticSys = gameCtrl.partCtrl.AttachParticlesTo(ParticleType.CRASH, transform);
		}else{ //Or explode immidetly
			Explode();
		}
			

//		Debug.Log("Killed! - forceDir: " + forceDir);
//		GameObject.Destroy(gameObject);
	}

	private void Explode(){
//		state == EnemyState.DESTROYED;

		gameCtrl.partCtrl.CreateParticlesAt(ParticleType.EXPLOSION, transform.position);
		gameCtrl.partCtrl.CreateParticlesAt(ParticleType.DEBRIS, transform.position);

		if (crashParticSys != null){ 
			crashParticSys.transform.parent = null;
			crashParticSys.Stop();
		}

		GameController.I.audioCtrl.PlayExplode();

		Destroy(gameObject);
	}

	



//	private IEnumerator GotHitEffect(){
//		
//		yield return new WaitForSeconds(2f);
//	}

	void OnCollisionEnter2D(Collision2D collision){
		CollidedWith(collision.collider);
	}

	void OnTriggerEnter2D(Collider2D coll){
		CollidedWith(coll);
	}

	void OnTriggerStay2D(Collider2D coll){
		CollidedWith(coll);
	}

//	void OnCollisionStay2D(Collision2D collision){
////		print ("OnTriggerEnter2D - enemy collided with: " + coll);
//		CollidedWith(collision.collider);
//	}

	void CollidedWith(Collider2D coll){
		if (coll.GetComponent<Building>() != null){
			Building building = coll.GetComponent<Building>();

			building.GotHit();

			GotHit();


		}else if (coll.GetComponent<Explosion>() != null){
			GotHit();
		}else if (coll.GetComponent<Bullet>() != null){
			Vector2 forceDir = (transform.position - coll.transform.position).normalized;

			GotHit(forceDir);
			coll.GetComponent<Bullet>().BulletHitTarget();
		}else if (coll.gameObject.layer == LayerMask.NameToLayer("Ground")){
			Explode();
		}
	}


//	void OnTriggerEnter2D(Collider2D coll){
		//		print ("OnTriggerEnter2D - bullet collided with: " + coll);


//	}


}

[System.Serializable]
public class EnemyStats{
	public EnemyDefinition def;
	public int currHp;
	
}