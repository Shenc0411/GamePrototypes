using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
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

    public override string GetTypeName()
    {
        return "House";
    }

    private void OnDestroy()
    {
        GameManager.instance.workers -= workersProvided;
        GameManager.instance.numHouses--;
    }
}
