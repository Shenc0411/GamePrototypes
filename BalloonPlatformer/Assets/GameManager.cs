using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WindZone
{
    public float startX, endX;
    public int orientation; //0 = right, 90 = up, 180 = left, 270 = down
    public float strength;
    public Vector2 dir;

    public WindZone(float startX, float endX, int ori, float strength)
    {
        this.startX = startX;
        this.endX = endX;
        this.strength = strength;
        this.orientation = ori;

        if (ori == 0)
        {
            dir = new Vector2(1, 0);
        }
        else if (ori == 90)
        {
            dir = new Vector2(0, 1);
        }
        else if (ori == 180)
        {
            dir = new Vector2(-1, 0);
        }
        else
        {
            dir = new Vector2(0, -1);
        }
        

    }

}

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
    private HashSet<GameObject> pumpGOs;
    public float probPumpStation;
    public float minWidth = 5f;
    public float maxWidth = 10f;

    public float minDistance = 1f;
    public float maxDistance = 3f;

    public float maxY = 0.5f;
    public float minY = -4.25f;

    public int score;


    public GameObject windZonePanel;
    public TextMeshProUGUI windZoneText;
    public RectTransform windOrientation;

    public WindZone windzone;

    private void Awake()
    {
        instance = this;

    }

    public void Initialize()
    {

        if(floorGOs != null)
        {
            foreach(GameObject GO in floorGOs)
            {
                Destroy(GO);
            }
        }

        if(pumpGOs != null)
        {
            foreach(GameObject GO in pumpGOs)
            {
                Destroy(GO);
            }
        }

        hasEnded = false;
        floorGOs = new HashSet<GameObject>();
        pumpGOs = new HashSet<GameObject>();
        GenerateFloor(new Vector2(-minWidth - (maxWidth - minWidth) / 2.0f, -3));

        for (int i = 0; i < 10; i++)
        {
            Vector2 rightMostPos = GetRightMostPosition();
            Vector2 offset = new Vector2(Random.Range(minDistance, maxDistance), Random.Range(-0.5f, 0.5f));
            Vector2 pos = rightMostPos + offset;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            GenerateFloor(pos);
        }

        gameoverPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        BC.Initialize();
        Initialize();
        SpikeGenerater.instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            BC.Initialize();
            Initialize();
            SpikeGenerater.instance.Initialize();
        }

        if (hasEnded)
            return;

        if (windzone == null)
        {

            float startX = BC.transform.position.x + Random.Range(20, 100);
            float endX = startX + Random.Range(10, 20);
            float strength = 5;
            int orientation = Random.Range(0, 4) * 90;
            windzone = new WindZone(startX, endX, orientation, strength);
        }
        else if(BC.transform.position.x < windzone.startX - 15f)
        {
            windZonePanel.SetActive(false);
        }
        else if (BC.transform.position.x >= windzone.startX - 15f && BC.transform.position.x < windzone.startX)
        {
            windZonePanel.SetActive(true);
            windZoneText.text = "Wind Zone in " + (int)(windzone.startX - BC.transform.position.x);
            windOrientation.localRotation = Quaternion.Euler(0, 0, windzone.orientation);
        }
        else if (BC.transform.position.x >= windzone.startX && BC.transform.position.x <= windzone.endX)
        {
            windZoneText.text = "Wind Zone End in " + (int)(windzone.endX - BC.transform.position.x);
            BC.RB.AddForce(windzone.strength * windzone.dir);
        }
        else if (BC.transform.position.x >= windzone.endX)
        {
            float startX = BC.transform.position.x + Random.Range(20, 100);
            float endX = startX + Random.Range(10, 20);
            float strength = 1;
            int orientation = Random.Range(0, 4) * 90;
            windzone = new WindZone(startX, endX, orientation, strength);
        }


        if (GetRightMostPosition().x < BC.transform.position.x + 20f)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 rightMostPos = GetRightMostPosition();
                Vector2 offset = new Vector2(Random.Range(minDistance, maxDistance), Random.Range(-0.5f, 0.5f));
                Vector2 pos = rightMostPos + offset;
                pos.y = Mathf.Clamp(pos.y, minY, maxY);
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
        GO.name = "Floor" + GO.GetHashCode();
        floorGOs.Add(GO);
        if(Random.Range(0.0f, 1.0f) < probPumpStation)
        {
            GameObject fGO = Instantiate(pumpStationPrefab);
            float x = Random.Range(-size.x / 2.0f + 0.5f, size.x / 2.0f - 0.5f) + GO.transform.position.x;
            float y = Random.Range(1f, 2f) + GO.transform.position.y;
            fGO.transform.position = new Vector3(x, y, 0.0f);
            fGO.name = "Pump -" + GO.name;
            pumpGOs.Add(fGO);
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
