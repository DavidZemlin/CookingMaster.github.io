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

// this appliance can be used to turn chopable items into combo items.
//      it also links to a "sideprep" counter top to store and combine combo items
public class CuttingBoard : Counter
{
    // ---data members---
    [SerializeField] private Counter sideBoard;

    // ---getters---
    public Counter GetSideBoard() { return sideBoard; }

    // ---primary methods---

    // if the cutting board is used, it will allow the player to chop chopable items
    public override void Use(Player usingPlayer)
    {
        Item item = GetItemOnCounter();
        if (item != null)
        {
            if (item.GetType() == typeof(Chopable))
            {
                Chopable chopableItem = (Chopable) item;
                chopableItem.SetCuttingBoard(this);
                usingPlayer.StartChop((Chopable) item);
            }
            else // send notice to hud controller if the item is not chopable
            {
                usingPlayer.GetHudController().NoticeUnchopableItem(usingPlayer);
            }
        }
    }

    // called after chopping is completed and attempts to move chopped item to the side board
    public void ShiftCurrentItemToSideBoard()
    {
        Item itemOnCuttingBoard = GetItemOnCounter();
        if (itemOnCuttingBoard != null)
        {
            Item itemOnSideBoard = GetSideBoard().GetItemOnCounter();
            if (itemOnSideBoard == null)
            {
                RemoveItem();
                GetSideBoard().receiveItem(itemOnCuttingBoard);
            }
            else if (itemOnSideBoard.GetType() == typeof(ComboItem) &&  itemOnCuttingBoard.GetType() == typeof(ComboItem))
            {
                ComboItem cuttingCombo = itemOnCuttingBoard.GetComponent<ComboItem>();
                ComboItem sideCombo = itemOnSideBoard.GetComponent<ComboItem>();
                sideCombo.Combine(cuttingCombo);
            }
        }
    }
}
