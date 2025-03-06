using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    Hotkeys Hotkeys;

    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject offerEssenceMenu;
    [SerializeField] private GameObject divinePointMenu;
    [SerializeField] private TextMeshProUGUI essenceText;

    private DivinePoint divinePoint;
    private LevelSystem levelSystem;

    void Start()
    {
        Hotkeys = FindObjectOfType<Hotkeys>();
        divinePoint = FindObjectOfType<DivinePoint>();
        levelSystem = FindObjectOfType<LevelSystem>();
        UpdateEssenceText();
    }

    void Update()
    {
        if (Hotkeys.HandleEscMenu() && !divinePoint.isPaused)
        {
            ToggleEscapeMenu();
        }
    }

    public void ToggleEscapeMenu() { escapeMenu.SetActive(!escapeMenu.activeSelf); }

    public void ToggleDivinePointMenu() { divinePointMenu.SetActive(!divinePointMenu.activeSelf); }

    public void TogglePause() { divinePoint.TogglePause(); }

    public void RestorePlayer() { divinePoint.RestorePlayer(); }

    public void ToggleOfferEssenceMenu() { offerEssenceMenu.SetActive(!offerEssenceMenu.activeSelf); }

    public void UpdateEssenceText()
    {
        if (essenceText == null) return;
        essenceText.text = "Essence: " + levelSystem.GetEssence().ToString();
    }
}