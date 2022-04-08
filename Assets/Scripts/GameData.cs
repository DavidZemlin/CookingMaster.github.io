//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.

using System.Collections.Generic;
using System.IO;
using UnityEngine;

// this script holds data that needs to be kept between scenes or game sessions
public class GameData : MonoBehaviour
{
    private const int NUMBER_OF_HIGH_SCORES = 10;
    private const string DEFAULT_NAME = "empty";

    private const string SAVE_FILE_NAME = "save.dat";
    public const string END_OF_CATAGORY_LINE = "--c--";

    [SerializeField] private HighScore[] highScores = new HighScore[NUMBER_OF_HIGH_SCORES];

    private void Awake()
    {
        // mark game data object as "don't destroy"
        DontDestroyOnLoad(gameObject);

        // initialize high score list;
        for (int i = 0; i < NUMBER_OF_HIGH_SCORES; i++)
        {
            highScores[i] = new HighScore(DEFAULT_NAME, 0);
        }
    }

    void OnApplicationQuit()
    {
        saveGameData();
    }

    // saves all game data that is meant to persist between game session.
    //      first this function packs all data into a list of strings
    //      then sends that list to the encryptPackage function for encryption (not implemented yet)
    //      then sends the encrypted list to the saveFile function to be turned into a file
    public void saveGameData()
    {
        List<string> dataPack = new List<string>();

        // save High Scores
        foreach (HighScore s in highScores)
        {
            dataPack.Add(s.getName());
            dataPack.Add(s.getScore().ToString());
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

        int lineNumber = 0;

        int counter = 0;
        while (END_OF_CATAGORY_LINE.CompareTo(dataPack[lineNumber]) != 0)
        {
            string name = "" + dataPack[lineNumber]; // used concatenation instead of copying, because it is easier on the eyes
            lineNumber++;
            int score = int.Parse(dataPack[lineNumber]);
            lineNumber++;
            highScores[0] = new HighScore(name, score);
            lineNumber++;
        }
    }

    // not implemented yet. currently this just returns a list identical to the one passed to it
    private List<string> decryptPackage(List<string> encryptPackage)
    {
        List<string> decryptedPack = new List<string>();

        foreach(string s in encryptPackage)
        {
            string tempString = "" + s; // used concatenation instead of copying, because it is easier on the eyes
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
            string tempString = "" + s; // used concatenation instead of copying, because it is easier on the eyes

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
        List<string> linesList = new List<string>();
        string[] lines = File.ReadAllLines(fileName);
        foreach (string s in lines)
        {
            linesList.Add(s);
        }
        return linesList;
    }

    // nested class for tracking high scores
    private class HighScore
    {
        private string name;
        private int score;

        public HighScore(string newName, int newScore)
        {
            name = newName;
            score = newScore;
        }

        public string getName() { return name; } // I like to make simple getter and setter functions use only one line
        public int getScore() { return score; }
    }
}
