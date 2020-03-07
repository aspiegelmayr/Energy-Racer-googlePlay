using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[Serializable]
public class Match
{
    public string matchID;
    public int hostScore;
    public int guestScore;
    public bool isOpen;
    public string hostName;
    public string guestName;

    public Match(string matchID, bool isOpen)
    {
        this.matchID = matchID;
        this.isOpen = isOpen;
    }

    public Match(string matchID, int hostScore, int guestScore, bool isOpen)
    {
        this.matchID = matchID;
        this.hostScore = hostScore;
        this.guestScore = guestScore;
        this.isOpen = isOpen;
    }

    public Match(string matchID, bool isOpen, string hostName)
    {
        this.matchID = matchID;
        this.isOpen = isOpen;
        this.hostName = hostName;
    }

    public Match(string matchID, string hostName, string guestName, int hostScore, int guestScore, bool isOpen)
    {
        this.matchID = matchID;
        this.hostScore = hostScore;
        this.guestScore = guestScore;
        this.isOpen = isOpen;
        this.hostName = hostName;
        this.guestName = guestName;
    }
}
