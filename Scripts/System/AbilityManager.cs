using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    LevelSystem LevelSystem;
    
    [SerializeField] private GameObject abilityOne;
    [SerializeField] private GameObject abilityTwo;
    [SerializeField] private GameObject abilityThree;
    
    public bool abilityOneUnlocked = false;
    public bool abilityTwoUnlocked = false;
    public bool abilityThreeUnlocked = false;
    
    void Start()
    {
        LevelSystem = FindObjectOfType<LevelSystem>();
        
        abilityOneUnlocked = SaveSystem.LoadBool("AbilityOneUnlocked");
        abilityTwoUnlocked = SaveSystem.LoadBool("AbilityTwoUnlocked");
        abilityThreeUnlocked = SaveSystem.LoadBool("AbilityThreeUnlocked");

        UpdateAbilityStates();
    }

    void Update()
    {
        UpdateAbilityStates();
    }
    
    public void LoadSavedAbilities(PlayerData data)
    {
        abilityOneUnlocked = data.abilityOneUnlocked;
        abilityTwoUnlocked = data.abilityTwoUnlocked;
        abilityThreeUnlocked = data.abilityThreeUnlocked;

        UpdateAbilityStates();
        Debug.Log("\nAbilities reset to last save state.");
    }

    private void UpdateAbilityStates()
    {
        if (abilityOneUnlocked)
        {
            abilityOne.SetActive(false);
        }
        if (abilityTwoUnlocked)
        {
            abilityTwo.SetActive(false);
        }
        if (abilityThreeUnlocked)
        {
            abilityThree.SetActive(false);
        }
    }

    public void UnlockAbilityOne()
    {
        abilityOneUnlocked = true;
        SaveSystem.SaveBool("AbilityOneUnlocked", true);
    }

    public void UnlockAbilityTwo()
    {
        abilityTwoUnlocked = true;
        SaveSystem.SaveBool("AbilityTwoUnlocked", true);
    }

    public void UnlockAbilityThree()
    {
        abilityThreeUnlocked = true;
        SaveSystem.SaveBool("AbilityThreeUnlocked", true);
    }

}