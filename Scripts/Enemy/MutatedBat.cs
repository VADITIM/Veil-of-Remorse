using UnityEngine;

public class MutatedBat : EnemyBase
{
    EnemyAI AI;
    private EnemyStateMachine stateMachine;
    
    float timer = 0;
    float timerDuration = 0.45f;

    private enum State { Moving, Chasing, Attacking, Dying }
    private State currentState;

    [Header("Attack Settings")]
    [SerializeField] private float raycastLength = 2.5f; 
    [SerializeField] private int raycastCount = 20; 
    [SerializeField] private LayerMask playerLayer; 

    private CircleCollider2D detectionCollider;
    private Transform playerTransform;

    private bool isWindingUp = false;
    private bool hasDealtDamage = false;

    private Vector3[] raycastDirections;
    private bool playerInAttackRange = false;

    protected override void Start()
    {
        base.Start();

        AI = GetComponent<EnemyAI>();

        health = 40;
        experiencePoints = 110;
        damage = 50;
        windupTime = .9f;
        attackDuration = 0.783f;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        stateMachine = GetComponent<EnemyStateMachine>();

        detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.radius = AI.GetDetectionRadius();
        detectionCollider.isTrigger = true;
        
        InitializeRaycastDirections();

        ChangeState(State.Moving);
    }

    private void InitializeRaycastDirections()
    {
        raycastDirections = new Vector3[raycastCount];
        float angleStep = 360f / raycastCount;
        
        for (int i = 0; i < raycastCount; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = Mathf.Cos(angle * Mathf.Deg2Rad);
            raycastDirections[i] = new Vector3(x, y, 0).normalized;
        }
    }

    protected override void Update()
    {
        if (currentState == State.Attacking) return; 
        base.Update();
        
        HandleExplosionRange();
        if (isAttacking) StartAttack();
    }

    private void HandleExplosionRange()
    {
        bool playerDetected = false;

        for (int i = 0; i < raycastCount; i++)
        {
            Vector3 direction = raycastDirections[i];
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastLength, playerLayer);
            
            Debug.DrawRay(transform.position, direction * raycastLength, hit ? Color.red : Color.green);
            
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerDetected = true;
            }
        }

        if (playerDetected)
        {
            HandleWindup();
        }
        else
        {
            ResetWindup();
        }
    }


    private void HandleWindup()
    {
        windupTime -= Time.deltaTime;
        if (windupTime <= 0)
        {
            windupTime = .9f;
            isAttacking = true;
        }
    }

    private void ResetWindup()
    {
        windupTime = .9f; 
        isWindingUp = false;
    }

    private bool canDealDamage = true;
    private float dealDamageTimer = 0f;
    private float dealDamageDuration = 0.2f;

    private void StartAttack()
    {
        attackDuration -= Time.deltaTime;

        if (attackDuration <= 0)
        {
            Die();
        }

        else if (!hasDealtDamage)
        {
            timer += Time.deltaTime;
            if (timer >= timerDuration)
            {
                dealDamageTimer += Time.deltaTime;

                if (dealDamageTimer >= dealDamageDuration)
                {
                    canDealDamage = false;
                }

                if (canDealDamage)
                {
                    DealDamageToPlayer();
                    hasDealtDamage = true;
                }
            }
        }
    }

    private void DealDamageToPlayer()
    {
        for (int i = 0; i < raycastCount; i++)
        {
            Vector3 direction = raycastDirections[i];
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastLength, playerLayer);
            
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Player player = hit.collider.GetComponent<Player>();
                player.TakeDamage(damage);
                return; 
            }
        }
        
        if (Vector2.Distance(transform.position, playerTransform.position) <= raycastLength)
        {
            Player player = playerTransform.GetComponent<Player>();
            player.TakeDamage(damage);
        }
    }

    public override void Die()
    {
        if (currentState == State.Dying) return; 
        
        currentState = State.Dying; 
        base.Die();
    }

    private void ChangeState(State newState)
    {
        if (currentState == State.Dying) return; 
        if (currentState == State.Attacking) return; 

        if (currentState == State.Attacking && newState != State.Attacking)
        {
            if (!isWindingUp)
            {
                isAttacking = false;
                stateMachine.SetAttacking(false);
            }
        }
        currentState = newState;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentState != State.Dying)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= detectionCollider.radius)
                ChangeState(State.Chasing);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isWindingUp && currentState != State.Dying)
        {
            if (!playerInAttackRange)
                ChangeState(State.Chasing);
        }
    }

    // void OnDrawGizmosSelected()
    // {
    //     if (raycastDirections != null)
    //     {
    //         Gizmos.color = Color.yellow;
    //         for (int i = 0; i < raycastCount; i++)
    //         {
    //             Gizmos.DrawRay(transform.position, raycastDirections[i] * raycastLength);
    //         }
    //     }
        
    //     Gizmos.color = new Color(1, 0, 0, 0.2f);
    //     Gizmos.DrawWireSphere(transform.position, raycastLength);
    // }
}