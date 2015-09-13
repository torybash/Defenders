using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	//Inspector refs
	[Header("References")]
	[SerializeField] Transform bulletSpawnPosT;

	//Prefabs
	[Header("Prefabs")]
	[SerializeField] GameObject bulletPrefab;

	//Variables
	float nextShotTime;

	public TurretStats stats;

	Building building;



	void Awake(){
		building = GetComponent<Building>();
	}

	public void Init(TurretStats stats){
		this.stats = stats;

	}

	public bool TryShootAt(Vector2 pos){
		if (building.isDestroyed) return false;

		if (Time.time > nextShotTime){
			ShootAt(pos);
			nextShotTime = Time.time + stats.cooldownDuration;

			return true;
		}
		return false;
	}

	public void ShootAt(Vector2 pos){
		GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, bulletSpawnPosT.position, Quaternion.identity);
		Bullet bullet = bulletGO.GetComponent<Bullet>();
		bullet.Init(pos, stats.type, stats.bulletSpeed);
	}
}

public class TurretStats{
	public ProjectileType type;
	public float cooldownDuration;
	public int maxBulletAmount;
	public float bulletSpeed;
	public int ammoMax;

	public static TurretStats DefaultTurret{
	get{
			TurretStats stats = new TurretStats();
			stats.cooldownDuration = 0.4f;
			stats.maxBulletAmount = -1;
			stats.type = ProjectileType.EXPLODING;
			stats.bulletSpeed = 15f;
			stats.ammoMax = 10;
			return stats;
		}
	}
}


public enum ProjectileType{
	NORMAL,
	EXPLODING,
	AMOUNT
}