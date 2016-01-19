using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveInfoPanel : MonoBehaviour {

	
	private enum State{
		DISABLED,
		INTRO,
		OUTRO_START,
		OUTRO_COUNTING_BUILDING_SCORE,
		OUTRO_COUNTING_AMMO_SCORE,
		OUTRO_BONUS_MONEY,
		OUTRO_END
	}

	[SerializeField] Text textHeader;
	[SerializeField] Text textBuildingsScore;
	[SerializeField] Text textAmmoScore;
	[SerializeField] Text textMoneyBonus;

	//Constants
	public float introTextDuration = 2f;
	public float introTextFadeTime = 0.5f;

	public float outroStartDuration = 0.6f;
	public float outroAfterCountWaitDuration = 0.6f;
	public float outroEndWaitDuration = 2.5f;


	public float outroCountBuildingSpeed = 0.4f;
	public float outroCountAmmoSpeed = 0.1f;


	//Variables
	float stateSwitchTime;
	WaveCompleteData currData;
	
	State state = State.DISABLED;

	int typeIncr;
	int objIncr;

	int objCount;
	int totScore;

	float nextCountTime;

	UIController uiCtrl;

	void Awake(){
		EnableElements(false);
	}

	void Update(){
		switch (state) {
		case State.OUTRO_START:
			if (Time.time > stateSwitchTime){
				state = State.OUTRO_COUNTING_BUILDING_SCORE;
				typeIncr = 0;
				objIncr = 0;

				objCount = 0;
				totScore = 0;
			}
			break;
		case State.OUTRO_COUNTING_BUILDING_SCORE:
			if (Time.time <= nextCountTime) return;

			//Has checked all types?
			if (typeIncr >= (int) System.Enum.GetValues(typeof(BuildingType)).Length){
				state = State.OUTRO_COUNTING_AMMO_SCORE;
				typeIncr = 0;
				objIncr = 0;
				
				objCount = 0;
				totScore = 0;
				break;
			}

			//any buildings left of typeIncr?
			BuildingType buildingType = (BuildingType) typeIncr;
			if (currData.buildingsLeft[buildingType] > objIncr){

				//Update UI
				objCount++;
				totScore += 100; //TODO

				nextCountTime = Time.time + outroCountBuildingSpeed;
				objIncr++;
			}else{
				typeIncr++;
				objIncr = 0;
			}

//			if (currData.buildingsLeft[


			textBuildingsScore.text = "" + objCount + " buildings = " + totScore;
			
			break;
		case State.OUTRO_COUNTING_AMMO_SCORE:
			
			if (Time.time <= nextCountTime) return;

			state = State.OUTRO_BONUS_MONEY;
			stateSwitchTime = Time.time + outroAfterCountWaitDuration;


			//Has checked all types?
//			if (typeIncr >= (int) ProjectileType.AMOUNT){
//				state = State.OUTRO_BONUS_MONEY;
//				stateSwitchTime = Time.time + outroAfterCountWaitDuration;
//				break;
//			}
			
			//any buildings left of typeIncr?
//			ProjectileType turretType = (ProjectileType) typeIncr;
//			if (currData.ammoLeft[turretType] > objIncr){
//				
//				//Update UI
//				objCount++;
//				totScore += 10; //TODO
//				
//				nextCountTime = Time.time + outroCountAmmoSpeed;
//				objIncr++;
//			}else{
//				typeIncr++;
//				objIncr = 0;
//			}
			
			//			if (currData.buildingsLeft[
			
			
//			textAmmoScore.text = "" + objCount + " ammo = " + totScore;
			
			break;
		case State.OUTRO_BONUS_MONEY:
			if (Time.time > stateSwitchTime){
				textMoneyBonus.text = "+ $" + currData.moneyBonus;
				state = State.OUTRO_END;
				stateSwitchTime = Time.time + outroEndWaitDuration;

			}
			break;
		case State.OUTRO_END:
			if (Time.time > stateSwitchTime){
				EnableElements(false);

				state = State.DISABLED;

				uiCtrl.WaveOutroComplete();
			}
			break;
		
		default:
			break;
		}

	}


	public void Init(UIController uiCtrl){
		this.uiCtrl = uiCtrl;
	}

	public void ShowWaveIntro(int waveNr){
		state = State.INTRO;

		EnableElements(true);

		textHeader.color = new Color(1, 1, 1, 1);
		textHeader.text = "Wave " + (waveNr + 1);

		StartCoroutine(FadeOutTextCR(textHeader, introTextFadeTime, introTextDuration));

	}


	public void ShowWaveOutro(WaveCompleteData data){
		state = State.OUTRO_START;
		currData = data;

		EnableElements(true);

		textHeader.color = new Color(1, 1, 1, 1);
		textHeader.text = "Wave complete!";

		stateSwitchTime = Time.time + outroStartDuration;
	}



	private void EnableElements(bool on){
		textHeader.gameObject.SetActive(on);
		textBuildingsScore.gameObject.SetActive(on);
		textAmmoScore.gameObject.SetActive(on);
		textMoneyBonus.gameObject.SetActive(on);

		if (on){
			textHeader.text = "";
			textBuildingsScore.text = "";
			textAmmoScore.text = "";
			textMoneyBonus.text = "";
		}
	}

	//COROUTINES
	
	private IEnumerator FadeOutTextCR(Text text, float duration, float delay){

		yield return new WaitForSeconds(delay);

		float startTime = Time.time;

		while (Time.time < startTime + duration && state == State.INTRO){
			float progress = ((startTime + duration - Time.time) / duration); // 1 -> 0
			text.color = new Color(1, 1, 1, progress);

			yield return null;
		}
		if (state == State.INTRO){
			text.color = new Color(1, 1, 1, 0);
			text.gameObject.SetActive(false);
			state = State.DISABLED;
		}
	}
}


public class WaveCompleteData{
	public Dictionary<ProjectileType, int> ammoLeft;
	public Dictionary<BuildingType, int> buildingsLeft;
	public int moneyBonus;
}
