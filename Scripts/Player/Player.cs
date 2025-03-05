using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] public int currentHealth;

    [SerializeField] Movement Movement;
    [SerializeField] LevelSystem LevelSystem;
    [SerializeField] AbilityManager AbilityManager;
    [SerializeField] RespawnManager RespawnManager;
    [SerializeField] HealthBar HealthBar;
    
    public bool damageTaken = false;
    public bool canTakeDamage = true;

    public int GetCurrentHealth() { return currentHealth; }

    void Start()
    {
        Movement = GetComponent<Movement>();
        LevelSystem = FindObjectOfType<LevelSystem>();
        AbilityManager = FindObjectOfType<AbilityManager>();
        RespawnManager = FindObjectOfType<RespawnManager>();
        HealthBar = FindObjectOfType<HealthBar>();

        currentHealth = maxHealth;
        HealthBar.SetMaxHealth(maxHealth);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) { SaveSystem.ResetSave(); }
        if (Input.GetKeyDown(KeyCode.F2)) { SaveSystem.SaveGame(Movement, LevelSystem, AbilityManager, this); }
    }

    public void TakeDamage(int amount)
    {
        if (damageTaken || !canTakeDamage) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        HealthBar.SetHealth();

        damageTaken = true;
        Invoke(nameof(ResetDamageTaken), 0.7f);

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    private void ResetDamageTaken()
    {
        damageTaken = false;
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        HealthBar.SetHealth();
    }

    private void Respawn()
    {
        PlayerData data = SaveSystem.LoadGame();

        if (data != null)
        {
            RestoreHealth();
            Movement.transform.position = new Vector3(data.checkpointX, data.checkpointY, data.checkpointZ);
            LevelSystem.LoadSavedData(data);
            AbilityManager.LoadSavedAbilities(data);
            RespawnManager.RespawnAllEnemies();
            Debug.Log("\nRespawned at last bonfire");
        }
        else Debug.LogWarning("\nNo save data found!");
    }
}