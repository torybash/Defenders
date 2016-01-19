using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyLibrary : ALibrary<EnemyDefinition, EnemyType> {

//	public EnemyDefinition[] enemyDefinitions;

}

[System.Serializable]
public class EnemyDefinition{
	public EnemyType type;

	public float speed;
	public int maxHp;
	public int minGold;
	public int maxGold;

	public override string ToString ()
	{
		this.GetType();
		string result = this.GetType().Name + " - [";
		for (int i = 0; i < this.GetType().GetFields().Length; i++) {
			System.Reflection.FieldInfo field = this.GetType().GetFields()[i];
			result += field.Name + "=" + field.GetValue(this);
			if (i < this.GetType().GetFields().Length - 1) result += ", ";
		}
		result += "]";
		return result;
	}
}

[System.Serializable]
public enum EnemyType{
	WALKER,
	ROVER,
	BIKE,
	PLANE
}