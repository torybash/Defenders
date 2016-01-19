using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

	[Header("UI references")]
	[SerializeField] Text textWaveIntro;
	[SerializeField] RectTransform panelBuildMenu;
	[SerializeField] RectTransform panelIntermissionMenu;
	[SerializeField] ScrollRect scrollRectBuildMenu;
	[SerializeField] LayoutGroup layoutBuildPanel;

	[SerializeField] InfoPanel infoPanel;
	[SerializeField] WaveInfoPanel waveInfoPanel;
	[SerializeField] BuildingInfoPanel buildingInfoPanel;

	[Header("Prefabs")]
	[SerializeField] GameObject buildPanelElementPrefab;

	//Variables & refs
	RectTransform contentRectTransform;
	RectTransform currentPanel;

	bool hasSelectedBuildingType;
	BuildingType selectedBuildingType;


	//Controller ref
	GameController gameCtrl;

	void Awake(){
		gameCtrl = GetComponent<GameController>();

		contentRectTransform = scrollRectBuildMenu.content;
	}


	public void Init(){
		//Initialize build panel
	
		foreach (BuildingType bt in Enum.GetValues(typeof(BuildingType))) {

			BuildingDefinition bd = BuildingLibrary.I.GetDefinition(bt);
			GameObject buildPanelElement = Instantiate(buildPanelElementPrefab);
			buildPanelElement.transform.SetParent(layoutBuildPanel.transform);
			buildPanelElement.transform.localScale = Vector3.one;
			buildPanelElement.GetComponent<Toggle>().group = layoutBuildPanel.GetComponent<ToggleGroup>();

			buildPanelElement.GetComponent<BuildElement>().Init(bd, this);
		}

		waveInfoPanel.Init(this);

	}

	public void ShowWaveIntro(int waveNr){
		waveInfoPanel.ShowWaveIntro(waveNr);

//		textWaveIntro.color = new Color(1, 1, 1, 1);
//		textWaveIntro.gameObject.SetActive(true);
//		textWaveIntro.text = "Wave " + (waveNr + 1);
//		
//		StartCoroutine(FadeOutTextCR(textWaveIntro, 0.5f, 2f));
	}

	public void ShowWaveOutro(int waveNr){

		WaveCompleteData data = new WaveCompleteData();
		data.ammoLeft = new Dictionary<ProjectileType, int>();
//		foreach (var item in gameCtrl.turretAmmo) data.ammoLeft.Add(item.Key, item.Value);

		data.buildingsLeft = new Dictionary<BuildingType, int>();
		for (int i = 0; i < System.Enum.GetValues(typeof(BuildingType)).Length; i++) {
			data.buildingsLeft.Add((BuildingType) i, 0);
		}
		foreach (Building item in gameCtrl.buildingCtrl.buildings) {

//			Debug.Log("bldng: "+ item + ", item.stats.type: " + item.stats.type);

			data.buildingsLeft[item.stats.type] += 1;
		}
		data.moneyBonus = gameCtrl.waveCtrl.GetAmountMoneyForWave(gameCtrl.currWaveIdx);

//		gameCtrl.turretAmmo
//		data.ammoLeft = 

		waveInfoPanel.ShowWaveOutro(data);

//		textWaveIntro.color = new Color(1, 1, 1, 1);
//		textWaveIntro.gameObject.SetActive(true);
//		textWaveIntro.text = "Wave completed!";
//		
//		StartCoroutine(FadeOutTextCR(textWaveIntro, 0.5f, 2f));
	}



	public void BuildClicked(){
		OpenPanel(panelBuildMenu);
		hasSelectedBuildingType = false;
	}

	public void SellClicked(){
//		panelBuildMenu.gameObject.SetActive(true);
	}

	public void ResearchClicked(){
//		panelBuildMenu.gameObject.SetActive(true);
	}

	public void CloseMenuClicked(){
		CloseCurrentPanel();
	}


	public void NextWaveClicked(){
		gameCtrl.GoToNextWave();
	}


	private void CloseCurrentPanel(){
		if (currentPanel != null) currentPanel.gameObject.SetActive(false);
		currentPanel = null;
	}

	private void OpenPanel(RectTransform panel){
		CloseCurrentPanel();
		currentPanel = panel;
		panel.gameObject.SetActive(true);
	}

	public void SelectBuildingType(BuildingType type){
		selectedBuildingType = type;
		hasSelectedBuildingType = true;
	}

	public void BuySelectedBuilding()
	{
		//Can afford building?
		if (gameCtrl.CanAffordBuilding(selectedBuildingType)){
			CloseCurrentPanel();
			gameCtrl.BuyBuilding(selectedBuildingType);
		}else{
			//TODO show message
		}
	}



	public void SwitchState(GameState state){

		switch (state) {
		case GameState.GAME:
			panelIntermissionMenu.gameObject.SetActive(false);
			break;

		case GameState.INTERMISSION:
			panelIntermissionMenu.gameObject.SetActive(true);
			break;

		default:
			break;
		}


	}

	public void UpdateInfoPanel(int money, int score, int powerUse, int powerMax, int rockets, int bullets){
		infoPanel.UpdateInfoPanel(money, score, powerUse, powerMax, rockets, bullets);
	}


	public void WaveOutroComplete(){
		gameCtrl.GoToIntermission();
	}


	public void BuildingClicked(Building building){
		OpenPanel(buildingInfoPanel.GetComponent<RectTransform>());
		buildingInfoPanel.Init(building);
	}



}
