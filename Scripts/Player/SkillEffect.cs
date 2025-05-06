using UnityEngine;
using System;

// AI GENERATED / UNUSED

[System.Serializable]
public abstract class SkillEffect
{
    public abstract void Apply(Player player);
    public abstract string GetDescription();
}

[System.Serializable]
public class StatIncreaseEffect : SkillEffect
{
    public string statName;
    public float value;
    public string customDescription;

    public override void Apply(Player player)
    {
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.ApplyPermanentStatIncrease(statName, value);
            Debug.Log($"Applied {statName} increase by {value}");
        }
    }

    public override string GetDescription()
    {
        if (!string.IsNullOrEmpty(customDescription))
            return customDescription;
            
        string formattedValue = value.ToString("+0.##;-0.##");
        
        switch (statName)
        {
            case "speed":
                return $"Movement Speed {formattedValue}";
            case "damage":
                return $"Damage {formattedValue}";
            case "critChance":
                return $"Critical Chance {(value * 100)}%";
            case "critMultiplier":
                return $"Critical Damage {formattedValue}x";
            case "dodgeCooldown":
                return $"Dodge Cooldown {-value}s";
            case "dodgeDistance":
                return $"Dodge Distance {formattedValue}";
            case "maxHealth":
                return $"Max Health {formattedValue}";
            default:
                return $"{statName} {formattedValue}";
        }
    }
}

[System.Serializable]
public class AbilityUnlockEffect : SkillEffect
{
    public string abilityName;
    public string abilityDescription;
    public Action<Player> unlockAction;

    public AbilityUnlockEffect(string name, string description, Action<Player> action)
    {
        abilityName = name;
        abilityDescription = description;
        unlockAction = action;
    }

    public override void Apply(Player player)
    {
        if (unlockAction != null)
        {
            unlockAction(player);
            Debug.Log($"Unlocked ability: {abilityName}");
        }
    }

    public override string GetDescription()
    {
        return $"Unlocks {abilityName}: {abilityDescription}";
    }
}
