using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingForMatch : MonoBehaviour
{
    public Text matchDetails, lineDescription;
    private string matchID;
    public string hostName;
    public string guestName;
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        lineDescription.text = "Lobby Name: \nSpieler 1: \nSpieler 2: \nLevel: ";
        matchID = Matchmaking.matchID;
        if (Matchmaking.role == "host")
        {
            UpdateMatchLevel();
            GetMatchDetails();
            InvokeRepeating("PlayerJoined", 0.0f, 5f);
        } else
        {
            AddGuestInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddGuestInfo()
    {
        GetMatchDetails();
        Match match = new Match(matchID, hostName, Matchmaking.guestName, 0, 0, level, false);
        RestClient.Put("https://energyracer.firebaseio.com/lobby/" + matchID + ".json", match).Then(response =>
        {
            GetMatchDetails();
        });
    }

    void DisplayMatchInfo()
    {
        matchDetails.text = matchID + "\n" + hostName + "\n" + guestName + "\n" + level;
    }

    private void UpdateMatchLevel()
    {
        RestClient.Put("https://energyracer.firebaseio.com/lobby/" + matchID + "/level.json", DistrictSelection.curDistrict);
    }

    void GetMatchDetails()
    {
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            var result = JSON.Parse(response.Text);
            if (result == null)
            {
                matchDetails.text = "error";
            }
            else
            {
                hostName = result["hostName"];
                guestName = result["guestName"];
                level = result["level"];
                DisplayMatchInfo();
            }
        });
    }

    private void PlayerJoined()
    {
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            var result = JSON.Parse(response.Text);
            if (!result["isOpen"])
            {
                guestName = result["guestName"];
                Board.isOnlineMultiplayer = true;
                SceneManager.LoadScene("Game");
            }
        });
    }
}
