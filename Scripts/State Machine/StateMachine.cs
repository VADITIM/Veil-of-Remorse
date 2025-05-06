using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] Movement Movement;
    [SerializeField] Dodge Dodge;
    [SerializeField] AttackStateMachine AttackStateMachine; 

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody2D rb;

    private bool facingRight = true;
    private Vector2 lookDirection;
    private float directionChangeThreshold = 0.1f;

    private readonly string horizontalParam = "Horizontal";
    private readonly string verticalParam = "Vertical";
    private readonly string idleParam = "idle";
    private readonly string isMovingParam = "isMoving";
    private readonly string isDodgingParam = "isDodging";
    private readonly string damageTaken = "damageTaken";

    private string normalAttacking1;
    private string normalAttacking2;
    private string normalAttacking3;
    private string isHeavyAttackingParam;

    private void HandleAnimator()
    {
        Vector2 directionForAnimator = GetDodgeDirection();

        float horizontal = facingRight ? directionForAnimator.x : -directionForAnimator.x;
        float vertical = directionForAnimator.y;

        animator.SetBool(idleParam, !Movement.IsMoving());
        animator.SetFloat(horizontalParam, horizontal);
        animator.SetFloat(verticalParam, vertical);
        animator.SetBool(isMovingParam, Movement.IsMoving());
        animator.SetBool(isDodgingParam, Dodge.isDodging);
        animator.SetBool(damageTaken, Player.damageTaken);
    }

    public void SetIdle()
    {
        animator.SetBool(isMovingParam, false);
        animator.SetBool(isDodgingParam, false);
    }
    
    void Start()
    {
        mainCamera = Camera.main;
        Movement = GetComponent<Movement>();
        Dodge = GetComponent<Dodge>();
        rb = GetComponent<Rigidbody2D>();
        
        if (AttackStateMachine == null)
            AttackStateMachine = FindObjectOfType<AttackStateMachine>();
            
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMouseDirection();
        HandleCharacterFacing();
        HandleAnimator();
    }

    private void HandleMouseDirection()
    {
        lookDirection = AttackStateMachine.GetLookDirection();
    }

    private void HandleCharacterFacing()
    {
        Vector2 directionToCheck;

        if (Dodge.isDodging)
        {
            directionToCheck = rb.velocity;
        }
        else
        {
            directionToCheck = lookDirection;
        }

        if (directionToCheck.x > directionChangeThreshold && !facingRight)
        {
            FlipCharacter();
        }
        else if (directionToCheck.x < -directionChangeThreshold && facingRight)
        {
            FlipCharacter();
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    private Vector2 GetDodgeDirection()
    {
        Vector2 directionForAnimator;
        if (Dodge.isDodging)
        {
            directionForAnimator = rb.velocity.normalized;
            if (directionForAnimator.magnitude < 0.1f) 
            {
                directionForAnimator = Dodge.GetDodgeDirection();
            }
        }
        else
        {
            directionForAnimator = lookDirection;
        }
        return directionForAnimator;
    }
    
    public bool IsFacingRight()
    {
        return facingRight;
    }
    
    public Vector2 GetLookDirection()
    {
        return lookDirection;
    }
}