using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject divinePointMenu;
    [SerializeField] private GameObject skillTreeMenu;

    [Header("HUD")]
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject LevelUpPopup;
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI essenceText;

    void Start()
    {
        UpdateEssenceText();
        skillTreeMenu.SetActive(false);
        divinePointMenu.SetActive(false);
    }

    void Update()
    {
        if (ClassManager.Instance.Hotkeys.HandleEscMenu() && !ClassManager.Instance.DivinePoint.isPaused)
        {
            ToggleEscapeMenu();
        }
    }
// -------------------------------------------------- GAME --------------------------------------------------

    public void SaveGame() 
    { 
        SaveSystem.SaveGame(ClassManager.Instance.Movement, ClassManager.Instance.LevelSystem, ClassManager.Instance.SkillTreeManager, ClassManager.Instance.Player, SoundManager.Instance); 
    } 

    public void TogglePause() 
    { 
        ClassManager.Instance.DivinePoint.TogglePause(); 
        HUD.SetActive(!HUD.activeSelf); 
    }

// -------------------------------------------------- MENU --------------------------------------------------

    public void ToggleEscapeMenu() 
    { 
        escapeMenu.SetActive(!escapeMenu.activeSelf); 
    }


    public void ToggleDivinePointMenu() 
    { 
        divinePointMenu.SetActive(!divinePointMenu.activeSelf); 
        LevelUpPopup.SetActive(false); 
    }
    

    public void ToggleOfferEssenceMenu() 
    { 
        skillTreeMenu.SetActive(!skillTreeMenu.activeSelf); 
        ClassManager.Instance.SkillTreeManager.UpdateSkillStates(); 
    }

// -------------------------------------------------- CAMERA --------------------------------------------------


    public void ResetCameraZoom() 
    { 
        ClassManager.Instance.CameraLogic.ZoomOut(0.5f); 
    }

// -------------------------------------------------- PLAYER --------------------------------------------------

    public void RestorePlayer() 
    { 
        ClassManager.Instance.DivinePoint.RestorePlayer(); 
    }

// -------------------------------------------------- OTHER --------------------------------------------------

    public void UpdateEssenceText()
    {
        if (essenceText != null && ClassManager.Instance.LevelSystem != null) 
        {
            essenceText.text = ClassManager.Instance.LevelSystem.GetEssence().ToString();
        }
    }

    public void KindlePower()
    {
        if (ClassManager.Instance.PlayerBar.MaxLevelReached()) return;

        ClassManager.Instance.LevelSystem.KindlePower(1);
    }
}