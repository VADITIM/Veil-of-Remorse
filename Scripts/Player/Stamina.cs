using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stamina : MonoBehaviour
{
    public PlayerBar PlayerBar;
    public Slider staminaBarSlider;

    private int posXChange = 33;
    private int widthChange = 228;

    public float newPosX;
    public float newWidth;

    private int initialPosX = 575;
    private int initialWidth = 1438;

    private Player player;
    private Coroutine regenCoroutine;

    [Header("Stamina Regeneration")]
    public float staminaRegenRate = 25f;
    public float regenDelay = 1f; 

    void Start()
    {
        PlayerBar = FindObjectOfType<PlayerBar>();
        staminaBarSlider = GetComponent<Slider>();
        player = FindObjectOfType<Player>();
    }

    public void SetStamina()
    {
        if (player == null)
            player = FindObjectOfType<Player>();

        if (PlayerBar == null)
            PlayerBar = FindObjectOfType<PlayerBar>();

        staminaBarSlider.value = player.GetCurrentStamina();
    }

    public void SetMaxStamina(int maxStamina)
    {
        staminaBarSlider.maxValue = maxStamina;
        UpdateStaminaBarVisuals();
    }

    public void UpgradeStaminaBar()
    {
        if (PlayerBar.currentLevel < PlayerBar.MAX_LEVEL)
        {
            PlayerBar.currentLevel++;
            UpdateStaminaBarVisuals();
        }
    }

    public void UpdateStaminaBarVisuals()
    {
        if (staminaBarSlider != null)
        {
            RectTransform rectTransform = staminaBarSlider.GetComponent<RectTransform>();
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

    public bool UseStamina(int amount)
    {
        if (player == null)
            player = FindObjectOfType<Player>();

        if (player.currentStamina >= amount)
        {
            player.currentStamina -= amount;
            SetStamina();

            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine);
            regenCoroutine = StartCoroutine(RegenerateStamina());

            return true;
        }

        return false;
    }

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(regenDelay);

        while (player.currentStamina < player.maxStamina)
        {
            if (ClassManager.Instance.AttackLogic.IsAttacking(true) || ClassManager.Instance.AttackLogic.isChargingHeavyAttack)
            {
                yield return null;
            }
            else
            {
                player.currentStamina += staminaRegenRate * Time.deltaTime;
                player.currentStamina = Mathf.Clamp(player.currentStamina, 0, player.maxStamina);
                SetStamina();
                yield return null;
            }
        }
    }
}
