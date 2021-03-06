//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.

using System.Collections.Generic;
using System.IO;
using UnityEngine;

// this script holds data that needs to be kept between scenes or game sessions
public class GameData : MonoBehaviour
{
    // ---data members---
    public const int NUMBER_OF_HIGH_SCORES = 10;
    public const string END_OF_CATAGORY_LINE = "--c--";

    private const string SAVE_FILE_NAME = "save.dat";
    private const string DEFAULT_NAME = "Player";
    private const int DEFAULT_SCORE = -1000;
    private const int DEFAULT_VOLUME = 100;

    private GameController gameController;
    private HighScore[] highScores = new HighScore[NUMBER_OF_HIGH_SCORES];
    private HighScore[] highScores2Player = new HighScore[NUMBER_OF_HIGH_SCORES];
    private string player1Name = DEFAULT_NAME;
    private string player2Name = DEFAULT_NAME;
    private int musicVolume = DEFAULT_VOLUME;
    private int soundVolume = DEFAULT_VOLUME;

    // getters
    public HighScore[] GetHighScore() { return highScores; }
    public HighScore[] GetHighScore2Player() { return highScores2Player; }
    public string GetPlayer1Name() { return player1Name; }
    public string GetPlayer2Name() { return player2Name; }
    public int GetMusicVolume() { return musicVolume; }
    public int GetSoundVolume() { return soundVolume; }

    //setters
    public void SetHighScore(HighScore[] newHighScores) { highScores = newHighScores; }
    public void SetHighScore2Player(HighScore[] newHighScores) { highScores2Player = newHighScores; }
    public void SetPlayer1Name(string newName) { player1Name = newName; }
    public void SetPlayer2Name(string newName) { player2Name = newName; }
    public void SetMusicVolume(int newVolume) { musicVolume = newVolume; }
    public void SetSoundVolume(int newVolume) { soundVolume = newVolume; }
    private void SetGameController(GameController newGameController) { gameController = newGameController; }

    // ---primary methods---

    // used instead of "awake"
    public void Initialize(GameController controller)
    {
        // mark game data object as "don't destroy"
        DontDestroyOnLoad(gameObject);

        // Initialize game controller references
        SetGameController(controller);

        // initialize player names;
        SetPlayer1Name(DEFAULT_NAME + " 1");
        SetPlayer2Name(DEFAULT_NAME + " 2");

        // initialize volume levels;
        SetMusicVolume(DEFAULT_VOLUME);
        SetSoundVolume(DEFAULT_VOLUME);

        // initialize high score lists;
        for (int i = 0; i < NUMBER_OF_HIGH_SCORES; i++)
        {
            highScores[i] = new HighScore(DEFAULT_NAME, DEFAULT_SCORE);
        }
        for (int i = 0; i < NUMBER_OF_HIGH_SCORES; i++)
        {
            highScores2Player[i] = new HighScore(DEFAULT_NAME, DEFAULT_SCORE);
        }

        // load data if there is any
        loadGameData();
    }

    // saves all game data that is meant to persist between game session.
    //      first this function packs all data into a list of strings
    //      then sends that list to the encryptPackage function for encryption (not implemented yet)
    //      then sends the encrypted list to the saveFile function to be turned into a file
    public void saveGameData()
    {
        List<string> dataPack = new List<string>();

        // save the last entered player names
        dataPack.Add(GetPlayer1Name());
        dataPack.Add(GetPlayer2Name());

        // save volume settings
        dataPack.Add(GetMusicVolume().ToString());
        dataPack.Add(GetSoundVolume().ToString());

        // save High Scores
        for (int i = 0; i < NUMBER_OF_HIGH_SCORES; i++)
        {
            dataPack.Add(highScores[i].GetName());
            dataPack.Add(highScores[i].GetScore().ToString());
        }
        dataPack.Add(END_OF_CATAGORY_LINE);

        for (int i = 0; i < NUMBER_OF_HIGH_SCORES; i++)
        {
            dataPack.Add(highScores2Player[i].GetName());
            dataPack.Add(highScores2Player[i].GetScore().ToString());
        }
        dataPack.Add(END_OF_CATAGORY_LINE);

        // save other data here...
        //     such as campaign progress and unlock-ables (future implementation)

        // condensed the helper functions to one line for encrypting and saving
        saveFile(encryptPackage(dataPack));
    }

    // loading function for retrieving game data from a file.
    public void loadGameData()
    {
        List<string> dataPack = new List<string>();
        dataPack = decryptPackage(LoadFile(SAVE_FILE_NAME));
        if (dataPack == null)
        {
            return;
        }

        int lineNumber = 0;

        // read the last entered player names
        SetPlayer1Name(dataPack[lineNumber]);
        lineNumber++;
        SetPlayer2Name(dataPack[lineNumber]);
        lineNumber++;

        // read volume settings
        SetMusicVolume(int.Parse(dataPack[lineNumber]));
        lineNumber++;
        SetSoundVolume(int.Parse(dataPack[lineNumber]));
        lineNumber++;

        // read 1 player high scores
        int counter = 0;
        while (END_OF_CATAGORY_LINE.CompareTo(dataPack[lineNumber]) != 0)
        {
            string name = "" + dataPack[lineNumber]; // change to to-string method later
            lineNumber++;
            int score = int.Parse(dataPack[lineNumber]);
            lineNumber++;
            highScores[counter] = new HighScore(name, score);
            counter++;
        }
        lineNumber++;

        // read 2 player high scores
        counter = 0;
        while (END_OF_CATAGORY_LINE.CompareTo(dataPack[lineNumber]) != 0)
        {
            string name = "" + dataPack[lineNumber]; // change to to-string method later
            lineNumber++;
            int score = int.Parse(dataPack[lineNumber]);
            lineNumber++;
            highScores2Player[counter] = new HighScore(name, score);
            counter++;
        }
    }

    // not implemented yet. currently this just returns a list identical to the one passed to it
    private List<string> decryptPackage(List<string> encryptPackage)
    {
        if (encryptPackage == null)
        {
            return null;
        }

        List<string> decryptedPack = new List<string>();

        foreach(string s in encryptPackage)
        {
            string tempString = "" + s; // change to to-string method later
            // decryption of tempString would happen here

            decryptedPack.Add(tempString);
        }

        return decryptedPack;
    }

    // not implemented yet. currently this just returns a list identical to the one passed to it
    private List<string> encryptPackage(List<string> dataPack)
    {
        List<string> encryptedPack = new List<string>();

        foreach (string s in dataPack)
        {
            string tempString = "" + s; // change to to-string method later

            // Encryption of tempString would happen here

            encryptedPack.Add(tempString);
        }

        return encryptedPack;
    }

    // saves a list of strings to a file
    public void saveFile(List<string> dataPack)
    {
        string[] lines = new string[dataPack.Count];

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = dataPack[i].ToString();
        }
        File.WriteAllLines(SAVE_FILE_NAME, lines);
    }

    // loads a file to a list of strings
    public List<string> LoadFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            List<string> linesList = new List<string>();
            string[] lines = File.ReadAllLines(fileName);
            foreach (string s in lines)
            {
                linesList.Add(s);
            }
            return linesList;
        }
        return null;
    }
}
