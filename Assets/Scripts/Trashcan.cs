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

// Trashcan destroys the left hand item of the player that uses it
//      players lose points for throwing out items
public class Trashcan : Counter
{
    // ---data members---
    private StageController stageController;

    // ---getters---
    private StageController GetStageController() { return stageController; }

    // ---setters---
    private void SetStageController(StageController stageCont) { stageController = stageCont; }

    // ---unity methods---
    private void Awake()
    {
        SetStageController(GameObject.FindGameObjectWithTag("Stage Controller").GetComponent<StageController>());
    }

    // ---primary methods---

    // this version of receiveItem will destroy the item and deduct score from the player who threw it out
    public override void receiveItem(Item item)
    {
        item.OnPlace(this);
        stageController.SubtractScore(item.GetLastHoldingPlayer().GetPlayerNumber(), item.GetScore());
        item.DestroyItem();
    }
}
