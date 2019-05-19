using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Item : MonoBehaviour {

    // Config
    public int id;
    public Image icon;
    public string name = "Item";
    public Types.Items type;
    public Vector3 inHandPosition;
    public Vector3 inHandRotation;

    private bool isEquiped = false;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        // Grab our rigidbody to be used throughout the program
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // If the item is equiped regularly update it's position to be next to the player 
        if (isEquiped)
        {
            UpdateItemPosition();
        }
    }

    // Ensure the weapon's position is always correct
    void UpdateItemPosition()
    {
        transform.localPosition = inHandPosition;
        transform.localEulerAngles = inHandRotation;
    }

    // Equip the weapon
    public void Equip(GameObject parent)
    {
        // Make the player object the parent of the weapon
        gameObject.transform.parent = parent.transform;
        // Move the weapon's position so it's next to the player
        UpdateItemPosition();
        // Disable the weapon's gravity
        rb.useGravity = false;
        // Disable the weapon's contraints so it can move freely with the player
        rb.constraints = RigidbodyConstraints.None;
        // Notify the system that the weapon has been equiped
        isEquiped = true;
    }

    public void Drop()
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
