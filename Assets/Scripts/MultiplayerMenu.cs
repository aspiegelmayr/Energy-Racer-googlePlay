using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{

    public Sprite[] availableCars;
    public Button[] carButtons;
    public Text[] playerTextFields;
    public Text titleText;
    public Button[] player1CarButtons, player2CarButtons;
    public Image[] selected;
    public Image[] player1Select, player2Select;
    public static Sprite player1Sprite, player2Sprite;
    public int[] selectedButtonIndices;
    public Button localMultiplayerStart;
    public InputField player1Nickname, player2Nickname;
    bool validNicknames;

    public Text errorMessage;

    // Start is called before the first frame update
    void Start()
    {
        Board.isMultiplayer = true;
        selectedButtonIndices = new int[2];
        splitArrays();
        HideButtonBackgrounds();
        displayCarSelection();
        errorMessage.enabled = false;
        localMultiplayerStart.onClick.AddListener(CheckSelection);
        for (int i = 0; i < carButtons.Length; i++)
        {
            carButtons[i].onClick.AddListener(ButtonClicked);
        }
    }

    public void CheckSelection()
    {
        CheckNicknames();
        if (Player1HasSelected() && Player2HasSelected() && validNicknames)
        {
            errorMessage.enabled = false;
            SubmitNicknames();
            Board.isMultiplayer = true;
            SceneManager.LoadScene("DistrictSelect");
        }
        else
        {
            if (!Player1HasSelected() && Player2HasSelected())
            {
                errorMessage.text = "Spieler 1, bitte wähle ein Auto";
            }
            else if (!Player2HasSelected() && Player1HasSelected())
            {
                errorMessage.text = "Spieler 2, bitte wähle ein Auto";
            }
            else if (!Player2HasSelected() && !Player1HasSelected())
            {
                errorMessage.text = "Bitte wählt eure Autos";
            }
           
        }
    }

    private void CheckNicknames()
    {
        if (player1Nickname.text == "" || player2Nickname.text == "")
        {
            errorMessage.text = "Bitte gebt eure Spitznamen ein";
            errorMessage.enabled = true;
        } else
        {
            SubmitNicknames();
            validNicknames = true;
        }
       
    }

    private void SubmitNicknames()
    {
        Board.player1Nickname = player1Nickname.text;
        Board.player2Nickname = player2Nickname.text;
    }

    void ButtonClicked()
    {
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        HighlightSelectedButton(GetIndexFromName(btnName));
    }

    void splitArrays()
    {
        player1CarButtons = new Button[carButtons.Length / 2];
        player2CarButtons = new Button[carButtons.Length / 2];

        player1Select = new Image[selected.Length / 2];
        player2Select = new Image[selected.Length / 2];

        for (int i = 0; i < carButtons.Length / 2; i++)
        {
            player1CarButtons[i] = carButtons[i];
            player1Select[i] = selected[i];
        }

        for (int i = carButtons.Length / 2; i < carButtons.Length; i++)
        {
            player2CarButtons[i - player1CarButtons.Length] = carButtons[i];
            player2Select[i - player1Select.Length] = selected[i];
        }
    }

    void HideButtonBackgrounds()
    {
        foreach (var selectedImg in selected)
        {
            selectedImg.enabled = false;
        }
    }

    void displayCarSelection()
    {
        for (int i = 0; i < player1CarButtons.Length; i++)
        {
            player1CarButtons[i].image.sprite = availableCars[i];
            player2CarButtons[i].image.sprite = availableCars[i];
        }

        titleText.text = "Autoauswahl";
        playerTextFields[0].text = "Spieler 1";
        playerTextFields[1].text = "Spieler 2";
    }


    void HighlightSelectedButton(int carNumber)
    {
        //check if one from player1Selected and one from player2Selected is active
        if (carNumber <= player1CarButtons.Length)
        {
            if (Player1HasSelected())
            {
                RemoveHighlightFromButton(1);
                EnableCar(selectedButtonIndices[0], 2);
            }
            selected[carNumber - 1].enabled = true;
            selectedButtonIndices[0] = carNumber - 1;
            player1Sprite = availableCars[carNumber - 1];

            DisableCar(selectedButtonIndices[0], 2);
        }
        else if (carNumber > player1CarButtons.Length)
        {
            if (Player2HasSelected())
            {
                RemoveHighlightFromButton(2);
                EnableCar(selectedButtonIndices[1], 1);
            }
            selected[carNumber - 1].enabled = true;
            selectedButtonIndices[1] = carNumber - 1;
            player2Sprite = availableCars[carNumber - player1CarButtons.Length - 1];
            DisableCar(selectedButtonIndices[1], 1);
        }


    }

    void RemoveHighlightFromButton(int forPlayerNumber)
    {
        if (forPlayerNumber == 1)
        {
            selected[selectedButtonIndices[0]].enabled = false;
        }
        else if (forPlayerNumber == 2)
        {
            selected[selectedButtonIndices[1]].enabled = false;
        }
    }


    void DeselectCar(int index, int playerNumber)
    {
        if (playerNumber == 1)
        {
            RemoveHighlightFromButton(index);
            EnableCar(index, 2);
        }
        else
        {
            EnableCar(index, 1);
        }
    }

    /// <summary>
    /// When one player has selected a car, the other player can't select the
    /// same car. Therefore the button is disabled for the other player.
    /// </summary>
    /// <param name="index">the index of the selected car button</param>
    void DisableCar(int index, int forPlayerNumber)
    {
        if (forPlayerNumber == 1)
        {
            player1CarButtons[index - player1CarButtons.Length].interactable = false;
        }

        if (forPlayerNumber == 2)
        {
            player2CarButtons[index].interactable = false;
        }
    }

    void EnableCar(int index, int forPlayerNumber)
    {
        if (forPlayerNumber == 1)
        {
            player1CarButtons[index - player1CarButtons.Length].interactable = true;
        }
        else if (forPlayerNumber == 2)
        {
            player2CarButtons[index].interactable = true;
        }

    }

    int GetIndexFromName(string btnName)
    {
        string numberOnly = Regex.Replace(btnName, "[^0-9.]", "");
        return Convert.ToInt32(numberOnly);
    }

    bool IsSelected(Button btn)
    {
        int index = GetIndexFromName(btn.name);
        if (selected[index].enabled)
        {
            return true;
        }
        return false;
    }

    bool Player1HasSelected()
    {
        foreach (var img in player1Select)
        {
            if (img.enabled)
            {
                return true;
            }
        }
        return false;
    }

    bool Player2HasSelected()
    {
        foreach (var img in player2Select)
        {
            if (img.enabled)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
