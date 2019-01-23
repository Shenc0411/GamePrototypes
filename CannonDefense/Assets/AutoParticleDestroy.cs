using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoParticleDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    private float timer = 0.0f;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= ps.main.duration)
        {
            Destroy(gameObject);
        }

    }
}
