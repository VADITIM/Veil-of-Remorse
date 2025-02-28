using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] public Collider2D normalAttack1Hitbox;  
    [SerializeField] public Collider2D normalAttack2Hitbox;  
    [SerializeField] public Collider2D normalAttack3Hitbox;  
    public bool isNormalAttacking1 = false;
    public bool isNormalAttacking2 = false;
    public bool isNormalAttacking3 = false;

    private float normalAttack1Duration = 0.283f;
    private float normalAttack2Duration = 0.316f;
    private float normalAttack3Duration = 0.2f;
    private float attackTimer1 = 0f;
    private float attackTimer2 = 0f;
    private float attackTimer3 = 0f;

    private Transform parentTransform;

    void Start()
    {
        parentTransform = transform.parent;
        if (normalAttack1Hitbox != null) normalAttack1Hitbox.enabled = false;
        if (normalAttack2Hitbox != null) normalAttack2Hitbox.enabled = false;
        if (normalAttack2Hitbox != null) normalAttack3Hitbox.enabled = false;
    }

    void Update()
    {
        HandleNormalAttackSequence();
        HandleNormalAttackTime();
    }

    private void HandleNormalAttackSequence()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isNormalAttacking1 && !isNormalAttacking2 && !isNormalAttacking3)
            {
                StartNormalAttack1();
            }
            else if (isNormalAttacking1 && attackTimer1 <= 0.15f)
            {
                StartNormalAttack2();
            }
            else if (isNormalAttacking2 && attackTimer2 <= 0.15f)
            {
                StartNormalAttack3();
            }
        }
    }

    private void StartNormalAttack1()
    {
        isNormalAttacking1 = true;
        attackTimer1 = normalAttack1Duration;
        normalAttack1Hitbox.enabled = true;
    }

    private void StartNormalAttack2()
    {
        isNormalAttacking1 = false;
        normalAttack1Hitbox.enabled = false;
        
        isNormalAttacking2 = true;
        attackTimer2 = normalAttack2Duration;
        normalAttack2Hitbox.enabled = true;
    }

    private void StartNormalAttack3()
    {
        isNormalAttacking2 = false;
        normalAttack2Hitbox.enabled = false;
        
        isNormalAttacking3 = true;
        attackTimer3 = normalAttack3Duration;
        normalAttack3Hitbox.enabled = true;
    }


    private void HandleNormalAttackTime()
    {
        if (isNormalAttacking1)
        {
            UpdateHitboxRotation(normalAttack1Hitbox);
            attackTimer1 -= Time.deltaTime;
            if (attackTimer1 <= 0f)
            {
                EndNormalAttack1();
            }
        }

        if (isNormalAttacking2)
        {
            UpdateHitboxRotation(normalAttack2Hitbox);
            attackTimer2 -= Time.deltaTime;
            if (attackTimer2 <= 0f)
            {
                EndNormalAttack2();
            }
        }

        if (isNormalAttacking3)
        {
            UpdateHitboxRotation(normalAttack3Hitbox);
            attackTimer3 -= Time.deltaTime;
            if (attackTimer3 <= 0f)
            {
                isNormalAttacking3 = false;
                if (normalAttack3Hitbox != null) normalAttack3Hitbox.enabled = false;
            }
        }
    }

    private void UpdateHitboxRotation(Collider2D hitbox)
    {
        if (hitbox != null && parentTransform != null)
        {
            hitbox.transform.rotation = parentTransform.rotation;
        }
    }

    private void EndNormalAttack1()
    {
        isNormalAttacking1 = false;
        if (normalAttack1Hitbox != null) normalAttack1Hitbox.enabled = false;
    }

    private void EndNormalAttack2()
    {
        isNormalAttacking2 = false;
        if (normalAttack2Hitbox != null) normalAttack2Hitbox.enabled = false;
    }

    private void EndNormalAttack3()
    {
        isNormalAttacking3 = false;
        if (normalAttack3Hitbox != null) normalAttack3Hitbox.enabled = false;
    }
}