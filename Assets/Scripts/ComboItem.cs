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
    public const int MAX_COMBO = 3;
    public const int BERRIES_SCORE = 8;
    public const int CARROTS_SCORE = 8;
    public const int LETTUCE_SCORE = 10;
    public const int RED_CABBAGE_SCORE = 7;
    public const int SQUASH_SCORE = 5;
    public const int TOMATO_SCORE = 6;

    [SerializeField] private GameObject[] ingrediantModels; // model 0 = plate. the rest will match this indexing of the ingredients enum
    [SerializeField] private Transform[] ingrediantModelSlots = new Transform[MAX_COMBO];
    [SerializeField] private Transform plateModelSlot;

    private bool plate;
    private ingredients[] contents = new ingredients[MAX_COMBO];

    // ---getters---
    public bool GetPlate() { return plate; }
    public ingredients[] GetContents() {return contents; }

    // ---setters---
    public void SetPlate(bool hasPlate) { plate = hasPlate; }
    public void SetContents(int index, ingredients ingredient) { contents[index] = ingredient; UpdateComboModel(); }

    // ---unity methods---
    private void Awake()
    {
        for (int i = 0; i < contents.Length; i++)
        {
            SetContents(i, ingredients.empty);
        }
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

        for (int i = 0; i < MAX_COMBO; i++)
        {
            if(contents[i] == ingredients.empty)
            {
                contents[i] = incomingItem.RemoveIngredient();
            }
        }
        UpdateComboModel();
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
            incomingItem.UpdateComboModel();
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

    // update the appearance of the combined food item
    public void UpdateComboModel()
    {
        if (plate)
        {
            SpawnModel(plateModelSlot, 0);
        }
        else if (plateModelSlot.childCount > 0)
        {
            foreach (Transform t in plateModelSlot)
            {
                Destroy(t.gameObject);
            }
        }
        foreach(Transform t in ingrediantModelSlots)
        {
            foreach(Transform ct in t)
            {
                Destroy(ct.gameObject);
            }
        }

        for (int i = 0; i < MAX_COMBO; i++)
        {
            ingredients ingredient = GetContents()[i];
            if (ingredient != ingredients.empty)
            {
                SpawnModel(ingrediantModelSlots[i], (int) ingredient);
            }
        }
    }

    // remove first ingredient and shift the other ingredients forward on the list
    public ingredients RemoveIngredient()
    {
        ingredients ingredientToRemove = GetContents()[0];
        for(int i = 0; i < MAX_COMBO - 1; i++)
        {
            SetContents(i, GetContents()[i + 1]);
        }
        SetContents(MAX_COMBO - 1, ingredients.empty);
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
