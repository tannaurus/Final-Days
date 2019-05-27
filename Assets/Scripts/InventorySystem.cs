using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // Config
    public int backpackSize = 1;
    public int pickUpRange = 3;

    public List<Types.Hand> backpack = new List<Types.Hand>();
    // An object on the player to hold all of the player's items
    public GameObject inventoryObject;

    void Update()
    {
        PlayerInputListener();
    }

    void PlayerInputListener()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PickUpItem();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    void PickUpItem()
    {
        // Grab height of the game object. This will help us scan for objects "in front of us"
        float objectHeight = transform.lossyScale.y;
        // Find any colliders within the weapon's range. Note: for the starting position, we're starting from the height's middle point.  
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + (objectHeight / 2), transform.position.z), pickUpRange);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Collider col = hitColliders[i];
            if (col.tag == "Item")
            {
                // Grab the weapon script on our found melee weapon
                Types.Hand foundItem = new Types.Hand(col.gameObject);
                Debug.Log(foundItem);
                Equip(foundItem);
            }
            i++;
        }
    }

    void Equip(Types.Hand hand)
    {
        if (CanEquip())
        {
            backpack.Add(hand);
            // Give the item the inventory game object to set its parent to
            hand.item.Equip(gameObject);
        }
    }

    void DropItem()
    {
        if (backpack[0] != null) {
            InventoryHelper.GetActiveItem(this).Drop();
            backpack.RemoveAt(0);
        }
    }

    bool CanEquip() {
        return backpack.Count < backpackSize;
    }

}