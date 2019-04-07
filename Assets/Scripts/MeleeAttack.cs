﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    public Weapon weapon; 
    public class Throw
    {
        public Vector3 dir;
        public float force;
        public Throw(Vector3 d, float f)
        {
            dir = d;
            force = f;
        }
    }

    private float attackCooldown;

    // Use this for initialization
    void Start () {
        FindActiveWeapon();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z)) {
            if (!weapon)
            {
                FindActiveWeapon();
            }
            Attack();
        }
    }

    void FindActiveWeapon()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Melee"))
            {
                weapon = child.GetComponentInChildren<Weapon>();
            }
        }
    }

    void LocateEnemyInWeaponRange()
    {
        // Grab height of the game object. This will help us scan for objects "in front of us"
        float objectHeight = transform.lossyScale.y;
        // Find any colliders within the weapon's range. Not that, for the starting position, we're starting from the height's middle point.  
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + (objectHeight / 2), transform.position.z), weapon.range);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Collider col = hitColliders[i];
            // Check to ensure we're interacting with only the tags we want to interact with.
            if (col.tag == "Infected")
            {
                // Give the Infected weapon damage.
                col.SendMessage("TakeDamage", weapon.damage);
                // Create a value that will act as the force given to the enemy being thrown in the direction of the attack.
                // The value that "damage" is multiplied is arbitrary to achieve the desired effect. 
                float throwForce = weapon.damage * 150;
                // Do the math to calculate the angle in which the target is from the enemy.
                Vector3 directionToTarget = col.transform.position - transform.position;
                // Normalize that Vector3 to get to direction only.
                directionToTarget.Normalize();
                // Create a new Throw class, giving it our two values.
                Throw attackThrow = new Throw(directionToTarget, throwForce);
                // Give both of those values to the "GetThrown" in order to throw the enemy backware
                col.SendMessage("GetThrown", attackThrow);
            }
            // Create a buffer between attacks
            attackCooldown = Time.time + weapon.speed;
            // Make sure we don't get stuck in a loop
            i++;
        }
    }

    void Attack()
    {
        // Only attack after the cooldown buffer time window has passed.
        if (Time.time > attackCooldown)
        {
            LocateEnemyInWeaponRange();
        }
    }
}