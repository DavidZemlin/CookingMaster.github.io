//This document and all its contents are copyrighted by David Zemlin and my not be used or reproduced without express written consent.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// simple class for storing high-score data
public class HighScore
{
    // ---data members---
    private string name;
    private int score;

    // ---getters---
    public string GetName() { return name; }
    public int GetScore() { return score; }

    public void SetScore(int newScore) { score = newScore; }
    public void SetName(string newName) { name = newName; }

    // ---constructors---
    public HighScore(string newName, int newScore)
    {
        name = newName;
        score = newScore;
    }
}
