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

// this class is the primary handler of stage data, initialization and flow
public class StageController : MonoBehaviour
{
    // ---data members---
    private GameController gameController;
    private HudController hudController;
    private AudioSource musicPlayer;
    private Player player1;
    private Player player2;
    private int player1Score;
    private int player2Score;

    // ---getters---
    private HudController GetHudController() { return hudController; }
    private AudioSource GetMusicPlayer() { return musicPlayer; }
    public int GetPlayer1Score() { return player1Score; }
    public int GetPlayer2Score() { return player2Score; }

    // ---setters---
    private void SetGameController(GameController gameCont) { gameController = gameCont; }
    private void SetHudController(HudController hudCont) { hudController = hudCont; }
    private void SetMusicPlayer(AudioSource audioScource) { musicPlayer = audioScource; }
    private void SetPlayer1Score(int newScore) { player1Score = newScore; }
    private void SetPlayer2Score(int newScore) { player2Score = newScore; }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(GameController controller)
    {
        // initialize variables
        SetGameController(controller);
        SetHudController(GameObject.FindGameObjectWithTag("HUD").GetComponent<HudController>());
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
    }

    public void AddScore(Player player, int score)
    {
        int playerNum = player.GetPlayerNumber();
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
    public void SubtractScore(Player player, int score)
    {
        int playerNum = player.GetPlayerNumber();
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
