using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIController : MonoBehaviour {

	[Header("UI references")]
	[SerializeField] Text textWaveIntro;


	public void ShowWaveIntro(int waveNr){
		textWaveIntro.color = new Color(1, 1, 1, 1);
		textWaveIntro.gameObject.SetActive(true);
		textWaveIntro.text = "Wave " + (waveNr + 1);

		StartCoroutine(FadeOutTextCR(textWaveIntro, 0.5f, 2f));
	}



	private IEnumerator FadeOutTextCR(Text text, float duration, float delay){

		yield return new WaitForSeconds(delay);

		float startTime = Time.time;

		while (Time.time < startTime + duration){
			float progress = ((startTime + duration - Time.time) / duration); // 1 -> 0
			text.color = new Color(1, 1, 1, progress);

			yield return null;
		}

		text.color = new Color(1, 1, 1, 0);
		text.gameObject.SetActive(false);
	}
}
