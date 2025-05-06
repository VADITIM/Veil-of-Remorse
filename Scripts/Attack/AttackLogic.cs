using UnityEngine;

public class AttackLogic : MonoBehaviour
{
    [Header("Normal Attack")]
    [SerializeField] public Collider2D normalAttack1Hitbox;  
    [SerializeField] public Collider2D normalAttack2Hitbox;  
    [SerializeField] public Collider2D normalAttack3Hitbox;  
    public bool isNormalAttacking1 = false;
    public bool isNormalAttacking2 = false;
    public bool isNormalAttacking3 = false;

    [Header("Heavy Attack")]
    [SerializeField] public Collider2D heavyAttackHitbox;
    public bool isHeavyAttacking = false;
    public bool isChargingHeavyAttack = false;

    private float heavyAttackChargeTime = 0f;
    private float heavyAttackMaxChargeDuration = 1.5f; 
    private float heavyAttackDuration = 0.45f; 
    private float heavyAttackTimer = 0f;

    private float heavyAttackMinDamage = 15f;
    private float heavyAttackMaxDamage = 45f;

    private float currentHeavyAttackDamage = 0f;
    private float heavyAttackDashForce = 3f;

    [Header("Attack Dash")]
    private float attackDash1Force = 1.5f;
    private float attackDash2Force = 1.5f;
    private float attackDash3Force = 4f;
    private float attackDashDuration = 0.1f;
    private float attackDashTimer = 0f;
    private bool isAttackDashing = false;

    private Vector2 attackDirection;

    public bool IsInBufferPeriod => inBufferPeriod; 
    public int NextAttack => nextAttack;    
    private int nextAttack = 0;

    private float[] normalAttackDurations = {0.283f, 0.316f, 0.2f};       
    private float[] attackDashForces = {1.5f, 1.5f, 4f};
    private float[] attackTimers = {0f, 0f, 0f};
    private bool[] isNormalAttacking = {false, false, false};

    private float normalAttack1Duration = 0.283f;
    private float normalAttack2Duration = 0.316f;
    private float normalAttack3Duration = 0.2f;
    private float attackTimer1 = 0f;
    private float attackTimer2 = 0f;
    private float attackTimer3 = 0f;

    private float bufferDuration = 0.2f;
    private float bufferTimer = 0f;
    private bool inBufferPeriod = false;

    private Transform parentTransform;
        
    private Vector2 GetMouseDirection() { return ClassManager.Instance.AttackStateMachine.GetLookDirection(); }
    public Vector2 GetAttackDashDirection() { return GetMouseDirection(); }
    public bool IsAttackDashing() { return isAttackDashing; }

    void Start()
    {
        parentTransform = transform.parent;
        
        normalAttack1Hitbox.enabled = false;
        normalAttack2Hitbox.enabled = false;
        normalAttack3Hitbox.enabled = false;
        heavyAttackHitbox.enabled = false;
        
        heavyAttackHitbox.enabled = false;
    }

    void Update()
    {
        HandleNormalAttackSequence();
        HandleNormalAttack();
        HandleHeavyAttack();
        HandleBufferPeriod();
        HandleAttackDash();
    }

    public bool IsAttacking(bool attacking)
    {
        if (isNormalAttacking1 || isNormalAttacking2 || isNormalAttacking3 || isHeavyAttacking)
            return attacking;
        else 
            return false;
    }

#region Attack Dash

    private void StartAttackDash()
    {
        attackDirection = GetMouseDirection();
        
        isAttackDashing = true;
        attackDashTimer = attackDashDuration;
    }
    
    private void HandleAttackDash()
    {
        if (isAttackDashing)
        {
            attackDashTimer -= Time.deltaTime;
            if (attackDashTimer <= 0f)
            {
                isAttackDashing = false;
            }
        }
    }
    
    public float GetCurrentAttackDashForce()
    {
        if (isNormalAttacking1) return attackDash1Force;
        if (isNormalAttacking2) return attackDash2Force;
        if (isNormalAttacking3) return attackDash3Force;
        if (isHeavyAttacking) return heavyAttackDashForce;
        return 0f;
    }

#endregion

#region Normal Attack

    private void HandleNormalAttack()
    {
        if (isNormalAttacking1)
        {
            UpdateHurtboxRotation(normalAttack1Hitbox);
            attackTimer1 -= Time.deltaTime;
            if (attackTimer1 <= 0f)
            {
                EndNormalAttack(1);
                StartBufferPeriod(2);
            }
        }
    
        if (isNormalAttacking2)
        {
            UpdateHurtboxRotation(normalAttack2Hitbox);
            attackTimer2 -= Time.deltaTime;
            if (attackTimer2 <= 0f)
            {
                EndNormalAttack(2);
                StartBufferPeriod(3);
            }
        }
    
        if (isNormalAttacking3)
        {
            UpdateHurtboxRotation(normalAttack3Hitbox);
            attackTimer3 -= Time.deltaTime;
            if (attackTimer3 <= 0f)
            {
                EndNormalAttack(3);
            }
        }
    }

    private void StartNormalAttacks(int attackNumber)
    {
        switch (attackNumber) 
        {
            case 1:
                isNormalAttacking1 = true;
                isNormalAttacking2 = false;
                isNormalAttacking3 = false;
                
                normalAttack1Hitbox.enabled = true;
                normalAttack2Hitbox.enabled = false;
                normalAttack3Hitbox.enabled = false;
                
                attackTimer1 = normalAttack1Duration;
                break;
                
            case 2:
                isNormalAttacking1 = false;
                isNormalAttacking2 = true;
                isNormalAttacking3 = false;
                
                normalAttack1Hitbox.enabled = false;
                normalAttack2Hitbox.enabled = true;
                normalAttack3Hitbox.enabled = false;
                
                attackTimer2 = normalAttack2Duration;
                break;
                
            case 3:
                isNormalAttacking1 = false;
                isNormalAttacking2 = false;
                isNormalAttacking3 = true;
                
                normalAttack1Hitbox.enabled = false;
                normalAttack2Hitbox.enabled = false;
                normalAttack3Hitbox.enabled = true;
                
                attackTimer3 = normalAttack3Duration;
                break;
        }
        
        inBufferPeriod = false;
        StartAttackDash();
    }

    private void HandleNormalAttackSequence()
    {
        if (Input.GetMouseButtonDown(0) && !ClassManager.Instance.Dodge.isDodging)
        {
            if (!isNormalAttacking1 && !isNormalAttacking2 && !isNormalAttacking3 && !inBufferPeriod && !isHeavyAttacking && !isChargingHeavyAttack)
            {
                if (!ClassManager.Instance.Stamina.UseStamina(10))
                {
                    return;
                }
                
                StartNormalAttacks(1);
            }
            else if (inBufferPeriod)
            {
                if (!ClassManager.Instance.Stamina.UseStamina(10))
                {
                    return;
                }
    
                StartNormalAttacks(nextAttack);
            }
        }
    }    

    private void EndNormalAttack(int attackNumber)
    {
        switch (attackNumber)
        {
            case 1:
                isNormalAttacking1 = false;
                if (normalAttack1Hitbox != null) normalAttack1Hitbox.enabled = false;
                break;
                
            case 2:
                isNormalAttacking2 = false;
                if (normalAttack2Hitbox != null) normalAttack2Hitbox.enabled = false;
                break;
                
            case 3:
                isNormalAttacking3 = false;
                if (normalAttack3Hitbox != null) normalAttack3Hitbox.enabled = false;
                break;
        }
    }

#endregion

#region Heavy Attack

    public float GetHeavyAttackDamage() { return currentHeavyAttackDamage; }

    private void HandleHeavyAttack()
    {
        if (isHeavyAttacking)
        {
            UpdateHurtboxRotation(heavyAttackHitbox);
            heavyAttackTimer -= Time.deltaTime;
            
            if (heavyAttackTimer <= 0f)
            {
                EndHeavyAttack();
            }
            return;
        }
    
        if (Input.GetMouseButtonDown(1) && !ClassManager.Instance.Dodge.isDodging && 
            !isNormalAttacking1 && !isNormalAttacking2 && !isNormalAttacking3 && 
            !isHeavyAttacking && !isChargingHeavyAttack)
        {
            if (!ClassManager.Instance.Stamina.UseStamina(45)) 
            {
                return;
            }
            
            StartChargingHeavyAttack();
        }
        
        if (isChargingHeavyAttack)
        {
            heavyAttackChargeTime += Time.deltaTime;
            
            float chargePercentage = Mathf.Clamp01(heavyAttackChargeTime / heavyAttackMaxChargeDuration);
            currentHeavyAttackDamage = Mathf.Lerp(heavyAttackMinDamage, heavyAttackMaxDamage, chargePercentage);
            
            heavyAttackDashForce = Mathf.Lerp(2.0f, 5.0f, chargePercentage);
            
            if (heavyAttackChargeTime >= heavyAttackMaxChargeDuration || Input.GetMouseButtonUp(1))
            {
                ReleaseHeavyAttack();
            }
        }
    }

    private void StartHeavyAttack()
    {
        isHeavyAttacking = true;
        heavyAttackTimer = heavyAttackDuration;
        heavyAttackHitbox.enabled = true;
        StartAttackDash();
    }

    private void StartChargingHeavyAttack()
    {
        isChargingHeavyAttack = true;
        heavyAttackChargeTime = 0f;
        currentHeavyAttackDamage = heavyAttackMinDamage;
    }

    private void ReleaseHeavyAttack()
    {
        isChargingHeavyAttack = false;
        StartHeavyAttack();
    }
    
    private void EndHeavyAttack()
    {
        isHeavyAttacking = false;
        heavyAttackHitbox.enabled = false;
        currentHeavyAttackDamage = 0f;
    }

#endregion


    private void StartBufferPeriod(int nextAttackNumber)
    {
        inBufferPeriod = true;
        bufferTimer = bufferDuration;
        nextAttack = nextAttackNumber;
    }

    private void HandleBufferPeriod()
    {
        if (inBufferPeriod)
        {
            bufferTimer -= Time.deltaTime;
            if (bufferTimer <= 0f)
            {
                inBufferPeriod = false;
                nextAttack = 0;
            }
        }
    }

    private void UpdateHurtboxRotation(Collider2D hitbox)
    {
        if (hitbox != null && parentTransform != null)
        {
            hitbox.transform.rotation = parentTransform.rotation;
        }
    }
}