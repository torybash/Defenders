using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteLibrary : MonoBehaviour {

	[SerializeField] List<BuildingSprite> buildingSprites;
	Dictionary<BuildingType, BuildingSprite> buildingSpritesDict;

	[SerializeField] List<EnemySprite> enemySprites;
    Dictionary<EnemyType, Sprite> enemySpritesDict;


	[SerializeField] List<ProjectileSprite> projectileSprites;
	Dictionary<ProjectileType, ProjectileSprite> projectileSpritesDict;



	public static SpriteLibrary I;

	void Awake(){
		if (I == null){
			I = this;
			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
			return;
		}
        buildingSpritesDict = new Dictionary<BuildingType, BuildingSprite>();
        enemySpritesDict = new Dictionary<EnemyType, Sprite>();
        projectileSpritesDict = new Dictionary<ProjectileType, ProjectileSprite>();
		foreach (BuildingSprite item in buildingSprites) {
			buildingSpritesDict.Add(item.type, item);
		}
		foreach (EnemySprite item in enemySprites) {
			enemySpritesDict.Add(item.type, item.sprite);
		}
		foreach (ProjectileSprite item in projectileSprites) {
			projectileSpritesDict.Add(item.type, item);
		}

	}

	public Sprite GetBuildingSprite(BuildingType type){
		return buildingSpritesDict[type].sprite;
	}

	public Sprite GetDestroyedBuildingSprite(BuildingType type){
		return buildingSpritesDict[type].destroyedSprite;
	}

	public Sprite GetBuildingTurretHeadSprite(BuildingType type){
		return buildingSpritesDict[type].turretHeadSprite;
	}

	public Sprite GetEnemySprite(EnemyType type){
        if (enemySpritesDict == null) {
            foreach (EnemySprite item in enemySprites) {
                if (item.type == type) return item.sprite;
            }
            return null;
        }
		return enemySpritesDict[type];
	}

	public Sprite GetProjectileSprite(ProjectileType type){
		return projectileSpritesDict[type].sprite;
	}
}


[System.Serializable]
public class BuildingSprite{
	public BuildingType type;
	public Sprite sprite;
	public Sprite destroyedSprite;
	public Sprite turretHeadSprite;
}

[System.Serializable]
public class EnemySprite{
	public EnemyType type;
	public Sprite sprite;
}


[System.Serializable]
public class ProjectileSprite{
	public ProjectileType type;
	public Sprite sprite;
}

