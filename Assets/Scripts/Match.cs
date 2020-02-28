using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[Serializable]
public class Match
{
    public string matchID;
    public int player1Score;
    public int player2Score;
    public bool isOpen;
    public string player1Name;
    public string player2Name;

    public Match(string matchID, bool isOpen)
    {
        this.matchID = matchID;
        this.isOpen = isOpen;
    }

    public Match(string matchID, int player1Score, int player2Score, bool isOpen)
    {
        this.matchID = matchID;
        this.player1Score = player1Score;
        this.player2Score = player2Score;
        this.isOpen = isOpen;
    }

    public Match(string matchID, bool isOpen, string player1Name)
    {
        this.matchID = matchID;
        this.isOpen = isOpen;
        this.player1Name = player1Name;
    }

    public Match(string matchID, string player1Name, string player2Name, int player1Score, int player2Score, bool isOpen)
    {
        this.matchID = matchID;
        this.player1Score = player1Score;
        this.player2Score = player2Score;
        this.isOpen = isOpen;
        this.player1Name = player1Name;
        this.player2Name = player2Name;
    }
}
