using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

    public Text ammoCount;
    public Inventory inventory;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (inventory != null)
        {
            ammoCount.text = inventory.getCurrentAmmoCount().ToString() + " / " + inventory.getTotalAmmoCount().ToString();
        }
	}
}
