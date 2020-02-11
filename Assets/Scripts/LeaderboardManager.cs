using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;

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

    string hash;
    UserData data;

    // Start is called before the first frame update
    void Start()
    {
        warningText.enabled = false;
        score = Board.startingMoves - Board.remainingMoves;
        scoreText.text = score + " Züge";
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
        RestClient.Post("https://energyracer.firebaseio.com/scorelist.json", user);
    }

    private void ShowScore()
    {
        RestClient.Get<UserData>("https://energyracer.firebaseio.com/scorelist/" + hash + ".json").Then(response =>
        {
            data = response;
        });
        if (data != null)
        {
            leaderboardNames.text = data.username;
            leaderboardScores.text = data.score + " Zuge";
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
        RestClient.Get<string>("https://energyracer.firebaseio.com/scorelist.json?orderBy=\"score\"&limitToLast=1").Then(response =>
        {
            hash = response;
        });
        ShowScore();
    }
}
