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

// special items that give players a boast when picked up
public class PowerUp : Item
{
    // ---Enums---
    public enum powerType
    {
        speed,
        time,
        score,
    }

    // ---data members---
    public const float SPEED_BOOST_MULTIPLIER = 2.0f;
    public const float TIME_UP = 30.0f;
    public const int SCORE_UP = 100;

    [SerializeField] private powerType powerUpType;
    [SerializeField] private GameObject Player1model;
    [SerializeField] private GameObject Player2model;
    private StageController stageController;
    private int playerNumber;

    // ---getters---
    public int GetPlayerNumber() { return playerNumber; }
    private powerType GetPowerUpType() { return powerUpType; }
    private GameObject GetPlayer1Model() { return Player1model; }
    private GameObject GetPlayer2Model() { return Player2model; }

    // ---setters---
    public void intSetPlayerNumber(int newValue) { playerNumber = newValue; }
    private void SetStageController(StageController stageCont) { stageController = stageCont; }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(int playerNum, StageController stageCont)
    {
        SetStageController(stageCont);
        intSetPlayerNumber(playerNum);
        if(playerNum == 1)
        {
            GetPlayer1Model().SetActive(true);
        }
        else
        {
            GetPlayer2Model().SetActive(true);
        }
    }

    // activate power-up
    public void Activate()
    {
        if (GetPowerUpType() == powerType.speed)
        {
            stageController.SpeedBoost(playerNumber, SPEED_BOOST_MULTIPLIER);
        }
        else if(GetPowerUpType() == powerType.time)
        {
            stageController.AddTimeToPlayerClock(playerNumber, TIME_UP);
        }
        if (GetPowerUpType() == powerType.score)
        {
            stageController.AddScore(playerNumber, SCORE_UP);
        }
        GetCurrentCounter().RemoveItem();
        Destroy(this.gameObject);
    }
}
