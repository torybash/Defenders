using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	//Inspector refs
	[Header("References")]
	[SerializeField] Transform bulletSpawnPosT;
	[SerializeField] Transform turretHead;

	//Prefabs
	[Header("Prefabs")]
	[SerializeField] GameObject bulletPrefab;

	//Variables / refs

    [SerializeField] TurretStats stats;

	Building building;

    public Building Building {
        get { return building; }
    }

	public Transform TurretHead{
		get{ return turretHead;}
	}


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
        stats.ammoLeft = stats.def.maxAmmo;
        stats.activated = true;

		BuildingType bldType = GetComponent<Building>().stats.def.type;
		GetComponent<Turret>().turretHead.gameObject.SetActive(true);
		GetComponent<Turret>().turretHead.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.I.GetBuildingTurretHeadSprite(bldType);


//		Debug.Log("initialized turret with TurretDefinition: "+  def);
	}

	public bool TryShootAt(Vector2 pos){
        if (building.isDestroyed || !stats.activated) return false;

		if (Time.time > stats.nextShotTime){
			ShootAt(pos);

            //stats.ammoLeft -= 1;
            //if (stats.ammoLeft <= 0) {
            //    stats.ammoLeft = stats.def.maxAmmo;
                //stats.nextShotTime = Time.time + stats.def.reloadDuration;
            //} else {
            stats.nextShotTime = Time.time + stats.def.cooldownDuration;
            //}

			return true;
		}
		return false;
	}

	public void ShootAt(Vector2 pos){

		Vector2 dir = pos - (Vector2)transform.position;
		if (turretHead != null) turretHead.rotation = Quaternion.AngleAxis(dir.VectorAngle(), Vector3.forward);

		GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, bulletSpawnPosT.position, Quaternion.identity);
		Bullet bullet = bulletGO.GetComponent<Bullet>();
		bullet.Init(pos, stats.def.type, stats.def.bulletSpeed);

		GameController.I.audioCtrl.PlayShoot();
	}

    public void ToggleActivated() {
        stats.activated = !stats.activated;
        if (stats.activated) {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        } else {
            GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }
}

[System.Serializable]
public class TurretStats{
	public TurretDefinition def;

	public float nextShotTime;
	public int currShotsOut;
    public int ammoLeft;

    public bool activated;
}