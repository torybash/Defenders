using UnityEditor;
using UnityEngine;
using System.Collections;

public class WaveEditor: EditorWindow {

	Rect window1;
	Rect window2;

	[MenuItem("Window/Wave editor")]
	static void ShowEditor() {
		WaveEditor editor = EditorWindow.GetWindow<WaveEditor>();
		editor.Init();
	
	}

	void Init(){

	}

	void OnGUI() {
		GUI.TextField(new Rect(0, 0, 50, 50), "WUUWU");

		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				Texture2D tex2D = new Texture2D(20, 20);
				Color32[] clrArr = new Color32[20 * 20];


				int rnd = Random.Range(0, byte.MaxValue);
				Debug.Log("wut - rnd: " + rnd);
				for (int k = 0; k < clrArr.Length; k++) {
					clrArr[k] = new Color32((byte)rnd, 0, (byte)(byte.MaxValue-rnd), byte.MaxValue);
				}
				tex2D.SetPixels32(clrArr);
				tex2D.Apply();
				GUI.DrawTexture(new Rect(i * 20, j * 20, 20, 20), tex2D);
			}
		}
	}
}
