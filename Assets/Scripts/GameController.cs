//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this script handles core application functions and scene loading
public class GameController : MonoBehaviour
{
    // ---data members---
    [SerializeField] private GameData gameData;

    // ---unity methods---
    void OnApplicationQuit()
    {
        gameData.saveGameData();
    }

    private void Awake()
    {
        // find game data object if it has not been assigned
        if (gameData == null)
        {
            GameObject dataObj = GameObject.FindGameObjectWithTag("GameData");
            gameData = dataObj.GetComponent<GameData>();
        }

        // mark this data object to not be destroyed on load
        DontDestroyOnLoad(gameObject);
    }

    // ---primary methods---

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
