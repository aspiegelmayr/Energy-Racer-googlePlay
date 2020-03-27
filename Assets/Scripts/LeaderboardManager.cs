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
    public Button submitBtn;

    string curLevel;

    public float period = 10f;

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
        districtText.text = curLevel;
        InvokeRepeating("GetData", 0.0f, 10f);
    }

    public void SendToDatabase()
    {
        submitBtn.interactable = false;
        if (nameInput.text == "")
        {
            warningText.enabled = true;
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

    private void GetData()
    {

        leaderboardNames.text = "Loading...";
        leaderboardScores.text = "";
        List<UserData> scorelist = new List<UserData>();
        RestClient.Get("https://energyracer.firebaseio.com/scorelist/" + curLevel + ".json?orderBy=\"score\"&limitToLast=10").Then(response =>
        {
            leaderboardNames.text = "";
            leaderboardScores.text = "";
            var result = JSON.Parse(response.Text);

            for (int i = 0; i < 10; i++)
            {
                if(result[i] != null) {
                    scorelist.Add(new UserData(result[i]["username"], result[i]["score"]));
                } else
                {
                    scorelist.Add(new UserData("no entry"));
                }
            }

            scorelist = scorelist.OrderByDescending(x => x.score).ToList();
            foreach (var entry in scorelist)
            {
                if (entry.username == "no entry")
                {
                    leaderboardNames.text += " \n";
                    leaderboardScores.text += " \n";
                }
                else
                {
                    leaderboardNames.text += entry.username + "\n";
                    leaderboardScores.text += entry.score + "\n";
                }
            }
        });
    }
}
