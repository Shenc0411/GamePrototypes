using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Building
{

    public static int _maxHealth = 100;

    protected override void Awake()
    {
        base.Awake();
        repairable = false;
        maxHealth = _maxHealth;
        health = _maxHealth;
    }

    public override string GetTypeName()
    {
        return "Core";
    }

    public override void OnDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.OnLose();
            Destroy(gameObject);
        }
    }
}
