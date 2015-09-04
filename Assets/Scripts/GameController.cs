using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	//Controller refs
	CameraController camCtrl;
	WaveController waveCtrl;
	UIController uiCtrl;

	//Object references
	[Header("References")]
	[SerializeField] Transform cannonMidPos;
	[SerializeField] Transform cannonRightPos;
	[SerializeField] Transform cannonLeftPos;

	//Prefabs
	[Header("Prefabs")]
	[SerializeField] GameObject scaffoldingPrefab;
	[SerializeField] GameObject buildingSelectorPrefab;
	[SerializeField] GameObject turretPrefab;
	[SerializeField] GameObject explosionPrefab;

	//Constants
	const int scaffoldsAmount = 8;
	const float scaffoldsYPos = -6.5f;
	const float enemySpawnYPos = 8.5f;
	const float enemyGoalYPos = -6.5f;
	const float height = 16f;
	const float width = 10f;

	//Variables & refs
	GameState state;

	GameObject selectedBuilding;
	GameObject buildingSelector;


	List<Cannon> cannons;

	public int currWave;
	public int score;
	public int highscore;
	public int money;

	void Awake(){
		camCtrl = GetComponent<CameraController>();
		waveCtrl = GetComponent<WaveController>();
		uiCtrl = GetComponent<UIController>();
	}


	void Start(){
		camCtrl.Init();
		waveCtrl.Init();
		Init();

		//DEBUG
		StartWave();
	}
	
	void Update () {
		switch (state) {
		case GameState.GAME:
			Vector2 mousePos = camCtrl.cam.ScreenToWorldPoint(Input.mousePosition);
			Shooting(mousePos);
//			BuildingClicking(mousePos);


			break;
		case GameState.INGAME_MENU:
			//TODO/DEBUG
			if (Input.GetMouseButtonDown(0)){
//				ConstructTurret();
			}
			break;
		default:
			break;
		}

		waveCtrl.GUpdate();
	}

	private void Shooting(Vector2 mousePos){
		if (Input.GetMouseButton(0)){
			if (mousePos.y > scaffoldsYPos + 0.5f){
				foreach (Cannon cannon in cannons) {
					cannon.TryShootAt(mousePos);
				}
			}
		}
	}

	private void BuildingClicking(Vector2 mousePos){
		if (Input.GetMouseButtonDown(0) && mousePos.y < scaffoldsYPos + 0.5f){
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
			if (hit.collider != null){
				//Show scaffold-selector
				if (buildingSelector == null) buildingSelector = Instantiate(buildingSelectorPrefab);
				buildingSelector.SetActive(true);
				buildingSelector.transform.position = hit.collider.transform.position;


				//Set values
				selectedBuilding = hit.collider.gameObject;
				state = GameState.INGAME_MENU;
			}
		}
	}

	

	private void Init(){
		//Set values
		cannons = new List<Cannon>();
		state = GameState.GAME;

		//Spawn scaffolding
		Vector2 spawnPos = new Vector2(0, scaffoldsYPos);
		for (int i = 0; i < scaffoldsAmount; i++) {

					//startX  width   offset
			spawnPos.x = -4f + 0.5f + 1f * i;

			GameObject scaffoldT = (GameObject) Instantiate(scaffoldingPrefab, spawnPos, Quaternion.identity);

		}

		//Spawn mid turret
		ConstructTurret(cannonMidPos.position);
	}


	private void ConstructTurret(Vector2 pos){
//		if (selectedBuilding == null) throw new UnassignedReferenceException("selectedScaffold == null");

		//Hide selector
//		buildingSelector.gameObject.SetActive(false);

		//Create turret
		GameObject cannonT = (GameObject) Instantiate(turretPrefab, pos, Quaternion.identity);
		Cannon cannon = cannonT.GetComponent<Cannon>();
		cannon.Init(CannonStats.DefaultCannon);

		//Destroy scaffold
//		GameObject.Destroy(selectedBuilding.gameObject);
//		selectedBuilding = null;

		//Set values
		cannons.Add(cannon);
		state = GameState.GAME;
	}

	private void StartWave(){
		waveCtrl.StartCurrentWave();
		uiCtrl.ShowWaveIntro(currWave);
	}

	public void WaveDestroyed(){

	}


	public void MakeExplosionAt(Vector2 pos){
		GameObject explosionGO = (GameObject) Instantiate(explosionPrefab, pos, Quaternion.identity);
		explosionGO.GetComponent<Explosion>().Init();

	}


	public bool IsOutsideBounds(Vector2 pos, float size = 0f){
		if (pos.x - size > width / 2f || pos.x + size < -width / 2f ||
		    pos.y - size > height / 2f || pos.y + size < -height / 2f)
		{
			return true;
		}
		return false;
	}


	public float GetEnemySpawnPosY(){
		return enemySpawnYPos;
	}

	public float GetEnemyGoalPosY(){
		return enemyGoalYPos;
	}
}


public enum GameState{
	MENU = 0,
	GAME = 1,
	INGAME_MENU = 2

}
