using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	//Controller/library refs
	public CameraController camCtrl;
	public WaveController waveCtrl;
	public UIController uiCtrl;
	public BuildingController buildingCtrl;

	public BuildingLibrary buildingLib;
	public SpriteLibrary spriteLib;


	//Object references
	[Header("References")]
	[SerializeField] Transform cannonMidPos;
	[SerializeField] Transform cannonRightPos;
	[SerializeField] Transform cannonLeftPos;

	//Prefabs
	[Header("Prefabs")]

	[SerializeField] GameObject explosionPrefab;

	//Constants
	public float scaffoldsYPos = -6f;
	public float enemySpawnYPos = 8.5f;
	public float enemyGoalYPos = -6.5f;
	public float height = 16f;
	public float width = 10f;
	public int buildFieldWidth = 9;
	public int buildFieldHeight = 3;
	public float buildingFieldYAboveBottom = 3.5f;

	public float endWaveDuration = 6f;

	//Variables & refs
	GameState state;
	IntermissionState intmsnState;




	public int currWave;
	public int score;
	public int highscore;
	public int money;
	public int powerUse;
	public int powerMax;

	public Dictionary<ProjectileType, int> turretAmmo = new Dictionary<ProjectileType, int>();


	void Awake(){
		camCtrl = GetComponent<CameraController>();
		waveCtrl = GetComponent<WaveController>();
		uiCtrl = GetComponent<UIController>();
		buildingCtrl = GetComponent<BuildingController>();

		buildingLib = GameObject.FindGameObjectWithTag("GameLibrary").GetComponent<BuildingLibrary>();
		spriteLib = GameObject.FindGameObjectWithTag("GameLibrary").GetComponent<SpriteLibrary>();

		for (int i = 0; i < (int)ProjectileType.AMOUNT; i++) {
			turretAmmo.Add((ProjectileType) i, 0);
		}
	}


	void Start(){
		camCtrl.Init();
		waveCtrl.Init();
		buildingCtrl.Init();
		uiCtrl.Init();
		Init();

		//DEBUG
		StartWave();
	}
	
	void Update () {
		Vector2 mousePos = camCtrl.cam.ScreenToWorldPoint(Input.mousePosition);
		bool mousePressed = Input.GetMouseButton(0);
		switch (state) {
		case GameState.GAME:

			if (mousePressed){
				Shooting(mousePos);
				if (mousePressed) buildingCtrl.BuildingClicking(mousePos);
			}


			break;
		case GameState.INTERMISSION:

			switch (intmsnState) {
			case IntermissionState.NORMAL:
				if (mousePressed) buildingCtrl.BuildingClicking(mousePos);
				break;
			case IntermissionState.PLACING_BUILDING:
				if (mousePressed) buildingCtrl.PlacingBuildingClicking(mousePos);
				break;
			default:
			break;
			}


			break;
		default:
			break;
		}

		waveCtrl.GUpdate();
	}

	private void Shooting(Vector2 mousePos){

		if (Time.time >= buildingCtrl.nextAllowedShotTime){
			foreach (Turret turret in buildingCtrl.turrets) {
				//Has ammo for turret?
				if (turretAmmo[turret.stats.type] > 0){

					bool turretShot = turret.TryShootAt(mousePos);
					if (turretShot){ 
						buildingCtrl.nextAllowedShotTime = Time.time + buildingCtrl.shotTimerOffset;
						turretAmmo[turret.stats.type] -= 1;
						UpdateInfoPanel();
						break;
					}
				}
			}
		}

	}


	

	private void Init(){

		money = 0;
		turretAmmo[ProjectileType.EXPLODING] = 10;


		//Set values
		UpdateInfoPanel();
	
	}

	public bool CanAffordBuilding(BuildingType type){
		if (money >= buildingLib.GetBuildingDefinition(type).buildPrice) return true;
		return false;
	}

	public void PlacedBuilding(BuildingType type){
		//Update values
		intmsnState = IntermissionState.NORMAL;

		//pay
		money -= buildingLib.GetBuildingDefinition(type).buildPrice;

		//UI
		UpdateInfoPanel();
	}

	private void UpdateInfoPanel(){
		//UI
		uiCtrl.UpdateInfoPanel(money, score, powerUse, powerMax, turretAmmo[ProjectileType.EXPLODING], turretAmmo[ProjectileType.NORMAL]);
	}

	public void BuyBuilding(BuildingType type){

		//Tell build contoller
		buildingCtrl.PlacingBuilding(type);

		//Update values
		intmsnState = IntermissionState.PLACING_BUILDING;


	}

	private void StartWave(){
		waveCtrl.StartCurrentWave();
		uiCtrl.ShowWaveIntro(currWave);
		buildingCtrl.InitWave();
		SwitchState(GameState.GAME);


	}

	public void TestFunc(){
		print("TestFunc!!!");
		uiCtrl.ShowWaveIntro(100);

	}


	public void WaveCompleted(){
		uiCtrl.ShowWaveOutro(currWave);

//		Timer.CallDelayed(GoToIntermission, endWaveDuration);

		SwitchState(GameState.WAVE_ENDED);
	}


	public void GoToIntermission(){
		SwitchState(GameState.INTERMISSION);
		buildingCtrl.IntermissionStarted();
		
		//DEBUG
		money += waveCtrl.GetAmountMoneyForWave(currWave);
		UpdateInfoPanel();
		
		UpdateAmmoAndPower();
	}

	public void MakeExplosionAt(Vector2 pos){
		GameObject explosionGO = (GameObject) Instantiate(explosionPrefab, pos, Quaternion.identity);
		explosionGO.GetComponent<Explosion>().Init();

	}

	public void GoToNextWave(){
		currWave++;
		StartWave();
	}


	public void UpdateAmmoAndPower(){
//		Dictionary<TurretType, int> ammo = new Dictionary<TurretType, int>();

		for (int i = 0; i < (int)ProjectileType.AMOUNT; i++) {
			turretAmmo[(ProjectileType) i] = 0;
		}
		powerUse = 0;
		powerMax = 0;
		foreach (Building building in buildingCtrl.buildings) {
			BuildingDefinition bd = buildingLib.GetBuildingDefinition(building.stats.type);
			if (building.stats.type == BuildingType.TURRET_ROCKET || building.stats.type == BuildingType.TURRET_MINIGUN){
				Turret turret = building.GetComponent<Turret>();
				turretAmmo[turret.stats.type] += turret.stats.ammoMax;
			}

			if (bd.powerUse > 0){
				powerMax += bd.powerUse;
			}else{
				powerUse += -bd.powerUse;
			}
		}

		UpdateInfoPanel();
	}

	public bool IsOutsideBounds(Vector2 pos, float size = 0f){
		if (pos.x - size > width / 2f || pos.x + size < -width / 2f ||
		    pos.y - size > height / 2f || pos.y + size < -height / 2f)
		{
			return true;
		}
		return false;
	}


	private void SwitchState(GameState state){
		this.state = state;

		switch (state) {
		case GameState.GAME:

			break;
		case GameState.INTERMISSION:
			intmsnState = IntermissionState.NORMAL;
			break;
		default:
			break;
		}

		uiCtrl.SwitchState(state);
	}



	//COROUTINES
//	private IEnumerator EndGame
}


public enum GameState{
	MENU = 0,
	GAME = 1,
	WAVE_ENDED = 2,
	INTERMISSION = 3

}

public enum IntermissionState{
	NORMAL = 0,
	PLACING_BUILDING = 1
}
