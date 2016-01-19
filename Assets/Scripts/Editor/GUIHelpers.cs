using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;





public class GUIHelpers{

	public static void ClassField<T>(SerializedProperty prop){
		for (int i = 0; i < typeof(T).GetFields().Length; i++) {
			FieldInfo field = typeof(T).GetFields()[i];
			SerializedProperty fieldProp = prop.FindPropertyRelative(field.Name);
			EditorGUILayout.PropertyField( fieldProp );
		}

	}


	public static void FixedFloatField(SerializedProperty property, float defaultValue)
	{
		FixedFloatField(property, _ => defaultValue);
	}

	public static void FixedFloatField(SerializedProperty property, System.Func<RectTransform, float> defaultValue)
	{
//		EditorGUILayout.GetControlRect();
//		EditorGUI.
	}


	public static void LayoutElementField(SerializedProperty property, float defaultValue)
	{
		LayoutElementField(property, _ => defaultValue);
	}

	public static void LayoutElementField(SerializedProperty property, System.Func<RectTransform, float> defaultValue)
	{

	}
}


//FixedWidthLabel class. Extends IDisposable, so that it can be used with the "using" keyword.
public class FixedWidthLabel : IDisposable
{
	private readonly ZeroIndent indentReset; //helper class to reset and restore indentation

	public FixedWidthLabel(GUIContent label)//	constructor.
	{//						state changes are applied here.
		EditorGUILayout.BeginHorizontal();// create a new horizontal group
		EditorGUILayout.LabelField(label,
			GUILayout.Width(GUI.skin.label.CalcSize(label).x +// actual label width
				9 * EditorGUI.indentLevel));//indentation from the left side. It's 9 pixels per indent level

		indentReset = new ZeroIndent();//helper class to have no indentation after the label
	}

	public FixedWidthLabel(string label)
		: this(new GUIContent(label))//alternative constructor, if we don't want to deal with GUIContents
	{
	}

	public void Dispose() //restore GUI state
	{
		indentReset.Dispose();//restore indentation
		EditorGUILayout.EndHorizontal();//finish horizontal group
	}
}

class ZeroIndent : IDisposable //helper class to clear indentation
{
	private readonly int originalIndent;//the original indentation value before we change the GUI state
	public ZeroIndent()
	{
		originalIndent = EditorGUI.indentLevel;//save original indentation
		EditorGUI.indentLevel = 0;//clear indentation
	}

	public void Dispose()
	{
		EditorGUI.indentLevel = originalIndent;//restore original indentation
	}
}