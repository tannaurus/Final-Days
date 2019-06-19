using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviorStates { LookingForPlayer, LookingForWater, Drinking, Wandering };

public static class BehaviorHelper
{

    public static List<int> GetAgeBreakpoints()
    {
        List<int> breakpoints = new List<int>();
        breakpoints.Add(10);
        breakpoints.Add(30);
        breakpoints.Add(50);
        breakpoints.Add(70);
        breakpoints.Add(90);
        return breakpoints;
    }

    public delegate void QuickIntSwitchFunc();
    public static int QuickIntSwitch(int exp, List<int> cases, List<int> values)
    {
        bool caseFound = false;
        for (int index = 0; index < cases.Count; index++)
        {
            if (cases[index] >= exp & !caseFound)
            {
                try
                {
                    return values[index];
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    System.ArgumentException funcEx = new System.ArgumentException("Failed to invoke behavior function", ex);
                    throw funcEx;
                }
            }
        }
        // Default to 90
        return 90;
    }

    public static bool AtEndOfPath(NavMeshAgent agent)
    {
        return !agent.pathPending & agent.remainingDistance <= agent.stoppingDistance;
    }

    public static GameObject FindNearestObjectInMemoryWithTag(string tag, Transform transform, List<GameObject> memory)
    {
        GameObject closestObj = null;
        foreach (GameObject obj in memory)
        {
            if (obj.tag != tag)
            {
                break;
            }
            if (closestObj == null)
            {
                closestObj = obj;
            }
            else
            {
                float currentClosest = Vector3.Distance(closestObj.transform.position, transform.position);
                float activeDistance = Vector3.Distance(obj.transform.position, transform.position);
                if (activeDistance < currentClosest)
                {
                    closestObj = obj;
                }
            }
        }
        return closestObj;
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