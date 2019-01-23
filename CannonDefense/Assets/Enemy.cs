using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 50f;

    private List<GameObject> bodyParts = new List<GameObject>();

    private float timer = 5.0f;

    private void Awake()
    {
        int count = transform.childCount;
        for(int i = 0; i < count; i++)
        {
            GameObject bodyPart = transform.GetChild(i).gameObject;
            
            Collider collider = bodyPart.GetComponent<Collider>();
            if(collider != null)
            {
                bodyParts.Add(bodyPart);
                collider.enabled = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            OnDamage(100);
        }
    }

    public void OnDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        foreach(GameObject bodyPart in bodyParts)
        {
            bodyPart.transform.SetParent(GameManager.instance.deadEnemyPartsParent.transform);

            bodyPart.GetComponent<Collider>().enabled = true;

            Rigidbody RB = bodyPart.AddComponent<Rigidbody>();
            RB.AddForce((bodyPart.transform.position - Camera.main.transform.position) * 2f, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }


}
