﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InfoPanel : MonoBehaviour {



	[SerializeField] Text moneyText;
	[SerializeField] Text scoreText;
	[SerializeField] Text powerText;
    //[SerializeField] Text rocketsText;
    //[SerializeField] Text bulletsText;
    [SerializeField] WeaponBox explodeBox;
    [SerializeField] WeaponBox normalBox;
    Dictionary<ProjectileType, WeaponBox> weaponBoxes = new Dictionary<ProjectileType, WeaponBox>();



    void Awake() {
        weaponBoxes.Add(ProjectileType.EXPLODING, explodeBox);
        weaponBoxes.Add(ProjectileType.NORMAL, normalBox);
    }

	void Start(){
		Vector2 btmOfScreenInScreenPos = new Vector2(0.5f, 0);
		Vector2 btmOfScreenInWorldPos = Camera.main.ViewportToWorldPoint(btmOfScreenInScreenPos);
		Vector2 btmOfBuildingsWorldPos = new Vector2(0, btmOfScreenInWorldPos.y + GameController.I.buildingFieldYAboveBottom - (GameController.I.buildFieldHeight / 2f));
		Vector2 btmOfBuildingsScreenPos = Camera.main.WorldToViewportPoint(btmOfBuildingsWorldPos);
	
//		Debug.Log("btmOfScreenInScreenPos: " + btmOfScreenInScreenPos);
//		Debug.Log("btmOfScreenInWorldPos: "+ btmOfScreenInWorldPos);
//		Debug.Log("btmOfBuildingsWorldPos: "+ btmOfBuildingsWorldPos);
//		Debug.Log("btmOfBuildingsScreenPos: " + btmOfBuildingsScreenPos);

		CanvasScaler cs = transform.parent.GetComponent<CanvasScaler>();
		btmOfBuildingsScreenPos.x = 0;
		GetComponent<RectTransform>().anchorMax = new Vector2(1f, btmOfBuildingsScreenPos.y);

	}

	public void UpdateInfoPanel(int money, int score, int powerUse, int powerMax, int rockets, int bullets){

		moneyText.text = "$" + money;
		scoreText.text = "Score: " + score;
		powerText.text = "¤" + powerUse + " / " + powerMax;

        RefreshWeaponBox(ProjectileType.NORMAL, bullets, bullets == 0, 0.6f);
        RefreshWeaponBox(ProjectileType.EXPLODING, rockets, rockets == 0, 1.5f);
    }

    public void RefreshWeaponBox(ProjectileType type, int count, bool startReload = false, float duration = 0) {
        weaponBoxes[type].UpdateCount(count);
        if (startReload) {
            weaponBoxes[type].StartReload(duration);
        }

    }
}
