using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerWin : MonoBehaviour
{
    public GameObject confettiObj;
    public Sprite sprite;
    private string matchID;

    // Start is called before the first frame update
    void Start()
    {
        StartGame.coins += 20;
        matchID = "test";
        // matchID = Matchmaking.matchID;
        GameObject obj = Instantiate(confettiObj);
        Destroy(obj, 6.0f);
    }

    public void Replay()
    {
        if(StartGame.coins < 10)
        {
            BackToMenu();
        }
        StartGame.coins -= 10;
        Match match = new Match();
        if (Matchmaking.role == "host") {
            match = new Match(matchID, Matchmaking.hostName, Matchmaking.guestName, 0, 0, Matchmaking.level, false, "yes", "");
        } else
        {
            match = new Match(matchID, Matchmaking.hostName, Matchmaking.guestName, 0, 0, Matchmaking.level, false, "", "yes");
        }
        RestClient.Put("https://energyracer.firebaseio.com/lobby/" + matchID + ".json", match).Then(response => {
            Debug.Log("ok");
            SceneManager.LoadScene("Game");
        });
    }

    public void BackToMenu()
    {
        RestClient.Delete("https://energyracer.firebaseio.com/lobby/" + matchID + ".json");
        SceneManager.LoadScene("StartScene");
    }
}
