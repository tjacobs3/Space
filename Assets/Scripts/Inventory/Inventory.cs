using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    public enum AmmoType { Pistol, Rifle };
    public List<Gun> equippableWeapons = new List<Gun>();
    public Dictionary<AmmoType, int> ammo = new Dictionary<AmmoType, int>()
    {
        {AmmoType.Pistol, 60},
        {AmmoType.Rifle, 60}
    };

    private Gun currentEquippedWeapon;
    private int currentEquippedIndex;

	// Use this for initialization
	void Start () {
        if (equippableWeapons.Count > 0)
        {
            currentEquippedIndex = 0;
            equipWeapon(equippableWeapons[0]); 
        }
	}
	
	// Update is called once per frame
	void Update () {
	    float dir = Input.GetAxis("Mouse ScrollWheel");
        if (dir != 0)
        {
            int newIndex = currentEquippedIndex;
            if (dir < 0)
                newIndex--;
            else
                newIndex++;

            newIndex = Mathf.Clamp(newIndex, 0, equippableWeapons.Count - 1);
            if (newIndex != currentEquippedIndex)
            {
                currentEquippedIndex = newIndex;
                equipWeapon(equippableWeapons[currentEquippedIndex]);
            }
        }
	}

    public void useAmmo(AmmoType type, int amount)
    {
        ammo[type] = ammo[type] - amount;
    }

    public int totalAmmo(AmmoType type)
    {
        return ammo[type];
    }

    public int getCurrentAmmoCount()
    {
        if (currentEquippedWeapon != null)
        {
            return currentEquippedWeapon.getRounds();
        }
        else
            return 0;
    }

    public int getTotalAmmoCount()
    {
        if (currentEquippedWeapon != null)
        {
            return ammo[currentEquippedWeapon.ammoType];
        }
        else
            return 0;
    }

    public void setEquippedWeapon(Gun weapon) {
        currentEquippedWeapon = weapon;
    }

    private void equipWeapon(Gun prefabWeapon)
    {
        if (currentEquippedWeapon != null)
        {
            ammo[currentEquippedWeapon.ammoType] += currentEquippedWeapon.getRounds();
            Destroy(currentEquippedWeapon.gameObject);
        }
        Gun gun = (Gun)Instantiate(prefabWeapon);
        gun.transform.parent = this.transform;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        setEquippedWeapon(gun);
    }
}
