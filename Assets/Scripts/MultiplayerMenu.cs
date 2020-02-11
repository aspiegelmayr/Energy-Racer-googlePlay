using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{

    public Sprite[] availableCars;
    public Button[] carButtons;
    public Text[] playerTextFields;
    public Text titleText;

    // Start is called before the first frame update
    void Start()
    {
        displayCarSelection();
        disableCar("1");
    }

    void displayCarSelection()
    {
        // carButtons 0 - length/2 - 1 = player 1
        // carButtons length/2 - length - 1 = player 2
        int carNumber = 0;
        for(int i = 0; i < carButtons.Length; i++)
        {
            if(i < availableCars.Length)
            {
                carNumber = i;
            } else
            {
                carNumber = i - availableCars.Length;
            }
            carButtons[i].image.sprite = availableCars[carNumber];
        }

        titleText.text = "Autoauswahl";
        playerTextFields[0].text = "Spieler 1";
        playerTextFields[1].text = "Spieler 2";
    }

    /// <summary>
    /// When one player has selected a car, the other player can't select the
    /// same car. Therefore the button is disabled for the other player.
    /// </summary>
    /// <param name="tag">the tag of the selected car</param>
    void disableCar(string tag)
    {
        foreach (var btn in FindObjectsOfType(typeof(Button)) as Button[])
        {
            if (btn.name == "car" + tag)
            {
                btn.interactable = false;
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
