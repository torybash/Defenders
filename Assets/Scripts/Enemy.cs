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
		goalPos = Vector2.zero;
		if (currTarget == null){
			goalPos = transform.position;
		}else{
			goalPos = currTarget.transform.position;
		}
		
	}

	void FixedUpdate(){ 
//		Debug.Log("stats.def.speed: " +stats.def.speed);
		GetComponent<Rigidbody2D>().AddForce(-rb.velocity, ForceMode2D.Impulse); //stop velocity
		vel = (goalPos - (Vector2)transform.position).normalized * stats.def.speed;
		GetComponent<Rigidbody2D>().AddForce(vel, ForceMode2D.Impulse);
	}


	public void Init(EnemyDefinition def){
		UpdateGoalPos();

		stats.def = def;
		stats.currHp = def.maxHp;

		//TODO MAKE LIBRAY INSTEAD!!!
		
	}


	public void GotHit(){
		//Debug

		stats.currHp -= 1;

		if (stats.currHp <= 0){ //Die
			gameCtrl.waveCtrl.EnemyKilled(this);

			GameObject.Destroy(gameObject);
		}else{ //Hit effect
			Color hitColor = new Color(1f, 0.5f, 0.5f, 1);
			if (currGotHitTween != null) currGotHitTween.Complete();
			currGotHitTween = HOTween.To(GetComponent<SpriteRenderer>(), 0.1f, new TweenParms().Prop("color", hitColor).Ease(EaseType.EaseInOutCirc).Loops(2, LoopType.Yoyo));

//			if (currGotHitCR != null) StopCoroutine(currGotHitCR);
//			currGotHitCR = StartCoroutine(GotHitEffect());
		}
	}

//	private IEnumerator GotHitEffect(){
//		
//		yield return new WaitForSeconds(2f);
//	}


	void OnTriggerStay2D(Collider2D coll){
//		print ("OnTriggerEnter2D - enemy collided with: " + coll);

		if (coll.GetComponent<Building>() != null){
			Building building = coll.GetComponent<Building>();

			building.GotHit();

			GotHit();


		}else if (coll.GetComponent<Explosion>() != null){
			GotHit();
		}
	}
}

[System.Serializable]
public class EnemyStats{
	public EnemyDefinition def;
	public int currHp;
	
}