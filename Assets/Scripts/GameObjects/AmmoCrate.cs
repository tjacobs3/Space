using UnityEngine;
using System.Collections;

public class AmmoCrate : Pickupable {
	public Inventory.AmmoType ammoType;
	public int ammoAmount;

	public override void pickup(GameObject picker) {
		Inventory inventory = picker.GetComponentInChildren<Inventory> ();
		if (inventory) {
			inventory.addAmmo (ammoType, ammoAmount);
		}
		base.pickup (picker);
	}
}
