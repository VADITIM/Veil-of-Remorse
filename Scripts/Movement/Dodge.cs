using UnityEngine;

// TEILS AI GENERATED

public class Dodge : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] Movement Movement;
    [SerializeField] AttackLogic Attack;
    private LayerMask wallLayerMask;

    public float dodgeDistance = 4f; 
    public float dodgeTime = .3f; 
    public float dodgeTimer;
    public float cooldown = 1f; 
    public float cooldownTimer;
    private float cooldownReduction = .0f;

    private int staminaCost = 20; 

    private Stamina staminaManager;

    private float GetCooldown() { return cooldown; }

    public bool isDodging = false;
    public bool canDodge = true;

    private Vector2 dodgeDirection; 
    private Vector2 startPosition; 

    void Start()
    {
        Player = GetComponent<Player>();
        Movement = GetComponent<Movement>();
        Attack = GetComponent<AttackLogic>();
        staminaManager = FindObjectOfType<Stamina>();
        wallLayerMask = LayerMask.GetMask("Wall"); 
    }

    public void UpdateDodge()
    {
        HandleDodge();
        UpdateCooldown();

        if (isDodging)
        {
            float elapsedTime = Time.time - (dodgeTimer - dodgeTime);
            float t = Mathf.Clamp01(elapsedTime / dodgeTime);
            Vector2 targetPosition = startPosition + (dodgeDirection * dodgeDistance);
            transform.position = new Vector3(Vector2.Lerp(startPosition, targetPosition, t).x, Vector2.Lerp(startPosition, targetPosition, t).y, transform.position.z);
        }
        else 
         dodgeTimer = 0;
    }

    public void ReduceDodgeCooldown(float reduction)
    {
        cooldownReduction = reduction;
        cooldown -= cooldown * cooldownReduction;
        Debug.Log("Dodge cooldown reduced by " + (cooldownReduction * 100) + "%");
    }
    
    private void HandleDodge()
    {
        if (Input.GetKey(KeyCode.Space) && !isDodging && canDodge)
        {
            if (!Movement.IsMoving()) return;
            if (Attack.IsAttacking(attacking: true)) return;
            
            if (!staminaManager.UseStamina(staminaCost) || isDodging)
            {
                return;
            }

            isDodging = true;
            canDodge = false;
            Player.canTakeDamage = false;

            dodgeDirection = Movement.movement.normalized;

            if (dodgeDirection.magnitude < 0.1f)
            {
                dodgeDirection = new Vector2(Movement.transform.right.x * (Movement.GetComponent<SpriteRenderer>().flipX ? -1 : 1), 0).normalized;
            }

            startPosition = transform.position;
            dodgeTimer = Time.time + dodgeTime;
        }

        if (isDodging)
        {
            float elapsedTime = Time.time - (dodgeTimer - dodgeTime);
            float t = Mathf.Clamp01(elapsedTime / dodgeTime);
            Vector2 targetPosition = startPosition + (dodgeDirection * dodgeDistance);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dodgeDirection, 0.5f, wallLayerMask);
            
            if (hit.collider != null)
            {
                isDodging = false;
                Player.canTakeDamage = true;
                return;
            }

            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
        }

        if (isDodging && Time.time >= dodgeTimer)
        {
            isDodging = false;
            Player.canTakeDamage = true;
        }
    }

    private void UpdateCooldown()
    {
        if (!canDodge)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
            {
                canDodge = true;
                cooldownTimer = cooldown;
            }
        }
    }

    public Vector2 GetDodgeDirection()
    {
        return dodgeDirection;
    }
}