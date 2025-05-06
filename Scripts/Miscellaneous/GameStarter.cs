using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private bool loadOnStart = true;
    [SerializeField] private string mainSceneName = "Main";
    
    [Header("Menus")]
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject controlsMenu;
    private bool isEscapeMenuActive = false;
    private bool isControlsMenuActive = false;
    
    private void Start()
    {
        if (loadOnStart)
        {
            StartGame();
        }
        
        if (escapeMenu != null)
            escapeMenu.SetActive(false);
        if (controlsMenu != null)
            controlsMenu.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleEscapeMenu();
        }
    }
    
    public void ToggleEscapeMenu()
    {
        if (escapeMenu == null) return;
        
        isEscapeMenuActive = !isEscapeMenuActive;
        escapeMenu.SetActive(isEscapeMenuActive);
        
        Time.timeScale = isEscapeMenuActive ? 0f : 1f;
        
        if (isEscapeMenuActive && isControlsMenuActive)
        {
            ToggleControlsMenu();
        }
    }
    
    public void ToggleControlsMenu()
    {
        if (controlsMenu == null) return;
        
        isControlsMenuActive = !isControlsMenuActive;
        controlsMenu.SetActive(isControlsMenuActive);
    }
    
    public void StartGame()
    {
        Debug.Log("Starting game, loading scene: " + mainSceneName);
        SceneManager.LoadScene(mainSceneName);
    }
    
    public void StartGameWithDelay(float delay)
    {
        Debug.Log($"Starting game with {delay}s delay");
        Invoke(nameof(StartGame), delay);
    }
}
