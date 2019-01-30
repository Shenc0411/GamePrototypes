using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject explosionFX;
    public GameObject fireFX;

    public GameObject coreGO;

    public GameObject bulletPrefab;

    public TextMeshProUGUI waveCounter;
    public TextMeshProUGUI waveTimerUI;
    public TextMeshProUGUI goldCounter;
    public TextMeshProUGUI workerCounter;
    public TextMeshProUGUI powerCounter;

    public GameObject buildingInfoPanel;
    public TextMeshProUGUI buildingInfoText;
    public Button repairButton;
    public Button upgradeButton;

    public Building selectedBuilding;

    public GameObject descriptionPanel;

    public GameObject losePanel;

    public TextMeshProUGUI houseDescription;
    public TextMeshProUGUI wallDescription;
    public TextMeshProUGUI powerplantDescription;
    public TextMeshProUGUI demolishDescription;

    public Button buildHouseButton;
    public Button buildWallButton;
    public Button buildPowerplantButton;

    public LayerMask groundLayer;
    public LayerMask buildingLayer;
    public LayerMask enemyLayer;

    public int initialGold = 500;

    private float incomeTimer;

    public int gold;
    public int workers;
    public int powerSupply;

    public int wave;

    public GameObject enemyPrefab;
    public float initialWaveInterval;
    public float waveTimer;
    public int initialEnemyPerWave;
    public int enemysLeft;
    public HashSet<Enemy> enemys;


    public int numHouses;

    public int numPowerplants;



    private void Awake()
    {

        wave = 1;

        instance = this;
        waveTimer = initialWaveInterval;

        gold = 500;
        incomeTimer = 1.0f;

        houseDescription.text = "House: \ncosts " + House.cost + " golds; provides " + 
            House.goldPerSecond + " golds per second; provides " + House.workersProvided + " workers"; 

        wallDescription.text = "Wall: \ncosts " + Wall.cost + " golds; costs " + Wall._upgradeCost + " golds, " + Wall._upgradePowerCost + " power supply to be upgraded to a turret";

        powerplantDescription.text = "Powerplant: \ncosts " + Powerplant.cost + " golds; provides " +
            Powerplant.powerSupply + " power supply; needs " + Powerplant.workersNeeded + " workers";
        
        demolishDescription.text = "Demolish:\n" + "Demolish a building; gets 50% of its current value";

        losePanel.SetActive(false);

        enemys = new HashSet<Enemy>();

        HideDescriptions();
        HideBuildingInfo();
    }

    // Start is called before the first frame update
    void Start()
    {
        BuildManager.instance.buildings.Add(coreGO.GetComponent<Building>());
    }

    // Update is called once per frame
    void Update()
    {
        //Handle Enemy Spawning
        waveTimer -= Time.deltaTime;
        if(waveTimer <= 0)
        {
            waveTimer = initialWaveInterval;
            SpawnEnemyWave();
            wave++;
        }

        //Update Info
        incomeTimer -= Time.deltaTime;
        if(incomeTimer <= 0)
        {
            gold += numHouses * House.goldPerSecond;
            incomeTimer = 1.0f;
        }
        

        UpdateUI();

        if(selectedBuilding != null)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RepairBuilding();
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                UpgradeBuilding();
            }
        }

        
    }


    public void UpdateUI()
    {
        waveCounter.text = "Waves: " + wave;
        goldCounter.text = "Gold: " + (int)gold;
        workerCounter.text = "Available Workers: " + workers;
        powerCounter.text = "PowerSupply: " + powerSupply;
        waveTimerUI.text = "Next Wave in " + (int)waveTimer + " secs";

        buildHouseButton.interactable = gold >= House.cost;

        buildWallButton.interactable = gold >= Wall.cost;

        buildPowerplantButton.interactable = gold >= Powerplant.cost && workers >= Powerplant.workersNeeded;

        if(selectedBuilding != null)
        {
            UpdateBuildingInfo();
        }

    }

    private void SpawnEnemyWave()
    {

        float angle = UnityEngine.Random.Range(0, Mathf.PI);
        float distance = UnityEngine.Random.Range(Enemy.minDistance, Enemy.maxDistance);
        Vector3 spawnPoint = coreGO.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;

        int numEnemey = (int)(initialEnemyPerWave * Mathf.Pow(wave, 1.2f));

        Debug.Log(numEnemey);

        for(int i = 0; i < numEnemey; i++)
        {
            GameObject GO = Instantiate(enemyPrefab);
            GO.transform.position = spawnPoint + new Vector3(UnityEngine.Random.Range(0.0f, 5.0f * numEnemey / initialEnemyPerWave / 3.0f), 0, UnityEngine.Random.Range(0.0f, 5.0f * numEnemey / initialEnemyPerWave / 3.0f));
            enemys.Add(GO.GetComponent<Enemy>());
        }

    }

    public void HideDescriptions()
    {
        descriptionPanel.SetActive(false);
        houseDescription.gameObject.SetActive(false);
        wallDescription.gameObject.SetActive(false);
        powerplantDescription.gameObject.SetActive(false);
        demolishDescription.gameObject.SetActive(false);
    }

    public void HideBuildingInfo()
    {
        buildingInfoPanel.SetActive(false);
        selectedBuilding = null;
    }

    public void ShowBuildingInfo()
    {
        DestructableObject DO = BuildManager.instance.prevHighlighted;
        if (DO != null)
        {
            GameObject GO = DO.gameObject;

            selectedBuilding = GO.GetComponent<Building>();

            UpdateBuildingInfo();

            buildingInfoPanel.transform.position = Input.mousePosition + new Vector3(buildingInfoPanel.GetComponent<RectTransform>().rect.width / 2.0f + 20, buildingInfoPanel.GetComponent<RectTransform>().rect.height / 2.0f + 20);
            buildingInfoPanel.SetActive(true);
        }

        
    }

    private void UpdateBuildingInfo()
    {
        buildingInfoText.text = selectedBuilding.GetTypeName() + "\nHealth: " + selectedBuilding.health + " / " + selectedBuilding.maxHealth;
        if (selectedBuilding.repairable)
        {
            buildingInfoText.text += "\nRepair Cost: " + selectedBuilding.CalculateRepairCost();
        }
        else
        {
            buildingInfoText.text += "\nCannot be repaired";
        }
        
        bool canRepair = selectedBuilding.repairable && selectedBuilding.health < selectedBuilding.maxHealth;
        bool canUpgrade = selectedBuilding.upgradable && gold >= selectedBuilding.upgradeCost && powerSupply >= selectedBuilding.upgradePowerCost && workers >= selectedBuilding.upgradeWorkerCost;
        upgradeButton.interactable = canUpgrade;
        repairButton.interactable = canRepair;

    }

    public void RepairBuilding()
    {
        bool canRepair = selectedBuilding.health < selectedBuilding.maxHealth;
        if (selectedBuilding != null && canRepair)
        {
            gold -= selectedBuilding.CalculateRepairCost();
            selectedBuilding.OnRepaired();
        }
    }

    public void UpgradeBuilding()
    {
        bool canUpgrade = selectedBuilding.upgradable && gold >= selectedBuilding.upgradeCost && powerSupply >= selectedBuilding.upgradePowerCost && workers >= selectedBuilding.upgradeWorkerCost;
        if (selectedBuilding != null && canUpgrade)
        {
            gold -= selectedBuilding.upgradeCost;
            powerSupply -= selectedBuilding.upgradePowerCost;
            workers -= selectedBuilding.upgradeWorkerCost;

            selectedBuilding.OnUpgrade();
        }
    }


    public void OnLose()
    {
        losePanel.SetActive(true);
    }
}
