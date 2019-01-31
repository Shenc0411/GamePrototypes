using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerplant : Building
{

    public static HashSet<Powerplant> activeSet = new HashSet<Powerplant>();

    public static int cost = 600;
    public static int powerSupply = 20;
    public static int workersNeeded = 15;
    public static int _maxHealth = 20;
    public static int costPerSecond = 30;

    protected override void Awake()
    {
        base.Awake();
        maxHealth = _maxHealth;
        health = _maxHealth;
        _cost = cost;
        repairable = true;
    }

    public override void OnPlaced()
    {
        base.OnPlaced();
        activeSet.Add(this);
    }

    public override string GetTypeName()
    {
        return "Powerplant";
    }

    public void OnDestroy()
    {
        if (activeSet.Contains(this))
        {
            activeSet.Remove(this);
        }

        BuildManager.instance.buildings.Remove(this);
    }

}
