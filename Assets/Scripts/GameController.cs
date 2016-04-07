﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	//Controller/library refs
	public CameraController camCtrl;
	public WaveController waveCtrl;
	public UIController uiCtrl;
	public BuildingController buildingCtrl;
	public ParticleController partCtrl;
	public AudioController audioCtrl;

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

    public GameState State {
        get { return state; }
    }


	public int currWaveIdx;
	public int score;
	public int highscore;
	public int money;
	public int powerUse;
	public int powerMax;

    public Dictionary<ProjectileType, int> turretAmmo = new Dictionary<ProjectileType, int>();
    public Dictionary<ProjectileType, float> turretReloadTimers = new Dictionary<ProjectileType, float>();


	[SerializeField] Transform groundPlane;
	[SerializeField] Transform ground;

	public Transform GroundPlane{
		get{ return groundPlane;}
	}

	public static GameController I;

	void Awake(){
		if (I == null){
			I = this;
		}else{
			Destroy(gameObject);
			return;
		}

		camCtrl = GetComponent<CameraController>();
		waveCtrl = GetComponent<WaveController>();
		uiCtrl = GetComponent<UIController>();
		buildingCtrl = GetComponent<BuildingController>();
		partCtrl = GetComponent<ParticleController>();
		audioCtrl = GetComponent<AudioController>();

        foreach (ProjectileType typ in System.Enum.GetValues(typeof(ProjectileType))) {
            turretAmmo[typ] = -1;
            turretReloadTimers[typ] = 0;
        }
	}


	void Start(){
		camCtrl.Init();
		waveCtrl.Init();
		buildingCtrl.Init();
		uiCtrl.Init();
		partCtrl.Init();
		audioCtrl.Init();
		Init();

		//DEBUG
		StartWave();
	}
	
	void Update () {
		Vector2 mousePos = camCtrl.cam.ScreenToWorldPoint(Input.mousePosition);
        bool mousePressed = Input.GetMouseButton(0);
        bool mouseClicked = Input.GetMouseButtonDown(0);
        switch (state) {
		case GameState.GAME:

			if (mousePressed){
                bool clickedBuilding = mouseClicked && buildingCtrl.BuildingClicking(mousePos);
				if (!clickedBuilding) Shooting(mousePos);
			}

			break;
		case GameState.INTERMISSION:

			switch (intmsnState) {
			case IntermissionState.NORMAL:
                if (mouseClicked) buildingCtrl.BuildingClicking(mousePos);
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
    

        //RELOAD
        foreach (ProjectileType typ in System.Enum.GetValues(typeof(ProjectileType))) {
            if (turretAmmo[typ] <= 0 && Time.time > turretReloadTimers[typ]) {
                foreach (Turret turret in buildingCtrl.turrets) {
                    if (turret.Stats.def.type == typ) {
                        turretAmmo[typ] += turret.Stats.def.maxAmmo;
                    }
                }
                //turretAmmo[typ] 
            }
        }
	}

	private void Shooting(Vector2 mousePos){

        //if (Time.time >= buildingCtrl.nextAllowedShotTime){
        //int c = 0;
		foreach (Turret turret in buildingCtrl.turrets) {
            //if (Time.time >= buildingCtrl.nextAllowedShotTime) {
            //Debug.Log("turret " + c++ + " trying shooting - type: " + turret.Building.type + ", nextAllowedShotTime: " + buildingCtrl.TurretSyncDataDict[turret.Building.type].nextAllowedShotTime + ", shotTimerOffset: " + buildingCtrl.TurretSyncDataDict[turret.Building.type].shotTimerOffset + ", time: "+ Time.time);
            if (turretAmmo[turret.Stats.def.type] <= 0) continue;

            if (Time.time >= buildingCtrl.TurretSyncDataDict[turret.Building.type].nextAllowedShotTime) {
                bool turretShot = turret.TryShootAt(mousePos);
			    if (turretShot){
                    //buildingCtrl.nextAllowedShotTime = Time.time + buildingCtrl.shotTimerOffset;
                    buildingCtrl.TurretSyncDataDict[turret.Building.type].nextAllowedShotTime = Time.time + buildingCtrl.TurretSyncDataDict[turret.Building.type].shotTimerOffset;

                    turretAmmo[turret.Stats.def.type]--;
                    if (turretAmmo[turret.Stats.def.type] <= 0) {
                        turretReloadTimers[turret.Stats.def.type] = Time.time + turret.Stats.def.reloadDuration;
                    }
                    UpdateInfoPanel();
				    break;
			    }
            }
		}
	}


	

	private void Init(){

		float groundHeight = ground.transform.localScale.y;
		ground.position = new Vector3(0, camCtrl.GetBottomY() + buildingFieldYAboveBottom - buildFieldHeight/2f - groundHeight/2f);

		money = 0;
        //turretAmmo[ProjectileType.EXPLODING] = 10;


		//Set values
		UpdateInfoPanel();
	
	}

	public bool CanAffordBuilding(BuildingType type){
		if (money >= BuildingLibrary.I.GetDefinition(type).buildPrice) return true;
		return false;
	}

	public void PlacedBuilding(BuildingType type){
		//Update values
		intmsnState = IntermissionState.NORMAL;

		//pay
		money -= BuildingLibrary.I.GetDefinition(type).buildPrice;

		//UI
		UpdateInfoPanel();
	}

	private void UpdateInfoPanel(){
		//UI
        uiCtrl.UpdateInfoPanel(money, score, powerUse, powerMax, turretAmmo[ProjectileType.EXPLODING], turretAmmo[ProjectileType.NORMAL]);
        //uiCtrl.UpdateInfoPanel(money, score, powerUse, powerMax, 0, 0);
	}

	public void BuyBuilding(BuildingType type){

		//Tell build contoller
		buildingCtrl.PlacingBuilding(type);

		//Update values
		intmsnState = IntermissionState.PLACING_BUILDING;


	}

	private void StartWave(){
		Debug.Log("StartWave " + currWaveIdx);
		waveCtrl.StartCurrentWave();
		uiCtrl.ShowWaveIntro(currWaveIdx);
		buildingCtrl.InitWave();
		SwitchState(GameState.GAME);


	}

	public void TestFunc(){
		print("TestFunc!!!");
		uiCtrl.ShowWaveIntro(100);

	}


	public void WaveCompleted(){
		uiCtrl.ShowWaveOutro(currWaveIdx);

//		Timer.CallDelayed(GoToIntermission, endWaveDuration);

		SwitchState(GameState.WAVE_ENDED);
	
		audioCtrl.PlayComplete();
	}


	public void GoToIntermission(){
		SwitchState(GameState.INTERMISSION);
		buildingCtrl.IntermissionStarted();
		
		//DEBUG
		money += waveCtrl.GetAmountMoneyForWave(currWaveIdx);
		UpdateInfoPanel();
		
		UpdateAmmoAndPower();
	}

	public void MakeExplosionAt(Vector2 pos){
		GameObject explosionGO = (GameObject) Instantiate(explosionPrefab, pos, Quaternion.identity);
		explosionGO.GetComponent<Explosion>().Init();

	}

	public void GoToNextWave(){
		currWaveIdx++;
		StartWave();
	}


	public void UpdateAmmoAndPower(){

        foreach (ProjectileType typ in System.Enum.GetValues(typeof(ProjectileType))) {
            turretAmmo[typ] = -1;
        }
		powerUse = 0;
		powerMax = 0;
		foreach (Building building in buildingCtrl.buildings) {
			if (building == null || building.stats == null) continue;
			BuildingDefinition bd = BuildingLibrary.I.GetDefinition(building.stats.def.type);
            if (building.stats.def.type == BuildingType.TURRET_ROCKET || building.stats.def.type == BuildingType.TURRET_MINIGUN) {
				Turret turret = building.GetComponent<Turret>();
                if (turretAmmo[turret.Stats.def.type] == -1) turretAmmo[turret.Stats.def.type] = 0;

                turretAmmo[turret.Stats.def.type] += turret.Stats.def.maxAmmo;
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

	public void LostGame(){
		Application.LoadLevel("Menu");
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
