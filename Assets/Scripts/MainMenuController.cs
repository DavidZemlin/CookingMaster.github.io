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
    [SerializeField] private TMP_InputField player1Name;
    [SerializeField] private TMP_InputField player2Name;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private AudioSource soundTester;

    private GameController gameController;
    private GameData gameData;

    // ---getters---
    private TMP_Text[] GetHighScoreText() { return highScoreText; }
    private TMP_Text[] GetHighScore2PlayerText() { return highScore2PlayerText; }
    private TMP_InputField GetPlayer1Name() { return player1Name; }
    private TMP_InputField GetPlayer2Name() { return player2Name; }
    private Slider GetMusicSlider() { return musicSlider; }
    private Slider GetSoundSlider() { return soundSlider; }
    private AudioSource GetSoundTester() { return soundTester; }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(GameData data, GameController controller)
    {
        // initialize variables
        gameController = controller;
        gameData = data;
    }

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
            GetHighScoreText()[i].SetText(highScores[i].getName() + " : " + highScores[i].getScore().ToString());
        }
        for (int i = 0; i < highScores2Player.Length; i++)
        {
            GetHighScore2PlayerText()[i].SetText(highScores2Player[i].getName() + " : " + highScores2Player[i].getScore().ToString());
        }
    }

    // change player 1 name
    public void ChangePlayer1Name()
    {
        gameData.SetPlayer1Name(GetPlayer1Name().textComponent.text);
    }

    // change player 2 name
    public void ChangePlayer2Name()
    {
        gameData.SetPlayer2Name(GetPlayer2Name().textComponent.text);
    }

    // change the music volume setting in game data
    public void ChangeMusicVolume()
    {
        gameData.SetMusicVolume((int) GetMusicSlider().value);
        gameController.AdjustMusicVolume();
    }

    // change the sound volume setting in game data
    public void ChangeSoundVolume()
    {
        gameData.SetSoundVolume((int) GetSoundSlider().value);
        soundTester.volume = ((float)gameData.GetSoundVolume() / 100.0f);
        soundTester.Play();
    }

    // read player 1 name into input field
    public void ReadPlayer1Name()
    {
        GetPlayer1Name().SetTextWithoutNotify(gameData.GetPlayer1Name());
    }

    // read player 2 name into input field
    public void ReadPlayer2Name()
    {
        GetPlayer2Name().SetTextWithoutNotify(gameData.GetPlayer2Name());
    }

    // update music volume slider to show the current volume setting in the game data
    public void ReadMusicVolume()
    {
        GetMusicSlider().SetValueWithoutNotify(gameData.GetMusicVolume());
    }

    // update sound volume slider to show the current volume setting in the game data
    public void ReadSoundVolume()
    {
        GetSoundSlider().SetValueWithoutNotify(gameData.GetSoundVolume());
    }
}
