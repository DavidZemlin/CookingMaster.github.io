//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class manages messages sent from various objects sends the appropriate response to the display.
//      including score and time updates, and warnings for things like,
//      trying to pickup an item when your hands are full
public class HudController : MonoBehaviour
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

    // Flashes a full hands warning on the hud
    public void NoticeFullHands(Player player)
    {
        // Future Feature
        Debug.Log(player.gameObject.name + "'s hands are full.");
    }

    // Flashes an unchoppable item warning on the hud
    public void NoticeUnchopableItem(Player player)
    {
        // Future Feature

        Debug.Log(player.gameObject.name + "can't chop that.");
    }
    
    public void NoticeAddPlayerScore(int player, int score)
    {
        // future feature
        Debug.Log("Player " + player + "'s score +" + score);
        Debug.Log("Player " + player + "'s score total = " + stageController.GetPlayer1Score());
    }

    public void NoticeSubtractPlayerScore(int player, int score)
    {
        // future feature
        Debug.Log("Player " + player + "'s score -" + score);
        Debug.Log("Player " + player + "'s score total = " + stageController.GetPlayer1Score());
    }
}
