using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    Movement Movement;
    LevelSystem LevelSystem;
    AbilityManager AbilityManager;
    RespawnManager RespawnManager;
    
    public bool damageTaken = false;
    public bool canTakeDamage = true;

    void Start()
    {
        currentHealth = maxHealth;
        Movement = GetComponent<Movement>();
        LevelSystem = FindObjectOfType<LevelSystem>();
        AbilityManager = FindObjectOfType<AbilityManager>();
        RespawnManager = FindObjectOfType<RespawnManager>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveSystem.ResetSave();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SaveSystem.SaveGame(Movement, LevelSystem, AbilityManager);
        }
    }

    public void TakeDamage(int amount)
    {
        if (damageTaken) return;
        if (!canTakeDamage) return;
        damageTaken = true;

        Debug.Log("Player took " + amount + " damage");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
            Respawn();
        }
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        Debug.Log("Health Restored");
    }

    private void Respawn()
    {
        if (Movement != null)
        {
            PlayerData data = SaveSystem.LoadGame();
            if (data != null)
            {
                Movement.transform.position = new Vector3(data.checkpointX, data.checkpointY, 0);
                RestoreHealth();
                RespawnManager.RespawnAllEnemies();
                Debug.Log("Respawned at last bonfire");
            }
            else
            {
                Debug.LogWarning("No save data found!");
            }
        }
        else
        {
            Debug.LogError("Movement script not found on player object");
        }
    }
} 
