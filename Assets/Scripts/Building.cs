using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	public bool isDestroyed = false;
	public BuildingStats stats;


//	public 

	BuildingController buildingCtrl;

	public void GotHit(){

		buildingCtrl.BuildingDestroyed(this);


	}


	public void Init(BuildingController buildingCtrl, BuildingDefinition bd){
		this.buildingCtrl = buildingCtrl;
//		this.type = type;

//		stats = 
	}
}

public class BuildingStats{

	public BuildingType type;
	public string name;
	public int powerUse;
	public float damage;
	public float speed;
	public int sellValue;
	public List<BuildingUpgrade> upgrades;


	public BuildingStats(BuildingDefinition bd){

	}
}

public class BuildingUpgrade{
	public string name;
	public int level;
}

