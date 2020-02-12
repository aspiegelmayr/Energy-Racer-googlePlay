using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using UnityEditor;
using SimpleJSON;

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

    string curLevel;

    public string hash;
    UserData data;

    // Start is called before the first frame update
    void Start()
    {
        curLevel = LevelSelection.districtName;
        if(curLevel == null)
        {
            curLevel = "testLevel";
        }

        warningText.enabled = false;
        score = Board.startingMoves - Board.remainingMoves;
        scoreText.text = score + " Züge";
        GetData();

    }

    private void Update()
    {
        GetData();
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
            nickname = nameInput.text;
            SubmitScore();
        }
    }

    private void SubmitScore()
    {
        UserData user = new UserData(nickname, score);
        RestClient.Post("https://energyracer.firebaseio.com/scorelist/" + curLevel + ".json", user).Then(response =>
        {
            sendBtn.enabled = false;
        });
    }

    private void ShowScore()
    {
        RestClient.Get<UserData>("https://energyracer.firebaseio.com/scorelist/" + curLevel + "/" + hash + ".json").Then(response =>
        {
            data = response;
        });
        if (data != null)
        {
            //leaderboardNames.text = data.username;
            //leaderboardScores.text = data.score + " Zuge";
        }
        else
        {
            leaderboardNames.text = "empty";
            leaderboardScores.text = "error";
        }
    }

    public void GetScores()
    {
        GetData();
    }

    private void GetData()
    {
        RestClient.Get("https://energyracer.firebaseio.com/scorelist/" + curLevel + ".json?orderBy=\"score\"&limitToLast=2").Then(response =>
        {
            string leaders = "leaders: \n";
            string scores = "scores: \n";
            hash = response.Text;
            var result = JSON.Parse(hash);
            for (int i = 0; i < 2; i++)
            {
                leaders += result[i]["username"] + "\n";
                scores += result[i]["score"] + "\n";
            }
            leaderboardNames.text = leaders;
            leaderboardScores.text = scores;
            Debug.Log(result);
        });
        
        ShowScore();
    }
}
