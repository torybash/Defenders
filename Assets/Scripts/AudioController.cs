using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	[SerializeField] AudioClip[] clips;

	AudioSource source;

	// Use this for initialization
	void Awake () {
		if (source == null)
			source = gameObject.AddComponent<AudioSource>();
	

	}

	public void Init(){

	}
	


	public void PlayShoot(){
		source.PlayOneShot(clips[0]);
	}

	public void PlayHit(){
		source.PlayOneShot(clips[1]);
	}

	public void PlayExplode(){
		source.PlayOneShot(clips[2]);
	}

	public void PlayCrashing(){
		source.PlayOneShot(clips[3]);
	}

	public void PlayComplete(){
		source.PlayOneShot(clips[4]);
	}
}
