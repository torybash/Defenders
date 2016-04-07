using UnityEngine;
using UnityEditor;
using System.Collections;

public class MultiScreenshotWindow : EditorWindow {


//	var hide : GameObject[];
//	Camera cam;
	RenderTexture rTex;


	MultiScreenshot screenshotter;

	[MenuItem ("Window/2T2_MultiScreenshot")]
	static void Create () {
		// Get existing open window or if none, make a new one:
		MultiScreenshotWindow window = (MultiScreenshotWindow)EditorWindow.GetWindow (typeof (MultiScreenshotWindow));

		window.CreateShotter();
		window.Show();

	}

	private void CreateShotter(){
		if (screenshotter == null){

			MultiScreenshot existingShotter = FindObjectOfType<MultiScreenshot>();

			if (existingShotter == null){
				GameObject go = Instantiate(Resources.Load("_MultiScreenshot", typeof(GameObject))) as GameObject;
				screenshotter = go.GetComponent<MultiScreenshot>();
				DontDestroyOnLoad(screenshotter);
			}else{
				screenshotter = existingShotter;
			}
		}
	}


	void OnGUI () {

//		if (Event.current.type == EventType.repaint)
//		switch (Event.current.type)
//		{case EventType.mouseUp:



		if (screenshotter != null){
			if (GUILayout.Button("Take screenshots")){
				screenshotter.CreateScreenshots();
			}
		}else{
			if (GUILayout.Button("Create shotter")){
				CreateShotter();
			}
		}
	}

	public void Woot(){
		
	}




//	void Derp(){
//		var path = System.IO.Directory.GetCurrentDirectory()+ "/AppStuff/AutoScreenshots";
//		Texture2D tex = new Texture2D((int)resolutions[0].x, (int)resolutions[0].y);
//		tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
//		tex.Apply();
//		var bytes = tex.EncodeToPNG();
//		var fullpath = System.String.Format("{0}/"+"TEST"+".png", path);
//		System.IO.File.WriteAllBytes(fullpath, bytes);
//	}
}
