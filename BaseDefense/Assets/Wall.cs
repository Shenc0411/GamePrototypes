using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Building
{
    public static HashSet<Wall> activeSet = new HashSet<Wall>();

    public static int cost = 50;
    public static int _maxHealth = 30;
    public static int _upgradeCost = 400;
    public static int _upgradePowerCost = 10;
    public static int _upgradeWorkerCost = 0;
    public static int _costPerSeocnd = 15;


    public GameObject turret;
    public bool upgraded;

    protected override void Awake()
    {
        base.Awake();
        maxHealth = _maxHealth;
        health = _maxHealth;
        _cost = cost;
        repairable = true;
        upgradable = true;
        upgradeCost = _upgradeCost;
        upgradePowerCost = _upgradePowerCost;
        upgradeWorkerCost = _upgradeWorkerCost;
        turret.SetActive(false);
        upgraded = false;
    }

    public override string GetTypeName()
    {
        return "Wall";
    }

    public override void OnPlaced()
    {
        base.OnPlaced();
        if (!activeSet.Contains(this))
        {
            activeSet.Add(this);
        }
    }
    public override void OnUpgrade()
    {
        base.OnUpgrade();
        turret.SetActive(true);
        upgradable = false;
        upgraded = true;

        _cost += _upgradeCost;
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
