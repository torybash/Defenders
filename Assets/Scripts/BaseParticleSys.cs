using UnityEngine;
using System.Collections;

public class BaseParticleSys : MonoBehaviour {

	protected ParticleSystem Ps{
		get; set;
	}

	void Awake(){
		if (GetComponent<ParticleSystem>() != null){
			Ps = GetComponent<ParticleSystem>();
		}
	}

	public virtual void Play(){
		Ps.Play();
	}

	public virtual void Stop(){

		Ps.Stop();
	}

	public virtual float GetLifetime(){
		return Ps.duration + Ps.startLifetime;
	}

	public virtual void Clear(){
		Ps.Clear();
	}


}
