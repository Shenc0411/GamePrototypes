using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public static float maxDistance = 60;
    public static float minDistance = 40;

    public static int maxHealth = 50;
    public static int minHealth = 30;
    public static float maxSpeed = 5;
    public static float minSpeed = 3;
    public static float explosionRadius = 2;
    public static int damage = 5;

    public float speed;

    public int health;

    private MeshRenderer MR;

    private Vector3 target;

    private void Awake()
    {
        MR = GetComponentInChildren<MeshRenderer>();
        target = GameManager.instance.coreGO.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(minHealth, maxHealth + 1);
        speed = Random.Range(minSpeed, maxSpeed);

        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = (target - transform.position).normalized * Time.deltaTime * speed;

        transform.position += movement;


        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, GameManager.instance.buildingLayer.value);

        if (colliders.Length > 0)
        {

            foreach(Collider collider in colliders)
            {
                Building building = collider.gameObject.GetComponent<Building>();
                if(building != null)
                {
                    building.OnDamage(damage);
                }
            }

            OnExplode();

        }
    }

    private void UpdateColor()
    {
        MR.material.color = Color.HSVToRGB(0, 1.0f, Mathf.Clamp01(health * 1.0f / maxHealth));
    }

    private void OnExplode()
    {
        GameObject GO = Instantiate(GameManager.instance.explosionFX);
        GO.transform.position = transform.position;
        GO.transform.localScale = Vector3.one * explosionRadius * 2.0f;
        Debug.Log("exploded!");
        GameManager.instance.enemys.Remove(this);
        Destroy(gameObject);
    }

    public void OnDamaged(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            GameManager.instance.enemys.Remove(this);
            Destroy(gameObject);
        }
        UpdateColor();
    }

}
