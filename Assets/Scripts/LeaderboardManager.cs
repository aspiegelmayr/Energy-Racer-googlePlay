using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using UnityEditor;
using SimpleJSON;
using System;
using System.Linq;

//https://energyracer.firebaseio.com/scorelist.json?orderBy="score"&limitToLast=2

public class LeaderboardManager : MonoBehaviour
{
    public static int score;
    public static string nickname;
    public Text scoreText;
    public InputField nameInput;
    public Text leaderboardNames;
    public Text leaderboardScores;
    public Text warningText;
    public Button sendBtn;
    public Text districtText;

    string curLevel;

    UserData data;

    // Start is called before the first frame update
    void Start()
    {
        leaderboardNames.text = "";
        leaderboardScores.text = "";
    curLevel = LevelSelection.districtName;
        if(curLevel == null)
        {
            curLevel = "testLevel";
        }

        
        warningText.enabled = false;
        score = Board.startingMoves - Board.remainingMoves;
        scoreText.text = score + " Moves";
        //districtText.text = curLevel;
        GetData();
    }

    private void Update()
    {
        
    }

    public void SendToDatabase()
    {
        if (nameInput.text == "")
        {
            warningText.enabled = true;
            warningText.text = "Bitte Namen eingeben!";
        }
        else
        {
            warningText.enabled = false;
            nickname = nameInput.text;
            SubmitScore();
        }
    }

    private void SubmitScore()
    {
        UserData data = new UserData(nickname, score);
        RestClient.Post("https://energyracer.firebaseio.com/scorelist/" + curLevel + ".json", data).Then(response =>
        {
            sendBtn.enabled = false;
        });
        GetData();
    }

    public void GetScores()
    {
        GetData();
    }

    private void GetData()
    {
        leaderboardNames.text = "";
        leaderboardScores.text = "";
        List<UserData> scorelist = new List<UserData>();
        RestClient.Get("https://energyracer.firebaseio.com/scorelist/" + curLevel + ".json?orderBy=\"score\"&limitToLast=10").Then(response =>
        {
            var result = JSON.Parse(response.Text);
            for (int i = 0; i < 10; i++)
            {
                scorelist.Add(new UserData(result[i]["username"], result[i]["score"]));
            }
            scorelist = scorelist.OrderByDescending(x => x.score).ToList();
            foreach (var entry in scorelist)
            {
                leaderboardNames.text += entry.username + "\n";
                leaderboardScores.text += entry.score + "\n";
            }
        });
    }
}
