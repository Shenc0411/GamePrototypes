using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{

    SpriteRenderer SR;


    public int mapSize;

    public float[,] values;

    public AnimationCurve noiseCurve;
    public float noiseScale;
    public Vector2 noiseOffset;

    private Texture2D valueTexture;
    private int width;
    private int height;

    // Start is called before the first frame update
    void Start()
    {

        SR = GetComponent<SpriteRenderer>();

        width = mapSize * 5;
        height = mapSize * 5;

        values = new float[width, height];

        valueTexture = new Texture2D(width, height);

        Sprite sprite = Sprite.Create(valueTexture, new Rect(0, 0, mapSize, mapSize), new Vector2(0.5f, 0.5f));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float v = Mathf.PerlinNoise(x * 1.0f / width * noiseScale + noiseOffset.x, y * 1.0f / height * noiseScale + noiseOffset.y);
                v = 1 - noiseCurve.Evaluate(v);
                values[x, y] = v;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color c = Color.black;
                c.a = values[x, y];
                valueTexture.SetPixel(x, y, c);
            }
        }
        valueTexture.filterMode = FilterMode.Point;
        valueTexture.Apply();
        
        SR.sprite = sprite;
        transform.localScale = new Vector3(mapSize, mapSize);
    }

}
