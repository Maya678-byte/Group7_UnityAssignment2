using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Flee,
        GoToGoal
    }
    
    public Transform player;
    public Transform goal;
    private Rigidbody rb;

   
    public float moveSpeed = 5f;
    private float idleDrag = 3f;
    private float moveDrag = 0.5f;
    
    public float detectRange = 10f;    
    public float goalReachDistance = 1f;

    public EnemyState currentState = EnemyState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleStateTransitions();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleStateTransitions()
    {
        if (currentState == EnemyState.GoToGoal)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange)
        {
            currentState = EnemyState.Flee;
        }
        else
        {
            currentState = EnemyState.Idle;
        }
    }

    void HandleMovement()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                DoIdle();
                break;

            case EnemyState.Flee:
                DoFlee();
                break;

            case EnemyState.GoToGoal:
                DoGoToGoal();
                break;
        }
    }

    void DoIdle()
    {
        rb.linearDamping = idleDrag;
    }

    void DoFlee()
    {
        rb.linearDamping = moveDrag;

        Vector3 direction = (transform.position - player.position);
        direction.y = 0f;
        direction.Normalize();

        Vector3 targetVel = direction * moveSpeed;
        rb.linearVelocity = new Vector3(targetVel.x, rb.linearVelocity.y, targetVel.z);
    }

    void DoGoToGoal()
    {
        rb.linearDamping = moveDrag;

        Vector3 toGoal = (goal.position - transform.position);
        toGoal.y = 0f;
        float distance = toGoal.magnitude;

        if (distance <= goalReachDistance)
        {
            currentState = EnemyState.Idle;
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        toGoal.Normalize();
        Vector3 targetVel = toGoal * moveSpeed;
        rb.linearVelocity = new Vector3(targetVel.x, rb.linearVelocity.y, targetVel.z);
    }
    public void StartGoingToGoal()
    {
        currentState = EnemyState.GoToGoal;
    }
}