//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for items that can be copped
public class Chopable : Item
{
    // ---data members---
    public const float BERRIES_CHOP_TIME = 8;
    public const float CARROTS_CHOP_TIME = 8;
    public const float LETTUCE_CHOP_TIME = 10;
    public const float RED_CABBAGE_CHOP_TIME = 7;
    public const float SQUASH_CHOP_TIME = 5;
    public const float TOMATO_CHOP_TIME = 6;

    [SerializeField] private ingredients ingredientType;
}
