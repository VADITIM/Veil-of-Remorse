using UnityEngine;
using System.Collections.Generic;

public class RaycastPathfinding : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    public int rayCount = 20;
    public float raycastDistance = 5f;
    public float updateInterval = 0.2f;
    public float cornerSmoothingDistance = 0.5f;
    public float obstacleAvoidanceStrength = 1.5f;
    public float obstacleDetectionBuffer = 0.5f;
    public LayerMask obstacleLayers;

    [Header("Path Optimization")]

    [Header("Failsafe Settings")]
    public float stuckVelocityThreshold = 0.1f; // Minimum velocity to consider "moving"
    public float stuckTimeThreshold = 0.2f;     // Time stopped before triggering failsafe
    public float minDistanceFromObstacle = 3f;  // Minimum distance to move away from obstacle

    private Vector2 lastPosition;
    private float stuckTimer;
    private EnemyBase enemyBase; // Changed from EnemyAI to EnemyBase
    private Transform playerTransform;
    private float pathTimer;
    private List<Vector2> potentialDirections = new List<Vector2>();
    private List<Vector2> validDirections = new List<Vector2>();
    private List<RaycastHit2D[]> raycastHits = new List<RaycastHit2D[]>();
    private List<Vector2> pathPoints = new List<Vector2>();
    private int currentPathPointIndex = 0;
    private Vector2 lastPlayerPosition;
    private float pathRecalculationThreshold = 1f;

    private void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateRayDirections();
    }

    private void GenerateRayDirections()
    {
        potentialDirections.Clear();
        float angleStep = 360f / rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            potentialDirections.Add(direction);
        }
    }

    public void UpdatePathfinding()
    {
        if (playerTransform == null) return;

        pathTimer -= Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Check if the enemy has stopped moving (failsafe trigger)
        if (enemyBase.rb.velocity.magnitude < stuckVelocityThreshold)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= stuckTimeThreshold)
            {
                TriggerFailsafePathRecalculation();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
        lastPosition = transform.position;

        if (pathTimer <= 0 || distanceToPlayer <= enemyBase.speed * 0.5f || // Adjusted for EnemyBase
            Vector2.Distance(lastPlayerPosition, playerTransform.position) > pathRecalculationThreshold)
        {
            CalculatePath();
            pathTimer = updateInterval;
            lastPlayerPosition = playerTransform.position;
        }

        FollowPath();
    }

    private void CalculatePath()
    {
        pathPoints.Clear();
        validDirections.Clear();
        raycastHits.Clear();
        currentPathPointIndex = 0;

        pathPoints.Add(transform.position);

        Vector2 directDirection = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;

        for (int i = 0; i < potentialDirections.Count; i++)
        {
            Vector2 rayDirection = potentialDirections[i];
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDirection, raycastDistance, obstacleLayers);
            raycastHits.Add(hits);

            float closestDistance = raycastDistance;
            foreach (var hit in hits)
            {
                if (hit.distance < closestDistance) closestDistance = hit.distance;
            }

            if (closestDistance > obstacleDetectionBuffer)
            {
                validDirections.Add(rayDirection);
                float directionScore = Vector2.Dot(rayDirection, directDirection);

                if (directionScore > 0.7f)
                {
                    RaycastHit2D directHit = Physics2D.Raycast(
                        transform.position + (Vector3)(rayDirection * closestDistance * 0.8f),
                        directDirection,
                        raycastDistance,
                        obstacleLayers);

                    if (directHit.collider == null)
                    {
                        Vector2 pathPoint = (Vector2)transform.position + rayDirection * closestDistance * 0.8f;
                        pathPoints.Add(pathPoint);
                    }
                }
            }
        }

        if (pathPoints.Count == 1)
        {
            FindPathAroundObstacles();
        }

        pathPoints.Add(playerTransform.position);
        SimplifyPath();
    }

    private void FindPathAroundObstacles()
    {
        Vector2 directDirection = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        float bestScore = -1f;
        Vector2 bestDirection = Vector2.zero;

        foreach (Vector2 direction in validDirections)
        {
            int index = potentialDirections.IndexOf(direction);
            RaycastHit2D[] hits = raycastHits[index];
            float distanceScore = raycastDistance;
            foreach (var hit in hits)
            {
                if (hit.distance < distanceScore) distanceScore = hit.distance;
            }

            float directionScore = Vector2.Dot(direction, directDirection) + 1;
            float totalScore = distanceScore * directionScore;

            if (totalScore > bestScore)
            {
                bestScore = totalScore;
                bestDirection = direction;
            }
        }

        if (bestScore > 0)
        {
            Vector2 pathPoint = (Vector2)transform.position + bestDirection * raycastDistance * 0.7f;
            pathPoints.Add(pathPoint);
        }
    }

    private void SimplifyPath()
    {
        if (pathPoints.Count <= 2) return;

        List<Vector2> simplifiedPath = new List<Vector2> { pathPoints[0] };
        int currentIndex = 0;

        while (currentIndex < pathPoints.Count - 1)
        {
            int nextIndex = pathPoints.Count - 1;
            for (int i = pathPoints.Count - 1; i > currentIndex; i--)
            {
                if (HasLineOfSight(pathPoints[currentIndex], pathPoints[i]))
                {
                    nextIndex = i;
                    break;
                }
            }
            simplifiedPath.Add(pathPoints[nextIndex]);
            currentIndex = nextIndex;
        }

        pathPoints = simplifiedPath;
    }

    private bool HasLineOfSight(Vector2 start, Vector2 end)
    {
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);
        RaycastHit2D hit = Physics2D.Raycast(start, direction, distance, obstacleLayers);
        return hit.collider == null;
    }

    private void FollowPath()
    {
        if (pathPoints.Count <= 1 || currentPathPointIndex >= pathPoints.Count)
        {
            MoveDirectlyOrAvoidObstacles();
            return;
        }

        Vector2 targetPoint = pathPoints[currentPathPointIndex];
        Vector2 moveDirection = (targetPoint - (Vector2)transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, targetPoint);

        if (!HasLineOfSight(transform.position, targetPoint))
        {
            CalculatePath();
            return;
        }

        Vector2 nextPosition = (Vector2)transform.position + moveDirection * enemyBase.speed * Time.deltaTime;
        if (Physics2D.OverlapCircle(nextPosition, obstacleDetectionBuffer, obstacleLayers))
        {
            Vector2 pushAway = Vector2.Perpendicular(moveDirection) * obstacleAvoidanceStrength;
            moveDirection += pushAway;
            moveDirection = moveDirection.normalized;
        }

        if (distanceToTarget < cornerSmoothingDistance)
        {
            currentPathPointIndex++;
            if (currentPathPointIndex >= pathPoints.Count)
            {
                MoveDirectlyOrAvoidObstacles();
                return;
            }
            targetPoint = pathPoints[currentPathPointIndex];
            moveDirection = (targetPoint - (Vector2)transform.position).normalized;
        }

        enemyBase.Move(moveDirection); // Updated to use EnemyBase.Move
    }

    private void MoveDirectlyOrAvoidObstacles()
    {
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, enemyBase.speed * 1.5f, obstacleLayers);

        if (hit.collider == null)
        {
            enemyBase.Move(directionToPlayer);
            return;
        }

        Vector2 avoidanceDirection = Vector2.Reflect(directionToPlayer, hit.normal).normalized;
        avoidanceDirection += directionToPlayer * 0.5f;
        enemyBase.Move(avoidanceDirection.normalized);
    }

    private void TriggerFailsafePathRecalculation()
    {
        // Find the nearest obstacle causing the stoppage
        RaycastHit2D[] closestHits = null;
        float closestDistance = raycastDistance;
        foreach (var hits in raycastHits)
        {
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestHits = hits;
                }
            }
        }

        if (closestHits == null) return; // No obstacle found, fallback to regular recalculation

        // Calculate a safe point at least 1f away from the obstacle
        Vector2 obstaclePosition = closestHits[0].point;
        Vector2 directionAway = ((Vector2)transform.position - obstaclePosition).normalized;
        Vector2 safePoint = obstaclePosition + directionAway * minDistanceFromObstacle;

        // Ensure the safe point is valid (not inside another obstacle)
        if (!Physics2D.OverlapCircle(safePoint, obstacleDetectionBuffer, obstacleLayers))
        {
            pathPoints.Clear();
            pathPoints.Add(transform.position);
            pathPoints.Add(safePoint); // Add the safe point as an intermediate waypoint
            pathPoints.Add(playerTransform.position);
            currentPathPointIndex = 0;
        }
        else
        {
            // If the safe point is invalid, try a detour
            Vector2 detourPoint = FindBestDetourPoint();
            pathPoints.Clear();
            pathPoints.Add(transform.position);
            pathPoints.Add(detourPoint);
            pathPoints.Add(playerTransform.position);
            currentPathPointIndex = 0;
        }
    }

    private Vector2 FindBestDetourPoint()
    {
        Vector2 bestPoint = transform.position;
        float bestScore = float.MinValue;
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;

        for (float angle = 0; angle < 360; angle += 45)
        {
            Vector2 testDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector2 testPoint = (Vector2)transform.position + testDir * raycastDistance * 0.5f;
            if (!Physics2D.Raycast(transform.position, testDir, raycastDistance * 0.5f, obstacleLayers))
            {
                float score = Vector2.Dot(testDir, directionToPlayer) + 1;
                if (score > bestScore)
                {
                    bestScore = score;
                    bestPoint = testPoint;
                }
            }
        }
        return bestPoint;
    }
}