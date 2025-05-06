using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Key Settings")]
    [SerializeField] private string keyID = "default"; 
    
    [Header("Visual Settings")]
    [SerializeField] private GameObject visualEffect; 
    
    private static string currentKey = ""; 
    
    private void Start()
    {
        if (gameObject.tag != "Key")
        {
            gameObject.tag = "Key";
            Debug.LogWarning("Key object tag was set to 'Key'");
        }
        
        string savedKeyStatus = "Key_Collected_" + keyID;
        if (SaveSystem.LoadBool(savedKeyStatus))
        {
            Debug.Log($"Key {keyID} was previously collected, removing from game.");
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(currentKey))
            {
                Debug.Log($"Player already has key: {currentKey}. Cannot pick up {keyID}.");
                return;
            }
            
            currentKey = keyID;
            string savedKeyStatus = "Key_Collected_" + keyID;
            SaveSystem.SaveBool(savedKeyStatus, true);
            
            if (visualEffect != null)
            {
                Instantiate(visualEffect, transform.position, Quaternion.identity);
            }
            
            Debug.Log($"Key collected: {keyID}");
            
            Destroy(gameObject);
        }
    }
    
    public static bool HasKey(string keyID)
    {
        return currentKey == keyID;
    }
    
    public static bool UseKey(string keyID)
    {
        if (HasKey(keyID))
        {
            // Clear the current key
            currentKey = "";
            return true;
        }
        return false;
    }
    
    public static string GetCurrentKey()
    {
        return currentKey;
    }
    
    public static void ClearKey()
    {
        currentKey = "";
    }
    
    public static void LoadKey(string keyID)
    {
        currentKey = keyID;
    }
    
    public static void ResetAllKeys()
    {
        currentKey = "";
    }
}