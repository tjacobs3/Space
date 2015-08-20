using UnityEngine;
using System.Collections;

public class s_gunLightController : MonoBehaviour {
	public bool lightEnabled = true;

	vp_FPPlayerEventHandler m_Player;
	void Awake()
	{
		m_Player = transform.GetComponent<vp_FPPlayerEventHandler>();
	}
	
	protected virtual void OnEnable()
	{
		if (m_Player != null)
			m_Player.Register(this);
	}
	protected virtual void OnDisable()
	{
		if (m_Player != null)
			m_Player.Unregister(this);
	}
	
	void OnMessage_ToggleLight()
	{
		lightEnabled = !lightEnabled;
		s_gunLight[] lights = transform.GetComponentsInChildren<s_gunLight> ();
		foreach (s_gunLight light in lights) {
			light.setLight(lightEnabled);
		}
	}
}
