using UnityEngine;
using System.Collections.Generic;

public class DivinePoint : MonoBehaviour
{
    ClassManager CM;

    public bool playerNearby = false;
    public bool isPaused = false;
    
    private List<EnemySpawnerZone> registeredSpawners = new List<EnemySpawnerZone>();

    void Start()
    {
        CM = FindObjectOfType<ClassManager>();
    }

    void Update()
    {
        if (playerNearby && !isPaused && CM.Hotkeys.HandleInteract())
        {
            CM.CameraLogic.Zoom(2, 0.5f);
            CM.UIManager.ToggleDivinePointMenu();
            CM.UIManager.TogglePause();
            CM.DivinePointAnimations.AnimateButtonsIn();
            CM.Movement.isMoving = false;
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
        CM.Player.RestoreHealth();
        CM.Player.RestoreStamina();
        CM.RespawnManager.RespawnAllEnemies();
        CM.Health.ResetHeals();
        
        foreach (var spawner in registeredSpawners)
        {
            if (spawner != null)
            {
                spawner.RespawnEnemies();
            }
        }
    }
    
    public void RegisterEnemySpawner(EnemySpawnerZone spawner)
    {
        if (spawner != null && !registeredSpawners.Contains(spawner))
        {
            registeredSpawners.Add(spawner);
        }
    }
    
    public void UnregisterEnemySpawner(EnemySpawnerZone spawner)
    {
        if (spawner != null)
        {
            registeredSpawners.Remove(spawner);
        }
    }
}