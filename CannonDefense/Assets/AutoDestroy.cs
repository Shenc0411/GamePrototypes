﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    public static float time = 10;

    private float timer = 0.0f;

    private bool bDestroy = false;

    private float destroyValue = 0.0f;

    private float destroyRate = 1.0f;

    private MeshRenderer MR;

    private int hitCounter = 2;

    private void Awake()
    {
        MR = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bDestroy)
        {
            timer += Time.deltaTime;
            if (timer >= time)
            {
                bDestroy = true;
            }
        }
        else
        {
            destroyValue += destroyRate * Time.deltaTime;
            if(destroyValue >= 1)
            {
                Destroy(gameObject);
            }
            else
            {
                MR.material.SetFloat("_DissolveAmount", destroyValue);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != gameObject.layer)
        {
            return;
        }

        float volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 10.0f) / 5f;

        if(volume > 0.1f)
        {
            GameManager.instance.PlayCollisionSFX(volume);
        }

    }

    public void OnHit()
    {
        hitCounter--;
        if(hitCounter <= 0)
        {
            bDestroy = true;
        }
    }

}