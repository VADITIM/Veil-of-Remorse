using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Player player;

    private Coroutine healthBarCoroutine;

    void Start()
    {
        player = FindObjectOfType<Player>();
        healthBar = GetComponent<Slider>();
        SetMaxHealth(player.maxHealth);
    }

    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void SetHealth()
    {
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
        }
        healthBarCoroutine = StartCoroutine(AnimateHealthBar(player.currentHealth));
    }

    private IEnumerator AnimateHealthBar(float targetValue)
    {
        float startValue = healthBar.value;
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 originalPosition = healthBar.transform.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            healthBar.value = Mathf.Lerp(startValue, targetValue, t);

            float shakeIntensity = 30f * (1f - t);
            float shakeOffset = Mathf.Sin(Time.time * 30f) * shakeIntensity;
            healthBar.transform.localPosition = originalPosition + new Vector3(shakeOffset, 0f, 0f);

            yield return null;
        }

        healthBar.value = targetValue;
        healthBar.transform.localPosition = originalPosition;
    }
}