using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;



    public Transform player;
    public Camera referenceCamera;
    public float spaceBetweenPlatform;
    [Min(50f)] public float distanceToOptimize;

    //Choose chance to spawn enemy
    public List<GameObject> enemies;
    [Range(0f, 100f)] public float enemySpawnChance = 50f;

    //Store all platform
    public List<GameObject> platforms;
    List<GameObject> spawnedObstacles;
    public Vector2 platformScale = new Vector2(1f, 1f);
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

        SetAllPlatformScale(platformScale);

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
        GameObject spawnedPlatform = Instantiate(platforms[Random.Range(0, platforms.Count)], spawnPos, Quaternion.identity);
        spawnedObstacles.Add(spawnedPlatform);
    }

    void SpawnEnemies(Vector3 spawnPos)
    {
        GameObject spawnedEnemy = Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPos, Quaternion.identity);
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

    void SetAllPlatformScale(Vector2 scale)
    {
        foreach (GameObject platform in platforms)
        {
            Vector3 baseScale = platform.transform.localScale;
            platform.transform.localScale = new Vector3(scale.x, scale.y, baseScale.z);
        }
    }
}