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
    public float defendDistanceFromGoal = 3f;  // how far in front of enemy goal the defender stands

    // Evasive settings
    public float evasiveRadius = 10f;         // how far away the evasive enemy starts to worry
    public float evasiveFleeMultiplier = 2f;  // how much faster it runs when escaping

    // Aggressive charge settings
    public float aggressiveChargeRange = 8f;      // distance at which aggressive enemy will launch
    public float aggressiveChargeImpulse = 25f;   // strength of the launch toward player
    public float aggressiveChargeCooldown = 2f;   // seconds between launches

    private float lastAggressiveChargeTime = -99f;

    private Rigidbody enemyRb;
    private GameObject playerGoal;
    private GameObject enemyGoal;
    private SpawnManagerX spawnManagerXScript;
    private GameObject player;  // used by Aggressive/Defensive/Evasive enemies

    // Choose what this enemy does (set this per prefab in the Inspector)
    public EnemyBehaviorType behaviorType = EnemyBehaviorType.Normal;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        playerGoal = GameObject.Find("Player Goal");
        enemyGoal = GameObject.Find("Enemy Goal");

        spawnManagerXScript = GameObject.Find("Spawn Manager")
                                        .GetComponent<SpawnManagerX>();

        // Base speed from SpawnManager * per-enemy multiplier 
        speed = spawnManagerXScript.enemySpeed * speedMultiplier;

        // Get the player reference from SpawnManager
        player = spawnManagerXScript.player;

        // Auto-detect behavior from name as a safety net
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

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        switch (behaviorType)
        {
            case EnemyBehaviorType.Normal:
                // Original behavior: move toward Player Goal
                if (playerGoal != null)
                {
                    moveDirection = (playerGoal.transform.position - transform.position).normalized;
                }
                break;

            case EnemyBehaviorType.Aggressive:
                // Chases the player directly, with occasional launch
                if (player != null)
                {
                    Vector3 toPlayer = (player.transform.position - transform.position).normalized;
                    moveDirection = toPlayer;

                    float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
                    bool canCharge = Time.time > lastAggressiveChargeTime + aggressiveChargeCooldown;

                    // Launch toward player when close enough and off cooldown
                    if (distToPlayer < aggressiveChargeRange && canCharge)
                    {
                        enemyRb.AddForce(toPlayer * aggressiveChargeImpulse, ForceMode.Impulse);
                        lastAggressiveChargeTime = Time.time;
                    }
                }
                else
                {
                    moveDirection = Vector3.zero;
                }
                break;

            case EnemyBehaviorType.Defensive:
                // Guard its own goal (Enemy Goal)
                if (player != null && enemyGoal != null)
                {
                    // Track player's X but stand in front of Enemy Goal
                    float targetX = player.transform.position.x;

                    // Stand a bit in front of Enemy Goal, toward the center of the field
                    float goalZ = enemyGoal.transform.position.z - defendDistanceFromGoal;

                    Vector3 defendPosition = new Vector3(targetX, transform.position.y, goalZ);
                    moveDirection = (defendPosition - transform.position).normalized;
                }
                else if (enemyGoal != null)
                {
                    // If no player reference, just move back to its own goal to defend
                    moveDirection = (enemyGoal.transform.position - transform.position).normalized;
                }
                break;

            case EnemyBehaviorType.Evasive:
                if (player != null)
                {
                    float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

                    if (distToPlayer < evasiveRadius)
                    {
                        // Very clearly run away from the player, faster than normal
                        Vector3 awayFromPlayer = (transform.position - player.transform.position).normalized;

                        //Does a little sideways strafe so it doesn't just go in a straight line
                        Vector3 sideStep = Vector3.Cross(awayFromPlayer, Vector3.up).normalized * 0.5f;

                        // Strong flee direction
                        moveDirection = (awayFromPlayer * evasiveFleeMultiplier + sideStep).normalized;
                    }
                    else
                    {
                        // When far away, just move slowly toward the goal instead of fully committing
                        if (playerGoal != null)
                        {
                            Vector3 towardGoal = (playerGoal.transform.position - transform.position).normalized;
                            moveDirection = towardGoal * 0.3f; // 30% of normal intensity
                        }
                    }
                }
                else
                {
                    // If no player reference, just do nothing 
                    moveDirection = Vector3.zero;
                }
                break;
        }

        // Apply continuous movement
        enemyRb.AddForce(moveDirection * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }
    }
}