using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpStation : MonoBehaviour
{
    public static float bonus = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.BC.size += bonus;
        GameManager.instance.BC.UpdateVisual();
        Destroy(gameObject);
    }
}
