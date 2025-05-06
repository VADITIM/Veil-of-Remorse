using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    public Image playerBarImage;
    public Sprite[] playerBarSprites; 
    public Health Health;
    public Stamina Stamina;
    
    public int currentLevel = 0;
    public const int MAX_LEVEL = 11;

    public int CurrentLevel
    {
        get { return currentLevel; }
    }

    void Start()
    {
        Health = FindObjectOfType<Health>();
        Stamina = FindObjectOfType<Stamina>();

        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        {
            SetPlayerBarLevel(data.playerBarLevel);
        }
        else
        {
            playerBarImage.sprite = playerBarSprites[0];
        }

    }

    public bool MaxLevelReached()
    {
        return currentLevel >= MAX_LEVEL;
    }

    public void SetPlayerBarLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 0, playerBarSprites.Length - 1);
        
        if (playerBarImage != null && playerBarSprites.Length > currentLevel)
        {
            playerBarImage.sprite = playerBarSprites[currentLevel];
            float baseScale = 2.481241f;
            playerBarImage.transform.localScale = baseScale * Vector3.one;
        }
        
        // Force update health and stamina UI even if paused
        Player player = FindObjectOfType<Player>();
        bool wasPaused = false;
        if (player != null)
        {
            DivinePoint divinePoint = FindObjectOfType<DivinePoint>();
            if (divinePoint != null && divinePoint.isPaused)
            {
                wasPaused = true;
                player.SetPaused(false); // Temporarily unpause to allow updates
            }
        }
        
        Health health = FindObjectOfType<Health>();
        if (health != null)
        {
            health.UpdateHealthBarVisuals();
        }
        
        Stamina stamina = FindObjectOfType<Stamina>();
        if (stamina != null)
        {
            stamina.UpdateStaminaBarVisuals();
        }
        
        // Restore pause state if needed
        if (wasPaused && player != null)
        {
            player.SetPaused(true);
        }
    }
    public void UpgradePlayerBar()
    {
        if (currentLevel < MAX_LEVEL)
        {
            currentLevel++;
            if (playerBarImage != null && playerBarSprites.Length > currentLevel)
            {
                playerBarImage.sprite = playerBarSprites[currentLevel];
            }
            
            // Find player and restore health to max
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                // Force update health and stamina even if paused
                bool wasPaused = false;
                DivinePoint divinePoint = FindObjectOfType<DivinePoint>();
                if (divinePoint != null && divinePoint.isPaused)
                {
                    wasPaused = true;
                    player.SetPaused(false);  // Temporarily unpause to allow updates
                }
                
                player.currentHealth = player.maxHealth;
                player.currentStamina = player.maxStamina;
                
                // Update UI
                if (Health != null)
                {
                    Health.SetHealth();
                    Health.UpdateHealthBarVisuals();
                }
                
                if (Stamina != null)
                {
                    Stamina.SetStamina();
                    Stamina.UpdateStaminaBarVisuals();
                }
                
                // Restore pause state if needed
                if (wasPaused)
                {
                    player.SetPaused(true);
                }
                
                // Save the game to persist these changes
                SaveSystem.SaveGame(ClassManager.Instance.Movement, ClassManager.Instance.LevelSystem, 
                                   FindObjectOfType<SkillTreeManager>(), player, 
                                   SoundManager.Instance);
            }
            
            Debug.Log($"PlayerBar upgraded to level {currentLevel}");
        }
    }
}