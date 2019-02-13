using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Player { Red, Blue, None };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject tilePrefab;

    public GameObject redPanel, bluePanel;

    public Button redPlaceButton, bluePlaceButton;

    public TextMeshProUGUI redScoreUI, redDrillUI, redInfo, blueScoreUI, blueDrillUI, blueInfo;

    private float redActiveX, redInactiveX;
    private float blueActiveX, blueInactiveX;

    private HashSet<Tile> tiles;

    [Range(5, 9)]
    public int mapSize;
    [Range(5, 20)]
    public int mapScale;


    public AnimationCurve noiseCurve;
    public float noiseScale;
    public Vector2 noiseOffset;

    public Player turn;
    public Tile redTile, blueTile;
    public float redScore, blueScore;
    public int redDrills, blueDrills;

    public bool exposeAll = false;

    private void Awake()
    {
        instance = this;
        tiles = new HashSet<Tile>();
    }

    private void Start()
    {
        noiseOffset.x = Random.Range(0, 100.0f);
        noiseOffset.y = Random.Range(0, 100.0f);

        GenerateTiles();
        turn = (Random.Range(0, 2) > 0) ? Player.Blue : Player.Red;
        
        redActiveX = redPanel.GetComponent<RectTransform>().position.x;
        redInactiveX = redActiveX - 300;
        blueActiveX = bluePanel.GetComponent<RectTransform>().position.x;
        blueInactiveX = blueActiveX + 300;

        redDrills = 5;
        blueDrills = 5;

        redScore = 0;
        blueScore = 0;

        SwitchTurn();

    }

    private void Update()
    {
        if(turn == Player.None)
        {
            return;
        }

        //if(turn == Player.Red)
        //{
        //    Vector3 rPos = redPanel.GetComponent<RectTransform>().position;
        //    rPos.x = Mathf.Lerp(rPos.x, redActiveX, 0.1f);
        //    redPanel.GetComponent<RectTransform>().position = rPos;
        //    Vector3 bPos = bluePanel.GetComponent<RectTransform>().position;
        //    bPos.x = Mathf.Lerp(bPos.x, blueInactiveX, 0.1f);
        //    bluePanel.GetComponent<RectTransform>().position = bPos;
        //}
        //else
        //{
        //    Vector3 rPos = redPanel.GetComponent<RectTransform>().position;
        //    rPos.x = Mathf.Lerp(rPos.x, redInactiveX, 0.1f);
        //    redPanel.GetComponent<RectTransform>().position = rPos;
        //    Vector3 bPos = bluePanel.GetComponent<RectTransform>().position;
        //    bPos.x = Mathf.Lerp(bPos.x, blueActiveX, 0.1f);
        //    bluePanel.GetComponent<RectTransform>().position = bPos;
        //}

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if(hit.collider != null)
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if(tile != null)
                {
                    if(turn == Player.Red)
                    {
                        if (Vector2.Distance(tile.transform.position, redTile.transform.position) <= tilePrefab.transform.localScale.x * tilePrefab.transform.localScale.x)
                        {
                            if(redTile == tile)
                            {
                                return;
                            }

                            redTile.isRedPawnOn = false;
                            redTile.RefreshVisual();
                            tile.isRedPawnOn = true;
                            tile.explored = true;
                            tile.RefreshVisual();
                            redTile = tile;

                            SwitchTurn();
                        }
                        else
                        {
                            Debug.LogError("INVALID MOVE");
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(tile.transform.position, blueTile.transform.position) <= tilePrefab.transform.localScale.x * tilePrefab.transform.localScale.x)
                        {
                            if(blueTile == tile)
                            {
                                return;
                            }

                            blueTile.isBluePawnOn = false;
                            blueTile.RefreshVisual();
                            tile.isBluePawnOn = true;
                            tile.explored = true;
                            tile.RefreshVisual();
                            blueTile = tile;

                            SwitchTurn();
                        }
                        else
                        {
                            Debug.LogError("INVALID MOVE");
                        }
                    }
                }
            }
        }
    }

    void UpdatePanels()
    {
        redScoreUI.text = "Oil Reserves:\n" + (int)redScore;
        redDrillUI.text = "Drills Left:\n" + redDrills;
        blueScoreUI.text = "Oil Reserves:\n" + (int)blueScore;
        blueDrillUI.text = "Drills Left:\n" + blueDrills;
    }

    void SwitchTurn()
    {
        if (turn == Player.Red)
        {
            turn = Player.Blue;
        }
        else if (turn == Player.Blue)
        {
            turn = Player.Red;
        }

        UpdatePanels();
        UpdateButtonInteractability();
        redTile.RefreshVisual();
        blueTile.RefreshVisual();


        if(redDrills == 0 && blueDrills == 0)
        {
            turn = Player.None;
            if (redScore > blueScore)
            {
                redInfo.text = "You Won";
                blueInfo.text = "You Lost";
            }
            else if (redScore == blueScore)
            {
                redInfo.text = "You Lost";
                blueInfo.text = "You Won";
            }
            else
            {
                redInfo.text = "Draw";
                blueInfo.text = "Draw";
            }

            foreach(Tile tile in tiles)
            {
                tile.explored = true;
                tile.RefreshVisual();
            }

        }
        else if (turn == Player.Red && redDrills <= 0) SwitchTurn();
        else if (turn == Player.Blue && blueDrills <= 0) SwitchTurn();

    }


    void UpdateButtonInteractability()
    {
        Debug.Log(redTile.redOccupancy + " " + redTile.blueOccupancy + " " + redDrills);
        Debug.Log(blueTile.redOccupancy + " " + blueTile.blueOccupancy + " " + blueDrills);
        if (turn == Player.Red)
        {
            redPlaceButton.interactable = !redTile.redOccupancy && !redTile.blueOccupancy && redDrills > 0;
            bluePlaceButton.interactable = false;
        }
        else if (turn == Player.Blue)
        {
            redPlaceButton.interactable = false;
            bluePlaceButton.interactable = !blueTile.redOccupancy && !blueTile.blueOccupancy && blueDrills > 0;
        }
    }

    public void RedPlaceDrill()
    {
        redDrills -= 1;
        redScore += redTile.totalValue;
        redTile.redOccupancy = true;
        SwitchTurn();
    }

    public void BluePlaceDrill()
    {
        blueDrills -= 1;
        blueScore += blueTile.totalValue;
        blueTile.blueOccupancy = true;
        SwitchTurn();
    }

    void GenerateTiles()
    {

        Vector2 upperLeft = new Vector2( -mapSize / 2.0f * tilePrefab.transform.localScale.x, mapSize / 2.0f * tilePrefab.transform.localScale.x);

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                GameObject newTileGO = Instantiate(tilePrefab);
                Vector3 pos = upperLeft;
                pos.x += x * tilePrefab.transform.localScale.x;
                pos.y -= y * tilePrefab.transform.localScale.x;
                newTileGO.transform.position = pos;

                Texture2D texture = new Texture2D(mapScale, mapScale);
                float valueSum = 0;

                for (int subX = 0; subX < mapScale; subX++)
                {
                    for (int subY = 0; subY < mapScale; subY++)
                    {
                        float v = Mathf.PerlinNoise((x * mapScale + subX + noiseOffset.x) / (mapScale * mapSize) * noiseScale, (y * mapScale + subY + noiseOffset.y) / (mapScale * mapSize) * noiseScale);

                        //v = 0;
                        //for (int i = -1; i <= 1; i++)
                        //{
                        //    for(int j = -1; j <= 1; j++)
                        //    {
                        //        v += Mathf.PerlinNoise(((x + i) * mapScale + subX + noiseOffset.x) / (mapScale * mapSize) * noiseScale, ((y + j) * mapScale + subY + noiseOffset.y) / (mapScale * mapSize) * noiseScale);
                        //    }
                        //}
                        //v /= 9.0f;
                        
                        v = noiseCurve.Evaluate(v);
                        Color color = v * Color.white;
                        color.a = 1.0f;
                        texture.SetPixel(subX, mapScale - subY - 1, color);
                        valueSum += 1 - v;
                    }
                }

                texture.filterMode = FilterMode.Point;
                texture.Apply();

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, mapScale, mapScale), new Vector2(0.5f, 0.5f), mapScale);

                Tile tile = newTileGO.GetComponent<Tile>();

                tiles.Add(tile);

                tile.MapGO.GetComponent<SpriteRenderer>().sprite = sprite;
                
                tile.totalValue = valueSum;

                tile.explored = exposeAll;

                if (x == mapSize / 2 && y == mapSize / 2)
                {
                    redTile = tile;
                    blueTile = tile;
                    tile.isBluePawnOn = true;
                    tile.isRedPawnOn = true;
                    tile.explored = true;
                }

                

                tile.RefreshVisual();
            }
        }

    }

}
