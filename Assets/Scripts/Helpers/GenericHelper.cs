using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericHelper
{

    public static List<GameObject> FindObjectsInLayer(int layer)
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        List<GameObject> layerObjects = new List<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                layerObjects.Add(obj);
            }
        }
        return layerObjects;
    }

    public static Transform FindChildTransformWithTag(string tag, Transform transform)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == tag)
            {
                return child;
            }
        }
        return null;
    }

    public static List<GameObject> RemoveGameObjectFromMemory(GameObject gameObject, List<GameObject> memory)
    {
        List<GameObject> newList = new List<GameObject>();
        foreach (GameObject obj in memory)
        {
            if (obj.GetInstanceID() != gameObject.GetInstanceID())
            {
                newList.Add(obj);
            }
        }
        return newList;
    }

}