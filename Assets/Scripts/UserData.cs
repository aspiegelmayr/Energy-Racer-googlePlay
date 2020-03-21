using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[Serializable]
public class UserData
{
    public string username;
    public int score;

    public UserData(string username, int score)
    {
        this.username = username;
        this.score = score;
    }

    public UserData(string username)
    {
        this.username = username;
    }
}
