using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject powerupPrefab;

    public GameObject bossPrefab;     // assign BossEnemy prefab
    public int bossWaveInterval = 3;  // boss appears every 3 waves 

    private float spawnRangeX = 10;
    private float spawnZMin = 15; // set min spawn Z
    private float spawnZMax = 25; // set max spawn Z

    public int enemyCount;
    public int waveCount = 1;    // starting wave logical index
    public float enemySpeed = 25;

    public GameObject player;

    // Ensure waveCount never starts at 0 from Inspector override
    // First wave would start during runtime and we don't want that
    void Start()
    {
        if (waveCount < 1)
        {
            waveCount = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    // Generate random spawn position for powerups and enemy balls
    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15); // make powerups spawn at player end

        // If no powerups remain, spawn a powerup
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(
                powerupPrefab,
                GenerateSpawnPosition() + powerupSpawnOffset,
                powerupPrefab.transform.rotation
            );
        }

        // Boss wave check
        // Only spawn boss if:
        // we have a bossPrefab
        // interval is valid
        // and we've reached at least that wave (so no boss on wave 1/2)
        if (bossPrefab != null &&
            bossWaveInterval > 0 &&
            waveCount >= bossWaveInterval &&
            waveCount % bossWaveInterval == 0)
        {
            Debug.Log($"BOSS WAVE! waveCount = {waveCount}");

            // Choose boss spawn position 
            Vector3 bossSpawnPos = GenerateSpawnPosition();
            Instantiate(
                bossPrefab,
                bossSpawnPos,
                bossPrefab.transform.rotation
            );

            waveCount++;
            enemySpeed = waveCount * 25;
            ResetPlayerPosition(); // put player back at start
            return; // skip spawning regular enemies this wave
        }

        // Normal enemy spawning
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyIndex;

            if (i == 0)
            {
                // Always spawn one normal enemy
                enemyIndex = 0;
            }
            else
            {
                enemyIndex = Random.Range(0, enemyPrefab.Length);
            }

            Instantiate(
                enemyPrefab[enemyIndex],
                GenerateSpawnPosition(),
                enemyPrefab[enemyIndex].transform.rotation
            );
        }

        waveCount++;
        enemySpeed = waveCount * 25;
        ResetPlayerPosition(); // put player back at start
    }

    // Move player back to position in front of own goal
    void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}