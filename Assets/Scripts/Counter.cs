using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : Appliance
{
    [SerializeField] private Transform itemSlot;
    private Item itemOnCounter;

    public Item GetItemOnCounter() { return itemOnCounter; }
    private Transform GetItemSlot() { return itemSlot; }

    private void SetItemOnCounter(Item item) { itemOnCounter = item; }

    public void recieveItem(Item item)
    {
        if (GetItemOnCounter() == null)
        {
            SetItemOnCounter(item);
            item.transform.parent = GetItemSlot();
            item.transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveItem()
    {

    }
}
