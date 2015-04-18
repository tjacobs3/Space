using UnityEngine;
using System.Collections;

public class VectorUtility {

	public static Vector3 RandVector(Vector3 v1, Vector3 v2) {
		float x = Random.Range(v1.x, v2.x);
		float y = Random.Range(v1.y, v2.y);
		float z = Random.Range(v1.z, v2.z);
		return new Vector3(x, y, z);
	}

	public static Vector3 normalize180(Vector3 v) {
		if(v.x > 180)
			v.x -= 360;
		if(v.y > 180)
			v.y -= 360;
		if(v.z > 180)
			v.z -= 360;
		return v;
	}
}
