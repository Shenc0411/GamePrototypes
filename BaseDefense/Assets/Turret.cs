using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public static float rangeSqr = 100;

    public static float fireInterval = 1.0f;

    private Enemy target;

    private float fireTimer;

    public Transform muzzle;

    // Start is called before the first frame update
    void Start()
    {
        transform.forward = transform.position - GameManager.instance.coreGO.transform.position;
        Quaternion rot = transform.localRotation;
        Vector3 rotE = rot.eulerAngles;
        rotE.x = 0;
        transform.localRotation = Quaternion.Euler(rotE);

        fireTimer = fireInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            float minSqr = float.MaxValue;
            foreach(Enemy enemy in GameManager.instance.enemys)
            {
                float sqr = (enemy.transform.position - GameManager.instance.coreGO.transform.position).sqrMagnitude;
                if (sqr < minSqr)
                {
                    minSqr = sqr;
                    target = enemy;
                }
            }
        }

        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

        if(target != null)
        {
            transform.forward = target.transform.position - transform.position;
            Quaternion rot = transform.localRotation;
            Vector3 rotE = rot.eulerAngles;
            rotE.x = 0;
            transform.localRotation = Quaternion.Euler(rotE);

            if(fireTimer <= 0)
            {
                Fire();
                fireTimer = fireInterval;
            }

        }

    }

    private void Fire()
    {
        GameObject GO = Instantiate(GameManager.instance.bulletPrefab);
        GO.transform.position = muzzle.position;
        GO.GetComponent<Bullet>().targetPos = target.transform.position;
    }
}
