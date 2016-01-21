﻿using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	//Inspector refs
	[Header("References")]
	[SerializeField] Transform bulletSpawnPosT;
	[SerializeField] Transform rotatePart;

	//Prefabs
	[Header("Prefabs")]
	[SerializeField] GameObject bulletPrefab;

	//Variables / refs

	TurretStats stats;

	Building building;



	public TurretStats Stats{
		get { return stats;}
	}


	void Awake(){
		building = GetComponent<Building>();
	}

	public void Init(TurretDefinition def){
		stats = new TurretStats();
		stats.def = def;
		stats.currShotsOut = 0;


		BuildingType bldType = GetComponent<Building>().stats.def.type;
		GetComponent<Turret>().rotatePart.gameObject.SetActive(true);
		GetComponent<Turret>().rotatePart.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.I.GetBuildingTurretHeadSprite(bldType);


//		Debug.Log("initialized turret with TurretDefinition: "+  def);
	}

	public bool TryShootAt(Vector2 pos){
		if (building.isDestroyed) return false;

		if (Time.time > stats.nextShotTime){
			ShootAt(pos);
			stats.nextShotTime = Time.time + stats.def.cooldownDuration;

			return true;
		}
		return false;
	}

	public void ShootAt(Vector2 pos){

		Vector2 dir = pos - (Vector2)transform.position;
		if (rotatePart != null) rotatePart.rotation = Quaternion.AngleAxis(dir.VectorAngle(), Vector3.forward);

		GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, bulletSpawnPosT.position, Quaternion.identity);
		Bullet bullet = bulletGO.GetComponent<Bullet>();
		bullet.Init(pos, stats.def.type, stats.def.bulletSpeed);
	}
}


public class TurretStats{
	public TurretDefinition def;

	public float nextShotTime;
	public int currShotsOut;
}