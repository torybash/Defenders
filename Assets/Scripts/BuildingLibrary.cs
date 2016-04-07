using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingLibrary : ALibrary<BuildingDefinition, BuildingType> {

//	[SerializeField] List<BuildingDefinition> buildingDefs;


}


[System.Serializable]
public class BuildingDefinition{
	public BuildingType type;

	public string name;
	public int buildPrice;
	public int hpMax;
	public int powerUse;

	public bool isTurret;
	public TurretDefinition turretDef;

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


[System.Serializable]
public class TurretDefinition{
	public ProjectileType type;
	public float cooldownDuration;
	public int maxBulletAmount;
    public float bulletSpeed;

    public float reloadDuration;
    public int maxAmmo;

	//IDEAS
	//overheatTime, ammoMax, ammoBeforeReloading+reloadTime, 

	public override string ToString ()
	{
		this.GetType();
		string result = this.GetType().Name + " - [";
		for (int i = 0; i < this.GetType().GetFields().Length; i++) {
			System.Reflection.FieldInfo field = this.GetType().GetFields()[i];
			result += field.Name + "=" + field.GetValue(this);
			if (i < this.GetType().GetFields().Length - 1) result += ", ";
		}
		result += "]";
		return result;
	}
}

[System.Serializable]
public enum ProjectileType{
	NORMAL,
	EXPLODING
}