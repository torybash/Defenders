using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveController : MonoBehaviour {

//	[Header("Waves")]
//	[SerializeField] List<WaveDefinition> waves;

	[Header("Prefabs")]
	[SerializeField] GameObject enemyPrefab;


	WaveDefinition currWave;
	float waveStartTime;

	int wavePartIncr;

	Dictionary<int, Enemy> currWaveEnemies;

	//Controller ref
	GameController gameCtrl;

	GameObject enemyContainer;

	void Awake(){
		gameCtrl = GetComponent<GameController>();

		enemyContainer = new GameObject("EnemyContainer");
	}

	public void Init(){
		//load currWave

		//Debug
	}

	public void StartCurrentWave(){
		currWave = WaveLibrary.I.GetDefinition(gameCtrl.currWaveIdx);
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
			while (Time.time > waveStartTime + part.time){
//				Debug.Log("Spawning part at: " + (Time.time - waveStartTime));
				StartCoroutine(SpawnWavePart(part));

				wavePartIncr++;
				if (wavePartIncr < currWave.waveParts.Count) part = currWave.waveParts[wavePartIncr];
				else break;
			}
		}
	}

	private IEnumerator SpawnWavePart(WavePart part){
		for (int i = 0; i < part.count; i++) {
			//				foreach (WaveEnemy waveEnemy in part.enemies) {
			GameObject enemyGO = (GameObject) Instantiate(enemyPrefab, new Vector2(part.startX, gameCtrl.enemySpawnYPos), Quaternion.identity);
			Enemy enemy = enemyGO.GetComponent<Enemy>();

			enemy.Init(EnemyLibrary.I.GetDefinition(part.type));
			currWaveEnemies.Add(enemyGO.GetInstanceID(), enemy);

			enemyGO.transform.SetParent(enemyContainer.transform);

			if (i < part.count - 1){ //if is not last part
				yield return new WaitForSeconds(part.interval);
			}
		}
	}


	public int GetAmountMoneyForWave(int waveNr){
		//TODO
		return (waveNr + 1) * 100;
	}
}

