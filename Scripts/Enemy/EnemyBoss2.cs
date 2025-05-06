using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BossEnemy2 : EnemyBase
{
    [Header("Boss Settings")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private int regenerationAmount = 100;
    [SerializeField] private float healthBarYOffset = 1.5f;

    public event Action OnEnemyDeath;

    private GameObject healthBarUI;
    private Slider healthSlider;
    private bool hasRegenerated = false;
    private int maxHealth;
    
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    protected override void Start()
    {
        base.Start();
        
        health = 120;
        experiencePoints = 1000;
        damage = 60;

        maxHealth = health;
        SetupHealthBar();
    }

    protected override void Update()
    {
        base.Update();
        UpdateHealthBar();
    }

    private void SetupHealthBar()
    {
        if (healthBarPrefab == null)
        {
            return;
        }

        healthBarUI = Instantiate(healthBarPrefab, transform.position + Vector3.up * healthBarYOffset, Quaternion.identity, transform);
        healthSlider = healthBarUI.GetComponentInChildren<Slider>();

        if (healthSlider == null)
        {
            return;
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (!hasRegenerated && health > 0 && health <= Mathf.CeilToInt(maxHealth * 0.1f))
        {
            RegenerateHealth();
            hasRegenerated = true;
        }

        UpdateHealthBar();
    }

    private void RegenerateHealth()
    {
        health += regenerationAmount;
        if (health > maxHealth) health = maxHealth;
    }

    private void UpdateHealthBar()
    {
        if (healthSlider == null) return;

        healthBarUI.transform.position = transform.position + Vector3.up * healthBarYOffset;
        healthSlider.value = Mathf.Clamp(health, 0, maxHealth);
    }

    public override void Die()
    {
        OnEnemyDeath?.Invoke();

        base.Die();
        if (healthBarUI != null)
            Destroy(healthBarUI);
    }
}
