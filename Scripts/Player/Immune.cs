using UnityEngine;

public class Immune : MonoBehaviour
{
    private Player Player;
    private SpriteRenderer spriteRenderer;
    
    private float cooldown = .3f;
    private float cooldownTimer;

    public static bool immune = false;

    private Color damageColor = new Color(1f, 0f, 0f, 0.8f); 
    private Color normalColor = Color.white;

    private void Start()
    {
        Player = FindObjectOfType<Player>();
        spriteRenderer = Player.GetComponent<SpriteRenderer>();
        cooldownTimer = cooldown;
    }

    void Update()
    {
        if (Player.damageTaken)
        {
            immune = true;
            spriteRenderer.color = damageColor;
            
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                Player.damageTaken = false;
                cooldownTimer = cooldown;
            }
        }
        else
        {
            immune = false;
            spriteRenderer.color = normalColor;
        }
    }
}