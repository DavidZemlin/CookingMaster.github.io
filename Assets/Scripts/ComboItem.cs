using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboItem : Item
{
    public const int MAX_COMBO = 3;
    public const int BERRIES_SCORE = 8;
    public const int CARROTS_SCORE = 8;
    public const int LETTUCE_SCORE = 10;
    public const int RED_CABBAGE_SCORE = 7;
    public const int SQUASH_SCORE = 5;
    public const int TOMATO_SCORE = 6;

    private bool hasPlate;
    private ingredients[] contents = new ingredients[MAX_COMBO];

    public void Combine(ComboItem incomingItem)
    {

    }
}
