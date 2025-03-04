using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private Movement Movement;
    private LevelSystem LevelSystem;
    private AbilityManager AbilityManager;
    private RespawnManager RespawnManager;
    
    public bool playerNearby = false;

    void Start()
    {
        Movement = FindObjectOfType<Movement>();
        LevelSystem = FindObjectOfType<LevelSystem>();
        AbilityManager = FindObjectOfType<AbilityManager>();
        RespawnManager = FindObjectOfType<RespawnManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
            playerNearby = true;
            Debug.Log("Bonfire discovered");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Resting at Bonfire");
            RespawnManager.RespawnAllEnemies();
            SaveSystem.SaveGame(Movement, LevelSystem, AbilityManager);
            FindObjectOfType<Player>().RestoreHealth(); // Optional healing system
        }
    }
}
