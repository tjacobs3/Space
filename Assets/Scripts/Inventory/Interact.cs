using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour {

	public float interactDistance = 1f;
	
	void Update () {
		if(Input.GetButtonDown("Interact")) {
			AttemptInteract();
		}
	}

	void AttemptInteract() {
		Debug.DrawLine(transform.position, transform.position + (transform.TransformDirection(Vector3.forward) * interactDistance), Color.white, 0.2f);
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast(transform.position, fwd, out hit, interactDistance)) {
			Pickupable pickupObject = hit.collider.gameObject.GetComponent<Pickupable>();
			if (pickupObject)
			{
				pickupObject.pickup(this.gameObject);
			}
		}
	}
}
