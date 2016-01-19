using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteLibrary : MonoBehaviour {

	[SerializeField] List<BuildingSprite> buildingSprites;
	Dictionary<BuildingType, BuildingSprite> buildingSpritesDict = new Dictionary<BuildingType, BuildingSprite>();

	[SerializeField] List<EnemySprite> enemySprites;
	Dictionary<EnemyType, Sprite> enemySpritesDict = new Dictionary<EnemyType, Sprite>();

	public static SpriteLibrary I;

	void Awake(){
		if (I == null){
			I = this;
			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
			return;
		}
		foreach (BuildingSprite item in buildingSprites) {
			buildingSpritesDict.Add(item.type, item);
		}
		foreach (EnemySprite item in enemySprites) {
			enemySpritesDict.Add(item.type, item.sprite);
		}

	}

	public Sprite GetBuildingSprite(BuildingType type){
		return buildingSpritesDict[type].sprite;
	}

	public Sprite GetDestroyedBuildingSprite(BuildingType type){
		return buildingSpritesDict[type].destroyedSprite;
	}

	public Sprite GetEnemySprite(EnemyType type){
		return enemySpritesDict[type];
	}
}


[System.Serializable]
public class BuildingSprite{
	public BuildingType type;
	public Sprite sprite;
	public Sprite destroyedSprite;
}

[System.Serializable]
public class EnemySprite{
	public EnemyType type;
	public Sprite sprite;
}

