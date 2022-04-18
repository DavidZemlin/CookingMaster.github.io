//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using UnityEngine;
using UnityEngine.SceneManagement;

// this script handles core application functions and serves as the main
//       connection between game managing classes and data managers
//       currently a lot of it's work is delegated to scene specific controllers
//       so consider merging this with the scene starter class and others
public class GameController : MonoBehaviour
{
    // ---data members---
    private GameData gameData;
    private int player1LastScore;
    private int player2LastScore;

    // ---getters---
    public GameData GetGameData() { return gameData; }
    public int GetPlayer1LastScore() { return player1LastScore; }
    public int GetPlayer2LastScore() { return player2LastScore; }

    // ---setters---
    private void SetGameData(GameData data) { gameData = data; }
    public void SetPlayer1LastScore(int newScore) { player1LastScore = newScore; }
    public void SetPlayer2LastScore(int newScore) { player2LastScore = newScore; }

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
    }

    // Update the high score rankings for 2 player games
    public void ApplyToHighScores2Player(int newScore, string playerName)
    {
        HighScore[] highScore = GetGameData().GetHighScore2Player();
        GetGameData().SetHighScore2Player(AddToScores(highScore, newScore, playerName));

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
                        highScoreOutput[i - 1].SetScore(newScore);
                        highScoreOutput[i - 1].SetName(playerName);
                        break;
                    }
                }
                else
                {
                    highScoreOutput[i].SetScore(newScore);
                    highScoreOutput[i].SetName(playerName);
                    break;
                }
            }
        }
        foreach (HighScore h in highScoreOutput)
        {
            Debug.Log(h.GetName() + " : " + h.GetScore());
        }
        return highScoreOutput;
    }

    // load scene - MainMenu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // load scene - MainMenuMainMenuReturnVersion
    //      this is the scene that is loaded after a 2 player game is finished
    public void LoadMainMenuReturnVersion()
    {
        SceneManager.LoadScene("MainMenuReturnVersion");
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
