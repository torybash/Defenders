using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class DebrisParticles : BaseParticleSys {

	[SerializeField] ParticleSystem templatePs;


	[SerializeField] Sprite[] sprites;
	[SerializeField] Shader particleShader;


	ParticleSystem[] childSystems;


	void Awake () {



		if (childSystems == null){
			childSystems = new ParticleSystem[sprites.Length];
			for (int i = 0; i < sprites.Length; i++) {

				ParticleSystem parSys = Instantiate(templatePs);
				parSys.transform.SetParent(transform);
				parSys.gameObject.SetActive(true);
				//			Debug.Log("parSys.GetComponent<ParticleSystemRenderer>(): " + );

				Material mat = new Material(particleShader);

				Sprite spr = sprites[i]; Texture2D tex2D = sprites[i].texture;
				Texture2D exactTex2D = new Texture2D((int)spr.textureRect.width, (int)sprites[i].textureRect.height,
					TextureFormat.ARGB32, true); 
				exactTex2D.filterMode = FilterMode.Trilinear;


				Color32[] clrArr = new Color32[exactTex2D.width * exactTex2D.height];
				Color32[] origClrArr = tex2D.GetPixels32();
				for (int x = 0; x < exactTex2D.width; x++) {
					for (int y = 0; y < exactTex2D.height; y++) {
						//Get orig pixel
						int origX = (int)spr.textureRect.xMin + x;
						int origY = (int)spr.textureRect.yMin + y;
						clrArr[x + y * exactTex2D.width] = origClrArr[origX + origY * spr.texture.width];
						//					Color clr = tex2D.GetPixel(origX, origY);

						//Set pixel
						//					exactTex2D.SetPixel(x, y, clr);
					}
				}
				exactTex2D.SetPixels32(clrArr);
				exactTex2D.Apply();

				mat.mainTexture = exactTex2D;
				parSys.GetComponent<ParticleSystemRenderer>().material = mat;

				childSystems[i] = parSys;
			}
		}


		templatePs.collision.SetPlane(0, GameController.I.GroundPlane);


//		ps = GetComponent<ParticleSystem>();

//		templatePs.GetComponent<CollisionMo




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

			childSystems[i].Emit(Random.Range(1, 3));
//			Color clr = clrFlags[Random.Range(0, clrFlags.Length)];

//			ParticleSystem.EmitParams prms = new ParticleSystem.EmitParams();
////			prms.startColor = clr;
//			ps.Emit(prms, 1);

			yield return new WaitForEndOfFrame();
		}


	}
}
