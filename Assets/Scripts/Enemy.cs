using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        Evasive,
        Aggressive,
        Done
    }

    public EnemyType enemyType;

    private Transform player;
    private Transform goal;
    private Rigidbody rb;

    public float moveSpeed = 5f;
    private float idleDrag = 3f;
    private float moveDrag = 0.5f;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    public float detectRange = 8f;
    public float goalReachDistance = 1f;

    public float aggressiveChargeRange = 8f;      
    public float aggressiveChargeImpulse = 35f;   
    public float aggressiveChargeCooldown = 2f;   
    private float lastAggressiveChargeTime = -99f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        goal = Goal.Instance.transform;
        player = PlayerMovement.Instance.transform;
    }

    void FixedUpdate()
    {
        if (enemyType == EnemyType.Done)
        {
            return;
        }

        if (ValueSingleton.Instance.health <= 0)
        {
            enemyType = EnemyType.Done;
        }
        
        Collider[] hits = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Out"));

        foreach (Collider hit in hits)
        {
            enemyType = EnemyType.Done;
            ValueSingleton.Instance.ballsOut += 1;
        }
        
        Collider[] s = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Goal"));

        foreach (Collider hit in s)
        {
            enemyType = EnemyType.Done;
            ValueSingleton.Instance.health -= 1;
            ValueSingleton.Instance.ballsOut += 1;
        }

        if (ValueSingleton.Instance.isSlow)
        {
            moveSpeed = 1f;
        }
        else
        {
            moveSpeed = ValueSingleton.Instance.wave * 0.5f + 2f;
        }

        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;

            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
            
            return;
        }
        
        switch (enemyType)
        {
            case EnemyType.Normal:
                DoNormal();
                break;

            case EnemyType.Evasive:
                DoEvasive();
                break;

            case EnemyType.Aggressive:
                DoAggressive();
                break;
        }
    }
    
    void DoNormal()
    {
        rb.linearDamping = moveDrag;

        Vector3 toGoal = (goal.position - transform.position);
        toGoal.y = 0f;

        if (toGoal.magnitude <= goalReachDistance)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        toGoal.Normalize();
        Vector3 targetVel = toGoal * moveSpeed;

        rb.linearVelocity = new Vector3(targetVel.x, rb.linearVelocity.y, targetVel.z);
    }

    void DoEvasive()
    {
        rb.linearDamping = moveDrag;

        Vector3 toGoal = (goal.position - transform.position);
        Vector3 toPlayer = (player.position - transform.position);

        toGoal.y = 0f;
        toPlayer.y = 0f;

        Vector3 finalDirection = toGoal.normalized;

        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer < detectRange)
        {
            Vector3 avoidDir = -toPlayer.normalized;

            float avoidStrength = 1f - (distanceToPlayer / detectRange);

            finalDirection = (toGoal.normalized + avoidDir * 0.7f * avoidStrength).normalized;
        }

        Vector3 targetVel = finalDirection * moveSpeed;

        rb.linearVelocity = new Vector3(targetVel.x, rb.linearVelocity.y, targetVel.z);
    }
    
    void DoAggressive()
    {
        rb.linearDamping = moveDrag;

        Vector3 toPlayer = (player.position - transform.position);
        toPlayer.y = 0f;

        float distanceToPlayer = toPlayer.magnitude;

        if (toPlayer.magnitude < 0.5f)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        toPlayer.Normalize();

        bool canCharge = Time.time > lastAggressiveChargeTime + aggressiveChargeCooldown;

        if (distanceToPlayer < aggressiveChargeRange && canCharge)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            rb.AddForce(toPlayer * aggressiveChargeImpulse, ForceMode.Impulse);
            lastAggressiveChargeTime = Time.time;

            return; 
        }
        Vector3 targetVel = toPlayer * moveSpeed * 1.2f;

        rb.linearVelocity = new Vector3(targetVel.x, rb.linearVelocity.y, targetVel.z);
    }
    public void ApplyKnockback(Vector3 force, float duration)
    {
        if (rb == null) return;

        isKnockedBack = true;
        knockbackTimer = duration;
        Vector3 v = rb.linearVelocity;
        v.x = 0f;
        v.z = 0f;
        rb.linearVelocity = v;

        rb.AddForce(force, ForceMode.Impulse);
    }
    
}