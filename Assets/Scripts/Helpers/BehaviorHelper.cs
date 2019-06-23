using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviorStates { LookingForPlayer, LookingForWater, LookingForFood, Wandering };

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

    // If a Being can perform a behavior (excludes pursuing the player)
    public static bool CanPerformBehavior(BehaviorStates desiredState, BehaviorStates currentState)
    {
        return desiredState == currentState | currentState == BehaviorStates.Wandering;
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
}