using TMPro;
using UnityEngine;

public class ShowCurrentKindleLevel : MonoBehaviour
{
    public TextMeshProUGUI kindleLevelText;

    void Start()
    {
        UpdateLevel();
    }

    void Update()
    {
        UpdateLevel();
    }

    public void UpdateLevel()
    {
        if (ClassManager.Instance == null || ClassManager.Instance.PlayerBar == null)
            return;
            
        int currentKindleLevel = ClassManager.Instance.PlayerBar.currentLevel;
        kindleLevelText.text = $"Kindle\n{currentKindleLevel} / 11";
    }
}
