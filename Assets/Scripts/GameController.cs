//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this script handles core application functions and scene loading
public class GameController : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    void OnApplicationQuit()
    {
        gameData.saveGameData();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Load1PlayerGame()
    {
        SceneManager.LoadScene("1PlayerStage1");
    }

    public void Load2PlayerGame()
    {
        SceneManager.LoadScene("2PlayerStage1");
    }
}
