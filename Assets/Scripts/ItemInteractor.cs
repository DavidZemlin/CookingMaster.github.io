//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections.Generic;
using UnityEngine;

// script for the collider that detects what is in the players current reach
public class ItemInteractor : MonoBehaviour
{
    // ---data members---
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> inRangeAppliances; // ---------------------------------- serialized for debug

    // ---unity methods---
    private void Awake()
    {
        // initialize null variables
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // add appliances to the list of intractable appliances in range if they enter this trigger
        if (other.CompareTag("Appliance"))
        {
            inRangeAppliances.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // remove appliance from the list of intractable appliances in range if it exits this trigger
        if (other.CompareTag("Appliance"))
        {
            inRangeAppliances.Remove(other.transform);
        }
    }

    // ---primary methods---

    // returns to closest appliance to the center of the intractor
    public Appliance GetAppliance()
    {
        Appliance foundAppliance = null;
        if (inRangeAppliances.Count < 1) // no appliance in range: return null
        {
            return null;
        }
        else if (inRangeAppliances.Count < 2) // only 1 appliance in range : no need to search list for closest
        {
            foundAppliance = inRangeAppliances[0].gameObject.GetComponent<Appliance>();
        }
        else // use custom distance script to find the closet appliance to aiming point
        {
            foundAppliance = CustomMath.closestTransform(transform, inRangeAppliances).gameObject.GetComponent<Appliance>();
        }
        return foundAppliance;
    }
}
