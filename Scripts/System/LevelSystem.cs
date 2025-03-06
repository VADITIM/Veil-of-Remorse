using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    UIManager UIManager;
    
    private int level = 1;
    private int essence = 0;
    private int experience = 0;
    private int experienceToNextLevel;
    
    public int Level => level;
    public int ExperienceToNextLevel => experienceToNextLevel;
    
    public int GetEssence() { return essence; }
    public int GetExperience() { return experience; }
    public int GetExperienceToNextLevel() { return experienceToNextLevel; }

    void Start()
    {
        UIManager = FindObjectOfType<UIManager>();

        SaveData();
        experienceToNextLevel = level * 100;
        UIManager.UpdateEssenceText();
    }

    void Update()
    {
        ShowLevel();
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        Debug.Log($"Gained: {amount} XP.\nNeeded: {experienceToNextLevel - experience}.");
        while (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    
    public void LevelUp()
    {
        level++;
        essence++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = level * 100;
        Debug.Log("\nLEVELED UP: " + Level);
        UIManager.UpdateEssenceText();
    }

    public void UseEssence(int amount)
    {
        if (essence >= amount)
        {
            essence -= amount;
            UIManager.UpdateEssenceText();
        }
        else
        {
            Debug.Log("Not enough essence to use.");
        }
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
        
        Debug.Log($"\nLevel, Essence, and XP reset to last save: Level {level}, Essence {essence}, XP {experience}.");
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