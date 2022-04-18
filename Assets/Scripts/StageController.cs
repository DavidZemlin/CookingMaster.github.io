//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections.Generic;
using UnityEngine;

// this class is the primary handler of stage data, initialization and level flow
public class StageController : MonoBehaviour
{
    // ---data members---
    public const float SPEED_BOOST_DURATION = 15.0f;

    [SerializeField] private ServingCounter[] servingCounters;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private int startingTimerValue;


    private GameController gameController;
    private HudController hudController;
    private AudioSource musicPlayer;
    private Player player1;
    private Player player2;
    private List<Counter> powerUpDropPoints = new List<Counter>();
    private int player1Score;
    private int player2Score;

    // these data member will be accessed directly, as they will be accessed at least every fixed update
    [Tooltip("This is the starting amount of seconds a customer will wait")]
    [SerializeField] private float startingCustomerPatience;
    [Tooltip("This amount will be multiplied by the current elapsed time (in seconds) and subtracted from the starting customer patience to calculate the patience of new customers. This number should be a VERY small fraction")]
    [SerializeField] private float patienceReductionFactor;
    [Tooltip("This allows you to set a minimum floor to the patience of a customer")]
    [SerializeField] private float minimumPatience;
    [Tooltip("This is the starting amount of seconds it will take for a new patron to come")]
    [SerializeField] private float startingBusinessPace;
    [Tooltip("This amount will be multiplied by the current elapsed time (in seconds) and subtracted from the starting Business Pace to calculate the time between new customer. This number should be a VERY small fraction")]
    [SerializeField] private float bussinessIncreaseFactor;
    [Tooltip("This allows you to set a minimum floor to the time between customers")]
    [SerializeField] private float minimumTimeBetweenCustomers;

    private float startTime;
    private float timeOfNextCustomerArival;
    private float currentPatience;
    private float currentBusinessPace;
    private float timeElapsed;
    private float timeElapsedThisStep;
    private float timeLeftPlayer1;
    private float timeLeftPlayer2;
    private float speedBoostLeftPlayer1;
    private float speedBoostLeftPlayer2;

    // ---getters---
    private HudController GetHudController() { return hudController; }
    private AudioSource GetMusicPlayer() { return musicPlayer; }
    private int GetStartingTimerValue() { return startingTimerValue; }
    private Player GetPlayer1() { return player1; }
    private Player GetPlayer2() { return player2; }
    public int GetPlayer1Score() { return player1Score; }
    public int GetPlayer2Score() { return player2Score; }
    public float GetPlayer1TimeLeft() { return timeLeftPlayer1; }
    public float GetPlayer2TimeLeft() { return timeLeftPlayer2; }

    // ---setters---
    private void SetGameController(GameController gameCont) { gameController = gameCont; }
    private void SetHudController(HudController hudCont) { hudController = hudCont; }
    private void SetMusicPlayer(AudioSource audioScource) { musicPlayer = audioScource; }
    private void SetPlayer1(Player newPlayer) { player1 = newPlayer; }
    private void SetPlayer2(Player newPlayer) { player2 = newPlayer; }
    private void SetPlayer1Score(int newScore) { player1Score = newScore; }
    private void SetPlayer2Score(int newScore) { player2Score = newScore; }
    private void SetPlayer1TimeLeft(float newTime) { timeLeftPlayer1 = newTime; }
    private void SetPlayer2TimeLeft(float newTime) { timeLeftPlayer2 = newTime; }

    // ---unity methods---
    private void FixedUpdate()
    {
        // update elapsed time
        float previousTimeElapsed = timeElapsed;
        timeElapsed = Time.timeSinceLevelLoad - startTime;
        timeElapsedThisStep = timeElapsed - previousTimeElapsed;
        timeLeftPlayer1 = timeLeftPlayer1 - timeElapsedThisStep;
        timeLeftPlayer2 = timeLeftPlayer2 - timeElapsedThisStep;

        currentPatience = startingCustomerPatience - (timeElapsed * patienceReductionFactor);
        currentBusinessPace = startingBusinessPace - (timeElapsed * bussinessIncreaseFactor);

        if (timeElapsed > timeOfNextCustomerArival)
        {
            SummonPatron();
            timeOfNextCustomerArival = timeElapsed + currentBusinessPace;
        }

        // manage active power-ups
        if (timeElapsed > speedBoostLeftPlayer1)
        {
            GetPlayer1().StopSpeedBoost();
        }
        if (player2 != null && timeElapsed > speedBoostLeftPlayer2)
        {
            GetPlayer2().StopSpeedBoost();
        }

        // check for time expiration and end the level if both player's time is out
        if (player1.gameObject.activeSelf && timeLeftPlayer1 <= 0.0f)
        {
            player1.gameObject.SetActive(false);
        }
        if (player2 != null && timeLeftPlayer2 <= 0.0f)
        {
            player2.gameObject.SetActive(false);
        }
        if (player2 != null)
        {
            if (!player1.gameObject.activeSelf && !player2.gameObject.activeSelf)
            {
                endStage();
            }
        }
        else
        {
            if (!player1.gameObject.activeSelf)
            {
                endStage();
            }
        }
    }
    
    // ---primary methods---

    // used instead of "awake"
    public void Initialize(GameController controller)
    {
        // initialize variables
        SetGameController(controller);
        SetHudController(GameObject.FindGameObjectWithTag("HUD").GetComponent<HudController>());
        hudController.Initialize(this, controller);
        SetMusicPlayer(gameObject.GetComponent<AudioSource>());

        GameObject player1Obj = GameObject.FindGameObjectWithTag("Player");
        GameObject player2Obj = GameObject.FindGameObjectWithTag("Player2");

        if(player1Obj != null)
        {
            // Initialize Player 1
            SetPlayer1(player1Obj.GetComponent<Player>());
            GetPlayer1().Initialize(this, GetHudController());
        }
        else
        {
            Debug.Log("Player 1 is missing! stop the show! Check his trailer!");
        }
        if (player2Obj != null)
        {
            // Initialize Player 2
            SetPlayer2(player2Obj.GetComponent<Player>());
            GetPlayer2().Initialize(this, GetHudController());
        }
        else
        {
            // here we need to put the hud controller into 1 player mode
            hudController.HidePlayer2Hud();
        }

        // find all counters
        GameObject[] appliences = GameObject.FindGameObjectsWithTag("Appliance");
        foreach(GameObject g in appliences)
        {
            Counter counterScript = g.GetComponent<Counter>();
            if (counterScript != null && counterScript.GetCanSpawnPowerUps())
            {
                powerUpDropPoints.Add(counterScript);
            }
        }

        // start  music
        GetMusicPlayer().volume = gameController.GetGameData().GetMusicVolume();
        GetMusicPlayer().Play();

        // fill player times and mark stage start time
        timeLeftPlayer1 = GetStartingTimerValue();
        timeLeftPlayer2 = GetStartingTimerValue();
        startTime = Time.timeSinceLevelLoad;
    }

    // brings a patron to one of the counters
    public void SummonPatron()
    {
        foreach (ServingCounter s in servingCounters)
        {
            if (!s.GetCustomerIsPresent())
            {
                s.SummonPatorn(currentPatience);
                return;
            }
        }
    }

    // adds score to a player
    public void AddScore(int playerNum, int score)
    {
        if (playerNum == 1)
        {
            SetPlayer1Score(GetPlayer1Score() + score);
        }
        else
        {
            SetPlayer2Score(GetPlayer2Score() + score);
        }
        hudController.NoticeAddPlayerScore(playerNum, score);
    }

    // subtracts score from a player
    public void SubtractScore(int playerNum, int score)
    {
        if (playerNum == 1 && player1.gameObject.activeSelf)
        {
            SetPlayer1Score(GetPlayer1Score() - score);
        }
        else if (playerNum == 2 && player2.gameObject.activeSelf)
        {
            SetPlayer2Score(GetPlayer2Score() - score);
        }
        hudController.NoticeSubtractPlayerScore(playerNum, score);
    }

    // spawns a random Power up in a random unoccupied space
    public void SpawnPowerUp(int playerNumber)
    {
        List<Counter> openCounters = new List<Counter>();
        foreach (Counter c in powerUpDropPoints)
        {
            if (c.GetItemOnCounter() == null)
            {
                openCounters.Add(c);
            }
        }

        if (openCounters.Count > 0)
        {
            int powerUpRoll = Random.Range(0, powerUps.Length);
            GameObject powerUpObj = Instantiate(powerUps[powerUpRoll]);
            PowerUp powerUpItem = powerUpObj.GetComponent<PowerUp>();
            powerUpItem.Initialize(playerNumber, this);

            int locationRoll = Random.Range(0, openCounters.Count);
            openCounters[locationRoll].receiveItem(powerUpItem);
        }
    }

    // activate speed boost for a player
    public void SpeedBoost(int playerNumber, float boostMultiplier)
    {
        if (playerNumber == 1)
        {
            speedBoostLeftPlayer1 = timeElapsed + SPEED_BOOST_DURATION;
            GetPlayer1().StartSpeedBoost(boostMultiplier);
        }
        else
        {
            speedBoostLeftPlayer2 = timeElapsed + SPEED_BOOST_DURATION;
            GetPlayer2().StartSpeedBoost(boostMultiplier);
        }
    }

    // increase Player time left
    public void AddTimeToPlayerClock(int playerNumber, float timeAdd)
    {
        if (playerNumber == 1)
        {
            SetPlayer1TimeLeft(GetPlayer1TimeLeft() + timeAdd);
        }
        else
        {
            SetPlayer2TimeLeft(GetPlayer2TimeLeft() + timeAdd);
        }
    }

    private void endStage()
    {
        if (player2 == null)
        {
            gameController.SetPlayer1LastScore(player1Score);
            gameController.ApplyToHighScores1Player(player1Score, gameController.GetGameData().GetPlayer1Name());
            gameController.LoadMainMenu();
        }
        else
        {
            gameController.SetPlayer1LastScore(player1Score);
            gameController.SetPlayer2LastScore(player2Score);
            gameController.ApplyToHighScores2Player(player1Score, gameController.GetGameData().GetPlayer1Name());
            gameController.ApplyToHighScores2Player(player2Score, gameController.GetGameData().GetPlayer2Name());
            gameController.LoadMainMenuReturnVersion();
        }
    }
}
