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
    public static string hostName;
    public static string guestName;
    public static string role;
    public Text notFoundText, noInputText;
    public InputField matchIDInput, nicknameInput;
    bool joined;
    bool isOpen;


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
        role = "guest";
        isOpen = false;
        matchID = matchIDInput.text;
        guestName = nicknameInput.text;
        if (matchID == "" || guestName == "")
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
                Match match = new Match(result["matchID"], result["hostName"], guestName, 0, 0, isOpen);
                RestClient.Put("https://energyracer.firebaseio.com/lobby/" + id + ".json", match).Then(reply =>
                {
                    result = JSON.Parse(reply.Text);
                    matchDetails.text = "Name: " + result["matchID"] + "\n" +
                    "Player 1: " + result["hostName"] + ": " + result["hostScore"] + "\n" +
                    "Player 2: " + result["guestName"] + ": " + result["guestScore"] + "\n" +
                    "isOpen: " + result["isOpen"];
                    hostName = result["hostName"];
                    Board.isOnlineMultiplayer = true;
                    if (!result["isOpen"])
                    {
                        SceneManager.LoadScene("Game");
                    }
                });
            }
        });
    }

    public void PostMatch()
    {
        joined = false;
        hostName = nicknameInput.text;
        activity.text = "hosting match";
        role = "host";
        matchID = matchIDInput.text;
        if (matchID == "" || hostName == "")
        {
            noInputText.enabled = true;
        } else
        {
            isOpen = true;
            Match match = new Match(matchID, true, hostName);
            RestClient.Put("https://energyracer.firebaseio.com/lobby/" + matchID + ".json", match).Then(response =>
            {
                GetMatchDetails(matchID);
                InvokeRepeating("PlayerJoined", 0.0f, 1f);
            });
            
        }
    }

    private void PlayerJoined()
    {
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            var result = JSON.Parse(response.Text);
            if (result["isOpen"] == false)
            {
                guestName = result["guestName"];
                SceneManager.LoadScene("Game");
            }
        });
    }
}
