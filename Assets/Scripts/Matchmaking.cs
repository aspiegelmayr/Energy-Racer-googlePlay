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
    public Button hostBtn, joinBtn;


    // Start is called before the first frame update
    void Start()
    {
        if (!Board.isOnlineMultiplayer)
        {
            joined = false;
            matchID = matchIDInput.text;

            noInputText.enabled = false;
            notFoundText.enabled = false;
        }
        else
        {
            Debug.Log("hu");
            Debug.Log("hostname: " + hostName);
            level = LevelSelection.districtNum;
            PostMatch();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchForMatch()
    {
        ActivateButtons(false);
        hostName = "";
        role = "guest";
        //isOpen = false;
        matchID = matchIDInput.text;
        guestName = nicknameInput.text;
        if (matchID == "" || guestName == "")
        {
            noInputText.enabled = true;
        } else
        {
            RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
            {
                var result = JSON.Parse(response.Text);
                if (result["isOpen"])
                {
                    isOpen = true;
                    Debug.Log("open");
                    GetMatchDetails(matchID);
                } else
                {
                    isOpen = false;
                    matchDetails.text = "Die Lobby " + matchID + " ist nicht offen. Bitte suche dir eine andere aus.";
                }
            });
        }
    }

    bool IsLobbyOpen()
    {
        bool open = false;
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            var result = JSON.Parse(response.Text);
            if (result["isOpen"])
            {
                open = true;
                Debug.Log("opem");
            }
        });
        return open;
    }

    void ActivateButtons(bool active)
    {
        if (active)
        {
            joinBtn.interactable = true;
            hostBtn.interactable = true;
        } else
        {
            joinBtn.interactable = false;
            hostBtn.interactable = false;
        }
    }

    public void IsValidLobbyName()
    {
        ActivateButtons(false);
        hostName = nicknameInput.text;
        matchID = matchIDInput.text;
        matchDetails.text = "";
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            
            var result = JSON.Parse(response.Text);
            if (result == null)
            {
                role = "host";
                Board.isOnlineMultiplayer = true;
                SceneManager.LoadScene("DistrictSelect");
            }
            else
            {
                matchDetails.text = "Match mit dem Namen " + matchID + " existiert bereits. \nBitte gib einen anderen Namen ein.";
                ActivateButtons(true);
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
                Match match = new Match(result["matchID"], result["hostName"], guestName, 0, 0, level, isOpen);
                RestClient.Put("https://energyracer.firebaseio.com/lobby/" + id + ".json", match).Then(reply =>
                {
                    result = JSON.Parse(reply.Text);
                    matchDetails.text = "Name: " + result["matchID"] + "\n" +
                    "Player 1: " + result["hostName"] + "\n" +
                    "Player 2: " + result["guestName"] + "\n";
                    //+ "Level: " + result["level"];
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
                            SceneManager.LoadScene("Game");
          
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
                SceneManager.LoadScene("Game");
            }
        });
    }
}
