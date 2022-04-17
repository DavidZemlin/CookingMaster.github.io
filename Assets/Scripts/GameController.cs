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

    // Update the high score rankings for 1 player games
    public void ApplyToHighScores1Player(int newScore, string playerName)
    {
        HighScore[] highScore = GetGameData().GetHighScore();
        GetGameData().SetHighScore(AddToScores(highScore, newScore, playerName));
        GetGameData().saveGameData();
    }

    // Update the high score rankings for 2 player games
    public void ApplyToHighScores2Player(int newScore, string playerName)
    {
        HighScore[] highScore = GetGameData().GetHighScore2Player();
        GetGameData().SetHighScore2Player(AddToScores(highScore, newScore, playerName));
        GetGameData().saveGameData();

    }

    // used to add a score into a generic array of high scores
    private HighScore[] AddToScores(HighScore[] highScoresInput, int newScore, string playerName)
    {
        HighScore[] highScoreOutput = highScoresInput;
        if (newScore > highScoreOutput[highScoreOutput.Length - 1].GetScore())
        {
            for (int i = highScoreOutput.Length - 1; i > 0; i--)
            {
                if (newScore > highScoreOutput[i - 1].GetScore())
                {
                    highScoreOutput[i].SetScore(highScoreOutput[i - 1].GetScore());
                    highScoreOutput[i].SetName(highScoreOutput[i - 1].GetName());
                    if (i == 1)
                    {
                        Debug.Log("111");
                        highScoreOutput[i - 1].SetScore(newScore);
                        highScoreOutput[i - 1].SetName(playerName);
                    }
                }
                else
                {
                    Debug.Log("222");
                    highScoreOutput[i].SetScore(newScore);
                    highScoreOutput[i].SetName(playerName);
                }
            }
        }
        return highScoreOutput;
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
