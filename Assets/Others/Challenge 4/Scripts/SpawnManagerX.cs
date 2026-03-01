using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject powerupPrefab;

    public GameObject bossPrefab;     
    public int bossWaveInterval = 3;  

    private float spawnRangeX = 10;
    private float spawnZMin = 15;
    private float spawnZMax = 25;

    public int enemyCount;
    public int waveCount = 1;    
    public float enemySpeed = 25;

    public GameObject player;

    // Score tracking
    public int goalsMade = 0;
    public int goalsConceded = 0;
    public int maxGoals = 5;

    public int waveNumber
    {
        get { return waveCount; }
    }

    void Start()
    {
        if (waveCount < 1)
        {
            waveCount = 1;
        }
    }

    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    public void AddGoalMade()
    {
        goalsMade++;
    }

    public void AddGoalConceded()
    {
        goalsConceded++;
    }

    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15);

        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(
                powerupPrefab,
                GenerateSpawnPosition() + powerupSpawnOffset,
                powerupPrefab.transform.rotation
            );
        }

        if (bossPrefab != null &&
            bossWaveInterval > 0 &&
            waveCount >= bossWaveInterval &&
            waveCount % bossWaveInterval == 0)
        {
            Vector3 bossSpawnPos = GenerateSpawnPosition();
            Instantiate(
                bossPrefab,
                bossSpawnPos,
                bossPrefab.transform.rotation
            );

            waveCount++;
            enemySpeed = waveCount * 25;
            ResetPlayerPosition();
            return;
        }

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyIndex = (i == 0) ? 0 : Random.Range(0, enemyPrefab.Length);

            Instantiate(
                enemyPrefab[enemyIndex],
                GenerateSpawnPosition(),
                enemyPrefab[enemyIndex].transform.rotation
            );
        }

        waveCount++;
        enemySpeed = waveCount * 25;
        ResetPlayerPosition();
    }

    void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}