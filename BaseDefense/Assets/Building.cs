using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    protected int _cost;
    public int maxHealth;
    public int health;

    public MeshRenderer[] MRs;
    public Dictionary<MeshRenderer, Color> colorMap = new Dictionary<MeshRenderer, Color>();

    public bool upgradable;
    public int upgradeCost;
    public int upgradePowerCost;
    public int upgradeWorkerCost;

    public bool repairable;

    protected virtual void Awake()
    {
        MRs = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer MR in MRs)
        {
            colorMap.Add(MR, MR.material.color);
        }
    }

    public virtual void OnPlaced()
    {

    }

    public virtual void OnDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int CalculateValue()
    {
        return (int)(_cost * health * 1.0f / maxHealth);
    }

    public int CalculateRepairCost()
    {
        return (int)(_cost * (1.0f - health * 1.0f / maxHealth));
    }

    public virtual void OnRepaired()
    {
        Debug.Log("Repaired " + name);
        health = maxHealth;
    }

    public virtual string GetTypeName()
    {
        return "Building";
    }

    public virtual void OnUpgrade()
    {
        Debug.Log("Upgraded " + name);
    }

}

