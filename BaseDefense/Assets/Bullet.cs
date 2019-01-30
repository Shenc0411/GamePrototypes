using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static float speed = 25f;
    public static float explosionRadius = 2f;
    public static int damage = 10;

    public Vector3 targetPos;

    private Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        GameObject GO = Instantiate(GameManager.instance.fireFX);
        GO.transform.position = transform.position;
        GO.transform.localScale = Vector3.one;

        dir = (targetPos - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        

        if(Physics.OverlapSphere(transform.position, transform.localScale.x, ~GameManager.instance.buildingLayer.value).Length > 0)
        {
            GameObject GO = Instantiate(GameManager.instance.explosionFX);
            GO.transform.position = transform.position;
            GO.transform.localScale = Vector3.one * explosionRadius * 2.0f;

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, GameManager.instance.enemyLayer.value);

            foreach(Collider collider in colliders)
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if(enemy != null)
                {
                    enemy.OnDamaged(damage);
                }
            }

            Destroy(gameObject);
        }
    }
}
