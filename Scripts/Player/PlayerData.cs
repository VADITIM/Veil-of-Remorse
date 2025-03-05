[System.Serializable]
public class PlayerData
{
    public float checkpointX, checkpointY, checkpointZ;
    public int level;
    public int skillPoints;
    public int experience;
    public int experienceToNextLevel;
    public int currentHealth;
    public bool abilityOneUnlocked;
    public bool abilityTwoUnlocked;
    public bool abilityThreeUnlocked;

    public PlayerData(Movement movement, LevelSystem levelSystem, AbilityManager abilityManager, Player player)
    {
        checkpointX = movement.transform.position.x;
        checkpointY = movement.transform.position.y;
        checkpointZ = movement.transform.position.z;

        level = levelSystem.Level;
        skillPoints = levelSystem.GetSkillPoints();
        experience = levelSystem.GetExperience();
        experienceToNextLevel = levelSystem.GetExperienceToNextLevel();

        currentHealth = player.currentHealth;

        abilityOneUnlocked = abilityManager.abilityOneUnlocked;
        abilityTwoUnlocked = abilityManager.abilityTwoUnlocked;
        abilityThreeUnlocked = abilityManager.abilityThreeUnlocked;
    }
}