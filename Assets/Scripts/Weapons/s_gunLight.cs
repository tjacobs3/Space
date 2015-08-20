using UnityEngine;
using System.Collections;

public class s_gunLight : MonoBehaviour {
	public Light flashLight;
	
	public void turnOff() {
		flashLight.enabled = false;
	}

	public void setLight(bool onOff) {
		flashLight.enabled = onOff;
	}
}
