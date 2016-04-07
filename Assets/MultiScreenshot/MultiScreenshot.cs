using UnityEngine;
using System.Collections;

public class MultiScreenshot : MonoBehaviour {

//	private string[] names = {"Default", "Default@2x", "Default-568h@2x", "Default-Landscape", "Default-Landscape@2x", "Default-Portrait", "Default-Portrait@2x"};
//	private Vector2[] resolutions = {new Vector2(320,480), new Vector2(640,960), new Vector2(640,1136), new Vector2(1024,768), new Vector2(2048,1536), new Vector2(768,1024), new Vector2(1536,2048)};

	[SerializeField] DeviceResolution[] devices;
	[SerializeField] bool landscape;

	

	public void CreateScreenshots(){

		
		StartCoroutine(CreateScreenshotsCR());


		//		Derp();
	}

	private IEnumerator CreateScreenshotsCR(){
		float originalTimeScale = Time.timeScale;
		Time.timeScale = 0.0f;
		try {
			var path = System.IO.Directory.GetCurrentDirectory()+ "/AppStuff/AutoScreenshots";
			System.IO.Directory.CreateDirectory(path);  
			yield return new WaitForEndOfFrame();
			//		System.Threading.Thread.Sleep(100);
			for(var i = 0; i < devices.Length; i++){
				Vector2 dimensions = landscape ? new Vector2(devices[i].dimensions.y, devices[i].dimensions.x) : new Vector2(devices[i].dimensions.x, devices[i].dimensions.y);

				RenderTexture rTex = new RenderTexture((int)dimensions.x, (int)dimensions.y, 24 );
				Camera.main.targetTexture = rTex;
				Camera.main.aspect = dimensions.x/dimensions.y;
				yield return new WaitForEndOfFrame();
				//			System.Threading.Thread.Sleep(100);
				RenderTexture.active = rTex;
				Texture2D tex = new Texture2D((int)dimensions.x, (int)dimensions.y, TextureFormat.RGB24, false);
				tex.filterMode = FilterMode.Point;

				tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
				tex.Apply();
				byte[] bytes = tex.EncodeToPNG();
				string dateStr = System.DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss");
				string fullpath = path + "/" + devices[i].name + "_" + dateStr + ".png";
				System.IO.File.WriteAllBytes(fullpath, bytes);
				yield return new WaitForEndOfFrame();
				//			System.Threading.Thread.Sleep(100);
			}
		} finally {
			Time.timeScale = originalTimeScale;  
			Camera.main.ResetAspect();
			Camera.main.targetTexture = null;
		}
	}
}

[System.Serializable]
public class DeviceResolution{
	public string name;
	public Vector2 dimensions;
}