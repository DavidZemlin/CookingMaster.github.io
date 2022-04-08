using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore
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
