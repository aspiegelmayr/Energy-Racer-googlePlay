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
    public Text notFoundText, noInputText, noCoinsWarning;
    public InputField matchIDInput, nicknameInput;
    bool joined;
    bool lobbyIsOpen;
    bool isOpen;
    public Text entryFeeText, coinText;
    int entryFee;


    // Start is called before the first frame update
    void Start()
    {
        joined = false;
        matchID = matchIDInput.text;
        HideWarnings();
        noInputText.enabled = false;
        notFoundText.enabled = false;
        noCoinsWarning.enabled = false;
        entryFee = 10;
        entryFeeText.text = "Einsatz: " + entryFee + " coins";
        coinText.text = "$" + StartGame.coins;
    }

    void HideWarnings()
    {
        noInputText.enabled = false;
        notFoundText.enabled = false;
        noCoinsWarning.enabled = false;
    }

    bool HasEnoughCoins()
    {
        if(StartGame.coins >= entryFee)
        {
            return true;
        }
        noCoinsWarning.enabled = true;
        return false;
    }

    bool IsInputEmpty()
    {
        if (nicknameInput.text == "" || matchIDInput.text == "")
        {
            noInputText.enabled = true;
            return true;
        }
        return false;
    }

    void PayEntryFee()
    {
        StartGame.coins -= entryFee;
    }

    public void SearchForMatch()
    {
        HideWarnings();
        PayEntryFee();
        if (HasEnoughCoins())
        {
            hostName = "";
            SetLobbyStatus();
            role = "guest";
            isOpen = false;
            matchID = matchIDInput.text;
            guestName = nicknameInput.text;
            if (IsInputEmpty())
            {
                noInputText.enabled = true;
            }
            else if (!lobbyIsOpen)
            {
                matchDetails.text = "Du kannst der Lobby \"" + matchID + "\" nicht beitreten. \nBitte suche dir eine andere aus.";
            }
            else
            {
                LoadNextScene();
            }
        }
    }

    void SetLobbyStatus()
    {
        RestClient.Get("https://energyracer.firebaseio.com/lobby/" + matchID + ".json").Then(response =>
        {
            var result = JSON.Parse(response.Text);
            Debug.Log(result["isOpen"]);
            if (result["isOpen"] == true)
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
        HideWarnings();
        PayEntryFee();
        if (!IsInputEmpty())
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
    }

    private void PostMatch()
    {
        if (HasEnoughCoins())
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
                    LoadNextScene();
                });
            }
        }
    }

    void LoadNextScene()
    {
        Board.isOnlineMultiplayer = true;
        if (role == "host")
        {
            SceneManager.LoadScene("DistrictSelect");
        }
        else
        {
            SceneManager.LoadScene("WaitingForMatch");
        }
    }
}
