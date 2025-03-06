using UnityEngine;

public class DivinePoint : MonoBehaviour
{
    private Movement Movement;
    private LevelSystem LevelSystem;
    private AbilityManager AbilityManager;
    private RespawnManager RespawnManager;
    private Player Player;
    private Hotkeys Hotkeys;
    private UIManager UIManager;

    public bool playerNearby = false;
    public bool isPaused = false;

    void Start()
    {
        Movement = FindObjectOfType<Movement>();
        LevelSystem = FindObjectOfType<LevelSystem>();
        AbilityManager = FindObjectOfType<AbilityManager>();
        RespawnManager = FindObjectOfType<RespawnManager>();
        Player = FindObjectOfType<Player>();
        Hotkeys = FindObjectOfType<Hotkeys>();
        UIManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (playerNearby && !isPaused && Hotkeys.HandleInteract())
        {
            SaveSystem.SaveGame(Movement, LevelSystem, AbilityManager, Player);
            UIManager.ToggleDivinePointMenu();
            TogglePause();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    public void RestorePlayer()
    {
        Player.RestoreHealth();
        RespawnManager.RespawnAllEnemies();
    }
}