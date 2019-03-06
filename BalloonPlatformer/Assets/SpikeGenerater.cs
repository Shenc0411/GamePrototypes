using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeGenerater : MonoBehaviour
{

    public static SpikeGenerater instance;

    private Dictionary<int, GameObject> lowerSpikeGOs;
    private Dictionary<int, GameObject> upperSpikeGOs;
    public GameObject spikePrefab;
    public GameObject wallPrefab;
    public GameObject balloonGO;
    public GameObject upperWall;


    public int centerX = 0;
    public int halfWidth = 10;

    private void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        if (lowerSpikeGOs != null)
        {
            foreach (KeyValuePair<int, GameObject> pair in lowerSpikeGOs)
            {
                Destroy(pair.Value);
            }
        }
        //if (upperSpikeGOs != null)
        //{
        //    foreach (KeyValuePair<int, GameObject> pair in upperSpikeGOs)
        //    {
        //        Destroy(pair.Value);
        //    }
        //}

        lowerSpikeGOs = new Dictionary<int, GameObject>();
        //upperSpikeGOs = new Dictionary<int, GameObject>();
        centerX = (int)balloonGO.transform.position.x;
        for (int i = -halfWidth; i <= halfWidth; i++)
        {
            GameObject GO = Instantiate(spikePrefab);
            GO.transform.position = new Vector3(i, -4.75f);
            lowerSpikeGOs.Add(i, GO);
            //GO = Instantiate(spikePrefab);
            //GO.transform.position = new Vector3(i, 4.75f);
            //GO.transform.up = new Vector3(0, -1);
            //upperSpikeGOs.Add(i, GO);
        }

        upperWall = Instantiate(wallPrefab);
        upperWall.transform.position = new Vector3(balloonGO.transform.position.x, 4.75f);
        upperWall.transform.localScale = new Vector3(30, 0.5f, 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int currentCenterX = (int)balloonGO.transform.position.x;

        upperWall.transform.position = new Vector3(currentCenterX, 4.75f);

        if (centerX != currentCenterX)
        {
            if(centerX < currentCenterX)
            {
                GameObject GO = lowerSpikeGOs[centerX - halfWidth];
                lowerSpikeGOs.Remove(centerX - halfWidth);
                Destroy(GO);
                //GO = upperSpikeGOs[centerX - halfWidth];
                //upperSpikeGOs.Remove(centerX - halfWidth);
                //Destroy(GO);
                GO = Instantiate(spikePrefab);
                GO.transform.position = new Vector3(currentCenterX + halfWidth, -4.75f);
                lowerSpikeGOs.Add(currentCenterX + halfWidth, GO);
                //GO = Instantiate(spikePrefab);
                //GO.transform.position = new Vector3(currentCenterX + halfWidth, 4.75f);
                //GO.transform.up = new Vector3(0, -1);
                //upperSpikeGOs.Add(currentCenterX + halfWidth, GO);
            }
            else
            {
                GameObject GO = lowerSpikeGOs[centerX + halfWidth];
                lowerSpikeGOs.Remove(centerX + halfWidth);
                Destroy(GO);
                //GO = upperSpikeGOs[centerX + halfWidth];
                //upperSpikeGOs.Remove(centerX + halfWidth);
                //Destroy(GO);
                GO = Instantiate(spikePrefab);
                GO.transform.position = new Vector3(currentCenterX - halfWidth, -4.75f);
                lowerSpikeGOs.Add(currentCenterX - halfWidth, GO);
                //GO = Instantiate(spikePrefab);
                //GO.transform.position = new Vector3(currentCenterX - halfWidth, 4.75f);
                //GO.transform.up = new Vector3(0, -1);
                //upperSpikeGOs.Add(currentCenterX - halfWidth, GO);
            }

            centerX = currentCenterX;
        }

    }
}
