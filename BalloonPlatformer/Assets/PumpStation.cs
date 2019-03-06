using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpStation : MonoBehaviour
{
    public float value = 0.5f;

    public void Awake()
    {
        value = Random.Range(0, 2) * 0.25f + 0.25f;
        value = Random.Range(0, 5) > 1 ? value : -value;
        if(value < 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        transform.localScale = value * Vector3.one;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.BC.size += value;
        GameManager.instance.BC.UpdateVisual();
        Destroy(gameObject);
    }
}
