using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private int level = 1;
    private int experience = 0;
    private int experienceToNextLevel = 100;
    
    public int Level => level;
    
    public int GetExperience()
    {
        return experience;
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        Debug.Log("Gained " + amount + " experience. Current experience: " + experience);
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }
    
    void Start()
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        {
            level = data.level;
            experience = data.experience;
            transform.position = new Vector3(data.checkpointX, data.checkpointY, 0);
        }
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel += 10;
        Debug.Log("Level up! Current level: " + Level);
    }

    void Update()
    {
        ShowLevel();
        ShowNextXP();
    }

    private void ShowLevel()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogWarning("Current level: " + level);
            Debug.LogWarning("Current experience: " + experience);
        }
    }
    private void ShowNextXP()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.LogWarning("Max experience: " + experienceToNextLevel);
            Debug.LogWarning("Experience to next level: " + (experienceToNextLevel - experience));
        }
    }
}
