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

public class DishTag : MonoBehaviour
{
    // ---data members---
    [SerializeField] private GameObject TagBG;
    [SerializeField] private Transform[] slots = new Transform[ItemStats.MAX_COMBO];
    [SerializeField] private GameObject[] ingredientImages;

    // ---getters---
    private GameObject GetTagBG() { return TagBG; }
    private Transform[] GetSlots() { return slots; }
    private GameObject[] GetingredientImages() { return ingredientImages; }

    // ---setters---

    // ---primary methods---
    public void ChangeTag(Item.ingredients[] ingredients)
    {
        GetTagBG().SetActive(false);
        for (int i = 0; i < ItemStats.MAX_COMBO; i++)
        {
            foreach (Transform t in slots[i])
            {
                Destroy(t.gameObject);
            }
            int imageNumber = (int)ingredients[i];
            GameObject tagPart = GameObject.Instantiate(GetingredientImages()[imageNumber]);
            tagPart.transform.parent = GetSlots()[i];
            tagPart.transform.localEulerAngles = Vector3.zero;
            tagPart.transform.localPosition = Vector3.zero;
            if (imageNumber != 0)
            {
                GetTagBG().SetActive(true);
            }
        }
    }
}
