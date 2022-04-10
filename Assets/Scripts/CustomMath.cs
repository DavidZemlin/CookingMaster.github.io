using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// use this class for custom or optimized math functions
public static class CustomMath
{
    // a method to find the closest transform from a list, using squared distance, which is cheaper than base distance methods
    public static Transform closestTransform(Transform originTransform, List<Transform> transforms)
    {
        Transform closest = null;
        float closestSquareDistance = Mathf.Infinity;
        Vector3 originPosition = originTransform.position;
        foreach (Transform t in transforms)
        {
            float distanceSquared = (t.transform.position - originPosition).sqrMagnitude;
            if (distanceSquared < closestSquareDistance)
            {
                closestSquareDistance = distanceSquared;
                closest = t;
            }
        }
        return closest;
    }
}
