using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    public enum AmmoType { Pistol, Rifle };
    public Dictionary<AmmoType, int> ammo = new Dictionary<AmmoType, int>()
    {
        {AmmoType.Pistol, 60},
        {AmmoType.Rifle, 60}
    };

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void useAmmo(AmmoType type)
    {
        ammo[type] = ammo[type] - 1;
    }

    public int totalAmmo(AmmoType type)
    {
        return ammo[type];
    }
}
