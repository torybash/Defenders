using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(BuildingLibrary))]
public class BuildingLibraryEditor : Editor {

//	SpriteLibrary sprLib;
	SerializedObject getTarget;
	SerializedProperty buildingDefs;
//
	

	int testInt;

	private bool[] showTurretDef;

	void OnEnable(){
//		sprLib = (SpriteLibrary)target;
//		getTarget = new SerializedObject(sprLib);
		buildingDefs = serializedObject.FindProperty("definitions"); // Find the List in our script and create a refrence of it
////		buildingSpritesList = getTarget.FindProperty("buildingSprites"); // Find the List in our script and create a refrence of it
	}

	public override void OnInspectorGUI(){

//		BuildingLibrary controller = target as BuildingLibrary;


		serializedObject.Update();

		EditorGUI.BeginChangeCheck();

		if (showTurretDef == null || showTurretDef.Length != buildingDefs.arraySize){
			showTurretDef = new bool[buildingDefs.arraySize];
		}

		for(int i = 0; i < buildingDefs.arraySize; i++){



			SerializedProperty bldProp = buildingDefs.GetArrayElementAtIndex(i);
//			buildingSprite.

			

			SerializedProperty bldType = bldProp.FindPropertyRelative("type");
//			SerializedProperty buildingSpriteSprite = buildingSprite.FindPropertyRelative("sprite");
//			SerializedProperty buildingSpriteDestroyedSprite = buildingSprite.FindPropertyRelative("destroyedSprite");

			GUIStyle style = new GUIStyle();
			style.fontStyle = FontStyle.Bold;
			EditorGUILayout.LabelField("" + bldType.enumDisplayNames[bldType.enumValueIndex], style);

			bool isTurret = false;
//			for (int j = 0; j < typeof(BuildingDefinition).GetFields().Length; j++) {
			for (int j = 0; j < typeof(BuildingDefinition).GetFields().Length; j++) {
				FieldInfo field = typeof(BuildingDefinition).GetFields()[j];

				if (field.Name.Equals("isTurret") && bldProp.FindPropertyRelative(field.Name).boolValue) isTurret = true;



				if (field.Name.Equals("turretDef")){
					if (isTurret){
						SerializedProperty turretDefProp = bldProp.FindPropertyRelative(field.Name);

						showTurretDef[i] = EditorGUILayout.Foldout(showTurretDef[i], "Turret Definition");
						if (showTurretDef[i]){
							EditorGUI.indentLevel = 2;
							for (int k = 0; k < typeof(TurretDefinition).GetFields().Length; k++) {
								FieldInfo tField = typeof(TurretDefinition).GetFields()[k];
								SerializedProperty turretProp = turretDefProp.FindPropertyRelative(tField.Name);
								EditorGUILayout.PropertyField( turretProp );
							}
							EditorGUI.indentLevel = 0;
						}
					}
				}else{
					SerializedProperty serProp = bldProp.FindPropertyRelative(field.Name);

					EditorGUILayout.PropertyField( serProp );
				}
			}



//			EditorGUILayout.PropertyField(buildingSpriteSprite);
//			EditorGUILayout.PropertyField(buildingSpriteDestroyedSprite);

		}

		if (GUILayout.Button ("Add")){
			buildingDefs.InsertArrayElementAtIndex(buildingDefs.arraySize);
		}

		if(EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

	}
}
