using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public float checkpointX, checkpointY, checkpointZ;
    public int level;
    public int essence;
    public int experience;
    public int experienceToNextLevel;
    public int currentHealth;
    public List<string> unlockedAbilities;
    public float cooldownReduction;

    public PlayerData(Movement movement, LevelSystem levelSystem, AbilityManager abilityManager, Player player)
    {
        checkpointX = movement.transform.position.x;
        checkpointY = movement.transform.position.y;
        checkpointZ = movement.transform.position.z;

        level = levelSystem.Level;
        essence = levelSystem.GetEssence();
        experience = levelSystem.GetExperience();
        experienceToNextLevel = levelSystem.GetExperienceToNextLevel();
        currentHealth = player.currentHealth;

        unlockedAbilities = new List<string>();
        foreach (var ability in abilityManager.unlockedAbilities)
        {
            if (ability.Value) // Only store unlocked abilities
            {
                unlockedAbilities.Add(ability.Key);
            }
        }
    }
}
