using System;
using UnityEngine;

/// <summary>
/// stores upgrades
/// </summary>
public class Upgrade
{
    public string upgradeName;
    public string description;
    public int cost;
    public int bonusMoves;
    public Sprite upgradeImg;
    public bool owned;
    public bool active;

    public Upgrade(string upgradeName, int cost, int bonusMoves, Sprite upgradeImg, bool owned)
    {
        this.upgradeName = upgradeName;
        this.cost = cost;
        this.bonusMoves = bonusMoves;
        this.upgradeImg = upgradeImg;
        this.owned = owned;
    }
}
