using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;



    public Transform player;
    public Camera referenceCamera;
    public float spaceBetweenPlatform;
    [Min(50f)] public float distanceToOptimize;

    [Header("Enemy")]
    public List<GameObject> enemies;
    public Vector2 enemyScale = new Vector2(1f,1f);
    [Range(0f, 100f)] public float enemySpawnChance = 50f;

    [Header("Platform")]
    public List<GameObject> platforms;
    public int safeSpawnAmount = 3;
    int safeSpawnCount = 0;
    List<GameObject> spawnedObstacles;
    public Vector2 platformScale = new Vector2(1f, 1f);
    public float scaleVariance = 1f;
    Vector3 lastObstacleSpawnPos;


    //Spawn Range
    int screenWidth;
    int screenHeight;
    [HideInInspector] public float minSpawnRange;
    [HideInInspector] public float maxSpawnRange;




    public bool isStart;


    private void Start()
    {
        if (instance) Debug.LogWarning("There is more than 1 instance of Platform Spawn Manager in the scene! REMOVE THE EXTRA");
        instance = this;

        
        

        spawnedObstacles = new List<GameObject>();
        lastObstacleSpawnPos = Vector3.zero;


        SetScreenBoundary();


    }

    void SetScreenBoundary()
    {
        //Set screen boundary
        screenWidth = instance.referenceCamera.pixelWidth;
        screenHeight = instance.referenceCamera.pixelHeight;

        minSpawnRange = (float)((screenWidth - screenHeight) / 2) / screenWidth;
        maxSpawnRange = (float)(screenWidth - (screenWidth - screenHeight) / 2) / screenWidth;
    }

    private void Update()
    {
        if (isStart)
        {
            SpawnObstacle();
            OptimizeObstacle(spawnedObstacles);
        }
    }


    void SpawnObstacle()
    {
        //Spawn if space between low screen and last obstacle exceed range
        if (instance.referenceCamera.ViewportToWorldPoint(new Vector3(0, 0)).y - lastObstacleSpawnPos.y <= -spaceBetweenPlatform)
        {



            Vector3 spawnPos = new Vector3(instance.referenceCamera.ViewportToWorldPoint(new Vector3(Random.Range(minSpawnRange, maxSpawnRange), 0)).x,
                lastObstacleSpawnPos.y - spaceBetweenPlatform,
                0);


            //Check if spawn enemy or not.
            if (Random.Range(0f, 100f) <= enemySpawnChance)
            {
                spawnPos = new Vector3(instance.referenceCamera.ViewportToWorldPoint(new Vector3(0.5f, 0)).x,
                lastObstacleSpawnPos.y - spaceBetweenPlatform,
                0);
                SpawnEnemies(spawnPos);
            }
            else
            {
                SpawnPlatform(spawnPos);
            }


            lastObstacleSpawnPos = spawnPos;
        }
    }

    void SpawnPlatform(Vector3 spawnPos)
    {
        if (safeSpawnCount % safeSpawnAmount == 0 && safeSpawnCount != 0)
        {
            GameObject spawnedPlatform = Instantiate(platforms[Random.Range(1, platforms.Count)], spawnPos, Quaternion.identity);
            SetAllPlatformScale(platformScale,spawnedPlatform);
            safeSpawnCount = 0;
            spawnedObstacles.Add(RandomScale(scaleVariance,spawnedPlatform));
        }
        else
        {
            GameObject spawnedPlatform = Instantiate(platforms[0], spawnPos, Quaternion.identity);
            SetAllPlatformScale(platformScale,spawnedPlatform);
            spawnedObstacles.Add(RandomScale(scaleVariance,spawnedPlatform));
        }

        safeSpawnCount += Random.Range(0, 3);

    }

    GameObject RandomScale(float scaleVariance, GameObject gameObject)
    {
        GameObject scaledObject = gameObject;
        float variance = Random.Range(-scaleVariance,scaleVariance);

        scaledObject.transform.localScale = new Vector3(
            scaledObject.transform.localScale.x + variance,
            scaledObject.transform.localScale.y,
            scaledObject.transform.localScale.z
            );

            return scaledObject;
    }


    void SpawnEnemies(Vector3 spawnPos)
    {
        GameObject spawnedEnemy = Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPos, Quaternion.identity);
        SetAllEnemiesScale(enemyScale,spawnedEnemy);
        spawnedObstacles.Add(spawnedEnemy);
    }


    //Set active to false if player is too far
    void OptimizeObstacle(List<GameObject> list)
    {
        foreach (GameObject spawned in list)
        {
            if (Vector3.Distance(player.transform.position, spawned.transform.position) >= distanceToOptimize && spawned.activeInHierarchy)
            {
                spawned.SetActive(false);
            }
            else if (Vector3.Distance(player.transform.position, spawned.transform.position) < distanceToOptimize && !spawned.activeInHierarchy)
            {
                spawned.SetActive(true);
            }
        }
    }

    void SetAllPlatformScale(Vector2 scale,GameObject gameObject)
    {
            Sprite sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            float baseScaleZ = gameObject.transform.localScale.z;

            gameObject.transform.localScale = new Vector3(scale.x / sprite.bounds.size.x, scale.y, baseScaleZ);
        
    }


    void SetAllEnemiesScale(Vector2 scale,GameObject gameObject)
    {
            float baseScaleZ = gameObject.transform.localScale.z;

            gameObject.transform.localScale = new Vector3(scale.x, scale.y, baseScaleZ);
        
    }
}
