using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public enum Owner { Left, Right, None};
    public TextMeshPro text;
    public Owner owner;
    public float minRadius, maxRadius, decreaseRate, increaseRate;
    public float radius;
    public float speed;
    public Rigidbody2D RB;
    SpriteRenderer SR;
    public Target overlappingTarget;
    public float reloadValue;
    public float reloadRate;
    public float minAlpha, maxAlpha;
    // Start is called before the first frame update
    void Awake()
    {
        overlappingTarget = null;
        radius = minRadius;
        transform.localScale = radius * Vector3.one;
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        if(owner == Owner.Left)
        {
            text.text = "L";
        }else if(owner == Owner.Right)
        {
            text.text = "R";
        }
        reloadValue = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.isGameOver)
        {
            return;
        }

        float x = 0, y = 0, trigger = 0;
        if(owner == Owner.Left)
        {
            x = Input.GetAxis("LeftX");
            y = Input.GetAxis("LeftY");
            trigger = Input.GetAxis("LeftTrigger");
        }
        else if(owner == Owner.Right)
        {
            x = Input.GetAxis("RightX");
            y = Input.GetAxis("RightY");
            trigger = Input.GetAxis("RightTrigger");
        }

        RB.AddForce(new Vector2(x, y) * speed);

        if(reloadValue < 1.0f)
        {
            reloadValue += reloadRate * Time.deltaTime;
        }

        bool canCapture = false;

        if (overlappingTarget != null)
        {
            Vector3 dir = transform.position - overlappingTarget.transform.position;
            dir.z = 0;
            float dist = dir.magnitude;
            float radius = transform.localScale.x;
            float otherRadius = overlappingTarget.transform.localScale.x;
            if (dist + otherRadius <= radius)
            {
                canCapture = true;
            }
        }

        if (trigger > 0.2f && reloadValue >= 1.0f)
        {

            if (canCapture)
            {
                GameManager.instance.targetGrid[overlappingTarget.x, overlappingTarget.y] = null;
                Destroy(overlappingTarget.gameObject);
                GameManager.instance.score++;
                GameManager.instance.UpdateScorePad();
            }

            reloadValue = 0.0f;

        }

        if (reloadValue >= 1.0f && canCapture)
        {
            Color c = Color.green;
            c.a = maxAlpha;
            SR.color = c;
        }
        else
        {
            Color c = Color.white;
            c.a = Mathf.Lerp(minAlpha, maxAlpha, reloadValue);
            SR.color = c;
        }
        

    }

    

}
