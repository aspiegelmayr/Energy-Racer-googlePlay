using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// manages start scene
/// </summary>
public class StartGame : MonoBehaviour
{
    public static int coins;
    public Text coinText;

    public static Car[] cars;
    public Sprite[] carImgs;

    public static Upgrade[] upgrades;
    public Sprite[] upgradeImgs;

    public static Car activeCar;
    public static Upgrade activeUpgrade;

    /// <summary>
    /// get the amount of coins from local storage
    /// </summary>
    void Start()
    {
        cars = new Car[3];
        upgrades = new Upgrade[3];
        coins = new int();
        createItems();
        GetFromPlayerPrefs();
        

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
        {
            int difficulty = LocationService.GetLevelDifficulty();
            SceneBackgroundInformation.SetBackground(difficulty);
        }
    }

    void GetFromPlayerPrefs()
    {
        foreach (var car in cars)
        {
            if (PlayerPrefs.GetString(car.carName) == "owned")
            {
                car.owned = true;
            }
        }

        foreach (var upgrade in upgrades)
        {
            if (PlayerPrefs.GetString(upgrade.upgradeName) == "owned")
            {
                upgrade.owned = true;
            }
        }

        coins = PlayerPrefs.GetInt("coins");

        activeCar = cars[PlayerPrefs.GetInt("activeCar")];
        activeUpgrade = upgrades[PlayerPrefs.GetInt("activeUpgrade")];

    }

    /// <summary>
    /// create all upgrades
    /// </summary>
    void createItems()
    {
        cars[0] = new Car("Standard Car", "standard car", 0, 0, true, carImgs[0]);
        cars[1] = new Car("Sports Car", "-2 needed moves", 200, 2, false, carImgs[1]);
        cars[2] = new Car("Super Car", "-3 needed moves", 400, 3, false, carImgs[2]);

        upgrades[0] = new Upgrade("Extra Move", "1 Move mehr", 75, 1, upgradeImgs[0], false);
        upgrades[1] = new Upgrade("Extra Move XL", "2 Moves mehr", 150, 2, upgradeImgs[1], false);
        upgrades[2] = new Upgrade("Extra Move XXL", "3 Moves mehr", 300, 3, upgradeImgs[2], false);
    }


    // Update is called once per frame
    void Update()
    {
    }
}
