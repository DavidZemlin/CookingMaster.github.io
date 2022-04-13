//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is for items that are in a plate-able state (including empty plates).
// combo items can be served or combined with other combo items
public class ComboItem : Item
{
    // ---data members---
    public const int MAX_COMBO = 3;
    public const int BERRIES_SCORE = 8;
    public const int CARROTS_SCORE = 8;
    public const int LETTUCE_SCORE = 10;
    public const int RED_CABBAGE_SCORE = 7;
    public const int SQUASH_SCORE = 5;
    public const int TOMATO_SCORE = 6;

    private bool hasPlate;
    private ingredients[] contents = new ingredients[MAX_COMBO];

    // ---primary methods---

    // combine this item with another combo item
    public void Combine(ComboItem incomingItem)
    {

    }
}
