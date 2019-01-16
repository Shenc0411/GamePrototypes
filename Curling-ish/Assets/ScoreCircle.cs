using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCircle : MonoBehaviour
{

    public float radius;
    public int score;

    private void Awake()
    {
        radius = transform.lossyScale.x / 2.0f;
    }

}
