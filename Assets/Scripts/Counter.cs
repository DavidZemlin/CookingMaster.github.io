﻿//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is for a type of appliance that can hold an item on top or inside it.
//      this is used for counter-tops in its base case and appliances like
//      cutting boards and cook tops, should derive from this class
public class Counter : Appliance
{
    // ---data members---
    [SerializeField] private Transform itemSlot;
    private Item itemOnCounter;

    // ---getters---
    public Item GetItemOnCounter() { return itemOnCounter; }
    private Transform GetItemSlot() { return itemSlot; }

    // ---setters---
    private void SetItemOnCounter(Item item) { itemOnCounter = item; }

    // ---primary methods---

    // called when an item is attempting to be placed on/in this counter
    public void recieveItem(Item item)
    {
        if (GetItemOnCounter() == null)
        {
            SetItemOnCounter(item);
            item.transform.parent = GetItemSlot();
            item.transform.localPosition = Vector3.zero;
            item.OnPlace();
        }
    }

    // called when an item has been removed from the counter
    public void RemoveItem()
    {
        SetItemOnCounter(null);
    }
}