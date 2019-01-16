using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private Rigidbody2D RB;
    public int type;
    public bool selectable;
    public float radius;
    public bool isMoving;
    [Range(1, 10)]
    public int scoreMultiplier;

    public PLAYER owner;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        selectable = true;
        radius = transform.lossyScale.x / 2.0f;
    }

    private void Update()
    {
        isMoving = RB.velocity.magnitude > 0.01f;

        if (!selectable)
        {
            Color color = owner == PLAYER.RED ? Color.red : Color.blue;

            Color.RGBToHSV(color, out float h, out float s, out float v);

            Vector2 p = new Vector2(transform.position.x, transform.position.y);

            color = Color.HSVToRGB(h, s, Mathf.Clamp01(1.5f / p.magnitude) * 0.8f + 0.2f);

            GetComponent<SpriteRenderer>().material.color = color;
        }

        

    }


    public void Shoot(Vector2 force)
    {
        RB.AddForce(force, ForceMode2D.Impulse);
        Physics2D.IgnoreLayerCollision(gameObject.layer, (int)Mathf.Log(GameManager.instance.blockerLayer, 2), true);
        selectable = false;
    }

    [System.Obsolete]
    public int CalculateScore()
    {
        if (selectable)
        {
            return 0;
        }

        int score = 0;

        float currentZ = float.MaxValue;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        foreach (ScoreCircle scoreCircle in GameManager.instance.scoreCircles)
        {
            float circleRadius = scoreCircle.radius;
            
            Vector2 cirPos = new Vector2(scoreCircle.transform.position.x, scoreCircle.transform.position.y);
            float dist = (pos - cirPos).magnitude;
            //Debug.Log(dist + " " + radius + " " + circleRadius);

            if((dist + radius) <= circleRadius && currentZ > scoreCircle.transform.position.z)
            {
                currentZ = scoreCircle.transform.position.z;
                score = scoreCircle.score;
            }

        }

        //score += (int)(Mathf.Clamp01(1.0f / pos.magnitude) * 4.0f);

        Color color = owner == PLAYER.RED ? Color.red : Color.blue;

        Color.RGBToHSV(color, out float h, out float s, out float v);

        Vector2 p = new Vector2(transform.position.x, transform.position.y);

        color = Color.HSVToRGB(h, s, Mathf.Clamp01(1.0f / p.magnitude) * 0.8f + 0.2f);

        GetComponent<SpriteRenderer>().material.color = color;

        return score * scoreMultiplier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float audioLevel = Mathf.Clamp01(collision.relativeVelocity.magnitude / 10.0f) * 0.5f;
        if(audioLevel > 0.05f)
        {
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.bounceSFX, audioLevel);
        }
    }

}
