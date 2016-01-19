using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveLibrary : ALibrary<WaveDefinition, int> {

}

[System.Serializable]
public class WaveDefinition{
	public int waveIdx;
	public List<WavePart> waveParts;

}

[System.Serializable]
public class WavePart{
	public float time;
	public float startX;
	public EnemyType type;
	public int count;
	public float interval;

}

public class WavePartComparer : IComparer
{
	public int Compare(object x, object y)
	{
		return ((Transform) x).name.CompareTo(((Transform) y).name);
	}
}

//[System.Serializable]
//public class WaveEnemy{
//	public EnemyType type;
//	public float startX;
//	public float goalX;
//}