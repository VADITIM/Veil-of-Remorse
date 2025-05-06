using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private string requiredKeyID = "default";
    [SerializeField] private string doorID = "door1"; 
    
    [Header("Effects")]
    [SerializeField] private GameObject openEffect; 
    [SerializeField] private AudioClip openSound; 
    [SerializeField] private AudioClip lockedSound;
    
    private AudioSource audioSource;
    private bool isOpen = false;
    
    private void Start()
    {
        if (gameObject.tag != "KeyDoor")
        {
            gameObject.tag = "KeyDoor";
            Debug.LogWarning("Door object tag was set to 'KeyDoor'");
        }
        
        if ((openSound != null || lockedSound != null) && audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        string savedDoorStatus = "Door_Opened_" + doorID;
        if (SaveSystem.LoadBool(savedDoorStatus))
        {
            Debug.Log($"Door {doorID} was previously opened, removing from game.");
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isOpen)
        {
            if (Key.HasKey(requiredKeyID))
            {
                OpenDoor();
            }
            else
            {
                if (lockedSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(lockedSound);
                }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            if (Key.HasKey(requiredKeyID))
            {
                OpenDoor();
            }
            else
            {
                if (lockedSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(lockedSound);
                }
            }
        }
    }
    
    public void OpenDoor()
    {
        if (Key.UseKey(requiredKeyID))
        {
            isOpen = true;

            string savedDoorStatus = "Door_Opened_" + doorID;
            SaveSystem.SaveBool(savedDoorStatus, true);
            
            if (openSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(openSound);
            }
            
            if (openEffect != null)
            {
                Instantiate(openEffect, transform.position, Quaternion.identity);
            }
            
            if (ClassManager.Instance != null)
            {
                SaveSystem.SaveGame(
                    ClassManager.Instance.Movement,
                    ClassManager.Instance.LevelSystem,
                    ClassManager.Instance.SkillTreeManager,
                    ClassManager.Instance.Player,
                    SoundManager.Instance
                );
            }
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }

    public static void ResetAllDoors()
    {
        Debug.Log("All door data has been reset.");
    }
}