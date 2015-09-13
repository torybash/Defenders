using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingLibrary : MonoBehaviour {

	[SerializeField] List<BuildingDefinition> buildingDefs;
	Dictionary<BuildingType, BuildingDefinition> buildingDefsDict = new Dictionary<BuildingType, BuildingDefinition>();

	void Awake(){
		foreach (BuildingDefinition bd in buildingDefs) {
			buildingDefsDict.Add(bd.type, bd);
		}
	}

	public List<BuildingDefinition> GetAllBuildingDefs(){
		return buildingDefs;
	}

	public BuildingDefinition GetBuildingDefinition(BuildingType type){
		return buildingDefsDict[type];
	}
}


[System.Serializable]
public class BuildingDefinition{
	public BuildingType type;
	public string name;
	public int buildPrice;
	public int powerUse;
	
	public ProjectileType projType;
	public float damage;
	public float speed;

	public List<BuildingUpgradeDefinition> upgrades;
}

[System.Serializable]
public class BuildingUpgradeDefinition{
	public BuildingUpgradeType type;
	public string name;
	public float incrPerLvl;
	public int startLvl;
	public int maxLvl;
}


[System.Serializable]
public enum BuildingUpgradeType{
	DAMAGE,
	SPEED,
	EXPLOSION_RADIUS
}