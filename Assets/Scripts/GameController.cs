//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this script handles core application functions and serves as the main
//       connection between game managing classes and data managers
public class GameController : MonoBehaviour
{
    // ---data members---
    private GameData gameData;

    // ---setters---
    private void SetGameData(GameData data) { gameData = data; }

    // ---getters---
    public GameData GetGameData() { return gameData; }

    // ---unity methods---
    void OnApplicationQuit()
    {
        GetGameData().saveGameData();
    }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(GameData data)
    {
        // set game data
        SetGameData(data);

        // mark this data object to not be destroyed on load
        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name == "StartUp")
        {
            LoadMainMenu();
        }
    }

    // load scene - MainMenu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // load scene - 1PlayerGame
    public void Load1PlayerGame()
    {
        SceneManager.LoadScene("1PlayerStage1");
    }

    // load scene - 2PlayerGame
    public void Load2PlayerGame()
    {
        SceneManager.LoadScene("2PlayerStage1");
    }
}
