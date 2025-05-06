using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemySpawnerZone : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private int minEnemyCount = 3;
    [SerializeField] private int maxEnemyCount = 8;
    [SerializeField] private float spawnHeightOffset = 0f;
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    public Transform enemiesContainer;

    [Header("References")]
    [SerializeField] private LevelSystem levelSystem;

    private BoxCollider2D spawnArea;
    private List<EnemyBase> currentSpawnedEnemies = new List<EnemyBase>();
    private bool hasSpawned = false;

    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        if (spawnArea == null)
        {
            Debug.LogError("BoxCollider2D is required on EnemySpawnerZone!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();

        // Initial spawn
        SpawnEnemies();

        enemiesContainer = GameObject.Find("Enemies")?.transform;

        if (enemiesContainer == null)
        {
            GameObject go = new GameObject("Enemies");
            go.transform.SetParent(null); // root level
            enemiesContainer = go.transform;
            go.hideFlags = HideFlags.None; // HideFlags.HideInHierarchy if you want it completely hidden
        }

    }

    public void SpawnEnemies()
    {
        // Clear any existing enemies first
        ClearCurrentEnemies();

        // Generate a new random count each time
        int enemyCount = Random.Range(minEnemyCount, maxEnemyCount + 1);
        
        for (int i = 0; i < enemyCount; i++)
        {
            if (enemyPrefabs.Count == 0)
            {
                Debug.LogWarning("No enemy prefabs assigned to spawner zone!");
                return;
            }

            // Pick a completely random enemy prefab each time
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            if (prefab == null) continue;

            // Generate a random position for this enemy
            Vector2 spawnPos = GetRandomPositionInCollider();
            GameObject spawnedEnemy = Instantiate(prefab, new Vector3(spawnPos.x, spawnPos.y, transform.position.z + spawnHeightOffset), Quaternion.identity, enemiesContainer);
            
            // Setup the enemy
            EnemyBase enemy = spawnedEnemy.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                if (levelSystem != null)
                {
                    enemy.LevelSystem = levelSystem;
                }
                currentSpawnedEnemies.Add(enemy);
            }
        }

        hasSpawned = true;
    }

    // Efficient method to respawn enemies by refreshing their values
    public void RefreshEnemyValues()
    {
        // First determine if we need a different number of enemies
        int newEnemyCount = Random.Range(minEnemyCount, maxEnemyCount + 1);
        
        // Remove excess enemies if we need fewer now
        while (currentSpawnedEnemies.Count > newEnemyCount)
        {
            int lastIndex = currentSpawnedEnemies.Count - 1;
            if (currentSpawnedEnemies[lastIndex] != null)
            {
                Destroy(currentSpawnedEnemies[lastIndex].gameObject);
            }
            currentSpawnedEnemies.RemoveAt(lastIndex);
        }
        
        // Update existing enemies by replacing them with new ones
        for (int i = 0; i < currentSpawnedEnemies.Count; i++)
        {
            // Replace all existing enemies with new ones at new positions
            if (currentSpawnedEnemies[i] != null)
            {
                Destroy(currentSpawnedEnemies[i].gameObject);
            }
            
            // Replace enemy at this index
            ReplaceEnemyAtIndex(i);
        }
        
        // Add new enemies if we need more
        while (currentSpawnedEnemies.Count < newEnemyCount)
        {
            AddNewRandomEnemy();
        }
        
        Debug.Log($"Refreshed to {currentSpawnedEnemies.Count} enemies in {gameObject.name}");
    }
    
    private void ReplaceEnemyAtIndex(int index)
    {
        // Pick a random enemy type
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        if (prefab == null) return;
        
        // New position
        Vector2 spawnPos = GetRandomPositionInCollider();
        
        // Create new enemy
        GameObject spawnedEnemy = Instantiate(prefab, new Vector3(spawnPos.x, spawnPos.y, transform.position.z + spawnHeightOffset), Quaternion.identity, enemiesContainer);
        
        // Setup
        EnemyBase enemy = spawnedEnemy.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            if (levelSystem != null)
            {
                enemy.LevelSystem = levelSystem;
            }
            
            // Replace at index
            if (index < currentSpawnedEnemies.Count)
            {
                currentSpawnedEnemies[index] = enemy;
            }
            else
            {
                currentSpawnedEnemies.Add(enemy);
            }
        }
    }
    
    private void AddNewRandomEnemy()
    {
        if (enemyPrefabs.Count == 0) return;
        
        // Choose random type
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        if (prefab == null) return;
        
        // Get position
        Vector2 spawnPos = GetRandomPositionInCollider();
        GameObject spawnedEnemy = Instantiate(prefab, new Vector3(spawnPos.x, spawnPos.y, transform.position.z + spawnHeightOffset), Quaternion.identity);
        
        // Setup the enemy
        EnemyBase enemy = spawnedEnemy.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            if (levelSystem != null)
            {
                enemy.LevelSystem = levelSystem;
            }
            currentSpawnedEnemies.Add(enemy);
        }
    }
    
    // Use the more efficient refresh method instead of complete respawn
    public void RespawnEnemies()
    {
        if (currentSpawnedEnemies.Count == 0 || !hasSpawned)
        {
            // First time or all enemies were destroyed - do a full spawn
            SpawnEnemies();
        }
        else
        {
            // More efficient - just refresh values
            RefreshEnemyValues();
        }
    }

    // Get a completely random position within the collider each time
    private Vector2 GetRandomPositionInCollider()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    public void ClearCurrentEnemies()
    {
        // Clean up any existing enemies
        foreach (var enemy in currentSpawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        currentSpawnedEnemies.Clear();
        hasSpawned = false;
    }

    public bool HasRemainingEnemies()
    {
        currentSpawnedEnemies.RemoveAll(e => e == null);
        return currentSpawnedEnemies.Count > 0;
    }

    // Helper method to add the spawner to the divine point respawn system
    public void RegisterWithDivinePoint(DivinePoint divinePoint)
    {
        // This can be called from the inspector or another script to connect this spawner to divine points
        divinePoint.RegisterEnemySpawner(this);
    }

    private void OnDrawGizmos()
    {
        // Draw a visual representation of the spawn area in the editor
        if (spawnArea == null)
            spawnArea = GetComponent<BoxCollider2D>();
        
        if (spawnArea != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Bounds bounds = spawnArea.bounds;
            Gizmos.DrawCube(bounds.center, bounds.size);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
