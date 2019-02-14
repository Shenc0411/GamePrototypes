using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControl : MonoBehaviour
{
    public Rigidbody2D RB;
    public float forceFactor = 10f;
    public float size = 2;
    public float speed = 0.5f;
    public GameObject circle;
    public GameObject triangle;

    public void Initialize()
    {
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        RB.velocity = Vector3.zero;
        RB.angularVelocity = 0;
        RB.constraints = RigidbodyConstraints2D.None;
        size = 2;
        UpdateVisual();
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        if(RB == null)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateVisual();
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.hasEnded && Input.GetKey(KeyCode.Space))
        {
            
            if(size >= 0.4f)
            {
                RB.AddForce(-transform.up * forceFactor);
                size -= speed * Time.deltaTime;
                UpdateVisual();
            }else if(RB.velocity.magnitude <= 0.1f)
            {
                GameManager.instance.OnGameOver();
            }
            
        }    
    }

    public void UpdateVisual()
    {
        if (size <= 1.0f)
        {
            circle.transform.localScale = new Vector3(size, 1.0f, 1.0f);
        }
        else
        {
            circle.transform.localScale = new Vector3(size, size, 1.0f);
            triangle.transform.localPosition = new Vector3(0, size / 2.0f + 0.05f, 1.0f);
        }
    }
}
