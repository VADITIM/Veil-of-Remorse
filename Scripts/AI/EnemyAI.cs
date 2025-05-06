using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : EnemyBase
{
    private float detectionRadius = 10f;
    private float attackRadius = 2f;
    public float moveSpeed = 1.5f;
    public float chaseSpeed = .9f;

    public float GetDetectionRadius() { return detectionRadius; }

    [Header("Wander Settings")]
    private float minMoveTime = .5f;
    private float maxMoveTime = 1f;
    private float idleTime = 1.5f;

    [Header("Pathfinding")]
    public bool usePathfinding = true;

    private enum State { Idle, Move, InPlayerSight, InAttackRange, Attack }
    private State currentState;

    private Transform playerTransform;
    private Vector2 moveDirection;
    private bool playerDetected = false;
    private float stateTimer;
    private EnemyPathfinding pathfinding;

    private const float detectionEnterBuffer = 0f;     
    private const float detectionExitBuffer = 1f;      


    private List<Vector2> path = new List<Vector2>();
    private int currentPathIndex = 0;

    protected override void Start()
    {
        base.Start();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        pathfinding = GetComponent<EnemyPathfinding>();

        if (pathfinding == null && usePathfinding)
        {
            pathfinding = gameObject.AddComponent<EnemyPathfinding>();
        }

        ChangeState(State.Idle);
    }

    protected override void Update()
    {
        base.Update();

        if (playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);

            if (!playerDetected && distance <= detectionRadius - detectionEnterBuffer)
            {
                playerDetected = true;
                ChangeState(State.InPlayerSight);
            }
            else if (playerDetected && distance > detectionRadius + detectionExitBuffer)
            {
                playerDetected = false;
                ChangeState(State.Move);
            }
        }

        if (playerDetected && usePathfinding && pathfinding != null)
        {
            path = pathfinding.FindPath(transform.position, playerTransform.position);
            FollowPath();
            return;
        }

        switch (currentState)
        {
            case State.Idle:
                HandleIdleState();
                break;
            case State.Move:
                HandleMoveState();
                break;
            case State.InPlayerSight:
                HandleInPlayerSightState();
                break;
            case State.InAttackRange:
                HandleInAttackRangeState();
                break;
        }
    }

    private void HandleIdleState()
    {
        Move(Vector2.zero);
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            ChangeState(State.Move);
        }
    }

    private void HandleMoveState()
    {
        Move(moveDirection.normalized * moveSpeed);
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            ChangeState(State.Idle);
        }
    }

    private void HandleInAttackRangeState()
    {
        Move(Vector2.zero);
    }

    private void FollowPath()
    {
        if (pathfinding == null) return;
        
        List<Vector2> path = pathfinding.currentPath;
        if (path == null || path.Count == 0)
        {
            // If we don't have a path, just move directly toward the player
            if (playerTransform != null)
            {
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, 1.5f, LayerMask.GetMask("Wall"));
                
                if (hit.collider != null)
                {
                    // If there's a wall, try to navigate around it
                    Vector2 avoidDirection = Vector2.Perpendicular(directionToPlayer).normalized;
                    
                    // Test both directions to see which is better
                    float leftDist = 0;
                    float rightDist = 0;
                    
                    RaycastHit2D leftHit = Physics2D.Raycast(transform.position, -avoidDirection, 1.5f, LayerMask.GetMask("Wall"));
                    RaycastHit2D rightHit = Physics2D.Raycast(transform.position, avoidDirection, 1.5f, LayerMask.GetMask("Wall"));
                    
                    leftDist = leftHit.collider != null ? leftHit.distance : 1.5f;
                    rightDist = rightHit.collider != null ? rightHit.distance : 1.5f;
                    
                    // Move in the direction with more space
                    Vector2 moveDir = leftDist > rightDist ? -avoidDirection : avoidDirection;
                    Move(moveDir * chaseSpeed);
                }
                else
                {
                    Move(directionToPlayer * chaseSpeed);
                }
            }
            return;
        }
        
        // First point in the path is where we're currently standing,
        int targetIndex = 1;
        
        // Skip points that we're already close to
        while (targetIndex < path.Count && 
               Vector2.Distance(transform.position, path[targetIndex]) < 0.5f)
        {
            targetIndex++;
        }
        
        // If we've reached the end of the path
        if (targetIndex >= path.Count)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) < attackRadius)
            {
                ChangeState(State.InAttackRange);
            }
            return;
        }
        
        // Move toward the next point
        Vector2 targetPosition = path[targetIndex];
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
        
        // Calculate distance to the target position
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        
        // Move toward the target
        Move(moveDirection * chaseSpeed);
        
        // Debug.DrawLine(transform.position, targetPosition, Color.green);
    }
    
    private void HandleInPlayerSightState()
    {
        // Check if we need to transition to attack range
        if (playerTransform != null && 
            Vector2.Distance(transform.position, playerTransform.position) < attackRadius)
        {
            ChangeState(State.InAttackRange);
            return;
        }
        
        // Let pathfinding handle movement
        if (usePathfinding && pathfinding != null)
        {
            // Path following is now handled in Update
        }
        else
        {
            // Direct movement if pathfinding is disabled
            if (playerTransform != null)
            {
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                Move(directionToPlayer * chaseSpeed);
            }
        }
    }

    private void ChangeState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                stateTimer = idleTime;
                break;
            case State.Move:
                float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
                moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                stateTimer = Random.Range(minMoveTime, maxMoveTime);
                break;
            case State.InPlayerSight:
                if (usePathfinding && pathfinding != null)
                {
                    path = pathfinding.FindPath(transform.position, playerTransform.position);
                    currentPathIndex = 0;
                }
                break;
            case State.InAttackRange:
                break;
        }
    }
}
