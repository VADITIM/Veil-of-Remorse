using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private Movement Movement;
    private LevelSystem LevelSystem;
    private AbilityManager AbilityManager;
    private RespawnManager RespawnManager;
    private Player Player;
    private Hotkeys Hotkeys;
    
    public bool playerNearby = false;

    void Start()
    {
        Movement = FindObjectOfType<Movement>();
        LevelSystem = FindObjectOfType<LevelSystem>();
        AbilityManager = FindObjectOfType<AbilityManager>();
        RespawnManager = FindObjectOfType<RespawnManager>();
        Player = FindObjectOfType<Player>();
        Hotkeys = FindObjectOfType<Hotkeys>();
    }

    void Update()
    {
        if (playerNearby && Hotkeys.HandleInteract())
        {
            RespawnManager.RespawnAllEnemies();
            SaveSystem.SaveGame(Movement, LevelSystem, AbilityManager, Player);
            FindObjectOfType<Player>().RestoreHealth();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}