using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
    public LevelSystem LevelSystem;
    private XPPopupManager XPPopupManager;
    public float speed = 2f; 

    [SerializeField] protected int health;
    [SerializeField] protected int experiencePoints;
    protected int damage;
    public bool isMoving;
    public bool isAttacking;
    public bool isDamaged;
    public bool isDead;
    private float immuneTime = 0.15f;
    
    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

    public float windupTime;
    public float attackDuration;

    public Rigidbody2D rb; 

    public int GetHealth() => health;
    public int GetExperiencePoints() => experiencePoints;

    protected virtual void Attack() { isAttacking = true; }
    public bool IsAttacking() { return isAttacking; }
    public bool IsMoving() { return isMoving; }
    public bool IsDamaged() { return isDamaged; }
    public bool IsDead() { return isDead; }

    protected virtual void Start()
    {
        LevelSystem = FindObjectOfType<LevelSystem>();
        XPPopupManager = FindObjectOfType<XPPopupManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        RespawnManager.Instance.RegisterEnemy(this);
    }

    protected virtual void Update()
    {
        isMoving = rb.velocity.magnitude > 0;

        if (isDamaged)
        {
            immuneTime -= Time.deltaTime;
            if (immuneTime <= 0)
            {
                isDamaged = false;
                immuneTime = 0.15f;
            }
        }
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }
    
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        isDamaged = true;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashEffect());
        
        StartCoroutine(ResetBool(isDamaged, .3f));

        if (health > 0) return;
        Die();
    }

    private IEnumerator FlashEffect()
    {
        if (spriteRenderer == null) 
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        Color originalColor = spriteRenderer.color;
        float flashDuration = 0.5f;
        float elapsed = 0f;
        
        while (elapsed < flashDuration)
        {
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(elapsed * 10, 1));
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        spriteRenderer.color = Color.white;
    }

    private IEnumerator ResetBool(bool value, float time)
    {
        yield return new WaitForSeconds(time);
        value = false;
    }

    public virtual void Die()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
            
        RespawnManager.Instance.SaveEnemyValues(this);

        Destroy(gameObject);
        isDead = true;

        if (ClassManager.Instance.AttackLogic.IsAttacking(true))
            XPPopupManager.ShowXPPopup(transform.position, experiencePoints);
            LevelSystem.GainExperience(experiencePoints);
    }

    public virtual void SetInitialValues(int newHealth, int newExp)
    {
        if (health == 0)
        {
            health = newHealth;
        }
        experiencePoints = newExp;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage / 2);
            }
        }
    }
}
