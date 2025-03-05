using UnityEngine;

public class Dodge : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] Movement Movement;
    [SerializeField] Attack Attack;

    private float dodgeDistance = 4f; 
    private float dodgeTime = .3f; 
    private float dodgeTimer;
    private float cooldown = 1f; 
    private float cooldownTimer;

    public bool isDodging = false;
    public bool canDodge = true;

    private Vector2 dodgeDirection; 
    private Vector2 startPosition; 

    void Start()
    {
        Player = GetComponent<Player>();
        Movement = GetComponent<Movement>();
        Attack = GetComponent<Attack>();
    }

    void Update()
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
    }

    private void HandleDodge()
    {
        
        if (Input.GetKey(KeyCode.Space) && !isDodging && canDodge)
        {
            if (!Movement.IsMoving()) return;
            if (Attack.IsAttacking(attacking: true)) return;

            isDodging = true;
            canDodge = false;
            Player.canTakeDamage = false;

            Rigidbody2D rb = Movement.GetComponent<Rigidbody2D>();
            dodgeDirection = rb.velocity.normalized;
            if (dodgeDirection.magnitude < 0.1f) 
            {
                dodgeDirection = new Vector2(Movement.transform.right.x * (Movement.GetComponent<SpriteRenderer>().flipX ? -1 : 1), 0).normalized;
            }

            startPosition = transform.position;

            dodgeTimer = Time.time + dodgeTime;
        }

        if (isDodging && Time.time >= dodgeTimer)
        {
            Vector2 targetPosition = startPosition + (dodgeDirection * dodgeDistance);
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
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