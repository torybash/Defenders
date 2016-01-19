using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(WaveLibrary))]
public class WaveLibraryEditor : Editor {

	SerializedObject getTarget;
	SerializedProperty buildingDefs;



	void OnEnable(){
//		sprLib = (SpriteLibrary)target;
//		getTarget = new SerializedObject(sprLib);
//		buildingDefs = serializedObject.FindProperty("buildingDefs"); // Find the List in our script and create a refrence of it
////		buildingSpritesList = getTarget.FindProperty("buildingSprites"); // Find the List in our script and create a refrence of it
	}


	void OnGUI(){
//		GUI.Box(new Rect(0, 0, 40, 40), new Texture2D(40, 40));

	}

	public override void OnInspectorGUI(){

//		DrawDefaultInspector();
		serializedObject.Update();
//		EditorGUI.DrawRect(new Rect(0, 0, 40, 40), Color.red);
//		EditorGUI.DrawPreviewTexture(new Rect(0, 40, 40, 40), new Texture2D(40, 40, TextureFormat.ARGB32, false));
//		EditorGUI.TextField(new Rect(0, 0, 40, 40), "EEH");
		EditorGUILayout.TextField("WAUW");

		Rect rect = GUILayoutUtility.GetRect (40, 40);
		EditorGUI.DrawRect(rect, Color.red);
		Rect rect2 = GUILayoutUtility.GetRect (40, 40);
		EditorGUI.DrawRect(rect2, Color.green);
		Rect rect3 = GUILayoutUtility.GetRect (40, 40);
		EditorGUI.DrawRect(rect3, Color.blue);



		int width = 200, height = 200;
		Rect panelRect = GUILayoutUtility.GetRect (200, 200);

//		EditorGUI.DropShadowLabel(panelRect, "WAUW"); 

		int xCount = 9, yCount = 10;
		int textWidth = 20;
		float eWidth = width / (float) xCount;
		float eHeight = height / (float) yCount;
		GUI.BeginGroup(panelRect);
		for (int y = 0; y < yCount; y++) {
			Rect tr = new Rect(0, y * eHeight, textWidth, eHeight);
			GUI.Label (tr, ""+ y);
			for (int x = 0; x < xCount; x++) {
				Rect r = new Rect(x * eWidth + textWidth, y * eHeight, eWidth, eHeight);
				if (GUI.RepeatButton(r, "WAUW")){

				}
			}
		}
		GUI.EndGroup();

//		EditorGUI.BeginChangeCheck();
//
//		if (showTurretDef == null || showTurretDef.Length != buildingDefs.arraySize){
//			showTurretDef = new bool[buildingDefs.arraySize];
//		}
//
//		for(int i = 0; i < buildingDefs.arraySize; i++){
//
//
//
//			SerializedProperty bldProp = buildingDefs.GetArrayElementAtIndex(i);
////			buildingSprite.
//
//			
//
//			SerializedProperty bldType = bldProp.FindPropertyRelative("type");
////			SerializedProperty buildingSpriteSprite = buildingSprite.FindPropertyRelative("sprite");
////			SerializedProperty buildingSpriteDestroyedSprite = buildingSprite.FindPropertyRelative("destroyedSprite");
//
//			GUIStyle style = new GUIStyle();
//			style.fontStyle = FontStyle.Bold;
//			EditorGUILayout.LabelField("" + bldType.enumDisplayNames[bldType.enumValueIndex], style);
//
//			bool isTurret = false;
////			for (int j = 0; j < typeof(BuildingDefinition).GetFields().Length; j++) {
//			for (int j = 0; j < typeof(BuildingDefinition).GetFields().Length; j++) {
//				FieldInfo field = typeof(BuildingDefinition).GetFields()[j];
//
//				if (field.Name.Equals("isTurret") && bldProp.FindPropertyRelative(field.Name).boolValue) isTurret = true;
//
//
//
//				if (field.Name.Equals("turretDef")){
//					if (isTurret){
//						SerializedProperty turretDefProp = bldProp.FindPropertyRelative(field.Name);
//
//						showTurretDef[i] = EditorGUILayout.Foldout(showTurretDef[i], "Turret Definition");
//						if (showTurretDef[i]){
//							EditorGUI.indentLevel = 2;
//							for (int k = 0; k < typeof(TurretDefinition).GetFields().Length; k++) {
//								FieldInfo tField = typeof(TurretDefinition).GetFields()[k];
//								SerializedProperty turretProp = turretDefProp.FindPropertyRelative(tField.Name);
//								EditorGUILayout.PropertyField( turretProp );
//							}
//							EditorGUI.indentLevel = 0;
//						}
//					}
//				}else{
//					SerializedProperty serProp = bldProp.FindPropertyRelative(field.Name);
//
//					EditorGUILayout.PropertyField( serProp );
//				}
//			}
//
//
//
////			EditorGUILayout.PropertyField(buildingSpriteSprite);
////			EditorGUILayout.PropertyField(buildingSpriteDestroyedSprite);
//
//		}
//
//		if (GUILayout.Button ("Add")){
//			buildingDefs.InsertArrayElementAtIndex(buildingDefs.arraySize);
//		}
//
//		if(EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

	}
}
