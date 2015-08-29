using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class BlinkingLight : MonoBehaviour {

	public float onMinTime = 1.0f;
	public float onMaxTime = 5.0f;
	public float offMinTime = 0.2f;
	public float offMaxTime = 0.5f;

	private Light light;

	// Use this for initialization
	void Start () {
		light = GetComponent<Light> ();
		StartCoroutine(LightCoroutine());
	}

	IEnumerator LightCoroutine ()
	{
		while (true) {
			light.enabled = !light.enabled;
			if (light.enabled)
				yield return new WaitForSeconds (Random.Range (onMinTime, onMaxTime));
			else
				yield return new WaitForSeconds (Random.Range (offMinTime, offMaxTime));
			yield return null;
		}
	}
}
