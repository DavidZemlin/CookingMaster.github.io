//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is a special type of crate that can dispense plates or combos
public class ComboCrate : Crate
{
    // ---data members---
    [SerializeField] private Item.ingredients[] plateContents = new Item.ingredients[ComboItem.MAX_COMBO];

    // ---primary methods---
    public override void Use(Player usingPlayer)
    {

        GameObject newItem = Instantiate(contents);
        newItem.transform.localPosition = Vector3.zero;
        ComboItem newItemScript = newItem.GetComponent<ComboItem>();
        usingPlayer.PickUpItem(newItemScript);
        newItemScript.SetPlate(true);
        for (int i = 0; i < plateContents.Length; i++)
        {
            newItemScript.SetContents(i, plateContents[i]);
        }
    }
}
