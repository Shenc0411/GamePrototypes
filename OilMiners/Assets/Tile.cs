using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public GameObject redPawn, bluePawn;
    public GameObject redOccupancyGO, blueOccupanyGO;
    public GameObject MapGO, FogGO;

    public bool isRedPawnOn, isBluePawnOn;
    public bool redOccupancy, blueOccupancy;
    public bool explored;

    public float totalValue;

    private void Update()
    {
        if (GameManager.instance.turn == Player.Red)
        {

            bluePawn.transform.localScale = Vector3.Lerp(bluePawn.transform.localScale, 0.25f * Vector2.one, 0.1f);

            redPawn.transform.localScale = Vector3.Lerp(redPawn.transform.localScale, 0.75f * Vector2.one, 0.1f);
  
        }
        else if (GameManager.instance.turn == Player.Blue)
        {

            bluePawn.transform.localScale = Vector3.Lerp(bluePawn.transform.localScale, 0.75f * Vector2.one, 0.1f);

            redPawn.transform.localScale = Vector3.Lerp(redPawn.transform.localScale, 0.25f * Vector2.one, 0.1f);

        }
    }

    public void RefreshVisual()
    {

        if (isBluePawnOn)
        {
            bluePawn.SetActive(true);
        }
        else
        {
            bluePawn.SetActive(false);
        }
        if (isRedPawnOn)
        {
            redPawn.SetActive(true);
        }
        else
        {
            redPawn.SetActive(false);
        }

        if (redOccupancy)
        {
            redOccupancyGO.SetActive(true);
            blueOccupanyGO.SetActive(false);
        }
        else if (blueOccupancy)
        {
            redOccupancyGO.SetActive(false);
            blueOccupanyGO.SetActive(true);
        }
        else
        {
            redOccupancyGO.SetActive(false);
            blueOccupanyGO.SetActive(false);
        }

        if (explored)
        {
            MapGO.SetActive(true);
            FogGO.SetActive(false);
        }
        else
        {
            MapGO.SetActive(true);
            FogGO.SetActive(true);
        }

    }

}
