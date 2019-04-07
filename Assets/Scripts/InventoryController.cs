using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    public Collider hand;
    public float pickUpRange = 3f;
    private float cooldown;
    private bool handsFull = false;
	
	// Update is called once per frame
	void Update () {
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
                // Give the Infected weapon damage.
                col.SendMessage("Equip", gameObject);
                hand = col;
                handsFull = true;
            }
            // Make sure we don't get stuck in a loop
            i++;
        }
    }

    void DropItem()
    {
        hand.SendMessage("Drop");
        handsFull = false;
    }

}
