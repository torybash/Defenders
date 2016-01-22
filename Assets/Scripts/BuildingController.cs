using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour {

	[Header("Prefabs")]
	[SerializeField] GameObject buildingFieldPrefab;
	[SerializeField] GameObject scaffoldingPrefab;
	[SerializeField] GameObject buildingSelectorPrefab;
	[SerializeField] GameObject turretPrefab;
	[SerializeField] GameObject buildingPrefab;
	[SerializeField] GameObject buildingGhostPrefab;

	//Controller ref
	GameController gameCtrl;

	//Variables & refs
	GameObject selectedBuilding;
	GameObject buildingSelector;

	public List<Turret> turrets;

	Building[,] buildingSlots;
	Vector2[,] slotToPosition;

	GameObject[,] buildingGhosts;


	public List<Building> buildings;

	Rect buildingFieldRect;

	BuildingType placingBuildingType;

	GameObject ghostContainer;

	public float shotTimerOffset;
	public float nextAllowedShotTime;

	void Awake(){
		gameCtrl = GetComponent<GameController>();

		buildingSlots = new Building[gameCtrl.buildFieldWidth, gameCtrl.buildFieldHeight];
		slotToPosition = new Vector2[gameCtrl.buildFieldWidth, gameCtrl.buildFieldHeight];
		buildingGhosts = new GameObject[gameCtrl.buildFieldWidth, gameCtrl.buildFieldHeight];


		buildings = new List<Building>();

		ghostContainer = new GameObject("GhostContainer");
	}

	public void Init(){
		//Init building field
		for (int x = 0; x < gameCtrl.buildFieldWidth; x++) {
			for (int y = 0; y < gameCtrl.buildFieldHeight; y++) {
				float posX = -4f + x * 1f; 
				float posY = gameCtrl.camCtrl.GetBottomY() + gameCtrl.buildingFieldYAboveBottom - 1f + y * 1f;
				slotToPosition[x, y] = new Vector2(posX, posY);
			}
		}
		buildingFieldRect = new Rect(-4.5f, gameCtrl.camCtrl.GetBottomY() + gameCtrl.buildingFieldYAboveBottom - 1.5f, 9f, 3f);

		//Init building ghosts
		for (int x = 0; x < gameCtrl.buildFieldWidth; x++) {
			for (int y = 0; y < gameCtrl.buildFieldHeight; y++) {
				GameObject buildingGO = (GameObject) Instantiate(buildingGhostPrefab, slotToPosition[x, y], Quaternion.identity);
				buildingGhosts[x, y] = buildingGO;
				buildingGO.transform.SetParent(ghostContainer.transform);
			}
		}
		HideBuildingGhosts();


		//Spawn building field
		Vector2 pos = new Vector2(0, gameCtrl.camCtrl.GetBottomY() + gameCtrl.buildingFieldYAboveBottom);
		GameObject buildingField = (GameObject) Instantiate(buildingFieldPrefab, pos, Quaternion.identity);

		//Init variables
		turrets = new List<Turret>();

		//Spawn btm center turret
		BuildBuilding(4, 0, BuildingType.TURRET_MINIGUN);
		BuildBuilding(3, 0, BuildingType.POWER_STATION);
		BuildBuilding(2, 0, BuildingType.POWER_STATION);
		BuildBuilding(6, 0, BuildingType.POWER_STATION);
		BuildBuilding(5, 0, BuildingType.POWER_STATION);
	}


	public void InitWave(){
		//TODO only add offset if weapons are the same
		float cooldown = turrets[0].Stats.def.cooldownDuration;
		float offset = cooldown / (float)turrets.Count;

		shotTimerOffset = offset;

//		print("shotTimerOffset: " + shotTimerOffset);
//		public float shotTimerOffset;

//		foreach (Turret item in turrets) {
//			item.shotTimerOffset = offset * c;
//		}
	}

	public void IntermissionStarted(){
		//Repair all buildings - TODO cost money or sumthin?
		foreach (var item in buildings) {
			item.SetBuildingState(false);
		}
	}


	public void BuildingClicking(Vector2 mousePos){
		RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
		if (hit.collider != null && hit.collider.GetComponent<Building>() != null){
			//Show scaffold-selector
			if (buildingSelector == null) buildingSelector = Instantiate(buildingSelectorPrefab);
			buildingSelector.SetActive(true);
			buildingSelector.transform.position = hit.collider.transform.position;



			//Set values
			selectedBuilding = hit.collider.gameObject;


			//UI
			gameCtrl.uiCtrl.BuildingClicked(selectedBuilding.GetComponent<Building>());
		}
	}


	public void PlacingBuildingClicking(Vector2 mousePos){
		print("PlacingBuilding -  buildingFieldRect.Contains(mousePos): " + buildingFieldRect.Contains(mousePos));
		if (buildingFieldRect.Contains(mousePos)){
			int x = (int) (mousePos.x + 4.5f);
			int y = (int) (mousePos.y + 1.5f - gameCtrl.camCtrl.GetBottomY() - gameCtrl.buildingFieldYAboveBottom);

			if (!buildingGhosts[x, y].activeSelf) return; //is in ghost position

			print("PlacingBuilding - x,y: " + x + ", " + y);


			BuildBuilding(x, y, placingBuildingType);

			gameCtrl.PlacedBuilding(placingBuildingType);
		}
	}

	public void PlacingBuilding(BuildingType type){
		placingBuildingType = type;

		//SHOW BUILDING GHOSTS TODO
		bool[,] allowedPlacements = new bool[gameCtrl.buildFieldWidth, gameCtrl.buildFieldHeight];
		for (int x = 0; x < gameCtrl.buildFieldWidth; x++) {
			for (int y = 0; y < gameCtrl.buildFieldHeight; y++) {
				Building building = buildingSlots[x, y];

				if (building != null){

					for (int i = 0; i < 4; i++) {
						int adjX = x, adjY = y;
						if (i == 0) adjX += 1;
						else if (i == 1) adjX -= 1;
						else if (i == 2) adjY += 1;
						else if (i == 3) adjY -= 1;

						if (adjX < 0 || adjX >= gameCtrl.buildFieldWidth || adjY < 0 || adjY >= gameCtrl.buildFieldHeight) continue;

						if (buildingSlots[adjX, adjY] == null){ //if does not already contain building
							allowedPlacements[adjX, adjY] = true;
						}
					}
				}
			}
		}

		for (int x = 0; x < gameCtrl.buildFieldWidth; x++) {
			for (int y = 0; y < gameCtrl.buildFieldHeight; y++) {
				if (allowedPlacements[x, y]){
					GameObject ghostGO = buildingGhosts[x, y];
					ghostGO.SetActive(true);

					ghostGO.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.I.GetBuildingSprite(type);
				}
			}
		}
	}

	private void HideBuildingGhosts(){
		for (int x = 0; x < gameCtrl.buildFieldWidth; x++) {
			for (int y = 0; y < gameCtrl.buildFieldHeight; y++) {
				buildingGhosts[x, y].SetActive(false);
			}
		}
	}

	public void BuildBuilding(int x, int y, BuildingType type){
		//Calc world pos
		float posX = -4f + x * 1f; 
		float posY = gameCtrl.camCtrl.GetBottomY() + gameCtrl.buildingFieldYAboveBottom - 1f + y * 1f;
		Vector2 pos = new Vector2(posX, posY);

		//Get building def
		BuildingDefinition bd = BuildingLibrary.I.GetDefinition(type);

		//Hide selector
		if (buildingSelector != null) buildingSelector.gameObject.SetActive(false);
		
		//Create building
		GameObject buildingGO = (GameObject) Instantiate(buildingPrefab, pos, Quaternion.identity);
		Building building = buildingGO.GetComponent<Building>();
		building.Init(type, bd);

		//Initialise cannon
		if (bd.isTurret){
			Turret turret = buildingGO.GetComponent<Turret>();
			turret.Init(bd.turretDef);
			turrets.Add(turret);
		}

		//Set sprite
		buildingGO.GetComponent<SpriteRenderer>().sprite = SpriteLibrary.I.GetBuildingSprite(type);

		//Destroy scaffold
//		if (selectedBuilding != null) GameObject.Destroy(selectedBuilding.gameObject);
//		selectedBuilding = null;

		//Set values
		buildingSlots[x, y] = building;
		buildings.Add(building);

		gameCtrl.UpdateAmmoAndPower();

		HideBuildingGhosts();
	}


	public Building GetClosestBuilding(Vector2 pos){
		float closestDist = float.MaxValue;
		Building closestBuilding = null;
		foreach (Building bld in buildings) {
			if (bld.isDestroyed) continue;
			float dist = Vector2.Distance(pos, bld.transform.position);
			if (dist < closestDist){
				closestDist = dist;
				closestBuilding = bld;
			}
		}


		return closestBuilding;
	}


	public void BuildingDestroyed(Building building){
		building.SetBuildingState(true);
	}

}


public enum BuildingType{
	TURRET_ROCKET = 0,
	TURRET_MINIGUN = 1,
	POWER_STATION = 2,
	WEAPON_FACTORY = 3,
	MONEY_FACTORY = 4
}