using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public int maxEnemyCount = 5;
    public GameObject enemyPrefab;
    public float spawnDelay = 1f;

    public static int curEnemyCount = 0;
    public static bool startSpawning = false;

    [SerializeField] float spawnZone = 10f;



    void Awake()
    {
        if (!enemyPrefab) Debug.Log("Assign an enemy to the EnemySpawner script.");
    }


    void Start()
    {
        startSpawning = true;
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
        curEnemyCount = 0;
    }



    void SpawnEnemy()
    {
        if (!startSpawning) return;

        if (curEnemyCount >= maxEnemyCount) return;

        Vector3 pos = new Vector3(1 * Random.Range(-spawnZone, spawnZone), 
                                  1 * Random.Range(-spawnZone, spawnZone),
                                  1 * Random.Range(-spawnZone, spawnZone));

        Instantiate(enemyPrefab, pos, Quaternion.identity);                 //spawn an enemy somewhere
        curEnemyCount++;                                                    //increase the enemy spawned count
    }

}
