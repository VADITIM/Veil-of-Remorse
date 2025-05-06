using UnityEngine;

public class Hotkeys : MonoBehaviour
{
    SoundManager SoundManager;

    void Start()
    {
        SoundManager = FindObjectOfType<SoundManager>();
    }
    
    public bool HandleInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
            return true;
        else
            return false;
    }

    public bool HandleEscMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            return true;
        else
            return false;
    }
}
