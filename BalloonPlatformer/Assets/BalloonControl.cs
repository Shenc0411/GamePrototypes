using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControl : MonoBehaviour
{
    public Rigidbody2D RB;
    public float forceFactor = 10f;
    public float size = 1;
    public float speed = 0.5f;
    public GameObject circle;
    public GameObject triangle;

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
            RB.AddForce(-transform.up * forceFactor);
            size -= speed * Time.deltaTime;

            UpdateVisual();
        }    
    }

    public void UpdateVisual()
    {
        if (size <= 0.4f && RB.velocity.magnitude < 0.1f)
        {
            GameManager.instance.OnGameOver();
        }
        else if (size <= 1.0f)
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
