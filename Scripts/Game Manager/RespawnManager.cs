using UnityEngine;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }
    [SerializeField] private List<GameObject> enemyPrefabs; // Assign all enemy prefabs in Inspector

    public GameObject FindPrefabForEnemy(EnemyBase enemy)
    {
        foreach (var prefab in enemyPrefabs)
        {
            if (prefab.GetComponent<EnemyBase>().GetType() == enemy.GetType())
            {
                return prefab;
            }
        }
        return null;
    }

    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject prefab;        
        public Vector3 initialPosition;   
        public int initialHealth;         
        public int experiencePoints;      
    }

    [SerializeField] private List<EnemySpawnData> enemySpawnData = new List<EnemySpawnData>();
    private List<EnemyBase> currentEnemies = new List<EnemyBase>();
    private LevelSystem levelSystem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();
        
        EnemyBase[] enemies = FindObjectsOfType<EnemyBase>();
        foreach (EnemyBase enemy in enemies)
        {
            RegisterEnemy(enemy);
        }

        if (currentEnemies.Count == 0 && enemySpawnData.Count > 0)
        {
            RespawnAllEnemies();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnAllEnemies();
        }
    }
    
    public void RegisterEnemy(EnemyBase enemy)
    {
        if (!currentEnemies.Contains(enemy))
        {
            currentEnemies.Add(enemy);
            if (levelSystem != null && enemy.LevelSystem == null)
            {
                enemy.LevelSystem = levelSystem;
            }

            bool foundMatch = false;
            foreach (var data in enemySpawnData)
            {
                if (data.prefab != null && 
                    data.prefab.GetComponent<EnemyBase>().GetType() == enemy.GetType() && 
                    Vector3.Distance(data.initialPosition, enemy.transform.position) < 0.1f)
                {
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
            {
                GameObject prefab = FindPrefabForEnemy(enemy);
                if (prefab != null)
                {
                    enemySpawnData.Add(new EnemySpawnData
                    {
                        prefab = prefab,
                        initialPosition = enemy.transform.position,
                        initialHealth = enemy.GetHealth(),
                        experiencePoints = enemy.GetExperiencePoints()
                    });
                }
                else
                {
                    Debug.LogWarning($"No prefab found for {enemy.name}. Assign prefabs in RespawnManager.");
                }
            }
        }
    }

    public void RespawnAllEnemies()
    {
        foreach (var enemy in currentEnemies.ToArray())
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        currentEnemies.Clear();

        foreach (var spawnData in enemySpawnData)
        {
            if (spawnData.prefab != null)
            {
                SpawnEnemy(spawnData);
            }
            else
            {
                Debug.LogWarning("Cannot respawn enemy: Prefab is missing in spawn data.");
            }
        }
    }

    private void SpawnEnemy(EnemySpawnData spawnData)
    {
        GameObject newEnemyObj = Instantiate(spawnData.prefab, spawnData.initialPosition, Quaternion.identity);
        EnemyBase newEnemy = newEnemyObj.GetComponent<EnemyBase>();
        
        if (levelSystem != null)
        {
            newEnemy.LevelSystem = levelSystem;
        }
        newEnemy.SetInitialValues(spawnData.initialHealth, spawnData.experiencePoints);
        
        currentEnemies.Add(newEnemy);
    }

    public void SaveEnemyValues(EnemyBase enemy)
    {
        foreach (var spawnData in enemySpawnData)
        {
            if (spawnData.prefab != null && 
                spawnData.prefab.GetComponent<EnemyBase>().GetType() == enemy.GetType() && 
                Vector3.Distance(spawnData.initialPosition, enemy.transform.position) < 0.1f)
            {
                spawnData.initialHealth = enemy.GetHealth();
                spawnData.experiencePoints = enemy.GetExperiencePoints();
                break;
            }
        }
        currentEnemies.Remove(enemy);
    }
}

