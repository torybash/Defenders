﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public bool isDestroyed = false;
	public BuildingStats stats;


	public BuildingType type;


	public void GotHit(){

		GameController.I.buildingCtrl.BuildingDestroyed(this);


	}

	public void SetBuildingState(bool isDestroyed){
		this.isDestroyed = isDestroyed;
		GetComponent<Collider2D>().enabled = !isDestroyed;

		//		Debug.Log("building.GetComponent<SpriteRenderer>(): " + building.GetComponent<SpriteRenderer>());
		if (isDestroyed){
			GetComponent<SpriteRenderer>().sprite = SpriteLibrary.I.GetDestroyedBuildingSprite(stats.def.type);
		
		}else{
			GetComponent<SpriteRenderer>().sprite = SpriteLibrary.I.GetBuildingSprite(stats.def.type);
		}

		if (stats.def.isTurret){
			if (GetComponent<Turret>().TurretHead != null) GetComponent<Turret>().TurretHead.gameObject.SetActive(!isDestroyed);
		}
	}


	public void Init(BuildingType type, BuildingDefinition bd, int x, int y){
		stats = new BuildingStats(bd, x, y);

	}
}

public class BuildingStats{

	public BuildingDefinition def;

	public int hpLeft;
	public List<BuildingUpgrade> upgrades;
    public int x;
    public int y;

	public BuildingStats(BuildingDefinition bd, int x, int y){
		this.def = bd;
        this.x = x;
        this.y = y;

		hpLeft = bd.hpMax;
//		upgrades = TODO
	}
}

public class BuildingUpgrade{
	public string name;
	public int level;
}

