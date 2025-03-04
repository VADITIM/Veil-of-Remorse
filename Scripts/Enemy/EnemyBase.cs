using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public LevelSystem LevelSystem;
    
    protected int health;
    protected int experiencePoints;

    public int GetHealth() => health;
    public int GetExperiencePoints() => experiencePoints;

    protected virtual void Start()
    {
        LevelSystem = FindObjectOfType<LevelSystem>();
        
        RespawnManager.Instance.RegisterEnemy(this);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health > 0) return;
        Die();
    }

    protected virtual void Die()
    {
        if (RespawnManager.Instance != null)
        {
            RespawnManager.Instance.SaveEnemyValues(this);
        }
        Destroy(gameObject);
        if (LevelSystem != null)
        {
            LevelSystem.GainExperience(experiencePoints);
        }
    }

    public virtual void SetInitialValues(int newHealth, int newExp)
    {
        health = newHealth;
        experiencePoints = newExp;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(50);
            }
        }
    }
}