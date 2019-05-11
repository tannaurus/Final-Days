﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    public Weapon activeWeapon;
    public Collider defaultWeapon;
    public float pickUpRange = 3f;

    private float cooldown;
    private bool handsFull = false;

    void Start()
    {
        EquipDefaultWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!handsFull)
            {
                PickUpItem();
            }
            else
            {
                DropItem();
            }
        }
    }

    void PickUpItem()
    {
        // Grab height of the game object. This will help us scan for objects "in front of us"
        float objectHeight = transform.lossyScale.y;
        // Find any colliders within the weapon's range. Not that, for the starting position, we're starting from the height's middle point.  
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + (objectHeight / 2), transform.position.z), pickUpRange);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Collider col = hitColliders[i];
            // Check to ensure we're interacting with only the tags we want to interact with.
            if (col.tag == "Melee")
            {
                // Grab the weapon script on our found melee weapon
                Weapon foundWeapon = col.gameObject.GetComponent<Weapon>();
                Equip(foundWeapon);
            }
            // Make sure we don't get stuck in a loop
            i++;
        }
    }

    void Equip(Weapon weapon)
    {
        // Tell the weapon collider to set this gameObject to be its parent
        weapon.SendMessage("Equip", gameObject);
        activeWeapon = weapon;
        handsFull = true;
    }

    void DropItem()
    {
        // Change the weapon to its dropped state
        activeWeapon.SendMessage("Drop");
        EquipDefaultWeapon();
        handsFull = false;
    }

    void EquipDefaultWeapon()
    {
        Debug.Log("WEAPON");
        Debug.Log(defaultWeapon.gameObject.GetComponent<Weapon>().damage);
        //Equip(newWeapon);
    }

}
