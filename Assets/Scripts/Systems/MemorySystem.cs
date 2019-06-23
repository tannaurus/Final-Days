using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySystem : MonoBehaviour
{
    private List<BehaviorTypes.LastSeen> objectMemory = new List<BehaviorTypes.LastSeen>();


    private void Start()
    {
        objectMemory = BuildObjectMemory();
    }

    // --- SET ----
    private List<BehaviorTypes.LastSeen> BuildObjectMemory()
    {
        List<BehaviorTypes.LastSeen> newMemory = new List<BehaviorTypes.LastSeen>();
        List<GameObject> objectsInMemory = GenericHelper.FindObjectsInLayer(9);
        foreach (GameObject obj in objectsInMemory)
        {
            newMemory.Add(new BehaviorTypes.LastSeen(obj, Time.time));
        }
        return newMemory;
    }


    // --- GET ----
    public BehaviorTypes.LastSeen FindNearestObjectInMemory(string tag, Transform transform)
    {
        BehaviorTypes.LastSeen closestObj = null;
        foreach (BehaviorTypes.LastSeen obj in objectMemory)
        {
            if (obj.gameObject.tag != tag)
            {
                continue;
            }
            if (closestObj == null)
            {
                closestObj = obj;
            }
            else
            {
                float currentClosest = Vector3.Distance(closestObj.gameObject.transform.position, transform.position);
                float activeDistance = Vector3.Distance(obj.gameObject.transform.position, transform.position);
                if (activeDistance < currentClosest)
                {
                    closestObj = obj;
                }
            }
        }
        return closestObj;
    }

    public GameObject FindNearestGameObjectInMemory(string tag, Transform transform)
    {
        BehaviorTypes.LastSeen lastSeen = FindNearestObjectInMemory(tag, transform);
        if (lastSeen == null)
        {
            return null;
        }
        return lastSeen.gameObject;
    }

    // -- DELETE ----
    public void RemoveGameObjectFromMemory(GameObject gameObject)
    {
        List<BehaviorTypes.LastSeen> newList = new List<BehaviorTypes.LastSeen>();
        foreach (BehaviorTypes.LastSeen obj in objectMemory)
        {
            if (obj.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                newList.Add(obj);
            }
        }
        objectMemory = newList;
    }

}
