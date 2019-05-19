using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Types
{
    public enum Items { Weapon, Misc };

    // When an item is equiped, we use this class to store it
    public class Hand
    {
        public GameObject obj;
        public Item item;
        public Hand(GameObject _obj)
        {
            obj = _obj;
            item = _obj.GetComponent<Item>();
        }
    }

    public class Throw
    {
        private float forceMultiplier = 1000f;
        public Vector3 dir;
        public float force;
        public Throw(Vector3 d, float f)
        {
            dir = d;
            force = (f * forceMultiplier);
        }
    }
}