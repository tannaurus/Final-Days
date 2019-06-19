using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class BeingHelper
{
    public static int DetermineViewAngle(int age)
    {
        List<int> values = new List<int>();
        values.Add(Random.Range(60, 160));
        values.Add(Random.Range(60, 120));
        values.Add(Random.Range(60, 100));
        values.Add(Random.Range(60, 90));
        values.Add(Random.Range(40, 80));
        return BehaviorHelper.QuickIntSwitch(age, BehaviorHelper.GetAgeBreakpoints(), values);
    }

    public static int DetermineViewDistance(int age)
    {
        List<int> values = new List<int>();
        values.Add(100);
        values.Add(120);
        values.Add(100);
        values.Add(80);
        values.Add(60);
        return BehaviorHelper.QuickIntSwitch(age, BehaviorHelper.GetAgeBreakpoints(), values);
    }

    public static int DetermineSuspiciousness(int age)
    {
        List<int> values = new List<int>();
        values.Add(30);
        values.Add(50);
        values.Add(30);
        values.Add(20);
        values.Add(10);
        return BehaviorHelper.QuickIntSwitch(age, BehaviorHelper.GetAgeBreakpoints(), values);
    }

    public static int DetermineSpeed(int age)
    {
        List<int> values = new List<int>();
        values.Add(30);
        values.Add(25);
        values.Add(20);
        values.Add(15);
        values.Add(10);
        return BehaviorHelper.QuickIntSwitch(age, BehaviorHelper.GetAgeBreakpoints(), values);
    }
}