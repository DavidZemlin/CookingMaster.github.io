using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for the collider that detects what is in the players current reach
public class ItemInteractor : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> inRangeItems; //-------------------------------------------------------------
    [SerializeField] private List<Transform> inRangeAppliances; //-------------------------------------------------------------
    // initialize null variables
    private void Awake()
    {
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }
    }

    // add items to the list of interactable items in range if they enter this trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            inRangeItems.Add(other.transform);
        }
        else if (other.CompareTag("Appliance"))
        {
            inRangeAppliances.Add(other.transform);
        }
    }

    // remove items to the list of interactable items in range if they exit this trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            inRangeItems.Remove(other.transform);
        }
        else if (other.CompareTag("Appliance"))
        {
            inRangeAppliances.Remove(other.transform);
        }
    }

    // returns to closest item to the center of the interactor //-----------------------------This is used for loose items should we implement them
    public Item getItem()
    {
        Item foundItem = null;
        if (inRangeItems.Count < 1)
        {
            return null;
        }
        else if (inRangeItems.Count < 2)
        {
            foundItem = inRangeItems[0].gameObject.GetComponent<Item>();
        }
        else
        {
            foundItem = CustomMath.closestTransform(transform, inRangeItems).gameObject.GetComponent<Item>();
        }
        return foundItem;
    }

    // returns to closest appliance to the center of the interactor
    public Appliance GetAppliance()
    {
        Appliance foundAppliance = null;
        if (inRangeAppliances.Count < 1)
        {
            return null;
        }
        else if (inRangeAppliances.Count < 2)
        {
            foundAppliance = inRangeAppliances[0].gameObject.GetComponent<Appliance>();
        }
        else
        {
            foundAppliance = CustomMath.closestTransform(transform, inRangeAppliances).gameObject.GetComponent<Appliance>();
        }
        return foundAppliance;
    }
}
