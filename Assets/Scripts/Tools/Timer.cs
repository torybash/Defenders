using UnityEngine;
using System;
using System.Collections;

public class Timer : MonoBehaviour {


	private static Timer instance;
	private static Timer Instance{
		get
		{
			if (instance != null) return instance;

			var t = Transform.FindObjectOfType<Timer>();

			if (t != null){
				return t;
			}

			var go = new GameObject("_Timer", typeof(Timer));
			return go.GetComponent<Timer>();
		}
	}


	void Awake(){
		instance = this;
	}

	public static void CallDelayed(Action function, float delay){
		Instance.DoCallDelayed(function, delay);
	}


	private void DoCallDelayed(Action function, float delay){
		StartCoroutine(CallDelayedCR(function, delay));
	}

	private IEnumerator CallDelayedCR(Action function, float delay){
		yield return new WaitForSeconds(delay);
		function();
	}

//	public delegate void CallFunction();
//
//	System.Threading.Timer timer;
//
//	public void CallDelayed(CallFunction f, float delay){
////		f();
//
//		timer = new System.Threading.Timer(obj => {f();}, null, (int)(delay * 1000), System.Threading.Timeout.Infinite);
//
////		StartCoroutine(CallDelayedCR(f, delay));
//	}


//	private IEnumerator CallDelayedCR(CallFunction f, float delay){
//		yield return new WaitForSeconds(delay);
//
//		f();
//	}
}
