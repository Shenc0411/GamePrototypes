using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{

    public static HashSet<House> activeSet = new HashSet<House>();

    public static int cost = 300;
    public static int workersProvided = 5;
    public static int goldPerSecond = 20;
    public static int _maxHealth = 10;

    protected override void Awake()
    {
        base.Awake();
        repairable = true;
        maxHealth = _maxHealth;
        health = _maxHealth;
        _cost = cost;
    }

    public override void OnPlaced()
    {
        base.OnPlaced();
        activeSet.Add(this);
    }

    public override string GetTypeName()
    {
        return "House";
    }

    private void OnDestroy()
    {

        if (activeSet.Contains(this))
        {
            activeSet.Remove(this);
        }

        BuildManager.instance.buildings.Remove(this);
    }
}
