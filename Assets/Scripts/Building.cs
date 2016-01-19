using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public bool isDestroyed = false;
	public BuildingStats stats;


	public BuildingType type;

	BuildingController buildingCtrl;

	public void GotHit(){

		buildingCtrl.BuildingDestroyed(this);


	}


	public void Init(BuildingController buildingCtrl, BuildingType type, BuildingDefinition bd){
		this.buildingCtrl = buildingCtrl;
		stats = new BuildingStats(type, bd);
	}
}

public class BuildingStats{

	public BuildingType type;
	public BuildingDefinition bd;

	public int hpLeft;
	public List<BuildingUpgrade> upgrades;


	public BuildingStats(BuildingType type, BuildingDefinition bd){
		this.bd = bd;
		this.type = type;

		hpLeft = bd.hpMax;
//		upgrades = TODO
	}
}

public class BuildingUpgrade{
	public string name;
	public int level;
}

