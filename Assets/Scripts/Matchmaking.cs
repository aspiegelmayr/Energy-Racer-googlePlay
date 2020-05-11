using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Matchmaking : MonoBehaviour
{
    public Text matchTitle;
    public Text matchDetails;
    public static string matchID;
    public static string hostName;
    public static string guestName;
    public static string role;
    public Text notFoundText, noInputText, nameTakenWarning, closedLobbyWarning;
    public InputField matchIDInput, nicknameInput;
    bool isOpen;
    public static int level;
    public Button hostBtn, joinBtn;
    public GameObject warningPanel;
    public Button yesBtn, noBtn;
 


    // Start is called before the first frame update
    void Start()
    {
        warningPanel.SetActive(false);
        if(role == "host")
        {
            ActivateButtons(false);
        }

        HideWarnings();

        if (!Board.isOnlineMultiplayer)
        {
            matchID = matchIDInput.text;
        }
        else
        {
            level = LevelSelection.districtNum;
            PostMatch();
        }
    }

    void HideWarnings()
    {
        noInputText.enabled = false;
        notFoundText.enabled = false;
        nameTakenWarning.enabled = false;
        closedLobbyWarning.enabled = false;
    }

    public void SearchForMatch()
    {
        HideWarnings();
        ActivateButtons(false);
        hostName = "";
        role = "guest";
        isOpen = false;
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
                    GetMatchDetails(matchID);
                } else
                {
                    isOpen = false;
                    closedLobbyWarning.enabled = true;
                }
            });
        }
    }

    public void BackButtonPressed()
    {
        warningPanel.SetActive(true);
    }

    public void ReturnToMenu(bool ret)
    {
        if (ret)
        {
            RestClient.Delete("https://energyracer.firebaseio.com/lobby/" + Matchmaking.matchID + ".json").Then(response => {
                role = "";
                SceneManager.LoadScene("StartScene");
            });
        } else
        {
            warningPanel.SetActive(false);
        }
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
        HideWarnings();
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
                //matchDetails.text = "Match mit dem Namen " + matchID + " existiert bereits. \nBitte gib einen anderen Namen ein.";
                nameTakenWarning.enabled = true;
                ActivateButtons(true);
            }
        });

    }

    void GetMatchDetails(string id)
    {
        HideWarnings();
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + id + ".json").Then(response =>
        {
            if (response == null)
            {
                notFoundText.enabled = true;
            }
            else
            {
                var result = JSON.Parse(response.Text);
                Match match = new Match();
                if (role == "host")
                {
                    match = new Match(result["matchID"], result["hostName"], guestName, 0, 0, level, isOpen);
                } else
                {
                    match = new Match(result["matchID"], result["hostName"], guestName, 0, 0, result["level"]+1, false);
                }
                RestClient.Put("https://energyracer.firebaseio.com/lobby/" + id + ".json", match).Then(reply =>
                {
                    matchTitle.enabled = true;
                    matchDetails.enabled =true;
                    result = JSON.Parse(reply.Text);
                    matchDetails.text = "Name: " + result["matchID"] + "\n" +
                    "Player 1: " + result["hostName"] + "\n" +
                    "Player 2: " + result["guestName"] + "\n"
                    + "Level: " + result["level"] + "\n";
                    level = result["level"];
                    if (result["isOpen"])
                    {
                        matchDetails.text += "Waiting...";
                    }
                    hostName = result["hostName"];
                    Board.curDistr = level;
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
