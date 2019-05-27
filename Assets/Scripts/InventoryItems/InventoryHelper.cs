using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryHelper
{

    public static Types.Hand GetActiveHand(InventorySystem inventory)
    {
        return inventory.backpack[0];
    }

    public static Types.Items GetActiveItemType(InventorySystem inventory)
    {
        return GetActiveItem(inventory).type;
    }

    public static Item GetActiveItem(InventorySystem inventory) {
        Types.Hand hand = GetActiveHand(inventory);
        if (hand == null)
        {
            return null;
        }
        return hand.item;
    }
}
