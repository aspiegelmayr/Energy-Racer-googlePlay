using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// stores cars
/// </summary>
public class Car
{
    public string carName;
    public string description;
    public int cost;
    public int movesReduction;
    public Sprite img;
    public bool owned;
    public bool active;

    public Car(string carName, int cost, int movesReduction, bool owned, Sprite img)
    {
        this.carName = carName;
        this.cost = cost;
        this.movesReduction = movesReduction;
        this.owned = owned;
        this.img = img;
    }
}
