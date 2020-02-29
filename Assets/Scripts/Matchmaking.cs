using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Matchmaking : MonoBehaviour
{
    public Text activity, matchDetails;
    public static string matchID;
    public static string nickname;
    public static string opponentNickname;
    public Text notFoundText, noInputText;
    public InputField matchIDInput, nicknameInput;
    bool joined;


    // Start is called before the first frame update
    void Start()
    {
        joined = false;
        matchID = matchIDInput.text;

        noInputText.enabled = false;
        notFoundText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchForMatch()
    {
        activity.text = "joining match";
        matchID = matchIDInput.text;
        nickname = nicknameInput.text;
        if (matchID == "" || nickname == "")
        {
            noInputText.enabled = true;
        }
        else
        {
            GetMatchDetails(matchID);
        }
    }

    void GetMatchDetails(string id)
    {
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + id + ".json").Then(response =>
        {
            if (response == null)
            {
                notFoundText.enabled = true;
            }
            else
            {
                var result = JSON.Parse(response.Text);
                Match match = new Match(result["matchID"], result["player1Name"], nickname, 0, 0, false);
                RestClient.Put("https://energyracer.firebaseio.com/lobby/" + id + ".json", match).Then(reply =>
                {
                    result = JSON.Parse(reply.Text);
                    matchDetails.text = "Name: " + result["matchID"] + "\n" +
                    "Player 1: " + result["player1Name"] + ": " + result["player1Score"] + "\n" +
                    "Player 2: " + result["player2Name"] + ": " + result["player2Score"] + "\n" +
                    "isOpen: " + result["isOpen"];
                    opponentNickname = result["player1Name"];
                    Board.isOnlineMultiplayer = true;
                    SceneManager.LoadScene("Game");
                });
            }
        });
    }

    public void PostMatch()
    {
        joined = false;
        nickname = nicknameInput.text;
        activity.text = "hosting match";
        matchID = matchIDInput.text;
        if (matchID == "" || nickname == "")
        {
            noInputText.enabled = true;
        } else
        {
            Match match = new Match(matchID, true, nickname);
            RestClient.Put("https://energyracer.firebaseio.com/lobby/" + matchID + ".json", match).Then(response =>
            {
                GetMatchDetails(matchID);
            });

            while (!joined)
            {
                InvokeRepeating("PlayerJoined", 0.0f, 5f);
            }
        }
    }

    private void PlayerJoined()
    {
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            var result = JSON.Parse(response.Text);
            if (result["isOpen"] == false)
            {
                joined = true;
                opponentNickname = result["player2Name"];
                SceneManager.LoadScene("GameScene");
            }
        });
    }
}
