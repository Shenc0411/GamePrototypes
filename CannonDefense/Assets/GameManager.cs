using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject enemyPartsParent;

    public LayerMask enemyLayerMask;
    public LayerMask loseLayerMask;

    public int enemyLayer;
    public int loseLayer;

    public GameObject cannonGO;

    public GameObject enemyPrefab;

    public float enemyMaxDistance;
    public float enemyMinDistance;

    public float enemySpawnInterval;
    private float enemySpawnTimer;
    public int enemyPerInterval;

    public AudioClip collisionSFX;
    private AudioSource AS;


    public bool enableDissolveShader;

    public bool enableParticleEffect;

    public bool enableSoundEffect;

    public bool enableSmoothCameraFollow;

    public bool enableAllowEnemyParts;

    public bool enableCameraShake;

    void Awake()
    {
        instance = this;

        enemyLayer = (int)Mathf.Log(enemyLayerMask.value, 2);
        loseLayer = (int)Mathf.Log(loseLayerMask.value, 2);

        AS = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemys(enemyPerInterval);
    }

    // Update is called once per frame
    void Update()
    {

        if (!enableAllowEnemyParts)
        {
            int n = enemyPartsParent.transform.childCount;

            for(int i = 0; i < n; i++)
            {
                enemyPartsParent.transform.GetChild(i).GetComponent<AutoDestroy>().Destroy();
            }
        }

        enemySpawnTimer += Time.deltaTime;
        if(enemySpawnTimer >= enemySpawnInterval)
        {
            SpawnEnemys(enemyPerInterval);
            enemySpawnTimer = 0;
        }

    }


    private void SpawnEnemys(int num)
    {

        for(int i = 0; i < num; i++)
        {
            SpawnEnemy();
        }

    }

    private void SpawnEnemy()
    {

        float angle = Mathf.Deg2Rad * Random.Range(0, 360);

        Vector3 unitCirclePos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        Vector3 position = unitCirclePos * Random.Range(enemyMinDistance, enemyMaxDistance);
        position.y = -7;

        Quaternion rotation = Quaternion.LookRotation(cannonGO.transform.position - position);

        GameObject enemyGO = Instantiate(enemyPrefab, position, rotation);

        enemyGO.GetComponent<NavMeshAgent>().SetDestination(cannonGO.transform.position);
    }

    public void PlayCollisionSFX(float volume)
    {
        AS.PlayOneShot(collisionSFX, volume);
    }

}
