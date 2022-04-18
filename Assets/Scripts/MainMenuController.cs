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
    [SerializeField] private TMP_Text WinnerMenuPlayer1Name;
    [SerializeField] private TMP_Text WinnerMenuPlayer2Name;
    [SerializeField] private TMP_Text WinnerMenuPlayer1Score;
    [SerializeField] private TMP_Text WinnerMenuPlayer2Score;
    [SerializeField] private GameObject WinnerMenuPlayer1Wins;
    [SerializeField] private GameObject WinnerMenuPlayer2Wins;
    [SerializeField] private GameObject WinnerMenuTieGame;

    private GameController gameController;
    private AudioSource musicPlayer;

    // ---setters---
    private void SetGameController(GameController gameCont) { gameController = gameCont; }
    private void SetMusicPlayer(AudioSource audioScource) { musicPlayer = audioScource; }

    // ---getters---
    private TMP_Text[] GetHighScoreText() { return highScoreText; }
    private TMP_Text[] GetHighScore2PlayerText() { return highScore2PlayerText; }
    private TMP_InputField GetPlayer1Name() { return player1Name; }
    private TMP_InputField GetPlayer2Name() { return player2Name; }
    private Slider GetMusicSlider() { return musicSlider; }
    private Slider GetSoundSlider() { return soundSlider; }
    private AudioSource GetSoundTester() { return soundTester; }
    private AudioSource GetMusicPlayer() { return musicPlayer; }
    private TMP_Text GetWinnerMenuPlayer1Name() { return WinnerMenuPlayer1Name; }
    private TMP_Text GetWinnerMenuPlayer2Name() { return WinnerMenuPlayer2Name; }
    private TMP_Text GetWinnerMenuPlayer1Score() { return WinnerMenuPlayer1Score; }
    private TMP_Text GetWinnerMenuPlayer2Score() { return WinnerMenuPlayer2Score; }
    private GameObject GetWinnerMenuPlayer1Wins() { return WinnerMenuPlayer1Wins; }
    private GameObject GetWinnerMenuPlayer2Wins() { return WinnerMenuPlayer2Wins; }
    private GameObject GetWinnerMenuTieGame() { return WinnerMenuTieGame; }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(GameController controller)
    {
        // initialize variables
        SetGameController(controller);
        SetMusicPlayer(gameObject.GetComponent<AudioSource>());

        // start  music
        AdjustMusicVolume();
        GetMusicPlayer().Play();
    }

    // load the winner screen and player names scores and who won
    public void LoadWinnerScrean()
    {
        if (gameController.GetPlayer1LastScore() > gameController.GetPlayer2LastScore())
        {
            GetWinnerMenuPlayer1Wins().SetActive(true);
            GetWinnerMenuPlayer2Wins().SetActive(false);
            GetWinnerMenuTieGame().SetActive(false);
        }
        else if (gameController.GetPlayer2LastScore() > gameController.GetPlayer1LastScore())
        {
            GetWinnerMenuPlayer1Wins().SetActive(false);
            GetWinnerMenuPlayer2Wins().SetActive(true);
            GetWinnerMenuTieGame().SetActive(false);
        }
        else
        {
            GetWinnerMenuPlayer1Wins().SetActive(false);
            GetWinnerMenuPlayer2Wins().SetActive(false);
            GetWinnerMenuTieGame().SetActive(true);
        }
        GetWinnerMenuPlayer1Name().SetText(gameController.GetGameData().GetPlayer1Name());
        GetWinnerMenuPlayer2Name().SetText(gameController.GetGameData().GetPlayer2Name());
        GetWinnerMenuPlayer1Score().SetText(gameController.GetPlayer1LastScore().ToString());
        GetWinnerMenuPlayer2Score().SetText(gameController.GetPlayer2LastScore().ToString());
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
        HighScore[] highScores = gameController.GetGameData().GetHighScore();
        HighScore[] highScores2Player = gameController.GetGameData().GetHighScore2Player();
        for (int i = 0; i < highScores.Length; i++)
        {
            GetHighScoreText()[i].SetText(highScores[i].GetName() + " : " + highScores[i].GetScore().ToString());
        }
        for (int i = 0; i < highScores2Player.Length; i++)
        {
            GetHighScore2PlayerText()[i].SetText(highScores2Player[i].GetName() + " : " + highScores2Player[i].GetScore().ToString());
        }
    }

    // change player 1 name
    public void ChangePlayer1Name()
    {
        gameController.GetGameData().SetPlayer1Name(GetPlayer1Name().textComponent.text);
    }

    // change player 2 name
    public void ChangePlayer2Name()
    {
        gameController.GetGameData().SetPlayer2Name(GetPlayer2Name().textComponent.text);
    }

    // change the music volume setting in game data
    public void ChangeMusicVolume()
    {
        gameController.GetGameData().SetMusicVolume((int) GetMusicSlider().value);
        AdjustMusicVolume();
    }

    // change the sound volume setting in game data
    public void ChangeSoundVolume()
    {
        gameController.GetGameData().SetSoundVolume((int) GetSoundSlider().value);
        soundTester.volume = ((float) gameController.GetGameData().GetSoundVolume() / 100.0f);
        soundTester.Play();
    }

    // read player 1 name into input field
    public void ReadPlayer1Name()
    {
        GetPlayer1Name().SetTextWithoutNotify(gameController.GetGameData().GetPlayer1Name());
    }

    // read player 2 name into input field
    public void ReadPlayer2Name()
    {
        GetPlayer2Name().SetTextWithoutNotify(gameController.GetGameData().GetPlayer2Name());
    }

    // update music volume slider to show the current volume setting in the game data
    public void ReadMusicVolume()
    {
        GetMusicSlider().SetValueWithoutNotify(gameController.GetGameData().GetMusicVolume());
    }

    // update sound volume slider to show the current volume setting in the game data
    public void ReadSoundVolume()
    {
        GetSoundSlider().SetValueWithoutNotify(gameController.GetGameData().GetSoundVolume());
    }

    // updates the music volume to the current level
    private void AdjustMusicVolume()
    {
        GetMusicPlayer().volume = ((float)gameController.GetGameData().GetMusicVolume() / 100.0f);
    }
}
