using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour {

	public virtual void pickup(GameObject picker) {
		GameObject.Destroy (this.gameObject);
	}
}
