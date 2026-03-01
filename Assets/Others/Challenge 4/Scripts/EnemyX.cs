using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Types of behavior an enemy can have
public enum EnemyBehaviorType
{
    Normal,      // goes toward Player Goal
    Aggressive,  // chases the Player and can launch at them
    Defensive,   // guards in front of Enemy Goal
    Evasive      // runs away from player when close
}

public class EnemyX : MonoBehaviour
{
    public float speed;
    public float speedMultiplier = 1f;

    // Defensive settings
    public float defendDistanceFromGoal = 3f;  

    // Evasive settings
    public float evasiveRadius = 10f;        
    public float evasiveFleeMultiplier = 2f; 

    // Aggressive charge settings
    public float aggressiveChargeRange = 8f;     
    public float aggressiveChargeImpulse = 25f;  
    public float aggressiveChargeCooldown = 2f;  

    // Added telegraph delay and aim randomness for smarter aggressive behavior
    public float aggressiveChargeDelay = 0.5f;      
    public float aggressiveAimRandomness = 0.5f;    

    private float lastAggressiveChargeTime = -99f;
    private bool isCharging = false;  // prevents overlapping charge routines

    private Rigidbody enemyRb;
    private GameObject playerGoal;
    private GameObject enemyGoal;
    private SpawnManagerX spawnManagerXScript;
    private GameObject player;  

    // Choose what this enemy does (set this per prefab in the Inspector)
    public EnemyBehaviorType behaviorType = EnemyBehaviorType.Normal;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        playerGoal = GameObject.Find("Player Goal");
        enemyGoal = GameObject.Find("Enemy Goal");

        spawnManagerXScript = GameObject.Find("Spawn Manager")
                                        .GetComponent<SpawnManagerX>();

        speed = spawnManagerXScript.enemySpeed * speedMultiplier;

        player = spawnManagerXScript.player;

        string n = gameObject.name;
        if (n.Contains("Aggressive"))
        {
            behaviorType = EnemyBehaviorType.Aggressive;
        }
        else if (n.Contains("Defensive"))
        {
            behaviorType = EnemyBehaviorType.Defensive;
        }
        else if (n.Contains("Evasive"))
        {
            behaviorType = EnemyBehaviorType.Evasive;
        }

        Debug.Log($"{gameObject.name} started with behaviorType = {behaviorType}, " +
                  $"player ref = {(player != null ? player.name : "NULL")}");
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        switch (behaviorType)
        {
            case EnemyBehaviorType.Normal:
                if (playerGoal != null)
                {
                    moveDirection = (playerGoal.transform.position - transform.position).normalized;
                }
                break;

            case EnemyBehaviorType.Aggressive:
                if (player != null)
                {
                    Vector3 toPlayer = (player.transform.position - transform.position).normalized;
                    moveDirection = toPlayer;

                    float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
                    bool canCharge = Time.time > lastAggressiveChargeTime + aggressiveChargeCooldown;

                    // Start smarter charge routine instead of instant impulse
                    if (distToPlayer < aggressiveChargeRange && canCharge && !isCharging)
                    {
                        StartCoroutine(AggressiveChargeRoutine(toPlayer));
                        lastAggressiveChargeTime = Time.time;
                    }
                }
                else
                {
                    moveDirection = Vector3.zero;
                }
                break;

            case EnemyBehaviorType.Defensive:
                if (player != null && enemyGoal != null)
                {
                    float targetX = player.transform.position.x;
                    float goalZ = enemyGoal.transform.position.z - defendDistanceFromGoal;

                    Vector3 defendPosition = new Vector3(targetX, transform.position.y, goalZ);
                    moveDirection = (defendPosition - transform.position).normalized;
                }
                else if (enemyGoal != null)
                {
                    moveDirection = (enemyGoal.transform.position - transform.position).normalized;
                }
                break;

            case EnemyBehaviorType.Evasive:
                if (player != null)
                {
                    float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

                    if (distToPlayer < evasiveRadius)
                    {
                        Vector3 awayFromPlayer = (transform.position - player.transform.position).normalized;
                        Vector3 sideStep = Vector3.Cross(awayFromPlayer, Vector3.up).normalized * 0.5f;
                        moveDirection = (awayFromPlayer * evasiveFleeMultiplier + sideStep).normalized;
                    }
                    else
                    {
                        if (playerGoal != null)
                        {
                            Vector3 towardGoal = (playerGoal.transform.position - transform.position).normalized;
                            moveDirection = towardGoal * 0.3f;
                        }
                    }
                }
                else
                {
                    moveDirection = Vector3.zero;
                }
                break;
        }

        enemyRb.AddForce(moveDirection * speed * Time.deltaTime);
    }

    // Coroutine adds delay and slight inaccuracy before aggressive charge
    IEnumerator AggressiveChargeRoutine(Vector3 baseDirection)
    {
        isCharging = true;

        yield return new WaitForSeconds(aggressiveChargeDelay);

        float randomOffsetX = Random.Range(-aggressiveAimRandomness, aggressiveAimRandomness);
        float randomOffsetZ = Random.Range(-aggressiveAimRandomness, aggressiveAimRandomness);

        Vector3 adjustedDirection = (baseDirection + new Vector3(randomOffsetX, 0, randomOffsetZ)).normalized;

        enemyRb.AddForce(adjustedDirection * aggressiveChargeImpulse, ForceMode.Impulse);

        isCharging = false;
    }

    private void OnCollisionEnter(Collision other)
{
    SpawnManagerX manager = GameObject.Find("Spawn Manager")
                                       .GetComponent<SpawnManagerX>();

    if (other.gameObject.name == "Enemy Goal")
    {
        manager.AddGoalMade();   // Player scored
        Destroy(gameObject);
    }
    else if (other.gameObject.name == "Player Goal")
    {
        manager.AddGoalConceded();  // Enemy scored
        Destroy(gameObject);
    }
}
}