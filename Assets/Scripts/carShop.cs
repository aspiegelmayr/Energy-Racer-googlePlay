using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// manages the car shop scene
/// </summary>
public class carShop : MonoBehaviour
{
    public Text coinText;
    public Text buyText;
    public Button yesButton;
    public Button noButton;
    public Button[] carButtons;
    public Button[] upgradeButtons;
    public Button upgrade2Button;
    public Button upgrade3Button;
    public Text[] carTitles;
    public Text[] carCosts;
    public Text[] upgradeTitles;
    public Text[] upgradeCosts;
    
    public static Car activeCar;
    public Upgrade activeUpgrade;
    public string buttonTag;

    public Image[] carImages;
    public Image[] upgradeImages;

    /// <summary>
    /// display buttons
    /// </summary>
    void Start()
    {
        hideConfirmDialog();
        coinText.text = "$" + StartGame.coins;

        fillTextfields();
        for(int i = 0; i < carButtons.Length; i++)
        {
            Button button = carButtons[i].GetComponent<Button>();
            button.onClick.AddListener(selectItem);
        }

        for(int i = 0; i < upgradeButtons.Length; i++)
        {
            Button button = upgradeButtons[i].GetComponent<Button>();
            button.onClick.AddListener(selectItem);

        }
    }


    
    /// <summary>
    /// display info about upgrades
    /// </summary>
    void fillTextfields()
    {
        for (int i = 0; i < carTitles.Length; i++)
        {
            fillOutInfo(carTitles[i], carCosts[i], carImages[i], StartGame.cars[i]);
        }

        for (int i = 0; i < upgradeTitles.Length; i++)
        {
            fillOutInfo(upgradeTitles[i], upgradeCosts[i], upgradeImages[i], StartGame.upgrades[i]);
        }
    }
    /// <summary>
    /// method for filling the text fields for cars
    /// </summary>
    /// <param name="nameText"></param> text field for car name
    /// <param name="costText"></param> text field for car cost
    /// <param name="car"></param> chosen car object
    void fillOutInfo(Text nameText, Text costText, Image carImage, Car car)
    {
        nameText.text = car.carName;
        carImage.sprite = car.img;
        
        if (activeCar == car)
        {
            costText.text = "aktiv";
        }
        else if (car.owned)
        {
            costText.text = "in Besitz";
        }
        else
        {
            costText.text = "$" + car.cost;
        }
    }

    /// <summary>
    /// method for filling the text fields for upgrades
    /// </summary>
    /// <param name="nameText"></param> text field for upgrade name
    /// <param name="costText"></param> text field for upgrade cost
    /// <param name="car"></param> chosen car object
    void fillOutInfo(Text nameText, Text costText, Image upgradeImage, Upgrade upgrade)
    {
        nameText.text = upgrade.upgradeName;
        upgradeImage.sprite = upgrade.upgradeImg;
        if (activeUpgrade == upgrade)
        {
            costText.text = "aktiv";
        }
        else if (upgrade.owned)
        {
            costText.text = "in Besitz";
        }
        else
        {
            costText.text = "$" + upgrade.cost;
        }
    }

    /// <summary>
    /// checks if clicked item is owned, if not lets the user buy the item.
    /// if owned lets the user equip the item
    /// </summary>
    void selectItem()
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);

        var selected = EventSystem.current.currentSelectedGameObject;
        buttonTag = selected.tag;
        var index = Convert.ToInt32(buttonTag) - 1;


        if (StartGame.cars[index].owned)
        {
            buyText.text = StartGame.cars[index].carName + " verwenden?";
        }
        else
        {
            buyText.text = StartGame.cars[index].cost + " Muenzen fuer " + StartGame.cars[index].carName + " eintauschen?";
        }

        buyText.color = Color.black;
        buyText.gameObject.SetActive(true);

        Button yes = yesButton.GetComponent<Button>();
        if (index == 0)
        {
            buyText.color = Color.blue;
            buyText.text = "Auto bereits in Verwendung!";
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
        }
        else
        {
            yes.onClick.AddListener(buyItem);
        }

        Button no = noButton.GetComponent<Button>();
        no.onClick.AddListener(hideConfirmDialog);
    }

    /// <summary>
    /// hides the dialog that asks if the user wants to buy an item
    /// </summary>
    void hideConfirmDialog()
    {
        buyText.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    void buyCar(int i)
    {
        if (validTransaction(StartGame.cars[i].cost) || StartGame.cars[i].owned)
        {
            if (!StartGame.cars[i].owned)
            {
                updateCoins(StartGame.cars[i].cost);

            }
            addNewCar(StartGame.cars[i]);
            activeCar = StartGame.cars[i];
            PlayerPrefs.SetInt("activeCar", i);
            PlayerPrefs.Save();
            fillTextfields();
        }
        else
        {
            warnAboutMoney();
        }
    }

    void buyUpgrade(int i, Text upgradeTitle, Text upgradeCost)
    {
        if (validTransaction(StartGame.upgrades[i].cost) || StartGame.upgrades[i].owned)
        {
            if (!StartGame.upgrades[i].owned)
            {
                updateCoins(StartGame.upgrades[i].cost);


            }
            addNewUpgrade(StartGame.upgrades[i]);
            activeUpgrade = StartGame.upgrades[i];
            PlayerPrefs.SetInt("activeUpgrade", i);
            PlayerPrefs.Save();
            fillTextfields();

        }
        else
        {
            warnAboutMoney();
        }
    }

    /// <summary>
    /// lets the user buy items
    /// </summary>
    void buyItem()
    {
        if (buttonTag != null)
        {
            int index = Convert.ToInt32(buttonTag) - 1;
            if (index >= 0 && index < carTitles.Length)
            {
                buyCar(index);
            }
            if (index >= carTitles.Length && index < carTitles.Length + upgradeTitles.Length)
            {
                buyUpgrade(index, upgradeTitles[index], upgradeCosts[index]);
            }
        }
    }

    /// <summary>
    /// called if not enough money available to buy item
    /// </summary>
    void warnAboutMoney()
    {
        buyText.color = Color.red;
        buyText.text = "nicht genug Geld!";
    }

    /// <summary>
    /// checks if user can buy the item
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    bool validTransaction(int cost)
    {
        return StartGame.coins >= cost;
    }

    /// <summary>
    /// updates current coins after buying an item
    /// </summary>
    /// <param name="cost"></param>
    void updateCoins(int cost)
    {
        StartGame.coins -= cost;
        PlayerPrefs.SetInt("coins", StartGame.coins);
        PlayerPrefs.Save();
        coinText.text = "$" + StartGame.coins;
    }

    void AddCarsToPlayerPrefs()
    {
        foreach (var car in StartGame.cars)
        {
            if (car.owned == true)
            {
                PlayerPrefs.SetString(car.carName, "owned");
                PlayerPrefs.Save();
            }
        }
    }

    void AddUpgradesToPlayerPrefs()
    {
        for (int i = 0; i < StartGame.upgrades.Length; i++)
        {
            if (StartGame.upgrades[i] != null)
            {
                PlayerPrefs.SetString(StartGame.upgrades[i].upgradeName, "owned");
                PlayerPrefs.Save();
            }
        }
    }

    void addNewCar(Car car)
    {
        car.owned = true;
        AddCarsToPlayerPrefs();
    }

    void addNewUpgrade(Upgrade upgrade)
    {
        upgrade.owned = true;
        AddUpgradesToPlayerPrefs();
    }
}
