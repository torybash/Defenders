using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleController : MonoBehaviour {

	[SerializeField] BaseParticleSys debrisParticles;
	[SerializeField] BaseParticleSys explosionParticles;
	[SerializeField] BaseParticleSys crashParticles;

	Dictionary<ParticleType, BaseParticleSys> particlePrefabDict = new Dictionary<ParticleType, BaseParticleSys>();

	Dictionary<ParticleType, ObjPool<BaseParticleSys>> particlesPool = new Dictionary<ParticleType, ObjPool<BaseParticleSys>>();


	GameObject systemContainer;

	void Awake(){
		systemContainer = new GameObject("ParticleSystemContainer");
	}

	public void Init(){
		foreach (ParticleType type in System.Enum.GetValues(typeof(ParticleType))) {
			particlesPool[type] = new ObjPool<BaseParticleSys>();
		}

		particlePrefabDict[ParticleType.EXPLOSION] = explosionParticles;
		particlePrefabDict[ParticleType.DEBRIS] = debrisParticles;
		particlePrefabDict[ParticleType.CRASH] = crashParticles;
	}

	public void CreateParticlesAt(ParticleType type, Vector2 pos){
		BaseParticleSys part = CreateAndStartParticles(type);

		if (part != null){
			part.transform.position = pos;
			part.transform.SetParent(systemContainer.transform);

			StartCoroutine(PlayCR(type, part));
		}
	}

	public BaseParticleSys AttachParticlesTo(ParticleType type, Transform parent){
		BaseParticleSys part = CreateAndStartParticles(type);

		if (part != null){
			part.transform.SetParent(parent);
			part.transform.localPosition = Vector2.zero;

			StartCoroutine(PlayCR(type, part));
		}
		return part;
	}

	private BaseParticleSys CreateAndStartParticles(ParticleType type){
		BaseParticleSys part = particlesPool[type].TryGet();
		if (part == null){
			part = (BaseParticleSys) Instantiate(particlePrefabDict[type]);
		}
		part.gameObject.SetActive(true);


		return part;
	}


	private IEnumerator PlayCR(ParticleType type, BaseParticleSys part){
		part.Play();
		yield return new WaitForSeconds(part.GetLifetime());
		ReturnSystem(type, part);

	}

	private void ReturnSystem(ParticleType type, BaseParticleSys part){
		part.Clear();
		part.gameObject.SetActive(false);
		particlesPool[type].Return(part);
	}
}


[System.Serializable]
public enum ParticleType{
	DEBRIS, //TODO Debris type?
	EXPLOSION,
	CRASH
}