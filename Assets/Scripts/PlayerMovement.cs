using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        sphereCol = GetComponent<SphereCollider>();
    }

    void Update()
    {
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
            jumpPressed = false;
        }
        
        if (smashPressed && !isGrounded)
        {
            Vector3 v = playerRb.linearVelocity;
            v.y = 0f;
            playerRb.linearVelocity = v;
            playerRb.AddForce(Vector3.down * smashForce, ForceMode.Impulse);
            smashPressed = false;
            Invoke("SpawnParticle",0.1f);
        }
    }

    void SpawnParticle()
    {
        GameObject p = Instantiate(particle, transform.position - new Vector3(0,0.2f,0), Quaternion.Euler(90,0,0));
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