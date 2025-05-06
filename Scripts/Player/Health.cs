using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public PlayerBar PlayerBar;
    public Slider healthBarSlider;
    public TextMeshProUGUI potionCountText;

    private int posXChange = 34;
    private int widthChange = 119;

    public float newPosX;
    public float newWidth;

    private int initialPosX = 586;
    private int initialWidth = 777; 
    
    public int maxHeals = 3;
    public int remainingHeals;
    public int healAmount = 50;

    void Start()
    {
        PlayerBar = FindObjectOfType<PlayerBar>();
        healthBarSlider = GetComponent<Slider>();
        remainingHeals = maxHeals;
        UpdatePotionCountDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && remainingHeals > 0)
        {
            Heal();
        }
    }

    public void SetHealth()
    {
        Player player = FindObjectOfType<Player>();
        PlayerBar = FindObjectOfType<PlayerBar>();

        healthBarSlider.value = player.currentHealth; 
        healthBarSlider.value = Mathf.Clamp(healthBarSlider.value, 0, healthBarSlider.maxValue);

        healthBarSlider.gameObject.SetActive(false); 
        healthBarSlider.gameObject.SetActive(true);
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthBarSlider.maxValue = maxHealth;
        UpdateHealthBarVisuals();
    }

    public void UpgradeHealthBar()
    {
        if (PlayerBar.currentLevel < PlayerBar.MAX_LEVEL)
        {
            PlayerBar.currentLevel++;
            UpdateHealthBarVisuals();
        }
    }

    public void UpdateHealthBarVisuals()
    {
        if (healthBarSlider != null)
        {
            RectTransform rectTransform = healthBarSlider.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                PlayerBar playerBar = FindObjectOfType<PlayerBar>();
                if (playerBar != null)
                {
                    int currentLevel = playerBar.currentLevel;
                    float newPosX = initialPosX + (posXChange * currentLevel);
                    float newWidth = initialWidth + (widthChange * currentLevel);
        
                    rectTransform.anchoredPosition = new Vector2(newPosX, rectTransform.anchoredPosition.y);
                    rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
                }
            }
        }
    }

    public void Heal()
    {
        if (remainingHeals <= 0) return;
        
        Player player = FindObjectOfType<Player>();
        if (player != null && player.currentHealth < player.maxHealth)
        {
            player.currentHealth += healAmount;
            if (player.currentHealth > player.maxHealth)
            {
                player.currentHealth = player.maxHealth;
            }
            remainingHeals--;
            SetHealth(); 
            UpdatePotionCountDisplay();
        }
    }

    public void ResetHeals()
    {
        remainingHeals = maxHeals;
        UpdatePotionCountDisplay(); 
    }
    
    public void UpdatePotionCountDisplay()
    {
        if (potionCountText != null)
        {
            potionCountText.text = remainingHeals.ToString();
        }
    }
}
