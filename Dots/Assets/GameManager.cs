using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController left, right;

    public GameObject wallPrefab;
    public GameObject targetPrefab;

    public GameObject deathPanel;
    public TextMeshProUGUI highestScore, currentScore, deathPanelScore;

    public float wallThickness;

    public float height, width;

    public float targetInitialRadius, targetFinalRadius, targetExpandingRate, targetDyingRate;

    public LayerMask expandingLayer, DiedLayer;

    public Target[,] targetGrid;
    int gridHeight, gridWidth;

    public float targetSpawnInterval;
    float targetSpawnTimer;

    public int score;

    public bool isGameOver;

    private void Awake()
    {
        GameObject leftWall = Instantiate(wallPrefab);
        leftWall.transform.position = new Vector3(-width / 2 - wallThickness / 2, 0);
        leftWall.transform.localScale = new Vector3(wallThickness, height);
        GameObject rightWall = Instantiate(wallPrefab);
        rightWall.transform.position = new Vector3(width / 2 + wallThickness / 2, 0);
        rightWall.transform.localScale = new Vector3(wallThickness, height);
        GameObject bottomWall = Instantiate(wallPrefab);
        bottomWall.transform.position = new Vector3(0, -height / 2 - wallThickness / 2);
        bottomWall.transform.localScale = new Vector3(width + wallThickness * 2, wallThickness);
        GameObject topWall = Instantiate(wallPrefab);
        topWall.transform.position = new Vector3(0, height / 2 + wallThickness / 2);
        topWall.transform.localScale = new Vector3(width + wallThickness * 2, wallThickness);
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {

        score = 0;
        targetSpawnTimer = 0;
        instance = this;
        gridHeight = (int)height - 1;
        gridWidth = (int)width - 1;

        if(targetGrid != null)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    if (targetGrid[i, j] != null)
                    {
                        Destroy(targetGrid[i, j].gameObject);
                    }
                }
            }
        }
        
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        left.transform.position = new Vector3(-3, 0, 0);
        right.transform.position = new Vector3(3, 0, 0);
        left.RB.velocity = Vector2.zero;
        right.RB.velocity = Vector2.zero;
        targetGrid = new Target[gridWidth, gridHeight];
        isGameOver = false;

        deathPanel.SetActive(false);

        if (!PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetInt("Score", 0);
        }
        highestScore.text = "Highest Score:\n" + PlayerPrefs.GetInt("Score");


        UpdateScorePad();

        GenerateNewTarget();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire2"))
        {
            Initialize();
        }

        if (isGameOver)
        {
            return;
        }

        targetSpawnTimer += Time.deltaTime;
        if(targetSpawnTimer >= targetSpawnInterval)
        {
            targetSpawnTimer -= targetSpawnInterval;
            GenerateNewTarget();
        }
    }

    private void GenerateNewTarget()
    {
        List<int> possibleIndices = new List<int>();
        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                if(targetGrid[i, j] == null)
                {
                    possibleIndices.Add(i * gridWidth + j);
                }
            }
        }

        if(possibleIndices.Count == 0)
        {
            OnDeath();
        }
        else
        {
            int index = possibleIndices[Random.Range(0, possibleIndices.Count)];
            Target target = Instantiate(targetPrefab).GetComponent<Target>();
            int x = index / gridWidth;
            int y = index % gridWidth;
            target.transform.position = new Vector3(x - gridWidth / 2, y - gridHeight / 2, 2);
            targetGrid[x, y] = target;
            target.x = x;
            target.y = y;
        }

    }

    public void OnDeath()
    {
        isGameOver = true;
        deathPanelScore.text = "Game Over\nYour Score: " + score;
        deathPanel.SetActive(true);
    }

    public void UpdateScorePad()
    {
        currentScore.text = "Current Score:\n" + score;
        if(score > PlayerPrefs.GetInt("Score"))
        {
            PlayerPrefs.SetInt("Score", score);
            highestScore.text = "Highest Score:\n" + PlayerPrefs.GetInt("Score");
        }
    }

}
