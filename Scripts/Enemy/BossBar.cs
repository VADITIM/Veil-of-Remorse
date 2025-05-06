using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    private BossEnemy boss;

    private void Start()
    {
        boss = GetComponentInParent<BossEnemy>();
        if (boss == null)
        {
            Debug.LogWarning("BossBar: Could not find BossEnemy in parent.");
            enabled = false;
            return;
        }

        // Slider goes from 0 to 1 (percentage)
        healthBarSlider.minValue = 0;
        healthBarSlider.maxValue = 1;
        healthBarSlider.value = 1;
    }

    private void Update()
    {
        if (boss == null || healthBarSlider == null) return;

        float percent = (float)boss.GetHealth() / boss.GetMaxHealth();
        healthBarSlider.value = Mathf.Clamp01(percent);
    }
}
