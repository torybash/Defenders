using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class DebrisParticles : BaseParticleSys {

	[SerializeField] ParticleSystem templatePs;


	Rect[] spriteRects;
	Color[] clrFlags;

	[SerializeField] Sprite[] sprites;
	[SerializeField] Shader particleShader;


	ParticleSystem[] childSystems;



	void Awake () {
//		ps = GetComponent<ParticleSystem>();

//		templatePs.GetComponent<CollisionMo
		templatePs.collision.SetPlane(0, GameController.I.GroundPlane);

		childSystems = new ParticleSystem[sprites.Length];
		for (int i = 0; i < sprites.Length; i++) {
			
			ParticleSystem parSys = Instantiate(templatePs);
			parSys.transform.SetParent(transform);
			parSys.gameObject.SetActive(true);
//			Debug.Log("parSys.GetComponent<ParticleSystemRenderer>(): " + );

			Material mat = new Material(particleShader);

			Sprite spr = sprites[i]; Texture2D tex2D = sprites[i].texture;
			Texture2D exactTex2D = new Texture2D((int)spr.textureRect.width, (int)sprites[i].textureRect.height,
				TextureFormat.ARGB32, false); 
			exactTex2D.filterMode = FilterMode.Point;


			tex2D.GetPixels32();
//			Color32[] clrArr = new Color32[exactTex2D.width * exactTex2D.height];
			for (int x = 0; x < exactTex2D.width; x++) {
				for (int y = 0; y < exactTex2D.height; y++) {
					//Get orig pixel
					int origX = (int)spr.textureRect.xMin + x;
					int origY = (int)spr.textureRect.yMin + y;
					Color clr = tex2D.GetPixel(origX, origY);

					//Set pixel
					exactTex2D.SetPixel(x, y, clr);
				}
			}
			exactTex2D.Apply();

//			TESTIMAGE.texture = exactTex2D;

//			exactTex2D = tex2D.ReadPixels(tex2D.textureRect, 0, 0);
			mat.mainTexture = exactTex2D;
//
			parSys.GetComponent<ParticleSystemRenderer>().material = mat;

			childSystems[i] = parSys;
		}



	}

	public override void Play(){
		StartCoroutine(EmitCR());
	}

	public override float GetLifetime(){
		return templatePs.duration + templatePs.startLifetime;
	}

	public override void Clear(){
		for (int i = 0; i < childSystems.Length; i++) {
			childSystems[i].Clear();
		}
	}


	private Color RectToColor(Rect rect){
		Color clr = new Color();
		clr.r = rect.position.x;
		clr.g = rect.position.y;
		clr.b = rect.size.x;
		clr.a = rect.size.y;
		return clr;
	}


	public IEnumerator EmitCR(){


//		Debug.Log("EMITING!");

		//Get random clrFlag (rect)
		for (int i = 0; i < childSystems.Length; i++) {

			childSystems[i].Emit(1);
//			Color clr = clrFlags[Random.Range(0, clrFlags.Length)];

//			ParticleSystem.EmitParams prms = new ParticleSystem.EmitParams();
////			prms.startColor = clr;
//			ps.Emit(prms, 1);

			yield return new WaitForEndOfFrame();
		}


	}
}
