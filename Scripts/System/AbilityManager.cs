using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject abilityOne;
    [SerializeField] private GameObject abilityTwo;
    [SerializeField] private GameObject abilityThree;
    
    public bool abilityOneUnlocked = false;
    public bool abilityTwoUnlocked = false;
    public bool abilityThreeUnlocked = false;
    
    void Start()
    {
        abilityOneUnlocked = SaveSystem.LoadBool("AbilityOneUnlocked");
        abilityTwoUnlocked = SaveSystem.LoadBool("AbilityTwoUnlocked");
        abilityThreeUnlocked = SaveSystem.LoadBool("AbilityThreeUnlocked");

        UpdateAbilityStates();
    }

    void Update()
    {
        UpdateAbilityStates();
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

    public void IsAbilityUnlocked()
    {
        if (abilityOneUnlocked)
        {
            Debug.Log("Ability One Unlocked");
        }
        if (abilityTwoUnlocked)
        {
            Debug.Log("Ability Two Unlocked");
        }
        if (abilityThreeUnlocked)
        {
            Debug.Log("Ability Three Unlocked");
        }
    }
}