//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using UnityEngine;

// this appliance is the basic ingredient container, when used by the play provides
//      an infinite amount of a type of ingredient (one item at a time)
public class Crate : Appliance
{
    // ---data members---
    [SerializeField] protected GameObject contents;

    // ---primary methods---
    public override void Use(Player usingPlayer)
    {
        GameObject newItem = Instantiate(contents);
        newItem.transform.localPosition = Vector3.zero;
        Item newItemScript = newItem.GetComponent<Item>();
        usingPlayer.PickUpItem(newItemScript);
    }
}
