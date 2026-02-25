//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//// 🔹 Add this enum ABOVE the class
//public enum EnemyBehaviorType
//{
//    Normal,     // goes toward Player Goal
//    Aggressive  // chases the Player
//}

//public class EnemyX : MonoBehaviour
//{
//    public float speed;
//    public float speedMultiplier = 1f;

//    private Rigidbody enemyRb;
//    private GameObject playerGoal;
//    private SpawnManagerX spawnManagerXScript;
//    private GameObject player;  // 🔹 for aggressive behavior

//    // 🔹 Choose what this enemy does (set per prefab in Inspector)
//    public EnemyBehaviorType behaviorType = EnemyBehaviorType.Normal;

//    // Start is called before the first frame update
//    void Start()
//    {
//        enemyRb = GetComponent<Rigidbody>();

//        playerGoal = GameObject.Find("Player Goal");
//        spawnManagerXScript = GameObject.Find("Spawn Manager")
//                                        .GetComponent<SpawnManagerX>();

//        // Global difficulty * per-enemy multiplier (Fast = 2, Normal = 1)
//        speed = spawnManagerXScript.enemySpeed * speedMultiplier;

//        // 🔹 We also need the player for Aggressive enemies
//        player = GameObject.Find("Player");
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Vector3 moveDirection = Vector3.zero;

//        // 🔹 Decide behavior based on type
//        switch (behaviorType)
//        {
//            case EnemyBehaviorType.Normal:
//                // Original behavior: move toward player goal
//                moveDirection = (playerGoal.transform.position - transform.position).normalized;
//                break;

//            case EnemyBehaviorType.Aggressive:
//                // New behavior: chase the player directly
//                if (player != null)
//                {
//                    moveDirection = (player.transform.position - transform.position).normalized;
//                }
//                break;
//        }

//        enemyRb.AddForce(moveDirection * speed * Time.deltaTime);
//    }

//    private void OnCollisionEnter(Collision other)
//    {
//        // If enemy collides with either goal, destroy it
//        if (other.gameObject.name == "Enemy Goal")
//        {
//            Destroy(gameObject);
//        }
//        else if (other.gameObject.name == "Player Goal")
//        {
//            Destroy(gameObject);
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Types of behavior an enemy can have
public enum EnemyBehaviorType
{
    Normal,     // goes toward Player Goal
    Aggressive  // chases the Player
}

public class EnemyX : MonoBehaviour
{
    public float speed;
    public float speedMultiplier = 1f;

    private Rigidbody enemyRb;
    private GameObject playerGoal;
    private SpawnManagerX spawnManagerXScript;
    private GameObject player;  // used by Aggressive enemies

    // Choose what this enemy does (set this per prefab in the Inspector)
    public EnemyBehaviorType behaviorType = EnemyBehaviorType.Normal;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        playerGoal = GameObject.Find("Player Goal");
        spawnManagerXScript = GameObject.Find("Spawn Manager")
                                        .GetComponent<SpawnManagerX>();

        // Base speed from SpawnManager * per-enemy multiplier (Fast = 2, Normal = 1)
        speed = spawnManagerXScript.enemySpeed * speedMultiplier;

        // ✅ Get the player reference from SpawnManager instead of Find("Player")
        player = spawnManagerXScript.player;

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
                // Original behavior: move toward player goal
                moveDirection = (playerGoal.transform.position - transform.position).normalized;
                break;

            case EnemyBehaviorType.Aggressive:
                // New behavior: chase the player directly
                if (player != null)
                {
                    moveDirection = (player.transform.position - transform.position).normalized;
                }
                else
                {
                    // If player is somehow missing, do nothing (so we notice it)
                    moveDirection = Vector3.zero;
                }
                break;
        }

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