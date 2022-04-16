//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
// ---------------------------------- serialized for debug
// ---data members---
// ---getters---
// ---setters---
// ---constructors---
// ---unity methods---
// ---primary methods---

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for items that can be copped
public class Chopable : Item
{
    // ---data members---
    public const float BERRIES_CHOP_TIME = 8.0f;
    public const float CARROTS_CHOP_TIME = 8.0f;
    public const float LETTUCE_CHOP_TIME = 10.0f;
    public const float RED_CABBAGE_CHOP_TIME = 7.0f;
    public const float SQUASH_CHOP_TIME = 5.0f;
    public const float TOMATO_CHOP_TIME = 6.0f;

    [SerializeField] private ingredients ingredientType;
    [SerializeField] private GameObject choppedPrefab;

    private CuttingBoard cuttingboard;
    private float choppingTimeLeft;

    // ---getters---
    public CuttingBoard GetCuttingBoard() { return cuttingboard; }
    public float GetChoppingTimeLeft() { return choppingTimeLeft; }

    // ---setters---
    public void SetCuttingBoard(CuttingBoard newCuttingBoard) { cuttingboard = newCuttingBoard; }
    public void SetChoppingTimeLeft(float chopTime) { choppingTimeLeft = chopTime; }

    // ---unity methods---
    private void Awake()
    {
        // set score of this type of ingredient
        if(ingredientType == ingredients.berrie)
        {
            SetChoppingTimeLeft(BERRIES_CHOP_TIME);
        }
        if (ingredientType == ingredients.carrots)
        {
            SetChoppingTimeLeft(CARROTS_CHOP_TIME);
        }
        if (ingredientType == ingredients.lettuce)
        {
            SetChoppingTimeLeft(LETTUCE_CHOP_TIME);
        }
        if (ingredientType == ingredients.redCabbage)
        {
            SetChoppingTimeLeft(RED_CABBAGE_CHOP_TIME);
        }
        if (ingredientType == ingredients.squash)
        {
            SetChoppingTimeLeft(SQUASH_CHOP_TIME);
        }
        if (ingredientType == ingredients.tomato)
        {
            SetChoppingTimeLeft(TOMATO_CHOP_TIME);
        }
    }

    // ---primary methods---

    // called after chopping is complete, replacing this item with a combo item of the correct type
    public void ReplaceWithCombo()
    {
        Counter location = GetCurrentCounter();
        location.RemoveItem();
        GameObject choppedItem = Instantiate(choppedPrefab);
        location.recieveItem(choppedItem.GetComponent<Item>());
        ComboItem comboScript = choppedItem.GetComponent<ComboItem>();
        comboScript.SetContents(0, ingredientType);

        Destroy(gameObject);
        GetCuttingBoard().ShiftCurrentItemToSideBoard();
    }
}
