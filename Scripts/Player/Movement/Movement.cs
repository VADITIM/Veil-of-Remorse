using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Attack Attack;
    
    private Rigidbody2D rb;  

    public float speed = 5f;    
    private Vector2 movement;   
    public bool isMoving;       

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
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
        if (Attack.IsAttacking(attacking: true))
        {
            rb.velocity = movement * speed / 2;
        }
        else
        {
            rb.velocity = movement * speed;
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }

}