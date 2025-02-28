using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] Movement Movement;
    [SerializeField] Dodge Dodge;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody2D rb;

    private bool facingRight = true;
    private Vector2 lookDirection;
    private float directionChangeThreshold = 0.1f;

    private readonly string horizontalParam = "Horizontal";
    private readonly string verticalParam = "Vertical";
    private readonly string isMovingParam = "isMoving";
    private readonly string isDodgingParam = "isDodging";

    private void HandleAnimator()
    {
        Vector2 directionForAnimator = GetDodgeDirection();

        float horizontal = facingRight ? directionForAnimator.x : -directionForAnimator.x;
        float vertical = directionForAnimator.y;

        animator.SetFloat(horizontalParam, horizontal);
        animator.SetFloat(verticalParam, vertical);
        animator.SetBool(isMovingParam, Movement.IsMoving());
        animator.SetBool(isDodgingParam, Dodge.isDodging);
    }

    void Start()
    {
        mainCamera = Camera.main;
        Movement = GetComponent<Movement>();
        Dodge = GetComponent<Dodge>();
        rb = GetComponent<Rigidbody2D>(); 

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
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        lookDirection = (mousePos - transform.position).normalized;
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
}