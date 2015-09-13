using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoPanel : MonoBehaviour {

	[SerializeField] Text moneyText;
	[SerializeField] Text scoreText;
	[SerializeField] Text powerText;
	[SerializeField] Text rocketsText;
	[SerializeField] Text bulletsText;


	public void UpdateInfoPanel(int money, int score, int powerUse, int powerMax, int rockets, int bullets){

		moneyText.text = "$" + money;
		scoreText.text = "Score: " + score;
		powerText.text = "¤" + powerUse + " / " + powerMax;
		rocketsText.text = "R: "+ rockets;
		bulletsText.text = "B: " + bullets;
	}
}
