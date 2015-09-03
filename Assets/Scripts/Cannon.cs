using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	//Inspector refs
	[Header("References")]
	[SerializeField] Transform bulletSpawnPosT;

	//Prefabs
	[Header("Prefabs")]
	[SerializeField] GameObject bulletPrefab;

	//Variables
	float nextShotTime;

	CannonStats stats;

	public void Init(CannonStats stats){
		this.stats = stats;
	}

	public void TryShootAt(Vector2 pos){

		if (Time.time > nextShotTime){
			ShootAt(pos);
			nextShotTime = Time.time + stats.cooldownDuration;
		}
	}

	public void ShootAt(Vector2 pos){
		GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, bulletSpawnPosT.position, Quaternion.identity);
		Bullet bullet = bulletGO.GetComponent<Bullet>();
		bullet.Init(pos, stats.bulletStats);
	}
}

public class CannonStats{
	public float cooldownDuration;
	public int maxBulletAmount;
	public BulletStats bulletStats;

	public static CannonStats DefaultCannon{
	get{
			CannonStats stats = new CannonStats();
			stats.cooldownDuration = 0.5f;
			stats.maxBulletAmount = -1;
			stats.bulletStats = new BulletStats();
			stats.bulletStats.bulletType = BulletType.NORMAL;
			stats.bulletStats.speed = 5f;
			return stats;
		}
	}
}