//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class contains all numerical stats for items in the game; stats such as score, or chopping time.
public static class ItemStats
{
    public const int UNCHOPPED_INGREDIENT_DIVISOR = 2; // amount to divide the score by for unchopped items
    public const int MAX_COMBO = 3;

    // scores
    public const int SCORE_PLATE = 5;
    public const int SCORE_BERRY = 8;
    public const int SCORE_CARROT = 8;
    public const int SCORE_LETTUCE = 10;
    public const int SCORE_RED_CABBAGE = 7;
    public const int SCORE_SQUASH = 5;
    public const int SCORE_TOMATO = 6;

    // chopping times
    public const float CHOP_TIME_BERRY = 8.0f;
    public const float CHOP_TIME_CARROT = 8.0f;
    public const float CHOP_TIME_LETTUCE = 10.0f;
    public const float CHOP_TIME_RED_CABBAGE = 7.0f;
    public const float CHOP_TIME_SQUASH = 5.0f;
    public const float CHOP_TIME_TOMATO = 6.0f;
}
