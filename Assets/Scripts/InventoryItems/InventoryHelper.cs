using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryHelper
{

    public static Types.Hand GetActiveHand(InventorySystem inventory)
    {
        return inventory.rightHand;
    }

    public static Types.Items GetActiveItemType(InventorySystem inventory)
    {
        return inventory.rightHand.item.type;
    }

    public static Item GetActiveItem(InventorySystem inventory) {
        Types.Hand hand = inventory.rightHand;
        if (hand.item)
        {
            return hand.item;
        }
        return null;
    }
}
