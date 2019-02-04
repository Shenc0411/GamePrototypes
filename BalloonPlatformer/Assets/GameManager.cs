using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreUI;
    public GameObject gameoverPanel;
    public TextMeshProUGUI gameoverUI;

    public bool hasEnded;
    public BalloonControl BC;
    public GameObject floorPrefab;
    public GameObject pumpStationPrefab;
    private HashSet<GameObject> floorGOs;
    public float probPumpStation;
    public float minWidth = 5f;
    public float maxWidth = 10f;

    public float minDistance = 1f;
    public float maxDistance = 3f;

    public float maxY = 1.0f;
    public float minY = -3.0f;

    public int score;

    private void Awake()
    {
        instance = this;
        hasEnded = false;
        floorGOs = new HashSet<GameObject>();
        GenerateFloor(new Vector2(-minWidth - (maxWidth - minWidth) / 2.0f, -2));

        for(int i = 0; i < 10; i++)
        {
            Vector2 rightMostPos = GetRightMostPosition();
            Vector2 offset = new Vector2(Random.Range(minDistance, maxDistance), 0);
            GenerateFloor(rightMostPos + offset);
        }

        gameoverPanel.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasEnded)
            return;

        if(GetRightMostPosition().x < BC.transform.position.x + 10f)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 rightMostPos = GetRightMostPosition();
                Vector2 offset = new Vector2(Random.Range(minDistance, maxDistance), 0);
                GenerateFloor(rightMostPos + offset);
            }
        }
        score = (int)BC.transform.position.x;
        scoreUI.text = "Score: " + score;
    }

    public void OnGameOver()
    {
        Debug.Log("GameOver!");

        gameoverPanel.SetActive(true);

        int highestScore = PlayerPrefs.HasKey("HS") ? PlayerPrefs.GetInt("HS") : 0;

        if(score > highestScore)
        {
            PlayerPrefs.SetInt("HS", score);
            highestScore = score;
        }

        gameoverUI.text = "Highest Score: " + highestScore;
        hasEnded = true;

        BC.RB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void GenerateFloor(Vector2 position)
    {
        GameObject GO = Instantiate(floorPrefab);
        Vector3 size = new Vector3(Random.Range(minWidth, maxWidth), 0.5f, 1.0f);
        GO.transform.position = new Vector3(position.x + size.x, position.y);
        GO.transform.localScale = size;
        floorGOs.Add(GO);
        float r = Random.Range(0.0f, 1.0f);
        if(r < probPumpStation)
        {
            GameObject fGO = Instantiate(pumpStationPrefab);
            float x = Random.Range(-size.x / 2.0f + 0.5f, size.x / 2.0f - 0.5f);
            fGO.transform.position = new Vector3(x + GO.transform.position.x, GO.transform.position.y + 1f, 0.0f);
        }
    }
    
    private Vector2 GetRightMostPosition()
    {
        Vector2 result = Vector2.zero;
        //HashSet<GameObject> toDelete = new HashSet<GameObject>();
        foreach(GameObject GO in floorGOs)
        {
            if(GO.transform.position.x + GO.transform.localScale.x / 2.0f > result.x)
            {
                result = GO.transform.position;
                result.x += GO.transform.localScale.x / 2.0f;
            }

            //if(GO.transform.position.x < BC.transform.position.x - 10f)
            //{
            //    toDelete.Add(GO);
            //}
        }

        //foreach(GameObject GO in toDelete)
        //{
        //    floorGOs.Remove(GO);
        //    Destroy(GO);
        //}

        return result;
    }

}
