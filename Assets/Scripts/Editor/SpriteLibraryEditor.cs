using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SpriteLibrary))]
public class SpriteLibraryEditor : Editor {

//	SpriteLibrary sprLib;
	SerializedObject getTarget;
	SerializedProperty buildingSpritesList;
	SerializedProperty enemySpritesList;
//

	int testInt;

	void OnEnable(){
//		sprLib = (SpriteLibrary)target;
//		getTarget = new SerializedObject(sprLib);
		buildingSpritesList = serializedObject.FindProperty("buildingSprites"); // Find the List in our script and create a refrence of it
		enemySpritesList = serializedObject.FindProperty("enemySprites"); // Find the List in our script and create a refrence of it
////		buildingSpritesList = getTarget.FindProperty("buildingSprites"); // Find the List in our script and create a refrence of it
	}

	public override void OnInspectorGUI(){

		serializedObject.Update();
		SpriteLibrary controller = target as SpriteLibrary;
//		EditorGUIUtility.LookLikeInspector();
//		EditorGUIUtility.LookLikeControls();
//		SerializedProperty tps = serializedObject.FindProperty ("buildingSprites");
//		buildingSpritesList.value
		EditorGUI.BeginChangeCheck();
//		EditorGUILayout.PropertyField(tps, true);


		EditorGUILayout.LabelField("Buildings");
		for(int i = 0; i < buildingSpritesList.arraySize; i++){



			SerializedProperty buildingSprite = buildingSpritesList.GetArrayElementAtIndex(i);
//			buildingSprite.

			SerializedProperty buildingSpriteType = buildingSprite.FindPropertyRelative("type");
			SerializedProperty buildingSpriteSprite = buildingSprite.FindPropertyRelative("sprite");
			SerializedProperty buildingSpriteDestroyedSprite = buildingSprite.FindPropertyRelative("destroyedSprite");

			GUIStyle style = new GUIStyle();
			style.fontStyle = FontStyle.Bold;
			EditorGUILayout.LabelField("" + buildingSpriteType.enumDisplayNames[buildingSpriteType.enumValueIndex], style);


//			buildingSprite
			EditorGUILayout.PropertyField( buildingSpriteType );

//			buildingSpriteType.enumValueIndex = (int)(BuildingType)EditorGUILayout.EnumPopup("Type:", (BuildingType)Enum.GetValues(typeof(BuildingType)).GetValue(buildingSpriteType.enumValueIndex));
//			buildingSpriteType.enumValueIndex = (int)EditorGUILayout.EnumPopup("",(BuildingType) buildingSpriteType.enumValueIndex);

//			buildingSpriteSprite.objectReferenceValue = 
			EditorGUILayout.PropertyField(buildingSpriteSprite);
			EditorGUILayout.PropertyField(buildingSpriteDestroyedSprite);

//			EditorGUILayout.EnumPopup(testInt);
		}

		if (GUILayout.Button ("Add")){
			buildingSpritesList.InsertArrayElementAtIndex(buildingSpritesList.arraySize);
		}

		EditorGUILayout.LabelField("Enemies");
		for(int i = 0; i < enemySpritesList.arraySize; i++){



			SerializedProperty bldProp = enemySpritesList.GetArrayElementAtIndex(i);
			//			buildingSprite.
			for (int j = 0; j < typeof(EnemySprite).GetFields().Length; j++) {
				System.Reflection.FieldInfo field = typeof(EnemySprite).GetFields()[j];

				SerializedProperty serProp = bldProp.FindPropertyRelative(field.Name);

				EditorGUILayout.PropertyField( serProp );
			}
		}

		if (GUILayout.Button ("Add")){
			enemySpritesList.InsertArrayElementAtIndex(enemySpritesList.arraySize);
		}

		if(EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

//		EditorGUIUtility.LookLikeControls();

		//Update our list
		
//		getTarget.Update();
//		
//		//Choose how to display the list<> Example purposes only
//		EditorGUILayout.Space ();
//		EditorGUILayout.Space ();
//		DisplayFieldType = (displayFieldType)EditorGUILayout.EnumPopup("",DisplayFieldType);
//		
//		//Resize our list
//		EditorGUILayout.Space ();
//		EditorGUILayout.Space ();
//		EditorGUILayout.LabelField("Define the list size with a number");
//		ListSize = ThisList.arraySize;
//		ListSize = EditorGUILayout.IntField ("List Size", ListSize);
//		
//		if(ListSize != ThisList.arraySize){
//			while(ListSize > ThisList.arraySize){
//				ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
//			}
//			while(ListSize < ThisList.arraySize){
//				ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
//			}
//		}
//		
//		EditorGUILayout.Space ();
//		EditorGUILayout.Space ();
//		EditorGUILayout.LabelField("Or");
//		EditorGUILayout.Space ();
//		EditorGUILayout.Space ();
//		
//		//Or add a new item to the List<> with a button
//		EditorGUILayout.LabelField("Add a new item with a button");
//		
//		if(GUILayout.Button("Add New")){
//			t.MyList.Add(new CustomList.MyClass());
//		}
//		
//		EditorGUILayout.Space ();
//		EditorGUILayout.Space ();
//		
//		//Display our list to the inspector window
//		
//		for(int i = 0; i < ThisList.arraySize; i++){
//			SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
//			SerializedProperty MyInt = MyListRef.FindPropertyRelative("AnInt");
//			SerializedProperty MyFloat = MyListRef.FindPropertyRelative("AnFloat");
//			SerializedProperty MyVect3 = MyListRef.FindPropertyRelative("AnVector3");
//			SerializedProperty MyGO = MyListRef.FindPropertyRelative("AnGO");
//			SerializedProperty MyArray = MyListRef.FindPropertyRelative("AnIntArray");
//			
//			
//			// Display the property fields in two ways.
//			
//			if(DisplayFieldType == 0){// Choose to display automatic or custom field types. This is only for example to help display automatic and custom fields.
//				//1. Automatic, No customization <-- Choose me I'm automatic and easy to setup
//				EditorGUILayout.LabelField("Automatic Field By Property Type");
//				EditorGUILayout.PropertyField(MyGO);
//				EditorGUILayout.PropertyField(MyInt);
//				EditorGUILayout.PropertyField(MyFloat);
//				EditorGUILayout.PropertyField(MyVect3);
//				
//				// Array fields with remove at index
//				EditorGUILayout.Space ();
//				EditorGUILayout.Space ();
//				EditorGUILayout.LabelField("Array Fields");
//				
//				if(GUILayout.Button("Add New Index",GUILayout.MaxWidth(130),GUILayout.MaxHeight(20))){
//					MyArray.InsertArrayElementAtIndex(MyArray.arraySize);
//					MyArray.GetArrayElementAtIndex(MyArray.arraySize -1).intValue = 0;
//				}
//				
//				for(int a = 0; a < MyArray.arraySize; a++){
//					EditorGUILayout.PropertyField(MyArray.GetArrayElementAtIndex(a));
//					if(GUILayout.Button("Remove  (" + a.ToString() + ")",GUILayout.MaxWidth(100),GUILayout.MaxHeight(15))){
//						MyArray.DeleteArrayElementAtIndex(a);
//					}
//				}
//			}else{
//				//Or
//				
//				//2 : Full custom GUI Layout <-- Choose me I can be fully customized with GUI options.
//				EditorGUILayout.LabelField("Customizable Field With GUI");
//				MyGO.objectReferenceValue = EditorGUILayout.ObjectField("My Custom Go", MyGO.objectReferenceValue, typeof(GameObject), true);
//				MyInt.intValue = EditorGUILayout.IntField("My Custom Int",MyInt.intValue);
//				MyFloat.floatValue = EditorGUILayout.FloatField("My Custom Float",MyFloat.floatValue);
//				MyVect3.vector3Value = EditorGUILayout.Vector3Field("My Custom Vector 3",MyVect3.vector3Value);
//				
//				
//				// Array fields with remove at index
//				EditorGUILayout.Space ();
//				EditorGUILayout.Space ();
//				EditorGUILayout.LabelField("Array Fields");
//				
//				if(GUILayout.Button("Add New Index",GUILayout.MaxWidth(130),GUILayout.MaxHeight(20))){
//					MyArray.InsertArrayElementAtIndex(MyArray.arraySize);
//					MyArray.GetArrayElementAtIndex(MyArray.arraySize -1).intValue = 0;
//				}
//				
//				for(int a = 0; a < MyArray.arraySize; a++){
//					EditorGUILayout.BeginHorizontal();
//					EditorGUILayout.LabelField("My Custom Int (" + a.ToString() + ")",GUILayout.MaxWidth(120));
//					MyArray.GetArrayElementAtIndex(a).intValue = EditorGUILayout.IntField("",MyArray.GetArrayElementAtIndex(a).intValue, GUILayout.MaxWidth(100));
//					if(GUILayout.Button("-",GUILayout.MaxWidth(15),GUILayout.MaxHeight(15))){
//						MyArray.DeleteArrayElementAtIndex(a);
//					}
//					EditorGUILayout.EndHorizontal();
//				}
//			}
//			
//			EditorGUILayout.Space ();
//			
//			//Remove this index from the List
//			EditorGUILayout.LabelField("Remove an index from the List<> with a button");
//			if(GUILayout.Button("Remove This Index (" + i.ToString() + ")")){
//				ThisList.DeleteArrayElementAtIndex(i);
//			}
//			EditorGUILayout.Space ();
//			EditorGUILayout.Space ();
//			EditorGUILayout.Space ();
//			EditorGUILayout.Space ();
//		}
//		
//		//Apply the changes to our list
//		GetTarget.ApplyModifiedProperties();
	}
}
