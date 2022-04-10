using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopable : Item
{
    public const float BERRIES_CHOP_TIME = 8;
    public const float CARROTS_CHOP_TIME = 8;
    public const float LETTUCE_CHOP_TIME = 10;
    public const float RED_CABBAGE_CHOP_TIME = 7;
    public const float SQUASH_CHOP_TIME = 5;
    public const float TOMATO_CHOP_TIME = 6;

    [SerializeField] private ingredients ingredientType;
}
