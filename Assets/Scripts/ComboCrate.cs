//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using UnityEngine;

// this is a special type of crate that can dispense plates or combos
public class ComboCrate : Crate
{
    // ---data members---
    [SerializeField] bool hasPlate = false;
    [SerializeField] private Item.ingredients[] plateContents = new Item.ingredients[ItemStats.MAX_COMBO];

    // ---getters---
    private bool GetHasPlate() { return hasPlate; }
    public Item.ingredients[] GetPlateContents() { return plateContents; } 

    // ---primary methods---

    // create a combo item and give it to the player
    public override void Use(Player usingPlayer)
    {
        bool hasIngredients = false;
        foreach (Item.ingredients i in GetPlateContents())
        {
            if(i != Item.ingredients.empty)
            {
                hasIngredients = true;
            }
        }

        if (hasIngredients || GetHasPlate())
        {
            GameObject newItem = Instantiate(contents);
            newItem.transform.localPosition = Vector3.zero;
            ComboItem newItemScript = newItem.GetComponent<ComboItem>();
            newItemScript.SetPlate(GetHasPlate());
            for (int i = 0; i < GetPlateContents().Length; i++)
            {
                newItemScript.SetContents(i, GetPlateContents()[i]);
            }
            newItemScript.UpdateCombo();
            usingPlayer.PickUpItem(newItemScript);
        }
    }
}
