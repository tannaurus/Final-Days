using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // Config
    public int backpackSize = 10;
    public int pickUpRange = 3;

    public Types.Hand rightHand;
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
            rightHand = hand;
        }
    }

    void DropItem()
    {
        if (rightHand != null) {
            rightHand.item.Drop();
        }
    }

    bool CanEquip() {
        return backpack.Count < backpackSize;
    }

}
//public class InventorySystem : MonoBehaviour
//{

//    public Weapon activeWeapon;

//    [SerializeField]
//    private float pickUpRange = 3f;
//    private Collider defaultWeapon;

//    private float cooldown;
//    private bool handsFull = false;

//    void Start()
//    {
//        EquipDefaultWeapon();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.X))
//        {
//            if (!handsFull)
//            {
//                PickUpItem();
//            }
//            else
//            {
//                DropItem();
//            }
//        }
//    }

//    void PickUpItem()
//    {
//        // Grab height of the game object. This will help us scan for objects "in front of us"
//        float objectHeight = transform.lossyScale.y;
//        // Find any colliders within the weapon's range. Not that, for the starting position, we're starting from the height's middle point.  
//        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + (objectHeight / 2), transform.position.z), pickUpRange);
//        int i = 0;
//        while (i < hitColliders.Length)
//        {
//            Collider col = hitColliders[i];
//            // Check to ensure we're interacting with only the tags we want to interact with.
//            if (col.tag == "Melee")
//            {
//                // Grab the weapon script on our found melee weapon
//                Weapon foundWeapon = col.gameObject.GetComponent<Weapon>();
//                Equip(foundWeapon);
//            }
//            // Make sure we don't get stuck in a loop
//            i++;
//        }
//    }

//    void Equip(Weapon weapon)
//    {
//        activeWeapon = weapon;
//        handsFull = true;
//    }

//    void DropItem()
//    {
//        // Change the weapon to its dropped state
//        activeWeapon.SendMessage("Drop");
//        EquipDefaultWeapon();
//        handsFull = false;
//    }

//    void EquipDefaultWeapon()
//    {
//        Debug.Log("WEAPON");
//        Debug.Log(defaultWeapon.gameObject.GetComponent<Weapon>().damage);
//        //Equip(newWeapon);
//    }

//}
