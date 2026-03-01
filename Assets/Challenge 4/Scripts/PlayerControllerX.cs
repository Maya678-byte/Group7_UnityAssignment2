using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10;
    private float powerupStrength = 25;

    private float turboBoost = 10;
    public ParticleSystem turboSmoke;

    // Smash ability variables
    [SerializeField] private float smashDownForce = 30f;
    [SerializeField] private float smashRadius = 5f;
    [SerializeField] private float smashImpactForce = 20f;
    private bool isSmashing = false;

    // Health system variables
    public int maxHealth = 3;
    public int currentHealth;
    public float damageCooldown = 1f;
    private float lastDamageTime = -99f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        // Initialize player health
        currentHealth = maxHealth;
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(focalPoint.transform.forward * turboBoost, ForceMode.Impulse);
            turboSmoke.Play();
        }

        // Trigger smash when pressing F while powerup is active
        if (Input.GetKeyDown(KeyCode.F) && hasPowerup)
        {
            StartSmash();
        }
    }

    // Applies downward force to initiate smash
    void StartSmash()
    {
        isSmashing = true;
        playerRb.AddForce(Vector3.down * smashDownForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        // When smashing and hitting the ground, apply area impact
        if (isSmashing && other.gameObject.CompareTag("Ground"))
        {
            SmashImpact();
            isSmashing = false;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            if (hasPowerup)
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }

            // Apply damage with cooldown protection
            if (Time.time > lastDamageTime + damageCooldown)
            {
                currentHealth--;
                lastDamageTime = Time.time;
            }
        }
    }

    // Pushes nearby enemies outward based on distance from player
    void SmashImpact()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, smashRadius);

        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                float forceMultiplier = 1 - (distance / smashRadius);

                if (forceMultiplier > 0)
                {
                    Vector3 direction = (enemy.transform.position - transform.position).normalized;
                    enemyRb.AddForce(direction * smashImpactForce * forceMultiplier, ForceMode.Impulse);
                }
            }
        }
    }
}