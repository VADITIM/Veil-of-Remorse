using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    
    [Header("Health and Stamina")]
    public int maxHealth = 100;
    public int currentHealth;
    public int maxStamina = 100;
    public float currentStamina;

    [Header("Collision Settings")]
    public GameObject colliderObject; 
    
    public bool damageTaken = false;
    public bool canTakeDamage = true;

    public int GetCurrentHealth() { return currentHealth; }
    public float GetCurrentStamina() { return currentStamina; }

    void Awake()
    {
        if (colliderObject == null)
        {
            colliderObject = transform.Find("Collider")?.gameObject;
        }
        
        gameObject.layer = LayerMask.NameToLayer("Player");
        colliderObject.layer = LayerMask.NameToLayer("PlayerCollision");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        PlayerData data = SaveSystem.LoadGame();

        if (data != null)
        {
            maxHealth = data.maxHealth;
            maxStamina = data.maxStamina;
            ClassManager.Instance.PlayerBar.SetPlayerBarLevel(data.playerBarLevel);
        }

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        
        ClassManager.Instance.Health.SetMaxHealth(maxHealth);
        ClassManager.Instance.Health.SetHealth();
    
        ClassManager.Instance.Stamina.SetMaxStamina(maxStamina);
        ClassManager.Instance.Stamina.SetStamina();
    }
    
    public void SetPaused(bool paused)
    {
        if (paused)
        {
            ClassManager.Instance.Health.SetMaxHealth(maxHealth);
            ClassManager.Instance.Health.SetHealth();
            ClassManager.Instance.Stamina.SetMaxStamina(maxStamina);
            ClassManager.Instance.Stamina.SetStamina();

            ClassManager.Instance.Movement.enabled = false;
            ClassManager.Instance.Dodge.enabled = false;
            ClassManager.Instance.Attack.enabled = false;
            rb.velocity = Vector2.zero; 
            ClassManager.Instance.StateMachine.SetIdle(); 
        }
        else
        {
            ClassManager.Instance.Movement.enabled = true;
            ClassManager.Instance.Dodge.enabled = true;
            ClassManager.Instance.Attack.enabled = true;
        }
    }

    void Update()
    {      
        if (ClassManager.Instance.DivinePoint.isPaused) 
        {
            SetPaused(true);
            return;
        }
        else 
        {
            SetPaused(false);
        }
        
        ClassManager.Instance.Movement.HandleMovement();
        ClassManager.Instance.Dodge.UpdateDodge();

        if (Input.GetKeyDown(KeyCode.F1)) { SaveSystem.ResetSave(); }
        // if (Input.GetKeyDown(KeyCode.F2)) { SaveSystem.SaveGame(ClassManager.Instance.Movement, ClassManager.Instance.LevelSystem, ClassManager.Instance.SkillTreeManager, this, SoundManager.Instance); }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;  
        currentHealth += amount;  
        currentHealth = Mathf.Min(currentHealth, maxHealth); 

        ClassManager.Instance.Health.SetMaxHealth(maxHealth); 
        ClassManager.Instance.Health.SetHealth(); 
        
        ClassManager.Instance.Health.UpdateHealthBarVisuals();
    }

    public void IncreaseMaxStamina(int amount)
    {
        maxStamina += amount;
        currentStamina += amount;
        currentStamina = Mathf.Min(currentStamina, maxStamina);

        ClassManager.Instance.Stamina.SetMaxStamina(maxStamina);
        ClassManager.Instance.Stamina.SetStamina();
    }

    public void TakeDamage(int amount)
    {
        if (damageTaken || !canTakeDamage) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        ClassManager.Instance.Health.SetHealth();

        damageTaken = true;

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        ClassManager.Instance.Health.SetHealth();
    }

    public void RestoreStamina()
    {
        currentStamina = maxStamina;
        ClassManager.Instance.Stamina.SetStamina();
    }

    public void Respawn()
    {
        PlayerData data = SaveSystem.LoadGame();
    
        if (data == null) return;
        
        ClassManager.Instance.Movement.transform.position = new Vector3(data.checkpointX, data.checkpointY, data.checkpointZ);
        
        maxHealth = data.maxHealth;
        maxStamina = data.maxStamina;
        
        ClassManager.Instance.PlayerBar.SetPlayerBarLevel(data.playerBarLevel);
        ClassManager.Instance.Health.SetMaxHealth(maxHealth);
        ClassManager.Instance.Health.SetHealth();
        currentHealth = maxHealth;
        ClassManager.Instance.Stamina.SetMaxStamina(maxStamina);
        ClassManager.Instance.Stamina.SetStamina();
        currentStamina = maxStamina;
        
        ClassManager.Instance.LevelSystem.LoadSavedData(data);
        ClassManager.Instance.SkillTreeManager.LoadSavedAbilities(data);
        ClassManager.Instance.RespawnManager.RespawnAllEnemies();

        RestoreHealth();
    }
}