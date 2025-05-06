using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] LevelUpPopup LevelUpPopup;
    [SerializeField] GameObject LevelUpPopupObject;
    
    private int level = 1;
    private int essence = 0;
    private int experience = 0;
    private int experienceToNextLevel;
    
    public int Level => level;
    public int ExperienceToNextLevel => experienceToNextLevel;
    [SerializeField] private GameObject HUD;
    
    public int GetEssence() { return essence; }
    public int GetExperience() { return experience; }
    public int GetExperienceToNextLevel() { return experienceToNextLevel; }

    void Start()
    {
        LevelUpPopup = FindObjectOfType<LevelUpPopup>();
        LevelUpPopupObject = FindObjectOfType<LevelUpPopup>().gameObject;
        LevelUpPopupObject.SetActive(false);
        

        SaveData();
        experienceToNextLevel = level * 100;
        ClassManager.Instance.UIManager.UpdateEssenceText();
    }

    void Update()
    {
        // ShowLevel();
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        Debug.Log($"Gained: {amount} XP.\nNeeded: {experienceToNextLevel - experience}.");
        while (experience >= experienceToNextLevel)
        {
            LevelUp();
            LevelUpPopup.ShowLevelUpPopup();
        }
    }
    
    public void LevelUp()
    {
        level++;
        essence++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = level * 100;
        Debug.Log("\nLEVELED UP: " + Level);
        ClassManager.Instance.UIManager.UpdateEssenceText();
    }

    public void UseEssence(int amount)
    {
        if (essence >= amount)
        {
            essence -= amount;
            ClassManager.Instance.UIManager.UpdateEssenceText();
        }
        // else { Debug.Log("Not enough essence to use."); }
    }

    public void KindlePower(int requiredEssence)
    {
        if (essence >= requiredEssence)
        {
            essence -= requiredEssence;
            
            HUD.SetActive(true);
            ClassManager.Instance.PlayerBar.UpgradePlayerBar();
            
            ClassManager.Instance.Player.IncreaseMaxHealth(50);
            ClassManager.Instance.Player.IncreaseMaxStamina(10);
            ClassManager.Instance.Health.SetHealth();
            ClassManager.Instance.Stamina.SetStamina();
            HUD.SetActive(false);
            
            SaveSystem.SaveGame(ClassManager.Instance.Movement, this, ClassManager.Instance.SkillTreeManager, ClassManager.Instance.Player, SoundManager.Instance);
            
            ClassManager.Instance.UIManager.UpdateEssenceText();
        }
        // else { Debug.Log("Not enough essence to kindle power."); }
    }

    

    private void SaveData()
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        {
            level = data.level;
            essence = data.essence;
            experience = data.experience;
            experienceToNextLevel = data.experienceToNextLevel; 
            transform.position = new Vector3(data.checkpointX, data.checkpointY, data.checkpointZ);
            
        }
    }

    public void LoadSavedData(PlayerData data)
    {
        level = data.level;
        essence = data.essence;
        experience = data.experience;
        experienceToNextLevel = data.experienceToNextLevel;
    
        Player player = FindObjectOfType<Player>();
        player.maxHealth = data.maxHealth;
        player.currentHealth = data.currentHealth;
        ClassManager.Instance.Health.SetMaxHealth(player.maxHealth);
        ClassManager.Instance.Health.SetHealth();

        player.maxStamina = data.maxStamina;
        player.currentStamina = data.currentStamina;
        ClassManager.Instance.Stamina.SetMaxStamina(player.maxStamina);
        ClassManager.Instance.Stamina.SetStamina();

        ClassManager.Instance.PlayerBar.SetPlayerBarLevel(data.playerBarLevel);
    }

    private void ShowLevel()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogWarning("\nLevel: " + level);
            Debug.LogWarning("\nXP: " + experience);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.LogWarning("\nMax XP: " + experienceToNextLevel);
            Debug.LogWarning("\nXP needed: " + (experienceToNextLevel - experience));
        }
    }
}