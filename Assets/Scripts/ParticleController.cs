using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleController : MonoBehaviour {

	[SerializeField] BaseParticleSys debrisParticles;
	[SerializeField] BaseParticleSys explosionParticles;

	Dictionary<ParticleType, BaseParticleSys> particlePrefabDict = new Dictionary<ParticleType, BaseParticleSys>();

	Dictionary<ParticleType, ObjPool<BaseParticleSys>> particlesPool = new Dictionary<ParticleType, ObjPool<BaseParticleSys>>();



	public void Init(){
		foreach (ParticleType type in System.Enum.GetValues(typeof(ParticleType))) {
			particlesPool[type] = new ObjPool<BaseParticleSys>();
		}

		particlePrefabDict[ParticleType.EXPLOSION] = explosionParticles;
		particlePrefabDict[ParticleType.DEBRIS] = debrisParticles;
	}

	public void CreateParticlesAt(ParticleType type, Vector2 pos){
		BaseParticleSys part = particlesPool[type].TryGet();
		if (part == null){
			part = (BaseParticleSys) Instantiate(particlePrefabDict[type]);
		}
		part.transform.position = pos;
		part.Play();

		StartCoroutine(CompletedCR(type, part));
//		part.
	}

	private IEnumerator CompletedCR(ParticleType type, BaseParticleSys part){
		yield return new WaitForSeconds(part.GetLifetime());
		ReturnParticle(type, part);

	}

	private void ReturnParticle(ParticleType type, BaseParticleSys part){
		part.Clear();
		particlesPool[type].Return(part);
	}
}


[System.Serializable]
public enum ParticleType{
	DEBRIS, //TODO Debris type?
	EXPLOSION
}