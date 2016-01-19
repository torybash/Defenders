using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InfoPanel : MonoBehaviour {



	[SerializeField] Text moneyText;
	[SerializeField] Text scoreText;
	[SerializeField] Text powerText;
	[SerializeField] Text rocketsText;
	[SerializeField] Text bulletsText;



	void Start(){
		Vector2 btmOfScreenInScreenPos = new Vector2(0.5f, 0);
		Vector2 btmOfScreenInWorldPos = Camera.main.ViewportToWorldPoint(btmOfScreenInScreenPos);
//		Vector3 
		Vector2 btmOfBuildingsWorldPos = new Vector2(0, btmOfScreenInWorldPos.y + GameController.I.buildingFieldYAboveBottom - (GameController.I.buildFieldHeight / 2f));
		Vector2 btmOfBuildingsScreenPos = Camera.main.WorldToViewportPoint(btmOfBuildingsWorldPos);
	
		Debug.Log("btmOfScreenInScreenPos: " + btmOfScreenInScreenPos);
		Debug.Log("btmOfScreenInWorldPos: "+ btmOfScreenInWorldPos);
		Debug.Log("btmOfBuildingsWorldPos: "+ btmOfBuildingsWorldPos);
		Debug.Log("btmOfBuildingsScreenPos: " + btmOfBuildingsScreenPos);

		CanvasScaler cs = transform.parent.GetComponent<CanvasScaler>();
		btmOfBuildingsScreenPos.x = 0;
		GetComponent<RectTransform>().anchorMax = new Vector2(1f, btmOfBuildingsScreenPos.y);

	}

	public void UpdateInfoPanel(int money, int score, int powerUse, int powerMax, int rockets, int bullets){

		moneyText.text = "$" + money;
		scoreText.text = "Score: " + score;
		powerText.text = "¤" + powerUse + " / " + powerMax;
		rocketsText.text = "R: "+ rockets;
		bulletsText.text = "B: " + bullets;
	}
}
