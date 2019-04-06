using System.Collections;
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

    // Use this for initialization
    void Start () {
        FindActiveWeapon();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z)) {
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
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), weapon.range);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Collider col = hitColliders[i];
            if (col.tag == "Infected")
            {
                col.SendMessage("TakeDamage", weapon.damage);
                // Create a value that will act as the force given to the enemy being thrown in the direction of the attack.
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
            i++;
        }
    }

    void Attack()
    {
        LocateEnemyInWeaponRange();
    }
}
