using UnityEngine;

public class IceSlide : MonoBehaviour
{
    [Header("Ice Settings")]
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private string iceTag = "IceZone";
    [SerializeField] private float minVelocityThreshold = 0.1f;
    [SerializeField] private bool disablePlayerControlOnIce = true;
    
    // References
    private Rigidbody2D rb;
    private Vector2 lastMovementDirection;
    private bool isOnIce = false;
    
    // Reference to player movement script (if needed)
    private Movement playerMovement;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Movement>();
        
        if (rb == null)
        {
            Debug.LogError("IceSlide requires a Rigidbody2D component!");
        }
    }
    
    private void Update()
    {
        // Store last movement direction when player is moving and not on ice
        if (!isOnIce && rb.velocity.sqrMagnitude > minVelocityThreshold)
        {
            lastMovementDirection = rb.velocity.normalized;
            Debug.Log("Storing movement direction: " + lastMovementDirection);
        }
    }
    
    private void FixedUpdate()
    {
        // Apply sliding effect when on ice
        if (isOnIce)
        {
            // Important: Make sure to overwrite velocity completely
            rb.velocity = lastMovementDirection * slideSpeed;
            Debug.Log("Sliding on ice with velocity: " + rb.velocity + " | isOnIce: " + isOnIce);
            
            // This is a more forceful approach if needed
            // rb.AddForce(lastMovementDirection * slideSpeed, ForceMode2D.Impulse);
        }
    }
    
    private void EnterIce()
    {
        if (!isOnIce)
        {
            isOnIce = true;
            Debug.Log("EnterIce called - Player is now on ice: " + isOnIce);
            
            // Disable player movement control if applicable
            if (disablePlayerControlOnIce && playerMovement != null)
            {
                playerMovement.enabled = false;
                Debug.Log("Disabled player movement control");
            }
            
            // Store current movement direction if we're already moving
            if (rb.velocity.sqrMagnitude > minVelocityThreshold)
            {
                lastMovementDirection = rb.velocity.normalized;
                Debug.Log("Ice slide direction: " + lastMovementDirection);
            }
            else if (lastMovementDirection == Vector2.zero)
            {
                // If we don't have a direction, use a default one
                lastMovementDirection = new Vector2(1, 0); // Default to sliding right
                Debug.Log("No velocity detected, using default slide direction: " + lastMovementDirection);
            }
            
            // Apply initial slide force
            rb.velocity = lastMovementDirection * slideSpeed;
        }
    }
    
    private void ExitIce()
    {
        if (isOnIce)
        {
            isOnIce = false;
            Debug.Log("ExitIce called - Player is no longer on ice: " + isOnIce);
            
            // Re-enable player movement if it was disabled
            if (disablePlayerControlOnIce && playerMovement != null)
            {
                playerMovement.enabled = true;
                Debug.Log("Re-enabled player movement control");
            }
            
            // Optional: add a small push when leaving ice to prevent getting stuck at edges
            rb.velocity = lastMovementDirection * slideSpeed * 0.5f;
        }
    }
    
    // Detect ice using trigger colliders
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(iceTag))
        {
            Debug.Log("Ice detected via OnTriggerEnter2D: " + other.name);
            EnterIce();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(iceTag))
        {
            Debug.Log("Left ice via OnTriggerExit2D: " + other.name);
            ExitIce();
        }
    }
    
    // Stay on ice if we remain inside the trigger
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(iceTag) && !isOnIce)
        {
            Debug.Log("Still on ice via OnTriggerStay2D: " + other.name);
            EnterIce();
        }
    }
    
    // For debugging
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "On Ice: " + isOnIce);
        GUI.Label(new Rect(10, 30, 300, 20), "Slide Direction: " + lastMovementDirection);
        GUI.Label(new Rect(10, 50, 300, 20), "Current Velocity: " + rb.velocity);
    }
}