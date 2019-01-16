using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PLAYER { RED, BLUE, NONE };

[System.Serializable]
public struct ArrVector3
{
    public List<Vector3> positions;
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject restartButton;

    public bool hasStarted;

    public TextMeshProUGUI scorePad;
    public TextMeshProUGUI message;

    public AudioSource audioSource;
    public AudioClip bounceSFX;

    public PLAYER turn;
    public LayerMask redLayer, blueLayer, blockerLayer;
    public Ball selectedBall;
    public float forceFactor = 4.0f;
    public int redScore, blueScore;
    public GameObject redBlocker, blueBlocker;
    public List<ScoreCircle> scoreCircles;
    public List<Ball> redBalls;
    public List<Ball> blueBalls;

    public List<ArrVector3> redPositions;

    public List<ArrVector3> bluePositions;

    private void Awake()
    {
        instance = this;

        hasStarted = false;

        GameObject[] scoreCircleGOs = GameObject.FindGameObjectsWithTag("ScoreCircle");
        GameObject[] redBallGOs = GameObject.FindGameObjectsWithTag("RedBall");
        GameObject[] blueBallGOs = GameObject.FindGameObjectsWithTag("BlueBall");

        foreach(GameObject scoreCircle in scoreCircleGOs)
        {
            scoreCircles.Add(scoreCircle.GetComponent<ScoreCircle>());
        }
        foreach(GameObject redBall in redBallGOs)
        {
            redBalls.Add(redBall.GetComponent<Ball>());
        }
        foreach (GameObject blueBall in blueBallGOs)
        {
            blueBalls.Add(blueBall.GetComponent<Ball>());
        }

    }

    private void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        hasStarted = true;

        restartButton.SetActive(false);

        turn = PLAYER.RED;
        redScore = 0;
        blueScore = 0;

        Dictionary<int, int> redIndex = new Dictionary<int, int>();
        foreach(Ball ball in redBalls)
        {

            ball.selectable = true;

            if (!redIndex.ContainsKey(ball.type))
            {
                redIndex.Add(ball.type, 0);
            }

            ball.transform.position = redPositions[ball.type].positions[redIndex[ball.type]];

            redIndex[ball.type]++;
        }

        Dictionary<int, int> blueIndex = new Dictionary<int, int>();
        foreach (Ball ball in blueBalls)
        {

            ball.selectable = true;

            if (!blueIndex.ContainsKey(ball.type))
            {
                blueIndex.Add(ball.type, 0);
            }

            ball.transform.position = bluePositions[ball.type].positions[blueIndex[ball.type]];

            blueIndex[ball.type]++;
        }

        UpdateScore();

        UpdateMessage();

    }

    private void Update()
    {
        if (!hasStarted)
        {
            return;
        }

        UpdateScore();

        if (turn == PLAYER.NONE)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(selectedBall == null)
            {
                LayerMask layer = turn == PLAYER.RED ? redLayer : blueLayer;
                Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f, layer);

                if (hit)
                {
                    Debug.Log(hit.transform.name);
                    Ball ball = hit.transform.gameObject.GetComponent<Ball>();
                    if (ball.selectable)
                    {
                        selectedBall = ball;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(selectedBall != null)
            {
                Vector2 pos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                Vector2 ballPos = new Vector2(selectedBall.transform.position.x, selectedBall.transform.position.y);
                Vector2 force = (ballPos - pos) * forceFactor;
                selectedBall.Shoot(force);
                selectedBall = null;
                
                SwitchTurn();
            }
        }

    }

    private void SwitchTurn()
    {
        turn = turn == PLAYER.BLUE ? PLAYER.RED : PLAYER.BLUE;
        UpdateMessage();
    }

    private void UpdateMessage()
    {
        message.text = turn == PLAYER.RED ? "Red" : "Blue";
        message.text += "'s turn";
    }

    public void UpdateScore()
    {
        redScore = 0;

        bool finished = true;
        bool turnsLeft = false;

        foreach(Ball ball in redBalls)
        {
            redScore += ball.CalculateScore();
            if(ball.isMoving || ball.selectable)
            {
                finished = false;
            }

            if (ball.selectable)
            {
                turnsLeft = true;
            }
        }

        blueScore = 0;

        foreach (Ball ball in blueBalls)
        {
            blueScore += ball.CalculateScore();
            if (ball.isMoving || ball.selectable)
            {
                finished = false;
            }

            if (ball.selectable)
            {
                turnsLeft = true;
            }
        }

        if (finished)
        {
            if (redScore == blueScore)
            {
                message.text = "Draw";
            }
            else if (redScore > blueScore)
            {
                message.text = "Red wins";
            }
            else
            {
                message.text = "Blue wins";
            }

            hasStarted = false;
            restartButton.SetActive(true);
        }
        else if (!turnsLeft)
        {
            message.text = "Waiting for results";
        }

        scorePad.text = "Red " + redScore + " : " + blueScore + " Blue";
    }
}
