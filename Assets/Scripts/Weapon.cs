using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float damage;
    public float range;
    public float attackAngle;
    public float speed;
    public Vector3 inHandPosition;
    public Vector3 inHandRotation;

    private bool isEquiped = false;
    private Rigidbody rb;

    void Start()
    {
        // Grab our rigidbody to be used throughout the program
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // If the item is equiped, update it's position to be next to the player regularly
        if (isEquiped) 
        {
            UpdateWeaponPosition();
        }
    }

    // Ensure the weapon's position is always correct
    void UpdateWeaponPosition()
    {
        transform.localPosition = inHandPosition;
        transform.localEulerAngles = inHandRotation;
    }

    // Equip the weapon
    void Equip(GameObject parent)
    {
        // Make the player object the parent of the weapon
        gameObject.transform.parent = parent.transform;
        // Move the weapon's position so it's next to the player
        UpdateWeaponPosition();
        // Disable the weapon's gravity
        rb.useGravity = false;
        // Disable the weapon's contraints so it can move freely with the player
        rb.constraints = RigidbodyConstraints.None;
        // Notify the system that the weapon has been equiped
        isEquiped = true;
    }

    void Drop()
    {
        // Clear the player's parent
        gameObject.transform.parent = null;
        // Re-enable gravity
        rb.useGravity = true;
        // Notify the system that this item has been dropped
        isEquiped = false;
        // NOTE: in the Equip method, we disable the RigidbodyContraints but we don't re-enable them here.
        // This is intentional, as it allows the weapon to appear in a "displayed" position after we drop it, rather
        // than rolling around on the ground.
    }



}
