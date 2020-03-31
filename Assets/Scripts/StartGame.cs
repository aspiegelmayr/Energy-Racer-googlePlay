using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// manages start scene
/// </summary>
public class StartGame : MonoBehaviour
{
    public static int coins;

    public static Car[] cars;

    public Sprite[] itemSprites;

    public static Sprite[] carSprites;

    public static Upgrade[] upgrades;
    public static Sprite[] upgradeSprites;

    public static Car activeCar;
    public static Upgrade activeUpgrade;

    public GameObject multiplayerPanel;

    public static string curLanguage;

    /// <summary>
    /// get the amount of coins from local storage
    /// </summary>
    void Start()
    {
        curLanguage = Lean.Localization.LeanLocalization.CurrentLanguage;
        Board.isMultiplayer = false;
        Board.isOnlineMultiplayer = false;
        multiplayerPanel.SetActive(false);
        cars = new Car[3];
        upgrades = new Upgrade[3];
        coins = new int();
        SetSprites();
        if (cars[0] == null || upgrades[0] == null)
        {
            createItems();
        }
        GetFromPlayerPrefs();
        

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
        {
            int difficulty = LocationService.GetLevelDifficulty();
            SceneBackgroundInformation.SetBackground(difficulty);
        }
    }

    void SetSprites()
    {
        carSprites = new Sprite[itemSprites.Length / 2];
        upgradeSprites = new Sprite[itemSprites.Length/2];
        for (int i = 0; i < itemSprites.Length/2; i++)
        {
            carSprites[i] = itemSprites[i];
        }

        for(int i = itemSprites.Length/2; i < itemSprites.Length; i++)
        {
            upgradeSprites[i-itemSprites.Length/2] = itemSprites[i];
        }
    }

    void GetFromPlayerPrefs()
    {
        var activeCarName = PlayerPrefs.GetString("activeCar");
        var activeUpgradeName = PlayerPrefs.GetString("activeUpgrade");

        foreach (var car in cars)
        {
            string own = PlayerPrefs.GetString(car.carName);
            Debug.Log(car.carName + own);
           
            if (PlayerPrefs.GetString(car.carName) == "owned")
            {
                car.owned = true;
                if(activeCarName == car.carName)
                {
                    activeCar = car;
                }
            }
        }

        foreach (var upgrade in upgrades)
        {
            if (PlayerPrefs.GetString(upgrade.upgradeName) == "owned")
            {
                upgrade.owned = true;
                if (activeUpgradeName == upgrade.upgradeName)
                {
                    activeUpgrade = upgrade;
                }
            }
        }

        coins = PlayerPrefs.GetInt("coins");
        if(activeCar == null)
        {
            activeCar = cars[0];
        }
    }

    /// <summary>
    /// create all upgrades
    /// </summary>
    void createItems()
    {
        cars[0] = new Car("Standard Car", 0, 0, true, carSprites[0]);
        cars[1] = new Car("Sports Car", 200, 2, false, carSprites[1]);
        cars[2] = new Car("Super Car", 400, 3, false, carSprites[2]);

        upgrades[0] = new Upgrade("Extra Move", 75, 1, upgradeSprites[0], false);
        upgrades[1] = new Upgrade("Extra Move XL", 150, 2, upgradeSprites[1], false);
        upgrades[2] = new Upgrade("Extra Move XXL", 300, 3, upgradeSprites[2], false);
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void HidePanel()
    {
        multiplayerPanel.SetActive(false);
    }

    public void ShowPanel()
    {
        multiplayerPanel.SetActive(true);
    }
}
