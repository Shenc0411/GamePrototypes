using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public static int maxHealth = 3;

    public static int minHealth = 1;

    public int health;

    private List<GameObject> bodyParts = new List<GameObject>();

    private List<MeshRenderer> MRs = new List<MeshRenderer>();

    private bool isFadingIn;

    private float fadeInValue;

    public float fadeInRate = 1.0f;

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

            MeshRenderer MR = bodyPart.GetComponent<MeshRenderer>();
            if(MR != null)
            {
                MRs.Add(MR);
            }

        }

        health = Random.Range(minHealth, maxHealth);

        UpdateColor();

        isFadingIn = true;

        fadeInValue = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isFadingIn)
        {
            fadeInValue -= fadeInRate * Time.deltaTime;
            if(fadeInValue < 0.0f)
            {
                isFadingIn = false;
                fadeInValue = 0.0f;
            }

            foreach (MeshRenderer MR in MRs)
            {
                MR.material.SetFloat("_DissolveAmount", fadeInValue);
            }
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if(Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        GetComponent<NavMeshAgent>().SetDestination(hit.point);
        //    }

        //}else if (Input.GetMouseButtonUp(1))
        //{
        //    OnDamage(100);
        //}

    }

    private void UpdateColor()
    {
        foreach(MeshRenderer MR in MRs)
        {
            Color color = MR.material.color;

            float h, s, v;

            Color.RGBToHSV(color, out h, out s, out v);

            v = health * 1.0f / maxHealth;

            MR.material.color = Color.HSVToRGB(h, s, v);
        }
    }

    public Rigidbody[] OnDamage(int damage)
    {
        health -= damage;

        health = health >= 0 ? health : 0;

        UpdateColor();

        if (health <= 0)
        {
            return OnDeath();
        }

        return null;
    }

    private Rigidbody[] OnDeath()
    {
        List<Rigidbody> partRBs = new List<Rigidbody>();

        foreach(GameObject bodyPart in bodyParts)
        {
            bodyPart.transform.SetParent(GameManager.instance.deadEnemyPartsParent.transform);

            bodyPart.GetComponent<Collider>().enabled = true;

            Rigidbody RB = bodyPart.AddComponent<Rigidbody>();
            partRBs.Add(RB);

            bodyPart.AddComponent<AutoDestroy>();
        }

        Destroy(gameObject);

        return partRBs.ToArray();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == GameManager.instance.loseLayer)
        {
            GameManager.instance.OnLose();
        }
    }

}
