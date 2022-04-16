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

public class ServingCounter : Counter
{
    // ---data members---
    private StageController stageController;
    private bool customerIsPresent;

    // ---getters---
    public bool GetCustomerIsPresent() { return customerIsPresent; }
    private StageController GetStageController() { return stageController; }

    // ---setters---
    private void SetCustomerIsPresent(bool newValue) { customerIsPresent = newValue; }
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
        item.DestroyItem();
    }
}
