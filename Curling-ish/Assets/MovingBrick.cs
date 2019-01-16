using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBrick : MonoBehaviour
{

    public float minY, maxY;
    public float speed;
    public int direction;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= minY || transform.position.y >= maxY)
        {
            direction = -direction;
        }

        transform.Translate(new Vector3(0, direction * speed * Time.deltaTime, 0));
    }
}
