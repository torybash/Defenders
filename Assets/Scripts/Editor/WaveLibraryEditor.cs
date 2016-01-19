using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(WaveLibrary))]
public class WaveLibraryEditor : Editor {

//	SerializedObject getTarget;
	SerializedProperty defsProp;

	bool[] showFoldout;

	WavePart[] addWavePart;
	int[] currSelectedPartIdx;

	WaveLibrary wavLib;

	GUIStyle headerStyle = new GUIStyle();
	GUIStyle redStyle = new GUIStyle();


	void OnEnable(){
		
		wavLib = (WaveLibrary)target;
//		getTarget = new SerializedObject(sprLib);
		defsProp = serializedObject.FindProperty("definitions"); // Find the List in our script and create a refrence of it
////		buildingSpritesList = getTarget.FindProperty("buildingSprites"); // Find the List in our script and create a refrence of it
	
		headerStyle.fontStyle = FontStyle.Bold;
	
		if (redStyle == null) redStyle = new GUIStyle();
//		redStyle.font.material.color = Color.red;

//		redStyle.normal.textColor = Color.red;
//		redStyle.normal.textColor = Color.red;
//		redStyle.
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
//		EditorGUILayout.TextField("WAUW");
//
//		Rect rect = GUILayoutUtility.GetRect (40, 40);
//		EditorGUI.DrawRect(rect, Color.red);
//		Rect rect2 = GUILayoutUtility.GetRect (40, 40);
//		EditorGUI.DrawRect(rect2, Color.green);
//		Rect rect3 = GUILayoutUtility.GetRect (40, 40);
//		EditorGUI.DrawRect(rect3, Color.blue);



//		int width = 200, height = 200;
//		Rect panelRect = GUILayoutUtility.GetRect (200, 200);

//		EditorGUI.DropShadowLabel(panelRect, "WAUW"); 

//		int xCount = 9, yCount = 10;
//		int textWidth = 20;
//		float eWidth = width / (float) xCount;
//		float eHeight = height / (float) yCount;
//		GUI.BeginGroup(panelRect);
//		for (int y = 0; y < yCount; y++) {
//			Rect tr = new Rect(0, y * eHeight, textWidth, eHeight);
//			GUI.Label (tr, ""+ y);
//			for (int x = 0; x < xCount; x++) {
//				Rect r = new Rect(x * eWidth + textWidth, y * eHeight, eWidth, eHeight);
//				if (GUI.RepeatButton(r, "WAUW")){
//
//				}
//			}
//		}
//		GUI.EndGroup();



		if (showFoldout == null || showFoldout.Length != defsProp.arraySize){
			showFoldout = new bool[defsProp.arraySize];
		}
		if (addWavePart == null || addWavePart.Length != defsProp.arraySize){
			addWavePart = new WavePart[defsProp.arraySize];
			for (int i = 0; i < addWavePart.Length; i++) {
				addWavePart[i] = new WavePart();
				addWavePart[i].count = 1;
				addWavePart[i].interval = 0.25f;
			}
		}
		if (currSelectedPartIdx == null || currSelectedPartIdx.Length != defsProp.arraySize){
			currSelectedPartIdx = new int[defsProp.arraySize];
			for (int i = 0; i < currSelectedPartIdx.Length; i++) {
				currSelectedPartIdx[i] = -1;
			} 
		}


		EditorGUI.BeginChangeCheck();


		for(int i = 0; i < defsProp.arraySize; i++){
			SerializedProperty defProp = defsProp.GetArrayElementAtIndex(i);
			showFoldout[i] = EditorGUILayout.Foldout(showFoldout[i], "Wave " + i);
			if (showFoldout[i]){
				EditorGUI.indentLevel = 1;

				SerializedProperty waveIdxProp = defProp.FindPropertyRelative("waveIdx");
				SerializedProperty wavePartsProp = defProp.FindPropertyRelative("waveParts");
				waveIdxProp.intValue = i;


//				Debug.Log("wavLib.GetDefinition(i): "+ wavLib.GetDefinitionList()[i]);
//				if (wavLib.GetDefinitionList()[i] != null) Debug.Log("wavLib.GetDefinition(i).waveParts: " + wavLib.GetDefinitionList()[i].waveParts);
//				float latestTime = 0;

				List<WavePart> waveParts = wavLib.GetDefinitionList()[i].waveParts;

//				waveParts.AddRange(wavLib.GetDefinitionList()[i].waveParts);
				List<WavePart> sortedWaveParts = waveParts.OrderBy(x => x.time).ToList();


				if (waveParts.Count > 0){
					WavePart lastWavePart = sortedWaveParts[sortedWaveParts.Count - 1];
					float duration = lastWavePart.time + lastWavePart.interval * lastWavePart.count;


					float width = EditorGUIUtility.currentViewWidth - 50, height = duration * 25 + 15;
					Rect wavePartsRect = GUILayoutUtility.GetRect(width, height);
					GUI.BeginGroup(wavePartsRect);
					GUI.Box(new Rect(0, 0, width, height - 1), "");
					int c = 0;
					foreach (WavePart wavePart in waveParts) {
						float x = width * ((wavePart.startX + 2.5f) / 5f); //0 - 200
						float y = wavePart.time * 25; //0 - duration*25

						Color origColor = GUI.backgroundColor;
						if (currSelectedPartIdx[i] == c){
							GUI.backgroundColor = Color.gray;
						}

						if (GUI.Button(new Rect(x, y, 20, 20), ""+wavePart.count)){
//							waveParts.Remove(wavePart);
//							break;
							currSelectedPartIdx[i] = c;
						}
						GUI.backgroundColor = origColor;
						c++;
					}
					GUI.EndGroup();
				}


				//EDIT PART
				if (currSelectedPartIdx[i] >= 0 && currSelectedPartIdx[i] < wavePartsProp.arraySize){
//					SerializedProperty partProp = wavePartsProp.GetArrayElementAtIndex(currSelectedPartIdx[i]);
					EditorGUILayout.LabelField("Edit part", headerStyle);
//					GUIHelpers.ClassField<WavePart>(partProp);
					WavePartField(waveParts[currSelectedPartIdx[i]]);

					Color origColor = GUI.backgroundColor;
					GUI.backgroundColor = Color.red;
					if (GUILayout.Button ("Remove")){
						waveParts.RemoveAt(currSelectedPartIdx[i]);
						currSelectedPartIdx[i] = -1;
					}
					GUI.backgroundColor = origColor;
				}



				//ADD PART UI


//				EditorGUI.indentLevel = 1;
				EditorGUILayout.LabelField("Add new part", headerStyle);
				WavePartField(addWavePart[i]);


				if (GUILayout.Button ("Add part")){
					
					wavePartsProp.InsertArrayElementAtIndex(wavePartsProp.arraySize);

					SerializedProperty wavePartProp = wavePartsProp.GetArrayElementAtIndex(wavePartsProp.arraySize - 1);
					wavePartProp.FindPropertyRelative("time").floatValue = addWavePart[i].time;
					wavePartProp.FindPropertyRelative("startX").floatValue = addWavePart[i].startX;
					wavePartProp.FindPropertyRelative("interval").floatValue = addWavePart[i].interval;
					wavePartProp.FindPropertyRelative("count").intValue = addWavePart[i].count;
					wavePartProp.FindPropertyRelative("type").enumValueIndex = (int)addWavePart[i].type;




//					Debug.Log("ADDED NEW PAAAART");
				}

				EditorGUI.indentLevel = 0;
			}
		}


		if (GUILayout.Button ("Add wave")){
			defsProp.InsertArrayElementAtIndex(defsProp.arraySize);
		}

		if(EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

		DrawDefaultInspector();

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


	private void WavePartField(WavePart wavePart){
		int startIndent = EditorGUI.indentLevel;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Time", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Time")).x + 15));
		EditorGUI.indentLevel = 0;
		EditorGUILayout.LabelField("StartX", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("StartX")).x + 15));
		EditorGUILayout.LabelField("Interval", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Interval")).x + 15));
		EditorGUILayout.LabelField("Count", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Count")).x + 15));
		EditorGUILayout.LabelField("Type", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Type")).x + 15));
		EditorGUILayout.EndHorizontal();

		EditorGUI.indentLevel = startIndent;
		EditorGUILayout.BeginHorizontal();
		wavePart.time = EditorGUILayout.FloatField(wavePart.time, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Time")).x + 15));
		EditorGUI.indentLevel = 0;
		wavePart.startX = EditorGUILayout.FloatField(wavePart.startX, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("StartX")).x + 15));
		wavePart.interval = EditorGUILayout.FloatField(wavePart.interval, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Interval")).x + 15));
		wavePart.count = EditorGUILayout.IntField(wavePart.count, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Count")).x + 15));
		wavePart.type = (EnemyType) EditorGUILayout.EnumPopup(wavePart.type, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Type")).x + 15));
		EditorGUI.indentLevel = startIndent;
		EditorGUILayout.EndHorizontal();
	}
}
