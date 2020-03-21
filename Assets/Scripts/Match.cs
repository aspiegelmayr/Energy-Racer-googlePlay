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
    public int level;

    public Match()
    {
        this.matchID = "error";
    }

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

    public Match(string matchID, string hostName, string guestName, int hostScore, int guestScore, int level, bool isOpen)
    {
        this.matchID = matchID;
        this.hostScore = hostScore;
        this.guestScore = guestScore;
        this.isOpen = isOpen;
        this.hostName = hostName;
        this.guestName = guestName;
        this.level = level;
    }

    public Match(string matchID, string hostName, string guestName, int hostScore, int guestScore, int level, bool isOpen, string hostReplay, string guestReplay)
    {
        this.matchID = matchID;
        this.hostScore = hostScore;
        this.guestScore = guestScore;
        this.isOpen = isOpen;
        this.hostName = hostName;
        this.guestName = guestName;
        this.level = level;
    }
}
