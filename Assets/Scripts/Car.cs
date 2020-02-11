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

    public Car(string carName, string description, int cost, int movesReduction, bool owned, Sprite img)
    {
        this.carName = carName;
        this.description = description;
        this.cost = cost;
        this.movesReduction = movesReduction;
        this.owned = owned;
        this.img = img;
    }

    public static implicit operator int(Car v)
    {
        throw new NotImplementedException();
    }
    /**
    public string carName;
    public string description;
    public int cost;
    public int movesReduction;
    public Image img;
    public string owned;

    public Car(string carName, string description, int cost, int movesReduction)
    {
        this.carName = carName;
        this.description = description;
        this.cost = cost;
        this.movesReduction = movesReduction;
        this.owned = "not owned";
    }
    */
}
