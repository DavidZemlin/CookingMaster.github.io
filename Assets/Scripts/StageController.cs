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

    // ---setters---
    private void SetGameController(GameController gameCont) { gameController = gameCont; }
    private void SetHudController(HudController hudCont) { hudController = hudCont; }
    private void SetMusicPlayer(AudioSource audioScource) { musicPlayer = audioScource; }

    // ---getters---
    private HudController GetHudController() { return hudController; }
    private AudioSource GetMusicPlayer() { return musicPlayer; }

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
}
