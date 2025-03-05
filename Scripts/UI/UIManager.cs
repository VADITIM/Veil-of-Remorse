using UnityEngine;

public class UIManager : MonoBehaviour
{
    Hotkeys Hotkeys;
    
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject levelUpMenu;
    
    void Start()
    {
        Hotkeys = FindObjectOfType<Hotkeys>();
    }

    void Update()
    {
        if (Hotkeys.HandleEscMenu())
        {
            ToggleEscapeMenu();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleLevelUpMenu();
        }
    }


    public void ToggleEscapeMenu()
    {
        escapeMenu.SetActive(!escapeMenu.activeSelf);
    }

    public void ToggleLevelUpMenu()
    {
        levelUpMenu.SetActive(!levelUpMenu.activeSelf);
    }
}
