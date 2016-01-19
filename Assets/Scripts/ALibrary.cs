using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//T : definition class, T2 : type enum
public abstract class ALibrary<T, T2> : MonoBehaviour {

	[SerializeField] List<T> definitions;
//	[SerializeField] List<ADefiniton> adefinitions;

	Dictionary<T2, T> dict = new Dictionary<T2, T>();

	public static ALibrary<T, T2> I{
		get; set;
	}

	void Awake () {
		if (I == null){
			I = this;
			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
			return;
		}

		foreach (T def in definitions) {
			//Find first field value
			T2 type = (T2)typeof(T).GetFields()[0].GetValue(def);
			dict.Add(type, def);
		}
	}

	

//	public void AddListElement(T def, T2 type){
//		dict.Add(type, def);
//	}

	public T GetDefinition(T2 type){
		return dict[type];
	}

//	public void UseList<T, T2>(List<T> defList, T2 type){
//		foreach (var item in defList) {
////			dict.Add()
//		}
//	}

}


public abstract class ADefiniton{

}