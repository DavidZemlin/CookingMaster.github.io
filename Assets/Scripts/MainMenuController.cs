using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    private GameController gameController;
    private GameData gameData;
    [SerializeField] private TMP_Text[] highScoreText = new TMP_Text[GameData.NUMBER_OF_HIGH_SCORES];
    [SerializeField] private TMP_Text[] highScore2PlayerText = new TMP_Text[GameData.NUMBER_OF_HIGH_SCORES];
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
    }

    public void Start1PlayerGame()
    {
        gameController.Load1PlayerGame();
    }

    public void Start2PlayerGame()
    {
        gameController.Load2PlayerGame();
    }

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

    public void ChangeMusicVolume()
    {
        gameData.SetMusicVolume((int) musicSlider.value);
    }

    public void ChangeSoundVolume()
    {
        gameData.SetSoundVolume((int) soundSlider.value);
    }

    public void ReadMusicVolume()
    {
        musicSlider.SetValueWithoutNotify(gameData.GetMusicVolume());
    }

    public void ReadSoundVolume()
    {
        soundSlider.SetValueWithoutNotify(gameData.GetSoundVolume());
    }
}
