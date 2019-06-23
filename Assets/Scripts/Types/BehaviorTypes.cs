using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorTypes
{

    // Used in memory systems
    public class LastSeen
    {
        public GameObject gameObject;
        public float lastSeen;

        public LastSeen(GameObject o, float t)
        {
            gameObject = o;
            lastSeen = t;
        }

        public Vector3? GetPosition()
        {
            if (!gameObject)
            {
                return null;
            }

            return gameObject.transform.position;
        }
    }
}
