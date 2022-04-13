//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// this class connects the main menu to relevant game data and functions
public class MainMenuController : MonoBehaviour
{
    // ---data members---
    [SerializeField] private TMP_Text[] highScoreText = new TMP_Text[GameData.NUMBER_OF_HIGH_SCORES];
    [SerializeField] private TMP_Text[] highScore2PlayerText = new TMP_Text[GameData.NUMBER_OF_HIGH_SCORES];
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private GameController gameController;
    private GameData gameData;

    // ---unity methods---
    private void Awake()
    {
        // initialize variables
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
    }

    // ---primary methods---

    // menu call to start a 1 player game
    public void Start1PlayerGame()
    {
        gameController.Load1PlayerGame();
    }

    // menu call to start a 2 player game
    public void Start2PlayerGame()
    {
        gameController.Load2PlayerGame();
    }

    // update the text of high score display to match the current data
    public void UpdateHighScores()
    {
        HighScore[] highScores = gameData.GetHighScore();
        HighScore[] highScores2Player = gameData.GetHighScore2Player();
        for (int i = 0; i < highScores.Length; i++)
        {
            highScoreText[i].SetText(highScores[i].getName() + " : " + highScores[i].getScore().ToString());
        }
        for (int i = 0; i < highScores2Player.Length; i++)
        {
            highScore2PlayerText[i].SetText(highScores2Player[i].getName() + " : " + highScores2Player[i].getScore().ToString());
        }
    }

    // change the music volume setting in game data
    public void ChangeMusicVolume()
    {
        gameData.SetMusicVolume((int) musicSlider.value);
    }

    // change the sound volume setting in game data
    public void ChangeSoundVolume()
    {
        gameData.SetSoundVolume((int) soundSlider.value);
    }

    // update music volume slider to show the current volume setting in the game data
    public void ReadMusicVolume()
    {
        musicSlider.SetValueWithoutNotify(gameData.GetMusicVolume());
    }

    // update sound volume slider to show the current volume setting in the game data
    public void ReadSoundVolume()
    {
        soundSlider.SetValueWithoutNotify(gameData.GetSoundVolume());
    }
}
