using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveController : MonoBehaviour {

	[Header("Waves")]
	[SerializeField] List<WaveDefinition> waves;

	[Header("Prefabs")]
	[SerializeField] GameObject enemyPrefab;


	WaveDefinition currWave;
	float waveStartTime;

	int wavePartIncr;

	//Controller ref
	GameController gameCtrl;

	void Awake(){
		gameCtrl = GetComponent<GameController>();
	}

	public void Init(){
		//load currWave

		//Debug
	}

	public void StartCurrentWave(){
		currWave = waves[gameCtrl.currWave];
		waveStartTime = Time.time;
		wavePartIncr = 0;
	}

	private void CurrentWaveOver(){
		currWave = null;
		//TODO

		//DEBUG restart
		StartCurrentWave();
	}

	public void GUpdate(){
		if (currWave != null){
			//Is wave over?
			if (wavePartIncr >= currWave.waveParts.Count) {
				CurrentWaveOver();
				return;
			}

			//Is time for next wave part?
			WavePart part = currWave.waveParts[wavePartIncr];
			if (Time.time > waveStartTime + part.time){
				foreach (WaveEnemy waveEnemy in part.enemies) {
					GameObject enemyGO = (GameObject) Instantiate(enemyPrefab, new Vector2(waveEnemy.startX, gameCtrl.GetEnemySpawnPosY()), Quaternion.identity);
					Enemy enemy = enemyGO.GetComponent<Enemy>();
					enemy.Init(waveEnemy.type, new Vector2(waveEnemy.goalX, gameCtrl.GetEnemyGoalPosY()));
				}

				wavePartIncr++;
			}
		}
	}
}

[System.Serializable]
public class WaveDefinition{
	public List<WavePart> waveParts;

}

[System.Serializable]
public class WavePart{
	public float time;
	public List<WaveEnemy> enemies;
}

[System.Serializable]
public class WaveEnemy{
	public EnemyType type;
	public float startX;
	public float goalX;
}

[System.Serializable]
public enum EnemyType{
	NORMAL = 0
}