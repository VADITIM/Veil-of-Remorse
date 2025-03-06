using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour
{
    Movement Movement;
    Player Player;
    LevelSystem LevelSystem;

    [SerializeField] private List<Ability> abilities = new List<Ability>();
    public Dictionary<string, bool> unlockedAbilities = new Dictionary<string, bool>();

    void Start()
    {
        Movement = FindObjectOfType<Movement>();
        Player = FindObjectOfType<Player>();
        LevelSystem = FindObjectOfType<LevelSystem>();

        LoadSavedAbilities(SaveSystem.LoadGame());
        UpdateAbilityStates();
    }

    public void UnlockAbility(string abilityID)
    {
        Ability ability = abilities.Find(a => a.ID == abilityID);
        if (ability == null)
        {
            Debug.LogError("Ability not found: " + abilityID);
            return;
        }

        if (LevelSystem.GetEssence() < ability.requiredEssence)
        {
            Debug.Log("Not enough essence.");
            return;
        }

        LevelSystem.UseEssence(ability.requiredEssence);
        unlockedAbilities[abilityID] = true;
        SaveUnlockedAbilities();
        UpdateAbilityStates();

        // Disable and fade the button
        StartCoroutine(FadeOutButton(ability.abilityButton));
    }

    private void UpdateAbilityStates()
    {
        foreach (Ability ability in abilities)
        {
            if (unlockedAbilities.ContainsKey(ability.ID) && unlockedAbilities[ability.ID])
            {
                ability.abilityButton.interactable = false; // Disable interaction
                StartCoroutine(FadeOutButton(ability.abilityButton)); // Fade out
            }
        }
    }

    private IEnumerator FadeOutButton(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage == null) yield break;

        float duration = 0.5f;
        float elapsedTime = 0f;
        Color startColor = buttonImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.3f); // 30% visibility

        button.interactable = false; // Disable clicking

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            buttonImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }
    }

    public void LoadSavedAbilities(PlayerData data)
    {
        unlockedAbilities.Clear();
        foreach (string abilityID in data.unlockedAbilities)
        {
            unlockedAbilities[abilityID] = true;
        }
        UpdateAbilityStates();
    }

    private void SaveUnlockedAbilities()
    {
        foreach (var ability in unlockedAbilities)
        {
            SaveSystem.SaveBool(ability.Key, ability.Value);
        }
        SaveSystem.SaveGame(Movement, LevelSystem, this, Player);
    }

    public bool IsAbilityUnlocked(string abilityID)
    {
        return unlockedAbilities.ContainsKey(abilityID) && unlockedAbilities[abilityID];
    }
}

[System.Serializable]
public class Ability
{
    public string ID;
    public Button abilityButton; // Button component instead of GameObject
    public int requiredEssence;
}
