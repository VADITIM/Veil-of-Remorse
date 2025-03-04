using UnityEngine;

public class InvincibilityFrames : MonoBehaviour
{
    private Player Player;
    
    private float cooldown = 1f;
    private float cooldownTimer;

    private void Start()
    {
        Player = FindObjectOfType<Player>();
        cooldownTimer = cooldown;
    }

    void Update()
    {
        if (Player.damageTaken)
        {
            Player.GetComponent<SpriteRenderer>().color = Color.red;
            
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                Player.damageTaken = false;
                cooldownTimer = cooldown;
                Debug.LogWarning("Player is no longer invincible");
            }
        }
        else
        {
            Player.GetComponent<SpriteRenderer>().color = Color.white;
        }        


    }
}
