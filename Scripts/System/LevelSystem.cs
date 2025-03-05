using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private int level = 1;
    public int skillPoints = 0;
    private int experience = 0;
    private int experienceToNextLevel;
    
    public int Level => level;
    public int ExperienceToNextLevel => experienceToNextLevel;
    
    public int GetSkillPoints() { return skillPoints; }
    public int GetExperience() { return experience; }
    public int GetExperienceToNextLevel() { return experienceToNextLevel; }

    void Start()
    {
        SaveData();
        experienceToNextLevel = level * 100;
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
        skillPoints++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = level * 100;
        Debug.Log("\nLEVELED UP: " + Level);
    }

    public void UseSkillPoints(int amount)
    {
        skillPoints -= amount;
    }

    private void SaveData()
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        {
            level = data.level;
            experience = data.experience;
            experienceToNextLevel = data.experienceToNextLevel; 
            transform.position = new Vector3(data.checkpointX, data.checkpointY, 0);
        }
    }

    public void LoadSavedData(PlayerData data)
    {
        level = data.level;
        experience = data.experience;
        experienceToNextLevel = data.experienceToNextLevel;
        
        Debug.Log($"\nLevel and XP reset to last save: Level {level}, XP {experience}.");
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