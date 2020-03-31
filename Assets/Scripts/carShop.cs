using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// manages the car shop scene
/// </summary>
public class carShop : MonoBehaviour
{
    public Car[] cars;
    public Upgrade[] upgrades;
    public Text carTitle, upgradeTitle, coinsTxt;
    public Text[] carNames, carCosts, upgradeNames, upgradeCosts;
    public Text inUseTxt, moneyWarningTxt, nameAndCostTxt;
    public GameObject confirmPanel;
    public Button[] carBtns, upgradeBtns;
    private string selectedTag;
    private int coins;
    public Text buyTxt, activateTxt;

    void Start()
    {
        coins = StartGame.coins;
        FillOutInfo();
    }

    void FillOutInfo()
    {
        coinsTxt.text = "$" + coins;
        cars = StartGame.cars;
        upgrades = StartGame.upgrades;
        for(int i = 0; i < cars.Length; i++)
        {
            if (cars[i].owned)
            {
                cars[i].cost = 0;
            }
            if (upgrades[i].owned)
            {
                upgrades[i].cost = 0;
            }
            carNames[i].text = cars[i].carName;
            carCosts[i].text = "$" + cars[i].cost;
            carBtns[i].image.sprite = StartGame.carSprites[i];
            upgradeNames[i].text = upgrades[i].upgradeName;
            upgradeCosts[i].text = "$" + upgrades[i].cost;
            upgradeBtns[i].image.sprite = StartGame.upgradeSprites[i];
            SetOwned();
            
            HideAllMsgs();
            SetOnClickListeners();
        }
    }

    void SetOwned()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i].owned)
            {
                if(Lean.Localization.LeanLocalization.CurrentLanguage == "English")
                {
                    carCosts[i].text = "owned";
                } else
                {
                    carCosts[i].text = "in Besitz";
                }
            }
            if(StartGame.activeCar.carName == cars[i].carName)
            {
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "English")
                {
                    carCosts[i].text = "active";
                }
                else
                {
                    carCosts[i].text = "aktiv";
                }
            }
            if (upgrades[i].owned)
            {
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "English")
                {
                    upgradeCosts[i].text = "owned";
                }
                else
                {
                    upgradeCosts[i].text = "in Besitz";
                }
            }
            if (StartGame.activeUpgrade != null && StartGame.activeUpgrade.upgradeName == upgrades[i].upgradeName)
            {
                if (Lean.Localization.LeanLocalization.CurrentLanguage == "English")
                {
                    upgradeCosts[i].text = "active";
                }
                else
                {
                    upgradeCosts[i].text = "aktiv";
                }
            }
        }
    }

    void HideAllMsgs()
    {
        inUseTxt.enabled = false;
        moneyWarningTxt.enabled = false;
        TogglePanel(false);
    }

    void SetOnClickListeners()
    {
        foreach(var btn in carBtns)
        {
            btn.onClick.AddListener(delegate () { TogglePanel(true); });
        }
        foreach(var btn in upgradeBtns)
        {
            btn.onClick.AddListener(delegate () { TogglePanel(true); });
        }
    }

    public void TogglePanel(bool active)
    {
        moneyWarningTxt.enabled = false;
        if (active)
        {
            selectedTag = EventSystem.current.currentSelectedGameObject.tag;
            var index = Convert.ToInt32(selectedTag) - 1;
            if (index < cars.Length)
            {
                SetConfirmDialogTxt(cars[index].owned);
                nameAndCostTxt.text = cars[index].carName + " - $" + cars[index].cost;
            } else
            {
                SetConfirmDialogTxt(upgrades[index - cars.Length].owned);
                nameAndCostTxt.text = upgrades[index-cars.Length].upgradeName + " - $" + upgrades[index-cars.Length].cost;
            }

            confirmPanel.SetActive(true);
        } else
        {
            confirmPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="owned">whether the item is owned</param>
    void SetConfirmDialogTxt(bool owned)
    {
        if (owned)
        {
            activateTxt.enabled = true;
            buyTxt.enabled = false;
        } else
        {
            activateTxt.enabled = false;
            buyTxt.enabled = true;
        }
    }

    public void BuyItem()
    {
        if (!CheckValidTransaction())
        {
            TogglePanel(false);
            moneyWarningTxt.enabled = true;
        }
        else
        {
            coinsTxt.text = "$" + coins;
            FillOutInfo();
        }
    }


    bool CheckValidTransaction()
    {
        var index = Convert.ToInt32(selectedTag)-1;
        if(index < cars.Length)
        {
            if(cars[index].cost <= coins)
            {
                StartGame.activeCar = cars[index];
                cars[index].owned = true;
                coins -= cars[index].cost;
                UpdatePlayerPrefs();
                return true;
            }
        } else
        {
            index -= cars.Length;
            if (upgrades[index].cost <= coins)
            {
                StartGame.activeUpgrade = upgrades[index];
                upgrades[index].owned = true;
                coins -= upgrades[index].cost;
                UpdatePlayerPrefs();
                return true;
            }
        }
        return false;
    }

    void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetInt("coins", coins);
        foreach (var car in cars)
        {
            if (car.owned)
            {
                PlayerPrefs.SetString(car.carName, "owned");
                PlayerPrefs.Save();
            }
        }

        foreach (var upgrade in upgrades)
        {
            if (upgrade.owned)
            {
                PlayerPrefs.SetString(upgrade.upgradeName, "owned");
                PlayerPrefs.Save();
            }
        }
        PlayerPrefs.SetString("activeCar", StartGame.activeCar.carName);
        if (StartGame.activeUpgrade != null)
        {
            PlayerPrefs.SetString("activeUpgrade", StartGame.activeUpgrade.upgradeName);
        }
        PlayerPrefs.Save();
    }
}
