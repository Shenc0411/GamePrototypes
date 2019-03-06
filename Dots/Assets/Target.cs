using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int x, y;
    float radius, initialRadius, finalRadius;
    SpriteRenderer SR;
    CircleCollider2D CC;
    bool isExpanding, isDying, isDecaying;
    float expandingRate, dyingRate;
    float expandingValue, dyingValue;
    float decayingRate = 0.05f, decayingValue;
    GameObject stayingDot;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        radius = GameManager.instance.targetInitialRadius;
        initialRadius = GameManager.instance.targetInitialRadius;
        finalRadius = GameManager.instance.targetFinalRadius;
        transform.localScale = radius * Vector3.one;
        SR = GetComponent<SpriteRenderer>();
        CC = GetComponent<CircleCollider2D>();
        SR.color = Color.white;
        expandingRate = GameManager.instance.targetExpandingRate;
        dyingRate = GameManager.instance.targetDyingRate;
        expandingValue = 0;
        dyingValue = 0;
        decayingValue = 0;
        isExpanding = true;
        isDying = false;
        isDecaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        if (isExpanding)
        {
            expandingValue += expandingRate * Time.deltaTime;
            radius = Mathf.Lerp(initialRadius, finalRadius, expandingValue);

            if(expandingValue >= 1.0f)
            {
                radius = finalRadius;
                isDying = true;
                isExpanding = false;
            }

            transform.localScale = radius * Vector3.one;

            SR.color = Color.Lerp(Color.white, Color.red, expandingValue);
        }
        else if (isDying)
        {
            dyingValue += dyingRate * Time.deltaTime;
            if(dyingValue >= 1.0f)
            {
                CC.isTrigger = false;
                if(stayingDot != null)
                {
                    stayingDot.SetActive(false);
                    if(!GameManager.instance.left.gameObject.activeSelf && !GameManager.instance.right.gameObject.activeSelf)
                    {
                        GameManager.instance.OnDeath();
                    }
                }
                isDying = false;
                isDecaying = true;
                //enabled = false;
            }
            SR.color = Color.Lerp(Color.red, Color.black, dyingValue);
        }
        else if (isDecaying)
        {
            decayingValue += decayingRate * Time.deltaTime;

            if (decayingValue >= 1)
            {
                GameManager.instance.targetGrid[x, y] = null;
                Destroy(gameObject);
            }
            Color c = SR.color;
            c.a = Mathf.Lerp(1.0f, 0.2f, decayingValue);
            SR.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController PC = collision.gameObject.GetComponent<PlayerController>();

        if(PC == null)
        {
            return;
        }

        stayingDot = collision.gameObject;
        PC.overlappingTarget = this;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController PC = collision.gameObject.GetComponent<PlayerController>();

        if (PC == null)
        {
            return;
        }

        stayingDot = null;
        PC.overlappingTarget = null;
    }

}
