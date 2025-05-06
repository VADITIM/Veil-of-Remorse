using System.Collections.Generic;

// AI GENERATED

[System.Serializable]
public class PlayerData
{
    public float checkpointX, checkpointY, checkpointZ;
    public int level;
    public int essence;
    public int experience;
    public int experienceToNextLevel;
    public int currentHealth;
    public int maxHealth;
    public float currentStamina;
    public int maxStamina;
    public int playerBarLevel;
    public List<string> unlockedSkills;
    public float cooldownReduction;
    public float musicVolume;
    public float sfxVolume;
    public bool musicMuted;
    public bool sfxMuted;
    public string currentKeyID;

    public PlayerData(Movement movement, LevelSystem levelSystem, SkillTreeManager abilityManager, Player player, SoundManager soundManager)
    {
        checkpointX = movement.transform.position.x;
        checkpointY = movement.transform.position.y;
        checkpointZ = movement.transform.position.z;

        level = levelSystem.Level;
        essence = levelSystem.GetEssence();
        experience = levelSystem.GetExperience();
        experienceToNextLevel = levelSystem.GetExperienceToNextLevel();
        currentHealth = player.currentHealth;
        maxHealth = player.maxHealth;
        currentStamina = player.currentStamina;
        maxStamina = player.maxStamina;
        playerBarLevel = ClassManager.Instance.PlayerBar.currentLevel;
        currentKeyID = Key.GetCurrentKey();

        unlockedSkills = new List<string>();
        foreach (var ability in abilityManager.unlockedSkills)
        {
            if (ability.Value)
            {
                unlockedSkills.Add(ability.Key);
            }
        }

        musicVolume = soundManager.musicSource.volume;
        sfxVolume = soundManager.sfxSource.volume;
        musicMuted = soundManager.musicSource.mute;
        sfxMuted = soundManager.sfxSource.mute;
    }
}