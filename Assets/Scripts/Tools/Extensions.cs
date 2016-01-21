using UnityEngine;
using System.Collections;

public static class Extensions {

	public static float VectorAngle(this Vector2 vec){
		return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
	}
}
