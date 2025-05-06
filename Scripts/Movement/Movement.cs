using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Player Player;
    [SerializeField] AttackLogic Attack;

    public float speed = 5f;    
    public Vector2 movement;   
    public bool isMoving;    

    public bool IsMoving() { return isMoving; }

    void Update()
    {
        HandleMovement();
    }

    public void HandleMovement()
    {
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) { movement.y = 1; }
        if (Input.GetKey(KeyCode.S)) { movement.y = -1; }
        if (Input.GetKey(KeyCode.A)) { movement.x = -1; }
        if (Input.GetKey(KeyCode.D)) { movement.x = 1; }

        movement = movement.normalized;

        isMoving = movement.magnitude > 0;
    }

    void FixedUpdate()
    {
        if (Attack.IsAttackDashing())
        {
            Vector2 dashDirection = Attack.GetAttackDashDirection();
            float dashForce = Attack.GetCurrentAttackDashForce();
            Player.rb.velocity = dashDirection * dashForce;
        }
        else if (Attack.IsAttacking(attacking: true))
        {
            Player.rb.velocity = movement * speed / 6;
        }
        else if (ClassManager.Instance.Attack.isChargingHeavyAttack)
        {
            Player.rb.velocity = speed / 8 * movement;
        }
        else
        {
            Player.rb.velocity = movement * speed;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }

}