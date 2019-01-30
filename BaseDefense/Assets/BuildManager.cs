using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public GameObject gridGO;

    public GameObject housePrefab;
    public GameObject powerplantPrefab;
    public GameObject wallPrefab;

    public Button demolishButton;

    public enum BuildingType { NONE, HOUSE, POWERPLANT, WALL, DEMOLISH};

    public BuildingType currentBuildingType;

    public GameObject currentPlacingGO;

    public DestructableObject prevHighlighted;

    public List<Building> buildings;

    private void Awake()
    {
        instance = this;
        currentBuildingType = BuildingType.NONE;
        buildings = new List<Building>();
    }

    // Start is called before the first frame update
    void Start()
    {
        HideGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager.instance.buildHouseButton.interactable)
        {
            OnBuildHouseClicked();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && GameManager.instance.buildPowerplantButton.interactable)
        {
            OnBuildPowerplantClicked();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && GameManager.instance.buildWallButton.interactable)
        {
            OnBuildWallClicked();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnDemolishClicked();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, GameManager.instance.buildingLayer.value))
        {
            DestructableObject DO = hit.collider.gameObject.GetComponent<DestructableObject>();
            if (prevHighlighted != DO)
            {
                DO.Highlight();
                if (prevHighlighted != null)
                {
                    prevHighlighted.DeHighlight();
                }
                prevHighlighted = DO;
            }
        }
        else if (prevHighlighted != null)
        {
            prevHighlighted.DeHighlight();
            prevHighlighted = null;
        }

        if (currentBuildingType == BuildingType.DEMOLISH)
        {

            if (Input.GetMouseButtonDown(0) && prevHighlighted != null)
            {
                //check if it is possible to destroy it

                if(prevHighlighted.gameObject.GetComponent<House>() != null)
                {

                    if(GameManager.instance.workers - House.workersProvided >= 0)
                    {
                        GameManager.instance.gold += prevHighlighted.gameObject.GetComponent<House>().CalculateValue() / 2;

                        if (prevHighlighted.gameObject == GameManager.instance.selectedBuilding.gameObject)
                        {
                            GameManager.instance.HideBuildingInfo();
                        }

                        Destroy(prevHighlighted.gameObject);
                        prevHighlighted = null;
                    }

                }
                else if(prevHighlighted.gameObject.GetComponent<Powerplant>() != null)
                {

                    if (GameManager.instance.powerSupply - Powerplant.powerSupply >= 0)
                    {
                        
                        GameManager.instance.gold += prevHighlighted.gameObject.GetComponent<Powerplant>().CalculateValue() / 2;

                        if (prevHighlighted.gameObject == GameManager.instance.selectedBuilding.gameObject)
                        {
                            GameManager.instance.HideBuildingInfo();
                        }

                        Destroy(prevHighlighted.gameObject);
                        prevHighlighted = null;
                    }

                }
                else if (prevHighlighted.gameObject.GetComponent<Wall>() != null)
                {
                    GameManager.instance.gold += prevHighlighted.gameObject.GetComponent<Wall>().CalculateValue() / 2;

                    if (prevHighlighted.gameObject == GameManager.instance.selectedBuilding.gameObject)
                    {
                        GameManager.instance.HideBuildingInfo();
                    }

                    Destroy(prevHighlighted.gameObject);
                    prevHighlighted = null;
                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                currentBuildingType = BuildingType.NONE;

                if (prevHighlighted != null)
                {
                    prevHighlighted.DeHighlight();
                    prevHighlighted = null;
                }

                demolishButton.image.color = Color.white;

                GameManager.instance.descriptionPanel.SetActive(false);
                GameManager.instance.demolishDescription.gameObject.SetActive(false);
            }


        }

        if(currentBuildingType == BuildingType.NONE)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (prevHighlighted != null)
                {
                    GameManager.instance.ShowBuildingInfo();
                }
                else
                {
                    GameManager.instance.HideBuildingInfo();
                }
                
            }
        }

    }

    public void OnPlacingCompleted()
    {
        HideGrid();

        if(currentPlacingGO.GetComponent<House>() != null)
        {
            GameManager.instance.gold -= House.cost;
            GameManager.instance.numHouses++;
            GameManager.instance.workers += House.workersProvided;
        }
        else if (currentPlacingGO.GetComponent<Wall>() != null)
        {
            GameManager.instance.gold -= Wall.cost;
        }
        else if (currentPlacingGO.GetComponent<Powerplant>() != null)
        {
            GameManager.instance.gold -= Powerplant.cost;
            GameManager.instance.numPowerplants++;
            GameManager.instance.workers -= Powerplant.workersNeeded;
            GameManager.instance.powerSupply += Powerplant.powerSupply;
        }

        buildings.Add(currentPlacingGO.GetComponent<Building>());

        currentPlacingGO = null;
        currentBuildingType = BuildingType.NONE;

        GameManager.instance.HideDescriptions();
    }

    public void OnPlacingCancelled()
    {
        HideGrid();
        currentPlacingGO = null;
        currentBuildingType = BuildingType.NONE;
        GameManager.instance.HideDescriptions();
    }

    private void OnPlacingStarted()
    {
        ShowGrid();
    }

    public void OnDemolishClicked()
    {
        if(currentBuildingType == BuildingType.DEMOLISH)
        {
            currentBuildingType = BuildingType.NONE;

            if (prevHighlighted != null)
            {
                prevHighlighted.DeHighlight();
                prevHighlighted = null;
            }

            demolishButton.image.color = Color.white;

            GameManager.instance.descriptionPanel.SetActive(false);
            GameManager.instance.demolishDescription.gameObject.SetActive(false);
        }
        else
        {
            if (currentPlacingGO != null)
            {
                Destroy(currentPlacingGO);
            }

            currentBuildingType = BuildingType.DEMOLISH;

            demolishButton.image.color = Color.yellow;

            GameManager.instance.HideDescriptions();
            GameManager.instance.descriptionPanel.SetActive(true);
            GameManager.instance.demolishDescription.gameObject.SetActive(true);
        }


    }

    public void OnBuildHouseClicked()
    {
        if (currentBuildingType == BuildingType.DEMOLISH)
        {
            currentBuildingType = BuildingType.NONE;

            if (prevHighlighted != null)
            {
                prevHighlighted.DeHighlight();
                prevHighlighted = null;
            }

            demolishButton.image.color = Color.white;

        }

        if (currentPlacingGO != null)
        {
            Destroy(currentPlacingGO);
        }


        OnPlacingStarted();
        currentPlacingGO = Instantiate(housePrefab);
        currentBuildingType = BuildingType.HOUSE;

        GameManager.instance.HideDescriptions();
        GameManager.instance.descriptionPanel.SetActive(true);
        GameManager.instance.houseDescription.gameObject.SetActive(true);
    }

    public void OnBuildWallClicked()
    {
        if (currentBuildingType == BuildingType.DEMOLISH)
        {
            currentBuildingType = BuildingType.NONE;

            if (prevHighlighted != null)
            {
                prevHighlighted.DeHighlight();
                prevHighlighted = null;
            }

            demolishButton.image.color = Color.white;

        }

        if (currentPlacingGO != null)
        {
            Destroy(currentPlacingGO);
        }


        OnPlacingStarted();
        currentPlacingGO = Instantiate(wallPrefab);
        currentBuildingType = BuildingType.WALL;

        GameManager.instance.HideDescriptions();
        GameManager.instance.descriptionPanel.SetActive(true);
        GameManager.instance.wallDescription.gameObject.SetActive(true);
    }

    public void OnBuildPowerplantClicked()
    {
        if (currentBuildingType == BuildingType.DEMOLISH)
        {
            currentBuildingType = BuildingType.NONE;

            if (prevHighlighted != null)
            {
                prevHighlighted.DeHighlight();
                prevHighlighted = null;
            }

            demolishButton.image.color = Color.white;

        }

        if (currentPlacingGO != null)
        {
            Destroy(currentPlacingGO);
        }

        OnPlacingStarted();
        currentPlacingGO = Instantiate(powerplantPrefab);
        currentBuildingType = BuildingType.POWERPLANT;

        GameManager.instance.HideDescriptions();
        GameManager.instance.descriptionPanel.SetActive(true);
        GameManager.instance.powerplantDescription.gameObject.SetActive(true);

    }

    private void ShowGrid()
    {
        gridGO.SetActive(true);
    }

    private void HideGrid()
    {
        gridGO.SetActive(false);
    }

}
