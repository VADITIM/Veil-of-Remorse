using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject levelUpMenu;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
