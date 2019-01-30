using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerplant : Building
{

    public static int cost = 600;
    public static int powerSupply = 20;
    public static int workersNeeded = 15;
    public static int _maxHealth = 20;

    protected override void Awake()
    {
        base.Awake();
        maxHealth = _maxHealth;
        health = _maxHealth;
        _cost = cost;
        repairable = true;
    }

    public override string GetTypeName()
    {
        return "Powerplant";
    }

    public void OnDestroy()
    {
        GameManager.instance.powerSupply -= Powerplant.powerSupply;
        GameManager.instance.numPowerplants--;
        GameManager.instance.workers += Powerplant.workersNeeded;
    }

}
