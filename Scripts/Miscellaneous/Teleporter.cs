using UnityEngine;

// AI GENERATED

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private Vector3 destinationPosition;
    [SerializeField] private bool useCustomPosition = false;
    [SerializeField] private float teleportDelay = 0f;
    [SerializeField] private bool isActive = true;
    [SerializeField] private BossEnemy linkedBoss;
    [SerializeField] private GameObject teleportEffect;
    [SerializeField] private GameObject arrivalEffect;
    [SerializeField] private AudioClip teleportSound;
    
    private AudioSource audioSource;
    
    private void Start()
    {
        if (teleportSound != null && audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (!useCustomPosition && destinationPoint == null)
        {
            Debug.LogWarning("Teleporter has no destination set. Please assign a destination Transform or enable useCustomPosition.");
        }
        
        if (linkedBoss == null)
        {
            isActive = true;
        }
        else if (!isActive)
        {
            linkedBoss.OnEnemyDeath += ActivateTeleporter;
        }
    }
    
    private void OnDestroy()
    {
        // Clean up event subscription
        if (linkedBoss != null)
        {
            linkedBoss.OnEnemyDeath -= ActivateTeleporter;
        }
    }
    
    // This method can be called when a boss is defeated
    public void ActivateTeleporter()
    {
        isActive = true;
        
        // Optional: Add visual/audio feedback when teleporter activates
        if (audioSource != null)
        {
            audioSource.Play();
        }
        
        // Optional: Show activation effect
        if (arrivalEffect != null)
        {
            Instantiate(arrivalEffect, transform.position, Quaternion.identity);
        }
        
        Debug.Log("Teleporter activated!");
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            TeleportPlayer(collision.gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }
    
    private void TeleportPlayer(GameObject player)
    {
        // Determine target position
        Vector3 targetPosition;
        
        if (useCustomPosition)
        {
            targetPosition = destinationPosition;
        }
        else if (destinationPoint != null)
        {
            targetPosition = destinationPoint.position;
        }
        else
        {
            Debug.LogError("Teleporter has no valid destination set!");
            return;
        }
        
        // Play teleport sound if available
        if (teleportSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(teleportSound);
        }
        
        // Play teleport effect if available
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
        }
        
        // Handle teleportation (with or without delay)
        if (teleportDelay > 0)
        {
            StartCoroutine(DelayedTeleport(player, targetPosition));
        }
        else
        {
            // Immediate teleport
            player.transform.position = targetPosition;
            
            // Play arrival effect if available
            if (arrivalEffect != null)
            {
                Instantiate(arrivalEffect, targetPosition, Quaternion.identity);
            }
        }
    }
    
    private System.Collections.IEnumerator DelayedTeleport(GameObject player, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(teleportDelay);
        
        // Teleport the player
        player.transform.position = targetPosition;
        
        // Play arrival effect if available
        if (arrivalEffect != null)
        {
            Instantiate(arrivalEffect, targetPosition, Quaternion.identity);
        }
    }
    
    // Public method to check if teleporter is active
    public bool IsActive()
    {
        return isActive;
    }
    
    // Public method to manually set activation state
    public void SetActive(bool active)
    {
        isActive = active;
    }
}