using UnityEngine;

public class Dodge : MonoBehaviour
{
    [SerializeField] Movement Movement;

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
        Movement = GetComponent<Movement>();
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
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
        }
    }

    private void HandleDodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging && canDodge)
        {
            isDodging = true;
            canDodge = false;

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
            transform.position = targetPosition;
            isDodging = false;
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