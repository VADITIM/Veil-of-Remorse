[System.Serializable]
public class PlayerData
{
    public float checkpointX, checkpointY;
    public int level;
    public int experience;

    public PlayerData(Movement movement, LevelSystem levelSystem, AbilityManager abilityManager)
    {
            checkpointX = movement.transform.position.x;
            checkpointY = movement.transform.position.y;

            level = levelSystem.Level;
            experience = levelSystem.GetExperience();
    }

}
