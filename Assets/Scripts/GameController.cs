//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this script handles core application functions and serves as the main connection between game managing classes
public class GameController : MonoBehaviour
{
    // ---data members---
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip stageMusic;

    private GameData gameData;
    private AudioSource musicPlayer;

    // ---unity methods---
    void OnApplicationQuit()
    {
        gameData.saveGameData();
    }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(GameData data)
    {
        // set game data
        gameData = data;

        // initialize music settings
        musicPlayer = gameObject.GetComponentInChildren<AudioSource>();
        AdjustMusicVolume();

        // mark this data object to not be destroyed on load
        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name == "StartUp")
        {
            LoadMainMenu();
        }
    }

    // updates the music volume to the current level
    public void AdjustMusicVolume()
    {
        musicPlayer.volume = ((float) gameData.GetMusicVolume() / 100.0f);
    }

    // load scene - MainMenu
    public void LoadMainMenu()
    {
        musicPlayer.clip = menuMusic;
        musicPlayer.Play();
        SceneManager.LoadScene("MainMenu");
    }

    // load scene - 1PlayerGame
    public void Load1PlayerGame()
    {
        musicPlayer.clip =stageMusic;
        musicPlayer.Play();
        SceneManager.LoadScene("1PlayerStage1");
    }

    // load scene - 2PlayerGame
    public void Load2PlayerGame()
    {
        musicPlayer.clip = stageMusic;
        musicPlayer.Play();
        SceneManager.LoadScene("2PlayerStage1");
    }
}
