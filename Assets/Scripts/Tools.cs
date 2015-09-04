using UnityEngine;
using System.Collections;

public static class Tools {
	

	public static float DirectionToAngle(Vector2 dir){

		float ang = Vector2.Angle(Vector2.zero, dir);
		Vector3 cross = Vector3.Cross(Vector2.zero, dir);
		
		if (cross.z > 0)
			ang = 360 - ang;

		return ang;
	}


	public static Quaternion DirectionToQuaternion(Vector2 dir){

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);

		return quart;
	}
}
