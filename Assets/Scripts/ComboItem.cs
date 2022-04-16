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

// this class is for items that are in a plate-able state (including empty plates).
// combo items can be served or combined with other combo items
public class ComboItem : Item
{
    // ---data members---
    [SerializeField] private DishTag dishTag;
    [SerializeField] private GameObject[] ingrediantModels; // model 0 = plate. the rest will match this indexing of the ingredients enum
    [SerializeField] private Transform[] ingrediantModelSlots = new Transform[ItemStats.MAX_COMBO];
    [SerializeField] private Transform plateModelSlot;

    private bool plate;
    private ingredients[] contents = new ingredients[ItemStats.MAX_COMBO];

    // ---getters---
    public bool GetPlate() { return plate; }
    public ingredients[] GetContents() {return contents; }
    private DishTag GetDishTag() { return dishTag; }

    // ---setters---
    public void SetPlate(bool hasPlate) { plate = hasPlate; }
    public void SetContents(int index, ingredients ingredient) { contents[index] = ingredient; }

    // ---unity methods---
    private void Awake()
    {
        for (int i = 0; i < contents.Length; i++)
        {
            SetContents(i, ingredients.empty);
            UpdateCombo();
        }
    }

    private void Update()
    {
        dishTag.transform.eulerAngles = Vector3.zero;
    }

    // ---primary methods---

    // combine this item with another combo item
    public void Combine(ComboItem incomingItem)
    {
        Debug.Log("Combine!!!");
        if (incomingItem.GetPlate() && !GetPlate())
        {
            incomingItem.SetPlate(false);
            SetPlate(true);
        }

        for (int i = 0; i < ItemStats.MAX_COMBO; i++)
        {
            if(contents[i] == ingredients.empty)
            {
                contents[i] = incomingItem.RemoveIngredient();
            }
        }
        UpdateCombo();
        if(incomingItem.isEmptyComboItem())
        {
            Player holdingPlayer = incomingItem.GetHoldingPlayer();
            Counter currentCounter = incomingItem.GetCurrentCounter();
            if (holdingPlayer != null)
            {
                incomingItem.GetHoldingPlayer().DestroyItemInLeftHand();
            }
            if (currentCounter != null)
            {
                currentCounter.RemoveItem();
                incomingItem.DestroyItem();
            }

        }
        else
        {
            incomingItem.UpdateCombo();
        }

    }

    // check if combo is empty
    public bool isEmptyComboItem()
    {
        bool empty = true;
        if (GetPlate())
        {
            empty = false;
        }
        else
        {
            foreach (Item.ingredients i in GetContents())
            {
                if (i != Item.ingredients.empty)
                {
                    empty = false;
                }
            }
        }
        return empty;
    }

    // update the appearance and stats of the combined food item
    public void UpdateCombo()
    {
        SetScore(0);

        // clear ingredient and plate models
        foreach (Transform t in plateModelSlot)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in ingrediantModelSlots)
        {
            foreach (Transform ct in t)
            {
                Destroy(ct.gameObject);
            }
        }

        if (plate)
        {
            SpawnModel(plateModelSlot, 0);
            SetScore(GetScore() + ItemStats.SCORE_PLATE);
        }

        for (int i = 0; i < ItemStats.MAX_COMBO; i++)
        {
            ingredients ingredient = GetContents()[i];
            if (ingredient == ingredients.berrie)
            {
                SpawnModel(ingrediantModelSlots[i], (int)ingredient);
                SetScore(GetScore() + ItemStats.SCORE_BERRY);
            }
            else if (ingredient == ingredients.carrots)
            {
                SpawnModel(ingrediantModelSlots[i], (int)ingredient);
                SetScore(GetScore() + ItemStats.SCORE_CARROT);
            }
            else if (ingredient == ingredients.lettuce)
            {
                SpawnModel(ingrediantModelSlots[i], (int)ingredient);
                SetScore(GetScore() + ItemStats.SCORE_LETTUCE);
            }
            else if (ingredient == ingredients.redCabbage)
            {
                SpawnModel(ingrediantModelSlots[i], (int)ingredient);
                SetScore(GetScore() + ItemStats.SCORE_RED_CABBAGE);
            }
            else if (ingredient == ingredients.squash)
            {
                SpawnModel(ingrediantModelSlots[i], (int)ingredient);
                SetScore(GetScore() + ItemStats.SCORE_SQUASH);
            }
            else if (ingredient == ingredients.tomato)
            {
                SpawnModel(ingrediantModelSlots[i], (int)ingredient);
                SetScore(GetScore() + ItemStats.SCORE_TOMATO);
            }
        }

        GetDishTag().ChangeTag(GetContents());
    }

    // remove first ingredient and shift the other ingredients forward on the list
    public ingredients RemoveIngredient()
    {
        ingredients ingredientToRemove = GetContents()[0];
        for(int i = 0; i < ItemStats.MAX_COMBO - 1; i++)
        {
            SetContents(i, GetContents()[i + 1]);
        }
        SetContents(ItemStats.MAX_COMBO - 1, ingredients.empty);
        return ingredientToRemove;
    }

    // helper method to spawn models for different parts of the combo
    private void SpawnModel(Transform slot, int type)
    {
        GameObject newMod = Instantiate(ingrediantModels[type]);
        newMod.transform.parent = slot;
        newMod.transform.localPosition = Vector3.zero;
        newMod.transform.localEulerAngles = Vector3.zero;
    }
}
