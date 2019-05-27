using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSystem : Weapon {

    private float attackCooldown = 0.1f;
    // This is handed to the InventoryHelper class so it must be public.
    public InventorySystem playerInventory;

    void Start() 
    {
        RefreshInventoryRef();
    }

    // Update is called once per frame
    void Update()
    {
        UserInputListener();
        RefreshInventoryRef();
    }

    void RefreshInventoryRef()
    {
        playerInventory = GetComponent<InventorySystem>();
    }

    void UserInputListener()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
    }

    List<Collider> LocateEnemyInWeaponRange(float range)
    {
        return ColliderHelper.FindNearbyColiders(transform, range, "Infected");
    }

    void Attack()
    {
        Types.Items itemType = InventoryHelper.GetActiveItemType(playerInventory);
        // Only attack after the cooldown buffer time window has passed.
        if (Time.time > attackCooldown && itemType == Types.Items.Weapon)
        {
            Weapon weapon = InventoryHelper.GetActiveItem(playerInventory) as Weapon;
            List<Collider> localEnemies = LocateEnemyInWeaponRange(weapon.range);
            foreach (Collider enemyCol in localEnemies)
            {
                Health enemyHealth = enemyCol.gameObject.GetComponent<Health>();
                // Give the Infected weapon damage.
                enemyHealth.TakeDamage(weapon.damage);
                // Do the math to calculate the angle in which the target is from the enemy.
                Vector3 directionToTarget = enemyCol.transform.position - transform.position;
                // Normalize that Vector3 to get to direction only.
                directionToTarget.Normalize();
                // Create a new Throw class, giving it our two values.
                Types.Throw attackThrow = new Types.Throw(directionToTarget, weapon.force);
                // Give both of those values to the "GetThrown" in order to throw the enemy backware
                enemyHealth.GetThrown(attackThrow);
                // Create a buffer between attacks
                attackCooldown = Time.time + weapon.speed;
            }
        }
    }


}
