using System.Collections.Generic;
using UnityEngine;

// TEILS AI GENERATED

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Movement movement;
    [SerializeField] private Dodge dodge;
    [SerializeField] private AttackLogic attackLogic;

    // Base stats
    [Header("Base Stats")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float baseDamage = 1f;
    [SerializeField] private float baseCritChance = 0.05f;
    [SerializeField] private float baseCritMultiplier = 1.5f;
    [SerializeField] private float baseDodgeCooldown = 1f;
    [SerializeField] private float baseDodgeDistance = 4f;

    // Current calculated stats
    [Header("Current Stats")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentDamage;
    [SerializeField] private float currentCritChance;
    [SerializeField] private float currentCritMultiplier;
    [SerializeField] private float currentDodgeCooldown;
    [SerializeField] private float currentDodgeDistance;

    // Buff tracking
    private Dictionary<string, List<float>> activeBuffs = new Dictionary<string, List<float>>();

    void Start()
    {
        player = GetComponent<Player>();
        movement = GetComponent<Movement>();
        dodge = GetComponent<Dodge>();
        attackLogic = GetComponent<AttackLogic>();

        // Initialize stats
        InitializeStats();
    }

    private void InitializeStats()
    {
        // Set initial values from base stats
        currentSpeed = baseSpeed;
        currentDamage = baseDamage;
        currentCritChance = baseCritChance;
        currentCritMultiplier = baseCritMultiplier;
        currentDodgeCooldown = baseDodgeCooldown;
        currentDodgeDistance = baseDodgeDistance;

        // Apply stats to components
        ApplyStats();
    }

    public void ApplyStats()
    {
        // Apply the stats to various components
        movement.speed = currentSpeed;
        dodge.cooldown = currentDodgeCooldown;
        dodge.dodgeDistance = currentDodgeDistance;
        
        // You would need to add these properties to your AttackLogic class
        if (attackLogic != null)
        {
            // Example: attackLogic.damage = currentDamage;
            // Example: attackLogic.critChance = currentCritChance;
            // Example: attackLogic.critMultiplier = currentCritMultiplier;
        }
    }

    // Apply a buff to a specific stat
    public void ApplyBuff(string statName, float value)
    {
        if (!activeBuffs.ContainsKey(statName))
        {
            activeBuffs[statName] = new List<float>();
        }
        
        activeBuffs[statName].Add(value);
        RecalculateStats();
    }

    // Remove a specific buff
    public void RemoveBuff(string statName, float value)
    {
        if (activeBuffs.ContainsKey(statName))
        {
            activeBuffs[statName].Remove(value);
            if (activeBuffs[statName].Count == 0)
            {
                activeBuffs.Remove(statName);
            }
            RecalculateStats();
        }
    }

    // Apply a permanent stat increase (for skill tree unlocks)
    public void ApplyPermanentStatIncrease(string statName, float value)
    {
        switch (statName)
        {
            case "speed":
                baseSpeed += value;
                break;
            case "damage":
                baseDamage += value;
                break;
            case "critChance":
                baseCritChance += value;
                break;
            case "critMultiplier":
                baseCritMultiplier += value;
                break;
            case "dodgeCooldown":
                baseDodgeCooldown -= value; // Decrease for cooldown reduction
                break;
            case "dodgeDistance":
                baseDodgeDistance += value;
                break;
            case "maxHealth":
                player.maxHealth += Mathf.RoundToInt(value);
                break;
        }
        
        RecalculateStats();
    }

    // Recalculate all stats based on base values and active buffs
    private void RecalculateStats()
    {
        // Reset to base values
        currentSpeed = baseSpeed;
        currentDamage = baseDamage;
        currentCritChance = baseCritChance;
        currentCritMultiplier = baseCritMultiplier;
        currentDodgeCooldown = baseDodgeCooldown;
        currentDodgeDistance = baseDodgeDistance;

        // Apply all active buffs
        foreach (var buffPair in activeBuffs)
        {
            foreach (float value in buffPair.Value)
            {
                switch (buffPair.Key)
                {
                    case "speed":
                        currentSpeed += value;
                        break;
                    case "damage":
                        currentDamage += value;
                        break;
                    case "critChance":
                        currentCritChance += value;
                        break;
                    case "critMultiplier":
                        currentCritMultiplier += value;
                        break;
                    case "dodgeCooldown":
                        currentDodgeCooldown -= value; // Decrease for cooldown reduction
                        break;
                    case "dodgeDistance":
                        currentDodgeDistance += value;
                        break;
                }
            }
        }

        // Ensure stats don't go below minimum values
        currentSpeed = Mathf.Max(1f, currentSpeed);
        currentDamage = Mathf.Max(0.1f, currentDamage);
        currentCritChance = Mathf.Clamp01(currentCritChance);
        currentCritMultiplier = Mathf.Max(1f, currentCritMultiplier);
        currentDodgeCooldown = Mathf.Max(0.1f, currentDodgeCooldown);
        currentDodgeDistance = Mathf.Max(1f, currentDodgeDistance);

        // Apply the calculated stats
        ApplyStats();
    }
    
    // Get current stat value
    public float GetStat(string statName)
    {
        switch (statName)
        {
            case "speed":
                return currentSpeed;
            case "damage":
                return currentDamage;
            case "critChance":
                return currentCritChance;
            case "critMultiplier":
                return currentCritMultiplier;
            case "dodgeCooldown":
                return currentDodgeCooldown;
            case "dodgeDistance":
                return currentDodgeDistance;
            case "maxHealth":
                return player.maxHealth;
            case "currentHealth":
                return player.currentHealth;
            default:
                return 0f;
        }
    }
}