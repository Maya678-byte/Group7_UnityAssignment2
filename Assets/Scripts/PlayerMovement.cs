using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    private Rigidbody playerRb;
    public GameObject camera;

    public float speed = 5f;
    public float jumpForce = 7f;
    public float smashForce = 20f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool jumpPressed;
    private bool smashPressed;

    private SphereCollider sphereCol;
    public GameObject particle;
    
    public float knockbackRadius = 2f;    
    public float maxKnockbackForce = 20f;
    public LayerMask enemyLayer;          
    
    private bool wasGrounded;
    private bool isSmashing;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // Destroy duplicate instances
            return;
        }
        Instance = this; // Set the instance to this object
    }
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        sphereCol = GetComponent<SphereCollider>();
        ValueSingleton.Instance.wave = 0;
        ValueSingleton.Instance.ballsOut = 0;
        ValueSingleton.Instance.health = 3;
        ValueSingleton.Instance.isBoost = false;
        ValueSingleton.Instance.isSlow = false;
        AudioSingleton.Instance.PlayNormal();
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                jumpPressed = true;
            }
            else
            {
                smashPressed = true;
            }
        }
    }

    void FixedUpdate()
    {
        float rayLength = sphereCol != null ? sphereCol.radius * transform.localScale.y + 0.1f : 1.1f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayer);
        
        if (!wasGrounded && isGrounded && isSmashing)
        {
            Smash();
            isSmashing = false;
        }

        if (ValueSingleton.Instance.isBoost)
        {
            speed = 7f;
        }
        else
        {
            speed = 5f;
        }

        wasGrounded = isGrounded;
        
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 forward = camera.transform.transform.forward;
        Vector3 right = camera.transform.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
        
        Vector3 currentVel = playerRb.linearVelocity;
        Vector3 targetVel = new Vector3(moveDirection.x * speed, currentVel.y, moveDirection.z * speed);
        playerRb.linearVelocity = targetVel;
        
        if (jumpPressed && isGrounded)
        {
            Vector3 v = playerRb.linearVelocity;
            v.y = 0f;
            playerRb.linearVelocity = v;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            AudioSingleton.Instance.PlayJump();
            jumpPressed = false;
        }
        
        if (smashPressed && !isGrounded)
        {
            Vector3 v = playerRb.linearVelocity;
            v.y = 0f;
            playerRb.linearVelocity = v;

            playerRb.AddForce(Vector3.down * smashForce, ForceMode.Impulse);

            smashPressed = false;
            isSmashing = true;   // mark that we are in smash mode
        }
    }

    void Smash()
    {
        AudioSingleton.Instance.PlaySmash();
        GameObject p = Instantiate(particle, transform.position - new Vector3(0,0.2f,0), Quaternion.Euler(90,0,0));
        KnockbackEnemies();
    }


    public void KnockbackEnemies()
    {
        Vector3 center = transform.position;

        // Find all colliders in a sphere around the player
        Collider[] hits = Physics.OverlapSphere(center, knockbackRadius, enemyLayer);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector3 dir = hit.transform.position - transform.position;
                float distance = dir.magnitude;
                dir.Normalize();
                float distanceFactor = 1f - Mathf.Clamp01(distance / knockbackRadius);
                float force = maxKnockbackForce * distanceFactor;
                enemy.ApplyKnockback(dir * force, 1f);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (sphereCol != null)
        {
            float rayLength = sphereCol.radius * transform.localScale.y + 0.1f;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
        }
    }
}