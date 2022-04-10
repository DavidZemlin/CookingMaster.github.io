using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Appliance
{
    [SerializeField] private GameObject contents;

    public override void Use(Player usingPlayer)
    {
        GameObject newItem = Instantiate(contents);
        newItem.transform.localPosition = Vector3.zero;
        Item newItemScript = newItem.GetComponent<Item>();
        usingPlayer.PickUpItem(newItemScript);
    }
}
