using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneSwitch : MonoBehaviour
{
  //  public static bool backgroundIsSet;

    public void Start()
    {
       // backgroundIsSet = false;
    }

    public void GoToMultiplayerMenu()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void GotoGameScene()
    {
        if (SceneManager.GetActiveScene().name == "MultiplayerMenu")
        {
            Board.isMultiplayer = true;
        }
        else
        {
            Board.isMultiplayer = false;
        }

        SceneManager.LoadScene("Game");
    }

    public void Update()
    {
    }

    public void GotoGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void GoToWinScene()
    {
        SceneManager.LoadScene("GameWon");
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GotoLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void GoToDistrictSelectScene()
    {
        if (SceneManager.GetActiveScene().name == "GameWon")
        {
            if (DistrictSelection.curDistrict >= DistrictSelection.unlockedDistricts)
                DistrictSelection.unlockedDistricts++;
            StartGame.coins += Board.earnedCoins;
            PlayerPrefs.SetInt("coins", StartGame.coins);
        }

        SceneManager.LoadScene("DistrictSelect");
    }

    public void GoToStartScene()
    {
        if (SceneManager.GetActiveScene().name == "GameWon")
        {
            if (DistrictSelection.curDistrict >= DistrictSelection.unlockedDistricts)
                DistrictSelection.unlockedDistricts++;
            StartGame.coins += Board.earnedCoins;
            PlayerPrefs.SetInt("coins", StartGame.coins);
        }

        SceneManager.LoadScene("StartScene");
    }

    public void GoToShopScene()
    {
        SceneManager.LoadScene("Shop");
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void GoToMultiplayerLobby()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }
}
