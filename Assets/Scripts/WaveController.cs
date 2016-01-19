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

	Dictionary<int, Enemy> currWaveEnemies;

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

		currWaveEnemies = new Dictionary<int, Enemy>();
	}

	private void CurrentWaveOver(){
		currWave = null;
		//TODO

		//DEBUG restart
//		StartCurrentWave();
	}


	public bool IsCurrentWaveComplete(){
		if (currWave == null && currWaveEnemies.Count == 0) return true;
		return false;
	}

	public void EnemyKilled(Enemy enemy){
		if (currWaveEnemies.ContainsKey(enemy.gameObject.GetInstanceID())){
			currWaveEnemies.Remove(enemy.gameObject.GetInstanceID());
		}
		if (IsCurrentWaveComplete()){
			gameCtrl.WaveCompleted();
		}
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
					GameObject enemyGO = (GameObject) Instantiate(enemyPrefab, new Vector2(waveEnemy.startX, gameCtrl.enemySpawnYPos), Quaternion.identity);
					Enemy enemy = enemyGO.GetComponent<Enemy>();
					enemy.Init(waveEnemy.type);
					currWaveEnemies.Add(enemyGO.GetInstanceID(), enemy);
				}

				wavePartIncr++;
			}
		}
	}


	public int GetAmountMoneyForWave(int waveNr){
		//TODO
		return (waveNr + 1) * 100;
	}
}

