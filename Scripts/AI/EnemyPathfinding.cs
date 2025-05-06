using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPathfinding : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallAvoidanceOffset = 1f;
    [SerializeField] private int maxPathPoints = 50;
    [SerializeField] private float pathUpdateInterval = 0.5f;
    [SerializeField] private float pathPointSpacing = 0.5f;
    [SerializeField] private bool showDebugPath = true;

    [Header("Path Smoothing")]
    [SerializeField] private float pathSmoothing = 0.5f;
    [SerializeField] private int smoothingIterations = 2;
    
    public List<Vector2> currentPath = new List<Vector2>();
    private Transform playerTransform;
    private Vector2 lastTargetPosition;
    private float lastPathUpdateTime = 0f;
    private bool isPathUpdateInProgress = false;
    private EnemyAI enemyAI;
    private bool isPlayerInPathRange = false;
    private Coroutine pathUpdateCoroutine;
    
    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        wallLayer = LayerMask.GetMask("Wall");
    }
    
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (enemyAI.usePathfinding)
        {
            // Start updating path regularly
            StartCoroutine(RegularPathUpdates());
        }
    }
    
    private IEnumerator RegularPathUpdates()
    {
        while (true)
        {
            if (isPlayerInPathRange && !isPathUpdateInProgress && playerTransform != null)
            {
                // Check if player has moved enough to warrant a new path
                Vector2 currentPlayerPos = playerTransform.position;
                float playerMoveDist = Vector2.Distance(lastTargetPosition, currentPlayerPos);
                
                if (playerMoveDist > 1.0f || Time.time - lastPathUpdateTime > pathUpdateInterval)
                {
                    UpdatePath();
                }
            }
            
            yield return new WaitForSeconds(0.2f); // Check regularly but not every frame
        }
    }
    
    public void UpdatePath()
    {
        if (isPathUpdateInProgress || playerTransform == null) return;
        
        if (pathUpdateCoroutine != null)
        {
            StopCoroutine(pathUpdateCoroutine);
        }
        
        pathUpdateCoroutine = StartCoroutine(CalculatePathAsync());
    }
    
    private IEnumerator CalculatePathAsync()
    {
        isPathUpdateInProgress = true;
        
        Vector2 startPos = transform.position;
        Vector2 targetPos = playerTransform.position;
        lastTargetPosition = targetPos;
        
        List<Vector2> newPath = new List<Vector2>();
        newPath.Add(startPos);
        
        bool hasLineOfSight = !Physics2D.Linecast(startPos, targetPos, wallLayer);
        
        if (hasLineOfSight)
        {
            newPath.Add(targetPos);
        }
        else
        {
            yield return GeneratePathWithObstacleAvoidance(startPos, targetPos, newPath);
            yield return SmoothPathAsync(newPath);
            if (newPath.Count > maxPathPoints)
            {
                SimplifyPath(newPath);
            }
        }
        
        currentPath = newPath;
        lastPathUpdateTime = Time.time;
        isPathUpdateInProgress = false;
    }
    
    private IEnumerator GeneratePathWithObstacleAvoidance(Vector2 start, Vector2 target, List<Vector2> path)
    {
        Vector2 currentPoint = start;
        int iterations = 0;
        int maxIterations = 100;  // Safety measure
        
        while (Vector2.Distance(currentPoint, target) > pathPointSpacing && iterations < maxIterations)
        {
            iterations++;
            
            // Direction to target
            Vector2 directionToTarget = (target - currentPoint).normalized;
            
            // How far to cast - max distance to not overshoot target
            float maxDistance = Mathf.Min(pathPointSpacing * 2, Vector2.Distance(currentPoint, target));
            RaycastHit2D hit = Physics2D.Raycast(currentPoint, directionToTarget, maxDistance, wallLayer);
            
            if (hit.collider == null)
            {
                // No wall, we can move toward the target
                Vector2 newPoint = currentPoint + directionToTarget * pathPointSpacing;
                path.Add(newPoint);
                currentPoint = newPoint;
                
                // Check for direct line of sight to target
                if (!Physics2D.Linecast(newPoint, target, wallLayer))
                {
                    // We can see the target now, add it and finish
                    path.Add(target);
                    break;
                }
            }
            else
            {
                // We hit a wall, need to navigate around it
                Vector2 hitNormal = hit.normal;
                Vector2 wallParallel = new Vector2(-hitNormal.y, hitNormal.x);
                
                // Try both directions around the obstacle
                Vector2 leftPoint = currentPoint - wallParallel * pathPointSpacing;
                Vector2 rightPoint = currentPoint + wallParallel * pathPointSpacing;
                
                // Check if going left or right gets us closer to target faster
                float leftScore = EvaluatePath(leftPoint, hit.point, target, wallLayer);
                float rightScore = EvaluatePath(rightPoint, hit.point, target, wallLayer);
                
                // Choose the better direction
                Vector2 nextPoint;
                if (leftScore < rightScore)
                {
                    nextPoint = leftPoint + hitNormal * wallAvoidanceOffset;
                }
                else
                {
                    nextPoint = rightPoint + hitNormal * wallAvoidanceOffset;
                }
                
                // Make sure we're not inside a wall
                while (Physics2D.OverlapCircle(nextPoint, 0.1f, wallLayer))
                {
                    nextPoint += hitNormal * 0.1f;
                }
                
                path.Add(nextPoint);
                currentPoint = nextPoint;
            }
            
            // Spread computation across frames
            if (iterations % 5 == 0)
                yield return null;
        }
    }
    
    private float EvaluatePath(Vector2 point, Vector2 obstacle, Vector2 target, LayerMask obstacleLayer)
    {
        // If this point is inside a wall, it's a terrible choice
        if (Physics2D.OverlapCircle(point, 0.1f, obstacleLayer))
        {
            return float.MaxValue;
        }
        
        // If we can see the target from here, it's the best path
        if (!Physics2D.Linecast(point, target, obstacleLayer))
        {
            return Vector2.Distance(point, target) * 0.5f;
        }
        
        // Otherwise score based on distance to target and how close to the obstacle
        float distanceToTarget = Vector2.Distance(point, target);
        float obstacleProximity = Vector2.Distance(point, obstacle);
        
        // Prefer paths away from obstacles but toward the target
        return distanceToTarget + (1.0f / obstacleProximity) * 2.0f;
    }
    
    private IEnumerator SmoothPathAsync(List<Vector2> pathToSmooth)
    {
        if (pathToSmooth.Count <= 2) yield break;
        
        for (int iteration = 0; iteration < smoothingIterations; iteration++)
        {
            List<Vector2> smoothedPoints = new List<Vector2>(pathToSmooth);
            
            // Skip first and last points (keep them as is)
            for (int i = 1; i < pathToSmooth.Count - 1; i++)
            {
                Vector2 prev = pathToSmooth[i - 1];
                Vector2 current = pathToSmooth[i];
                Vector2 next = pathToSmooth[i + 1];
                
                // Calculate smoothed position (weighted average)
                Vector2 smoothed = current * (1 - pathSmoothing) + 
                                 (prev + next) * 0.5f * pathSmoothing;
                
                bool intersectsWall = false;
                
                if (Physics2D.Linecast(prev, smoothed, wallLayer) || 
                    Physics2D.Linecast(smoothed, next, wallLayer))
                {
                    intersectsWall = true;
                }
                
                if (!intersectsWall)
                {
                    smoothedPoints[i] = smoothed;
                }
            }
            
            pathToSmooth = smoothedPoints;
            
            yield return null;
        }
    }
    
    private void SimplifyPath(List<Vector2> pathToSimplify)
    {
        if (pathToSimplify.Count <= maxPathPoints) return;
        
        List<Vector2> simplified = new List<Vector2>();
        simplified.Add(pathToSimplify[0]); 
        
        // Calculate how many points to skip
        float skipFactor = (float)(pathToSimplify.Count - 2) / (maxPathPoints - 2);
        
        for (int i = 1; i < maxPathPoints - 1; i++)
        {
            int index = Mathf.FloorToInt(i * skipFactor) + 1;
            index = Mathf.Clamp(index, 1, pathToSimplify.Count - 2);
            simplified.Add(pathToSimplify[index]);
        }
        
        simplified.Add(pathToSimplify[pathToSimplify.Count - 1]);
        
        pathToSimplify.Clear();
        pathToSimplify.AddRange(simplified);
    }
    
    public void OnPathRangeEnter(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPathRange = true;
            UpdatePath(); 
        }
    }
    
    public void OnPathRangeExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPathRange = false;
            currentPath.Clear();
        }
    }
    
    public List<Vector2> FindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        if (currentPath.Count == 0 || 
            Vector2.Distance(lastTargetPosition, targetPosition) > 1.0f)
        {
            UpdatePath();
        }
        
        return currentPath;
    }
    
    // private void OnDrawGizmos()
    // {
    //     if (!showDebugPath || currentPath == null || currentPath.Count < 2) return;
        
    //     Gizmos.color = Color.yellow;
        
    //     for (int i = 0; i < currentPath.Count - 1; i++)
    //     {
    //         Gizmos.DrawLine(currentPath[i], currentPath[i + 1]);
    //         Gizmos.DrawWireSphere(currentPath[i], 0.2f);
    //     }
        
    //     Gizmos.DrawWireSphere(currentPath[currentPath.Count - 1], 0.2f);
    // }
}