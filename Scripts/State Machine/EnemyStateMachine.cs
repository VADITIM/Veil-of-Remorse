using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{

    EnemyBase EnemyBase;

    [SerializeField] public Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool facingRight = true;
    private Vector2 lookDirection;
    private float directionChangeThreshold = 0.1f;

    private readonly string isMovingParam = "isMoving";
    private readonly string isAttackingParam = "isAttacking";
    private readonly string isDamagedParam = "isDamaged";
    private readonly string isDeadParam = "isDead";

    public void SetAttacking(bool isAttacking)
    {
        EnemyBase.isAttacking = isAttacking;
        animator.SetBool(isAttackingParam, isAttacking);
    }

    private void HandleAnimator()
    {
        animator.SetBool(isMovingParam, EnemyBase.IsMoving());
        animator.SetBool(isAttackingParam, EnemyBase.IsAttacking());
        animator.SetBool(isDamagedParam, EnemyBase.IsDamaged());
        animator.SetBool(isDeadParam, EnemyBase.IsDead());
    }

    void Start()
    {
        EnemyBase = GetComponent<EnemyBase>();

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        HandleCharacterFacing();
        HandleAnimator();
    }

    private void HandleCharacterFacing()
    {
        Vector2 directionToCheck;

        directionToCheck = lookDirection;

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
}