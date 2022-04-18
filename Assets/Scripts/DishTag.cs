//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using UnityEngine;

// manages the tag that is used to show dish contents
public class DishTag : MonoBehaviour
{
    // ---data members---
    [SerializeField] private GameObject plateImage;
    [SerializeField] private Transform[] slots = new Transform[ItemStats.MAX_COMBO];
    [SerializeField] private GameObject[] ingredientImages;

    // ---getters---
    private GameObject GetPlateImage() { return plateImage; }
    private Transform[] GetSlots() { return slots; }
    private GameObject[] GetingredientImages() { return ingredientImages; }

    // ---primary methods---
    public void ChangeTag(Item.ingredients[] ingredients, bool hasPlate)
    {
        GetPlateImage().SetActive(false);
        if (hasPlate)
        {
            GetPlateImage().SetActive(true);
        }
        for (int i = 0; i < ItemStats.MAX_COMBO; i++)
        {
            foreach (Transform t in slots[i])
            {
                Destroy(t.gameObject);
            }
            int imageNumber = (int)ingredients[i];
            GameObject tagPart = GameObject.Instantiate(GetingredientImages()[imageNumber]);
            tagPart.transform.SetParent(GetSlots()[i]);
            tagPart.transform.localEulerAngles = Vector3.zero;
            tagPart.transform.localPosition = Vector3.zero;
            tagPart.transform.localScale = Vector3.one;
        }
    }
}
