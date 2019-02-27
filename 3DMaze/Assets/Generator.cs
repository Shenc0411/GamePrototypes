using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public int height, width, levels;
    public int[,,] maze;

    public float verticalProb = 0.2f;

    private readonly Vector3[] directions = { new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0) };

    private List<Vector3> possibleEndPoints = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMaze()
    {

        maze = GenerateMazeData();

        GenerateVisual(maze);
    }

    private int[,,] GenerateMazeData()
    {
        int[,,] mazeData = new int[levels, height, width]; // 0 = wall, 1 = empty

        Vector3 startPoint = new Vector3(0, 0, 0);
        Vector3 endPoint = new Vector3(Random.Range(levels / 2, levels), Random.Range(height / 2, height), Random.Range(width / 2, width));

        CreatePaths(startPoint, mazeData);

        return mazeData;
    }

    private void GenerateVisual(int[,,] mazeData)
    {
        for(int i = 0; i < levels * 2 + 1; i++)
        {
            for(int j = -1; j <= height; j++)
            {
                for(int k = -1; k <= width; k++)
                {
                    if (i % 2 == 0 || j == -1 || j == height || k == -1 || k == width || mazeData[i / 2, j, k] == 0)
                    {
                        GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        GO.transform.position = new Vector3(j, i, k);

                        if (i > 1)
                        {
                            Color c = Color.gray;
                            c.a = 0.1f;
                            GO.GetComponent<MeshRenderer>().material.color = c;
                        }

                    }
                }
            }
        }
    }

    private void CreatePaths(Vector3 startPoint, int[,,] mazeData)
    {

        List<Vector3> openList = new List<Vector3>
        {
            startPoint
        };

        while (openList.Count > 0)
        {
            Shuffle(openList);
            Vector3 currentPoint = openList[0];
            openList.RemoveAt(0);
            bool hasNext = false;

            foreach (Vector3 dir in directions)
            {
                Vector3 nextPoint = dir + currentPoint;

                if (nextPoint.x < 0 || nextPoint.x >= levels || nextPoint.y < 0 || nextPoint.y >= height || nextPoint.z < 0 || nextPoint.z >= width || mazeData[(int)nextPoint.x, (int)nextPoint.y, (int)nextPoint.z] != 0)
                {
                    continue;
                }

                bool pointOccupied = false;
                bool valid = true;
                foreach (Vector3 d in directions)
                {
                    Vector3 neighbor = nextPoint + d;
                    if (neighbor.x < 0 || neighbor.x >= levels || neighbor.y < 0 || neighbor.y >= height || neighbor.z < 0 || neighbor.z >= width)
                    {
                        continue;
                    }
                    if (mazeData[(int)neighbor.x, (int)neighbor.y, (int)neighbor.z] == 1)
                    {
                        if (pointOccupied)
                        {
                            valid = false;
                            break;
                        }
                        else
                        {
                            pointOccupied = true;
                        }
                    }
                }

                if (valid)
                {
                    if (dir.x == 0 || (dir.x != 0 && Random.Range(0.0f, 1.0f) < verticalProb))
                    {
                        mazeData[(int)nextPoint.x, (int)nextPoint.y, (int)nextPoint.z] = 1;
                        openList.Add(nextPoint);
                        hasNext = true;
                    }
                }

            }

            if (!hasNext)
            {
                possibleEndPoints.Add(currentPoint);
            }

        }

    }

    public void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
