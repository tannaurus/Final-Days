using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericHelper
{

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

}