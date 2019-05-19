using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderHelper
{
    public static List<Collider> FindNearbyColiders(Transform transform, float range, string tag)
    {
        // Grab height of the game object. This will help us scan for objects "in front of us"
        float objectHeight = transform.lossyScale.y;
        // Find any colliders within the weapon's range. Note: for the starting position, we're starting from the height's middle point.  
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + (objectHeight / 2), transform.position.z), range);
        List<Collider> foundColliders = new List<Collider>();
        int i = 0;
        while (i < hitColliders.Length)
        {
            Collider col = hitColliders[i];
            if (col.tag == tag)
            {
                foundColliders.Add(col);
            }
            i++;
        }
        Debug.Log(foundColliders);
        return foundColliders;
    }
}