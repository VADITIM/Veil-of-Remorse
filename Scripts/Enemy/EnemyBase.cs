using UnityEngine;
using System.Collections.Generic; 

public class EnemyBase : MonoBehaviour
{
    public LevelSystem LevelSystem;
    public AIData aiData; 
    public float maxSpeed = 3f; 

    protected int health;
    protected int experiencePoints;
    protected int damage;
    public bool isMoving;
    public bool isAttacking;

    private Rigidbody2D rb; 

    public int GetHealth() => health;
    public int GetExperiencePoints() => experiencePoints;

    protected virtual void Start()
    {
        LevelSystem = FindObjectOfType<LevelSystem>();
        RespawnManager.Instance.RegisterEnemy(this);
        rb = GetComponent<Rigidbody2D>();
        rb = gameObject.AddComponent<Rigidbody2D>(); 
        rb.gravityScale = 0; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
        aiData.obstacles = new List<Transform>();
    }

    protected virtual void Update()
    {
        isMoving = rb.velocity.magnitude > 0;
    }

    protected virtual void Attack()
    {
        isAttacking = true;
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * maxSpeed;
    }
    
    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health > 0) return;
        Die();
    }

    public virtual void Die()
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

    public bool IsMoving()
    {
        return isMoving;
    }
    
    public bool IsAttacking()
    {
        return isAttacking;
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
                player.TakeDamage(damage);
            }
        }
    }
}