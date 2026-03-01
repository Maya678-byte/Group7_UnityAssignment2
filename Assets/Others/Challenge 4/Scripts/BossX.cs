using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyX : MonoBehaviour
{
    [Header("Movement")]
    public float speedMultiplier = 1.5f;   // The boss is a bit faster than normal enemies
    private float speed;
    private Rigidbody bossRb;
    private GameObject playerGoal;
    private SpawnManagerX spawnManagerXScript;

    [Header("Minion Summoning")]
    public GameObject miniEnemyPrefab;     // Assign a mini enemy prefab 
    public float summonInterval = 5f;      // Seconds between summon attempts
    public int maxActiveMinions = 4;       // Max 4 enemies active on field so it doesn't flood it 
    public float summonRadius = 3f;        // How far from boss to spawn minions

    private float lastSummonTime = -99f;
    private List<GameObject> activeMinions = new List<GameObject>();

    void Start()
    {
        bossRb = GetComponent<Rigidbody>();

        playerGoal = GameObject.Find("Player Goal");
        spawnManagerXScript = GameObject.Find("Spawn Manager")
                                        .GetComponent<SpawnManagerX>();

        // Use the same base enemySpeed but scaled for the boss
        speed = spawnManagerXScript.enemySpeed * speedMultiplier;
    }

    void Update()
    {
        MoveTowardPlayerGoal();
        HandleSummoning();
    }

    private void MoveTowardPlayerGoal()
    {
        if (playerGoal == null) return;

        Vector3 dir = (playerGoal.transform.position - transform.position).normalized;
        bossRb.AddForce(dir * speed * Time.deltaTime);
    }

    private void HandleSummoning()
    {
        // Clean out dead minions from the list
        activeMinions.RemoveAll(minion => minion == null);

        // Only summon if we have room and cooldown is done
        if (miniEnemyPrefab == null) return;
        if (activeMinions.Count >= maxActiveMinions) return;

        if (Time.time > lastSummonTime + summonInterval)
        {
            // Random position in a circle around the boss
            Vector2 offset2D = Random.insideUnitCircle * summonRadius;
            Vector3 spawnPos = new Vector3(
                transform.position.x + offset2D.x,
                transform.position.y,
                transform.position.z + offset2D.y
            );

            GameObject minion = Instantiate(
                miniEnemyPrefab,
                spawnPos,
                miniEnemyPrefab.transform.rotation
            );

            activeMinions.Add(minion);
            lastSummonTime = Time.time;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // If boss collides with either goal, destroy it just like other enemies
        if (other.gameObject.name == "Enemy Goal" ||
            other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }
    }
}
