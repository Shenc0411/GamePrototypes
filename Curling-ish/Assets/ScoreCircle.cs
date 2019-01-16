using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCircle : MonoBehaviour
{

    public float radius;
    public int score;

    public int redScore;
    public int blueScore;

    private HashSet<Ball> inside = new HashSet<Ball>();

    private void Awake()
    {
        radius = transform.lossyScale.x / 2.0f;
        redScore = 0;
        blueScore = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            Vector2 dir = new Vector2(ball.transform.position.x - transform.position.x, ball.transform.position.y - transform.position.y);
            float dist = dir.magnitude;
            bool isInside = dist + ball.radius <= radius;

            if (!inside.Contains(ball) && isInside)
            {
                inside.Add(ball);
                GameManager.instance.audioSource.PlayOneShot(GameManager.instance.scoreSFX, 0.1f);

                if (ball.owner == PLAYER.RED)
                {
                    redScore += ball.scoreMultiplier * score;
                }
                else
                {
                    blueScore += ball.scoreMultiplier * score;
                }
            }
            else if (inside.Contains(ball) && !isInside)
            {
                inside.Remove(ball);
                GameManager.instance.audioSource.PlayOneShot(GameManager.instance.unscoreSFX, 0.1f);

                if (ball.owner == PLAYER.RED)
                {
                    redScore -= ball.scoreMultiplier * score;
                }
                else
                {
                    blueScore -= ball.scoreMultiplier * score;
                }
            }

        }


    }

}
