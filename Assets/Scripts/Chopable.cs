//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using UnityEngine;

// class for items that can be copped
public class Chopable : Item
{
    // ---data members---
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
            SetChoppingTimeLeft(ItemStats.CHOP_TIME_BERRY);
            SetScore(ItemStats.SCORE_BERRY / ItemStats.UNCHOPPED_INGREDIENT_DIVISOR);
        }
        if (ingredientType == ingredients.carrots)
        {
            SetChoppingTimeLeft(ItemStats.CHOP_TIME_CARROT);
            SetScore(ItemStats.SCORE_CARROT / ItemStats.UNCHOPPED_INGREDIENT_DIVISOR);
        }
        if (ingredientType == ingredients.lettuce)
        {
            SetChoppingTimeLeft(ItemStats.CHOP_TIME_LETTUCE);
            SetScore(ItemStats.SCORE_LETTUCE / ItemStats.UNCHOPPED_INGREDIENT_DIVISOR);
        }
        if (ingredientType == ingredients.redCabbage)
        {
            SetChoppingTimeLeft(ItemStats.CHOP_TIME_RED_CABBAGE);
            SetScore(ItemStats.SCORE_RED_CABBAGE / ItemStats.UNCHOPPED_INGREDIENT_DIVISOR);
        }
        if (ingredientType == ingredients.squash)
        {
            SetChoppingTimeLeft(ItemStats.CHOP_TIME_SQUASH);
            SetScore(ItemStats.SCORE_SQUASH / ItemStats.UNCHOPPED_INGREDIENT_DIVISOR);
        }
        if (ingredientType == ingredients.tomato)
        {
            SetChoppingTimeLeft(ItemStats.CHOP_TIME_TOMATO);
            SetScore(ItemStats.SCORE_TOMATO / ItemStats.UNCHOPPED_INGREDIENT_DIVISOR);
        }
    }

    // ---primary methods---

    // called after chopping is complete, replacing this item with a combo item of the correct type
    public void ReplaceWithCombo()
    {
        Counter location = GetCurrentCounter();
        location.RemoveItem();
        GameObject choppedItem = Instantiate(choppedPrefab);
        location.receiveItem(choppedItem.GetComponent<Item>());
        ComboItem comboScript = choppedItem.GetComponent<ComboItem>();
        comboScript.SetContents(0, ingredientType);
        comboScript.UpdateCombo();

        Destroy(gameObject);
        GetCuttingBoard().ShiftCurrentItemToSideBoard();
    }
}
