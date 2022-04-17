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

// this class is the primary handler of stage data, initialization and level flow
public class StageController : MonoBehaviour
{
    // ---data members---
    public const float STARTING_TIMER = 180.0f;

    [SerializeField] private ServingCounter[] servingCounters;

    private GameController gameController;
    private HudController hudController;
    private AudioSource musicPlayer;
    private Player player1;
    private Player player2;
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
    [SerializeField] private float currentPatience; // ---------------------------------- serialized for debug
    [SerializeField] private float currentBusinessPace; // ---------------------------------- serialized for debug
    [SerializeField] private float timeElapsed; // ---------------------------------- serialized for debug
    [SerializeField] private float timeElapsedThisStep; // ---------------------------------- serialized for debug
    [SerializeField] private float timeLeftPlayer1; // ---------------------------------- serialized for debug
    [SerializeField] private float timeLeftPlayer2; // ---------------------------------- serialized for debug

    // ---getters---
    private HudController GetHudController() { return hudController; }
    private AudioSource GetMusicPlayer() { return musicPlayer; }
    public int GetPlayer1Score() { return player1Score; }
    public int GetPlayer2Score() { return player2Score; }
    public float GetPlayer1TimeLeft() { return timeLeftPlayer1; }
    public float GetPlayer2TimeLeft() { return timeLeftPlayer2; }

    // ---setters---
    private void SetGameController(GameController gameCont) { gameController = gameCont; }
    private void SetHudController(HudController hudCont) { hudController = hudCont; }
    private void SetMusicPlayer(AudioSource audioScource) { musicPlayer = audioScource; }
    private void SetPlayer1Score(int newScore) { player1Score = newScore; }
    private void SetPlayer2Score(int newScore) { player2Score = newScore; }

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
            player1 = player1Obj.GetComponent<Player>();
            player1.Initialize(this, GetHudController());
        }
        else
        {
            Debug.Log("Player 1 is missing! stop the show! Check his trailer!");
        }
        if (player2Obj != null)
        {
            // Initialize Player 2
            player2 = player2Obj.GetComponent<Player>();
            player2.Initialize(this, GetHudController());
        }
        else
        {
            // here we need to put the hud controller into 1 player mode
        }

        // start  music
        GetMusicPlayer().volume = gameController.GetGameData().GetMusicVolume();
        GetMusicPlayer().Play();

        // fill player times and mark stage start time
        timeLeftPlayer1 = STARTING_TIMER;
        timeLeftPlayer2 = STARTING_TIMER;
        startTime = Time.timeSinceLevelLoad;
    }

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

    public void SubtractScore(int playerNum, int score)
    {
        if (playerNum == 1)
        {
            SetPlayer1Score(GetPlayer1Score() - score);
        }
        else
        {
            SetPlayer2Score(GetPlayer2Score() - score);
        }
        hudController.NoticeSubtractPlayerScore(playerNum, score);
    }
}
