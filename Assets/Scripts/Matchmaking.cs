using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Matchmaking : MonoBehaviour
{
    public Text matchDetails;
    public static string matchID;
    public static string hostName;
    public static string guestName;
    public static string role;
    public Text notFoundText, noInputText;
    public InputField matchIDInput, nicknameInput;
    bool joined;
    bool lobbyIsOpen;
    bool isOpen;
    public static int level;


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
        hostName = "";
        role = "guest";
        isOpen = false;
        matchID = matchIDInput.text;
        guestName = nicknameInput.text;
        GetLobbyStatus();
        if (matchID == "" || guestName == "")
        {
            noInputText.enabled = true;
        } else if (!lobbyIsOpen)
        {
            matchDetails.text = "Die Lobby \"" + matchID + "\" ist nicht offen. \nBitte suche dir eine andere aus.";
        }
        else
        {
            GetMatchDetails(matchID);
        }
    }

    void GetLobbyStatus()
    {
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            var result = JSON.Parse(response.Text);
            if (result["isOpen"])
            {
                lobbyIsOpen = true;
            } else
            {
                lobbyIsOpen = false;
            }
        });
    }

    public void IsValidLobbyName()
    {
        hostName = nicknameInput.text;
        matchID = matchIDInput.text;
        matchDetails.text = "";
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            
            var result = JSON.Parse(response.Text);
            if (result == null)
            {
                PostMatch();
            }
            else
            {
                matchDetails.text = "Match mit dem Namen " + matchID + " existiert bereits. \nBitte gib einen anderen Namen ein.";
            }
        });

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
                    "Player 1: " + result["hostName"] + "\n" +
                    "Player 2: " + result["guestName"] + "\n" +
                    "Level: " + result["level"];
                    if (result["isOpen"])
                    {
                        matchDetails.text += "Warten auf Spieler...";
                    }
                    if(result["level"] != 0)
                    {
                        level = result["level"];
                    }
                    hostName = result["hostName"];
                    Board.isOnlineMultiplayer = true;
                    if (!result["isOpen"])
                    {
                        Board.isOnlineMultiplayer = true;
                        if(role == "host")
                        {
                            SceneManager.LoadScene("DistrictSelect");
                        } else
                        {
                            SceneManager.LoadScene("Game");
                        }
          
                    }
                });
            }
        });
    }

    private void PostMatch()
    {
            guestName = "";
            joined = false;
            role = "host";
        if (matchID == "" || hostName == "")
        {
            noInputText.enabled = true;
        }
        else
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
                Board.isOnlineMultiplayer = true;
                SceneManager.LoadScene("DistrictSelect");
            }
        });
    }
}
