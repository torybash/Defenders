using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildElement : MonoBehaviour {

	[SerializeField] Text nameText;
	[SerializeField] Text priceText;
	[SerializeField] Text powerText;

	public BuildingType buildingType;

	UIController uiCtrl;

	Toggle toggle;

	void Awake(){
		toggle = GetComponent<Toggle>();
	}

	public void Init(BuildingDefinition bd, UIController uiCtrl){

		nameText.text = bd.name;
		priceText.text = "$" + bd.buildPrice;
		powerText.text = "¤" + bd.powerUse;

		buildingType = bd.type;

		this.uiCtrl = uiCtrl;
	}


	public void ValueChanged(){
		print("ValueChanged - isOn: "+ toggle.isOn);
		if (toggle.isOn) uiCtrl.SelectBuildingType(buildingType);
	}
}
