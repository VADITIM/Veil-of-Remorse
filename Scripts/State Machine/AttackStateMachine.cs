using UnityEngine;

public class AttackStateMachine : MonoBehaviour
{
    [SerializeField] Attack Attack;
    [SerializeField] private Animator attackAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera mainCamera;

    private Vector2 lookDirection;
    private readonly string isNormalAttacking1Param = "isNormalAttacking1";
    private readonly string isNormalAttacking2Param = "isNormalAttacking2";
    private readonly string isNormalAttacking3Param = "isNormalAttacking3";

    private void HandleAnimator()
    {
        attackAnimator.SetBool(isNormalAttacking1Param, Attack.isNormalAttacking1);
        attackAnimator.SetBool(isNormalAttacking2Param, Attack.isNormalAttacking2);
        attackAnimator.SetBool(isNormalAttacking3Param, Attack.isNormalAttacking3);

        if (Attack.IsInBufferPeriod)
        {
            if (Attack.NextAttack == 2)
            {
                attackAnimator.SetBool(isNormalAttacking1Param, true); 
            }
            else if (Attack.NextAttack == 3)
            {
                attackAnimator.SetBool(isNormalAttacking2Param, true); 
            }
        }
    }
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleMouseDirection();
        HandleAttackRotation();
        HandleAnimator();
    }

    private void HandleMouseDirection()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        lookDirection = (mousePos - transform.position).normalized;
    }

    private void HandleAttackRotation()
    {
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 70f);
    }
}