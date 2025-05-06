using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Skill
{
    public string ID;
    public string name;
    public string description;
    public Button skillButton;
    public int requiredEssence;
}

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;

    Movement Movement;
    Player Player;
    LevelSystem LevelSystem;

    [SerializeField] public List<Skill> skills = new List<Skill>();
    [SerializeField] public Dictionary<string, bool> unlockedSkills = new Dictionary<string, bool>();

    [Header("UI Elements")]
    [SerializeField] private GameObject skillDescriptionPanel;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;

    [SerializeField] private GameObject skillNamePanel;
    [SerializeField] private TextMeshProUGUI skillNameText;
    
    [SerializeField] private TextMeshProUGUI essenceRequiredText; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Movement = FindObjectOfType<Movement>();
        Player = FindObjectOfType<Player>();
        LevelSystem = FindObjectOfType<LevelSystem>();

        skillDescriptionPanel.SetActive(false);
        skillNamePanel.SetActive(false);
        LoadSavedAbilities(SaveSystem.LoadGame());
        UpdateSkillStates();
    }

    public void HoverAbility(string skillID, string name, string skillDescription)
    {
        Skill skill = skills.Find(a => a.ID == skillID);
        if (skill == null) return;

        skillNameText.text = name; 
        skillDescriptionText.text = skillDescription; 
        essenceRequiredText.text = $"Essence Required: {skill.requiredEssence}";

        bool hasEnoughEssence = LevelSystem.GetEssence() >= skill.requiredEssence;
        bool canBeUnlocked = CanUnlockSkill(skillID);
        bool isUnlocked = IsSkillUnlocked(skillID);

        if (isUnlocked)
        {
            essenceRequiredText.color = new Color(0, 0, 0, 0);
        }
        else if (hasEnoughEssence && canBeUnlocked)
        {
            essenceRequiredText.color = Color.green;
        }
        else
        {
            essenceRequiredText.color = Color.red;
        }

        skillNamePanel.SetActive(true);
        skillDescriptionPanel.SetActive(true);
    }

    public void ExitHover()
    {
        skillNamePanel.SetActive(false);
        skillDescriptionPanel.SetActive(false);
    }

    public void UnlockAbility(string skillID)
    {
        if (!CanUnlockSkill(skillID))
        {
            return;
        }

        Skill skill = skills.Find(a => a.ID == skillID);
        if (skill == null)
        {
            return;
        }

        if (LevelSystem.GetEssence() < skill.requiredEssence)
        {
            return;
        }

        LevelSystem.UseEssence(skill.requiredEssence);
        unlockedSkills[skillID] = true;
        SaveUnlockedAbilities();

        StartCoroutine(FadeOutButton(skill.skillButton));
        UpdateSkillStates();
    }

    private bool CanUnlockSkill(string skillID)
    {
        bool result = true;

        if ((skillID == "A1" || skillID == "A2" || skillID == "A3" ||
            skillID == "S1" || skillID == "M1" || skillID == "M2" || skillID == "M3") &&
            !IsSkillUnlocked("Dodge"))
        {
            result = false;
        }

        if (skillID == "A1i" && !IsSkillUnlocked("A1")) result = false;
        if (skillID == "A1ii" && !IsSkillUnlocked("A1i")) result = false;
        if (skillID == "A2i" && !IsSkillUnlocked("A2")) result = false;
        if (skillID == "A2ii" && !IsSkillUnlocked("A2i")) result = false;
        if (skillID == "A3i" && !IsSkillUnlocked("A3")) result = false;
        if (skillID == "A3ii" && !IsSkillUnlocked("A3i")) result = false;

        if (skillID == "M1i" && !IsSkillUnlocked("M1")) result = false;
        if (skillID == "M1ii" && !IsSkillUnlocked("M1i")) result = false;
        if (skillID == "M2i" && !IsSkillUnlocked("M2")) result = false;
        if (skillID == "M2ii" && !IsSkillUnlocked("M2i")) result = false;
        if (skillID == "M3i" && !IsSkillUnlocked("M3")) result = false;
        if (skillID == "M3ii" && !IsSkillUnlocked("M3i")) result = false;

        if (skillID == "S2" && !IsSkillUnlocked("S1")) result = false;
        if (skillID == "S3" && !IsSkillUnlocked("S2")) result = false;

        if (skillID == "S1i" && !IsSkillUnlocked("S1")) result = false;
        if (skillID == "S1ii" && !IsSkillUnlocked("S1")) result = false;
        if (skillID == "S2i" && !IsSkillUnlocked("S2")) result = false;
        if (skillID == "S2ii" && !IsSkillUnlocked("S2")) result = false;
        if (skillID == "S3i" && !IsSkillUnlocked("S3")) result = false;
        if (skillID == "S3ii" && !IsSkillUnlocked("S3")) result = false;

        return result;
    }

    public void UpdateSkillStates()
    {
        foreach (Skill skill in skills)
        {
            bool isUnlocked = IsSkillUnlocked(skill.ID);
            bool canBeUnlocked = CanUnlockSkill(skill.ID);

            skill.skillButton.interactable = !isUnlocked && canBeUnlocked;

            Image buttonImage = skill.skillButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                if (isUnlocked)
                {
                    buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, .9f);
                }
                else if (canBeUnlocked)
                {
                    buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, .6f);
                }
                else
                {
                    buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0f); // Set alpha to 0.2 instead of 0 (to prevent invisibility)
                }
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
        Color endColor = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, .9f);

        button.interactable = false;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            buttonImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        buttonImage.color = endColor; 
        UpdateSkillStates();
    }

    public void LoadSavedAbilities(PlayerData data)
    {
        unlockedSkills.Clear();
        foreach (string skillID in data.unlockedSkills)
        {
            unlockedSkills[skillID] = true;
        }
        UpdateSkillStates();
    }

    private void SaveUnlockedAbilities()
    {
        foreach (var skill in unlockedSkills)
        {
            SaveSystem.SaveBool(skill.Key, skill.Value);
        }
        SaveSystem.SaveGame(Movement, LevelSystem, this, Player, SoundManager.Instance);
    }

    public bool IsSkillUnlocked(string skillID)
    {
        return unlockedSkills.ContainsKey(skillID) && unlockedSkills[skillID];
    }
}